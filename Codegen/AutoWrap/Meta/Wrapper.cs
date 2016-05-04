#region GPL license
/*
 * This source file is part of the AutoWrap code generator of the
 * MOGRE project (http://mogre.sourceforge.net), copyright (c) 
 * 
 * Copyright (C) 2006-2007 Argiris Kirtzidis
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AutoWrap.Meta
{
    public class Wrapper
    {
        /// <summary>
        /// Header text for all auto-generated source files.
        /// </summary>
        public static readonly string HEADER_TEXT =
              "/*  This file was produced by the C++/CLI AutoWrapper utility.\n"
            + " *          Copyright (c) 2006 by Argiris Kirtzidis\n"
            + " *          Copyright (c) 2010 by Manski\n"
            + " */\n\n";

        /// <summary>
        /// Contains a list of all native .h files for which code is going to be generated. For each file there is
        /// a list containing all wrappable (see <see cref="IsTypeWrappable"/>) types defined in this include file.
        /// The types in the lists have the following order:
        /// 1. enums and internal defs
        /// 2. native pointer value types
        /// 3. everything else (mostly classes)
        /// </summary>
        internal readonly SortedList<string, WrappedFile> IncludeFiles = new SortedList<string, WrappedFile>();

        /// <summary>
        /// This event is fired whenever a native .h files has been wrapped (i.e. when the corresponding
        /// CLR .cpp and .h files were generated).
        /// </summary>
        public event EventHandler<IncludeFileWrapEventArgs> IncludeFileWrapped;

        /// <summary>
        /// Path where to store the generated .h files.
        /// </summary>
        private readonly string _includePath;

        /// <summary>
        /// Path where to store the generated .cpp files.
        /// </summary>
        private readonly string _sourcePath;

        private readonly MetaDefinition _metaDef;

        private readonly List<ClassCodeProducer> _postClassProducers = new List<ClassCodeProducer>();
        private readonly List<ClassCodeProducer> _preClassProducers = new List<ClassCodeProducer>();
        private readonly List<AbstractTypeDefinition> _usedTypes = new List<AbstractTypeDefinition>();
        private readonly List<string> _preDeclarations = new List<string>();
        private readonly List<AbstractTypeDefinition> _pragmaMakePublicTypes = new List<AbstractTypeDefinition>();

        /// <summary>
        /// All classes that are not sealed.
        /// </summary>
        private readonly List<ClassDefinition> _overridables = new List<ClassDefinition>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="meta">the information about the data type to be wrapped</param>
        /// <param name="includePath">the path where to store the generated .h files</param>
        /// <param name="sourcePath">the path where to store the generated .cpp files</param>
        public Wrapper(MetaDefinition meta, string includePath, string sourcePath)
        {
            _includePath = includePath;
            _sourcePath = sourcePath;
            _metaDef = meta;

            foreach (NamespaceDefinition nsDef in meta.Namespaces)
            {
                foreach (AbstractTypeDefinition typeDef in nsDef.ContainedTypes)
                {
                    // If we can't wrap this type (for whatever reasons), skip it.
                    if (!IsTypeWrappable(typeDef))
                        continue;

                    WrappedFile inclFile;
                    if (!IncludeFiles.TryGetValue(typeDef.IncludeFileName, out inclFile))
                    {
                        inclFile = new WrappedFile();
                        IncludeFiles.Add(typeDef.IncludeFileName, inclFile);
                    }

                    // Insert type into the list. See documentation of "IncludeFiles" for the ordering.
                    if (typeDef is EnumDefinition || typeDef.IsInternalTypeDef)
                    {
                        inclFile.ContainedTypes.Insert(0, typeDef);
                    }
                    else if (typeDef.HasWrapType(WrapTypes.NativePtrValueType))
                    {
                        //put it after enums and before other classes
                        int i;
                        // Find the first type that is neither enum nor internal type. Insert type there.
                        for (i = 0; i < inclFile.ContainedTypes.Count; i++)
                        {
                            if (!(typeDef is EnumDefinition) && !typeDef.IsInternalTypeDef)
                                break;
                        }

                        inclFile.ContainedTypes.Insert(i, typeDef);
                    }
                    else
                        inclFile.ContainedTypes.Add(typeDef);

                    if (typeDef.HasWrapType(WrapTypes.Overridable))
                        _overridables.Add((ClassDefinition)typeDef);
                }

                foreach (AbstractTypeDefinition type in nsDef.ContainedTypes)
                {
                    if (type is EnumDefinition && IncludeFiles.ContainsKey(type.IncludeFileName))
                    {
                        if (!IncludeFiles[type.IncludeFileName].ContainedTypes.Contains(type))
                            IncludeFiles[type.IncludeFileName].ContainedTypes.Insert(0, type);
                    }
                }
            }
        }

        public void AddPreClassProducer(ClassCodeProducer producer)
        {
            _preClassProducers.Add(producer);
        }

        public void AddPostClassProducer(ClassCodeProducer producer)
        {
            _postClassProducers.Add(producer);
        }

        public void AddUsedType(AbstractTypeDefinition type)
        {
            _usedTypes.Add(type);
        }

        /// <summary>
        /// Determines whether the specified type can be wrapped (i.e. whether code files can be produced for it).
        /// </summary>
        /// <param name="type">the type to be checked</param>
        public static bool IsTypeWrappable(AbstractTypeDefinition type)
        {
            if (type.Name.StartsWith("DLL_"))
                //It's DLL function pointers of OgrePlatformManager.h
                return false;

            // For now, ignore un-named enums in the namespace
            if (type.Name.StartsWith("@"))
                return false;

            // Get explicit type or a new type if type has ReplaceBy attribute
            type = (type.IsNested) ? type.SurroundingClass.DetermineType(type.Name)
                       : type.Namespace.DetermineType(type.Name);

            if (type.HasAttribute<CustomClassInclCodeAttribute>())
                return true;

            // Check for "Ignore" only AFTER checking for a custom implementation (see previous condition).
            if (type.IsIgnored)
                return false;

            if (type.HasAttribute<WrapTypeAttribute>())
                return true;

            if (type.IsSharedPtr)
            {
                type.AddAttribute(new WrapTypeAttribute(WrapTypes.SharedPtr));
                return true;
            }

            if (type is ClassDefinition)
            {
                //
                // Classes
                //
                // Allow only classes marked as CLRObject or Singleton.
                ClassDefinition cls = type as ClassDefinition;
                if (cls.HasAttribute<CLRObjectAttribute>(true))
                {
                    if (!cls.HasAttribute<WrapTypeAttribute>(false))
                        cls.AddAttribute(new WrapTypeAttribute(WrapTypes.NonOverridable));
                    return true;
                }

                if (cls.IsSingleton)
                {
                    cls.AddAttribute(new WrapTypeAttribute(WrapTypes.Singleton));
                    return true;
                }

                return false;
            }

            if (type is TypedefDefinition)
            {
                //
                // Typedefs
                //
                if (type.IsSTLContainer)
                {
                    foreach (ITypeMember m in (type as TypedefDefinition).TypeParams)
                    {
                        AbstractTypeDefinition mt = m.MemberType;
                        if (!mt.IsValueType && !mt.IsPureManagedClass && !IsTypeWrappable(mt))
                            return false;
                    }

                    return true;
                }

                if (type is DefIterator)
                {
                    //
                    // Iterators
                    //
                    if (IsTypeWrappable((type as DefIterator).TypeParams[0].ParamType))
                    {
                        if ((type as DefIterator).IsConstIterator)
                        {
                            try
                            {
                                type.DetermineType<AbstractTypeDefinition>(type.Name.Substring("Const".Length));
                                return false;
                            }
                            catch
                            {
                                return true;
                            }
                        }

                        return true;
                    }

                    return false;
                }

                if (((TypedefDefinition)type).BaseType is DefInternal || ((TypedefDefinition)type).BaseType.HasAttribute<ValueTypeAttribute>())
                    return true;

                return IsTypeWrappable((type as TypedefDefinition).BaseType);
            }

            return false;
        }

        /// <summary>
        /// Generates the C++/CLI source and header files.
        /// </summary>
        public void GenerateCodeFiles()
        {
            StringBuilder builder = new StringBuilder();
            _preDeclarations.Clear();
            _pragmaMakePublicTypes.Clear();

            //
            // Generate all source files from header files.
            //
            foreach (string includeFile in IncludeFiles.Keys)
            {
                // Strip ".h" from the file name
                string baseFileName = GetCLRIncludeFileName(includeFile.Substring(0, includeFile.Length - 2));

                string incFile = _includePath + "\\" + baseFileName + ".h";
                string cppFile = _sourcePath + "\\" + baseFileName + ".cpp";

                // Header file
                string header = GenerateIncludeFileCodeForIncludeFile(includeFile);
                WriteToFile(incFile, header, true);

                // Source file
                string content = GenerateCppFileCodeForIncludeFile(includeFile);
                if (content != null)
                {
                    // There is a .cpp file for the .h file.
                    WriteToFile(cppFile, content, true);
                }

                if (IncludeFileWrapped != null)
                    IncludeFileWrapped(this, new IncludeFileWrapEventArgs(includeFile));
            }

            //
            // Create PreDeclarations.h
            //
            builder.Clear();
            foreach (string decl in _preDeclarations)
            {
                builder.AppendLine(decl);
            }

            WriteToFile(_includePath + "\\PreDeclarations.h", builder.ToString(), true);

            //
            // Create MakePublicDeclarations.h
            //
            builder.Clear();
            List<AbstractTypeDefinition> typesForMakePublic = new List<AbstractTypeDefinition>();

            foreach (AbstractTypeDefinition t in _pragmaMakePublicTypes)
            {
                if (!(t is ClassDefinition) || t.IsNested || t.IsIgnored)
                    continue;

                AbstractTypeDefinition type = t.DetermineType<AbstractTypeDefinition>(t.Name);

                if (!type.FullyQualifiedNativeName.StartsWith(_metaDef.NativeNamespace + "::") || !(type is ClassDefinition) || type.IsTemplate)
                    continue;

                if (!typesForMakePublic.Contains(type))
                    typesForMakePublic.Add(type);
            }

            builder.AppendLine("#pragma once");
            builder.AppendLine();

            builder.AppendLine("namespace " + _metaDef.NativeNamespace);
            builder.AppendLine("{");

            foreach (AbstractTypeDefinition type in typesForMakePublic)
            {
                if (type is StructDefinition)
                    builder.Append("struct ");
                else
                    builder.Append("class ");

                builder.AppendLine(type.Name + ";");
            }

            builder.AppendLine("}");
            builder.AppendLine();

            foreach (AbstractTypeDefinition type in typesForMakePublic)
            {
                builder.AppendLine("#pragma make_public( " + type.FullyQualifiedNativeName + " )");
            }

            WriteToFile(_includePath + "\\MakePublicDeclarations.h", builder.ToString(), true);

            //
            // Create CLRObjects.inc
            //
            builder.Clear();
            List<ClassDefinition> clrObjs = new List<ClassDefinition>();

            foreach (string include in IncludeFiles.Keys)
            {
                foreach (AbstractTypeDefinition t in IncludeFiles[include].ContainedTypes)
                    AddCLRObjects(t, clrObjs);
            }

            foreach (ClassDefinition cls in clrObjs)
            {
                string name = cls.Name;
                ClassDefinition parent = cls;
                while (parent.SurroundingClass != null)
                {
                    parent = parent.SurroundingClass;
                    name = parent.Name + "_" + name;
                }

                builder.AppendLine("CLROBJECT( " + name + " )");
            }

            WriteToFile(_includePath + "\\CLRObjects.inc", builder.ToString(), true);
        }

        public string GetInitCLRObjectFuncSignature(ClassDefinition cls)
        {
            if (!cls.HasAttribute<CLRObjectAttribute>(true))
                throw new Exception("class is not subclass of CLRObject");

            string name = cls.Name;
            ClassDefinition parent = cls;
            while (parent.SurroundingClass != null)
            {
                parent = parent.SurroundingClass;
                name = parent.Name + "_" + name;
            }

            return "void _Init_CLRObject_" + name + "(CLRObject* pClrObj)";
        }

        private void AddCLRObjects(AbstractTypeDefinition t, List<ClassDefinition> clrObjs)
        {
            ClassDefinition cls = t as ClassDefinition;

            if (cls == null)
                return;

            if (cls.IsIgnored || cls.ProtectionLevel != ProtectionLevel.Public)
                return;

            if (cls.HasAttribute<CLRObjectAttribute>(true))
                clrObjs.Add(cls);

            foreach (AbstractTypeDefinition nested in cls.NestedTypes)
                AddCLRObjects(nested, clrObjs);
        }

        public void ProduceSubclassCodeFiles(System.Windows.Forms.ProgressBar bar)
        {
            bar.Minimum = 0;
            bar.Maximum = _overridables.Count;
            bar.Step = 1;
            bar.Value = 0;

            foreach (ClassDefinition type in _overridables)
            {
                string wrapFile = "Subclass" + type.Name;
                string incFile = _includePath + "\\" + wrapFile + ".h";
                string cppFile = _sourcePath + "\\" + wrapFile + ".cpp";

                WriteToFile(incFile, GenerateIncludeFileCodeForOverridable(type), true);

                WriteToFile(cppFile, GenerateCppFileCodeForOverridable(type), true);

                bar.Value++;
                bar.Refresh();
            }
        }

        /// <summary>
        /// Writes the contents to the specified file. Checks whether the content has actually
        /// changed to prevent unnecessary rebuilds.
        /// </summary>
        protected void WriteToFile(string file, string contents, bool addHeader)
        {
            if (addHeader)
                contents = HEADER_TEXT.Replace("\n", _metaDef.CodeStyleDef.NewLineCharacters) + contents;

            // Check whether the contents are identical. Don't overwrite the file to prevent
            // the file from being compiled again (when it hasn't been changed).
            if (File.Exists(file))
            {
                string filecontent;
                using (StreamReader inp = new StreamReader(file, Encoding.UTF8))
                {
                    filecontent = inp.ReadToEnd();
                }

                if (contents == filecontent)
                    return;
            }

            using (StreamWriter writer = File.CreateText(file))
            {
                writer.Write(contents);
            }
        }

        /// <summary>
        /// Returns the CLR .h file name for a native .h file name.
        /// </summary>
        /// <param name="nativeIncludeFileName">the native .h file name</param>
        protected string GetCLRIncludeFileName(string nativeIncludeFileName)
        {
            // Native files from subdirectories (e.g. "WIN32") will go directly in the CLR directory
            // with their directory names being part of the file name (e.g. "WIN32_MyFileName.h").
            nativeIncludeFileName = nativeIncludeFileName.Replace('/', '_').Replace('\\', '_');

            if (nativeIncludeFileName.StartsWith(_metaDef.NativeNamespace))
                return _metaDef.ManagedNamespace + nativeIncludeFileName.Substring(_metaDef.NativeNamespace.Length);

            return _metaDef.ManagedNamespace + "-" + nativeIncludeFileName;
        }

        /// <summary>
        /// Generates the C++/CLI code for the CLR .h file.
        /// </summary>
        /// <param name="includeFile">the name of the native .h file from which to generate the code</param>
        public string GenerateIncludeFileCodeForIncludeFile(string includeFile)
        {
            _usedTypes.Clear();

            _preClassProducers.Clear();
            _postClassProducers.Clear();

            SourceCodeStringBuilder sbTypes = new SourceCodeStringBuilder(_metaDef.CodeStyleDef);
            foreach (AbstractTypeDefinition t in IncludeFiles[includeFile].ContainedTypes)
            {
                IncAddType(t, sbTypes);
            }

            foreach (ClassCodeProducer producer in _postClassProducers)
            {
                producer.GenerateCode();
            }

            foreach (ClassCodeProducer producer in _preClassProducers)
            {
                producer.GenerateCodeAtBeginning();
            }

            SourceCodeStringBuilder sb = new SourceCodeStringBuilder(_metaDef.CodeStyleDef);
            sb.AppendLine("#pragma once\n");

            IncAddIncludeFiles(includeFile, _usedTypes, sb);

            sb.AppendLine("namespace {0}", _metaDef.ManagedNamespace);

            sb.BeginBlock();
            sb.AppendLine(sbTypes.ToString());
            sb.EndBlock();

            return sb.ToString();
        }

        /// <summary>
        /// Genereates the content of a CLR .cpp file for a native .h file. Returns <c>null</c>, if there is no content
        /// for a .cpp file (i.e. if no .cpp file shall be created).
        /// </summary>
        /// <param name="includeFile">the name of the native .h file from which to generate the code</param>
        public string GenerateCppFileCodeForIncludeFile(string includeFileName)
        {
            _usedTypes.Clear();

            _preClassProducers.Clear();
            _postClassProducers.Clear();

            SourceCodeStringBuilder contentsb = new SourceCodeStringBuilder(_metaDef.CodeStyleDef);

            //
            // Generate content
            //
            foreach (AbstractTypeDefinition t in IncludeFiles[includeFileName].ContainedTypes)
            {
                CppAddType(t, contentsb);
            }

            foreach (ClassCodeProducer producer in _postClassProducers)
            {
                producer.GenerateCode();
            }

            foreach (ClassCodeProducer producer in _preClassProducers)
            {
                producer.GenerateCodeAtBeginning();
            }

            string content = contentsb.ToString();
            if (content == "")
            {
                // No content for the .cpp file. Happens for some constructs that'll only have .h files.
                return null;
            }

            //
            // Generate surroundings (include statements and namespace block)
            //
            contentsb.Clear();

            CppAddIncludeFiles(includeFileName, _usedTypes, contentsb);

            contentsb.AppendLine("namespace {0}", _metaDef.ManagedNamespace);

            contentsb.BeginBlock();
            contentsb.AppendLine(content);

            contentsb.EndBlock();

            return contentsb.ToString();
        }

        public string GenerateIncludeFileCodeForOverridable(ClassDefinition type)
        {
            _usedTypes.Clear();

            _preClassProducers.Clear();
            _postClassProducers.Clear();

            SourceCodeStringBuilder sbTypes = new SourceCodeStringBuilder(_metaDef.CodeStyleDef);

            new IncSubclassingClassProducer(this._metaDef, this, type, sbTypes, null).GenerateCode();
            if (type.HasAttribute<InterfacesForOverridableAttribute>())
            {
                List<ClassDefinition[]> interfaces = type.GetAttribute<InterfacesForOverridableAttribute>().Interfaces;
                foreach (ClassDefinition[] ifaces in interfaces)
                {
                    new IncSubclassingClassProducer(this._metaDef, this, type, sbTypes, ifaces).GenerateCode();
                }
            }

            foreach (ClassCodeProducer producer in _postClassProducers)
            {
                if (!(producer is NativeProtectedTypesProxy)
                    && !(producer is NativeProtectedStaticsProxy))
                    producer.GenerateCode();
            }

            foreach (ClassCodeProducer producer in _preClassProducers)
            {
                if (!(producer is NativeProtectedTypesProxy)
                    && !(producer is NativeProtectedStaticsProxy))
                    producer.GenerateCodeAtBeginning();
            }

            SourceCodeStringBuilder sb = new SourceCodeStringBuilder(_metaDef.CodeStyleDef);
            sb.AppendLine("#pragma once\n");

            sb.AppendLine("namespace {0}", _metaDef.ManagedNamespace);

            sb.BeginBlock();
            sb.AppendLine(sbTypes.ToString());
            sb.EndBlock();

            return sb.ToString();
        }

        public string GenerateCppFileCodeForOverridable(ClassDefinition type)
        {
            SourceCodeStringBuilder sb = new SourceCodeStringBuilder(_metaDef.CodeStyleDef);

            sb.AppendLine("#include \"MogreStableHeaders.h\"");
            sb.AppendLine("#include \"Subclass" + type.Name + ".h\"\n");

            sb.AppendLine("namespace {0}", _metaDef.ManagedNamespace);

            sb.BeginBlock();

            _preClassProducers.Clear();
            _postClassProducers.Clear();

            new CppSubclassingClassProducer(this._metaDef, this, type, sb, null).GenerateCode();
            if (type.HasAttribute<InterfacesForOverridableAttribute>())
            {
                List<ClassDefinition[]> interfaces = type.GetAttribute<InterfacesForOverridableAttribute>().Interfaces;
                foreach (ClassDefinition[] ifaces in interfaces)
                {
                    new CppSubclassingClassProducer(this._metaDef, this, type, sb, ifaces).GenerateCode();
                }
            }

            foreach (ClassCodeProducer producer in _postClassProducers)
            {
                if (!(producer is NativeProtectedTypesProxy)
                    && !(producer is NativeProtectedStaticsProxy))
                    producer.GenerateCode();
            }

            foreach (ClassCodeProducer producer in _preClassProducers)
            {
                if (!(producer is NativeProtectedTypesProxy)
                    && !(producer is NativeProtectedStaticsProxy))
                    producer.GenerateCodeAtBeginning();
            }

            sb.EndBlock();

            return sb.ToString();
        }

        public void IncAddType(AbstractTypeDefinition t, SourceCodeStringBuilder sb)
        {
            if (t.HasAttribute<CustomClassInclCodeAttribute>())
            {
                sb.AppendLine(t.GetAttribute<CustomClassInclCodeAttribute>().Text);
                return;
            }

            if (t is ClassDefinition)
            {
                if (!t.HasAttribute<WrapTypeAttribute>())
                {
                    //Ignore
                }
                else
                {
                    switch (t.GetAttribute<WrapTypeAttribute>().WrapType)
                    {
                        case WrapTypes.NonOverridable:
                            new NonOverridableClassInclProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.Overridable:
                            new IncOverridableClassProducer(_metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.NativeDirector:
                            new NativeDirectorClassInclProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.Interface:
                            new InterfaceClassInclProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            new IncOverridableClassProducer(_metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.Singleton:
                            new SingletonClassInclProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.ReadOnlyStruct:
                            new ReadOnlyStructClassInclProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.ValueType:
                            new ValueClassInclProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.NativePtrValueType:
                            new NativePtrValueClassInclProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.CLRHandle:
                            new CLRHandleClassInclProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.PlainWrapper:
                            new PlainWrapperClassInclProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.SharedPtr:
                            IncAddSharedPtrType(t, sb);
                            break;
                    }
                }
            }
            else if (t is EnumDefinition)
            {
                IncAddEnum(t as EnumDefinition, sb);
            }
            else if (t is TypedefDefinition)
            {
                TypedefDefinition explicitType;

                if (t.IsUnnamedSTLContainer)
                    explicitType = t as TypedefDefinition;
                else
                    explicitType = (t.IsNested) ? t.SurroundingClass.DetermineType<TypedefDefinition>(t.Name)
                                       : t.Namespace.DetermineType<TypedefDefinition>(t.Name);

                if (t.HasWrapType(WrapTypes.SharedPtr))
                    IncAddSharedPtrType(t, sb);
                else if (explicitType.IsSTLContainer)
                    IncAddSTLContainer(explicitType, sb);
                else if (explicitType is DefIterator)
                    IncAddIterator(explicitType as DefIterator, sb);
                else if (explicitType.BaseType is DefInternal
                         || (explicitType.Name != "String" && explicitType.Name != "UTFString" && explicitType.IsTypedefOfInternalType))
                    IncAddInternalTypeDef(explicitType, sb);
                else if (explicitType.BaseType.HasAttribute<ValueTypeAttribute>())
                    IncAddValueTypeTypeDef(explicitType, sb);
            }
        }

        public void CppAddType(AbstractTypeDefinition t, SourceCodeStringBuilder sb)
        {
            if (t.HasAttribute<CustomCppClassDefinitionAttribute>())
            {
                string txt = t.GetAttribute<CustomCppClassDefinitionAttribute>().Text;
                sb.AppendLine(txt);
                return;
            }

            if (t is ClassDefinition)
            {
                if (!t.HasAttribute<WrapTypeAttribute>())
                {
                    //Ignore
                }
                else
                {
                    switch (t.GetAttribute<WrapTypeAttribute>().WrapType)
                    {
                        case WrapTypes.NonOverridable:
                            new NonOverridableClassCppProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.Overridable:
                            new OverridableClassCppProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.Interface:
                            new OverridableClassCppProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.NativeDirector:
                            new NativeDirectorClassCppProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.NativePtrValueType:
                            new NativePtrValueClassCppProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.Singleton:
                            new SingletonClassCppProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.CLRHandle:
                            new CLRHandleClassCppProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                        case WrapTypes.PlainWrapper:
                            new PlainWrapperClassCppProducer(this._metaDef, this, t as ClassDefinition, sb).GenerateCode();
                            break;
                    }
                }
            }
            else if (t is TypedefDefinition)
            {
                TypedefDefinition explicitType;

                if (t.IsUnnamedSTLContainer)
                    explicitType = t as TypedefDefinition;
                else
                    explicitType = (t.IsNested) ? t.SurroundingClass.DetermineType<TypedefDefinition>(t.Name)
                                       : t.Namespace.DetermineType<TypedefDefinition>(t.Name);

                if (explicitType.IsSTLContainer)
                    CppAddSTLContainer(explicitType, sb);
                else if (explicitType is DefIterator)
                    CppAddIterator(explicitType as DefIterator, sb);
            }
        }

        //public void IncAddEnum(DefEnum enm, IndentStringBuilder sb)
        //{
        //    IncAddEnum(enm, sb, false);
        //}
        public void IncAddEnum(EnumDefinition enm, SourceCodeStringBuilder sb) //, bool inProtectedTypesProxy)
        {
            if (enm.Name[0] == '@')
                return;

            string summary = enm.Summary;
            if (!String.IsNullOrEmpty(summary))
            {
                sb.AppendLine("/**");
                sb.AppendLine("<summary>{0}</summary>", CodeStyleDefinition.ToCamelCase(summary));
                sb.AppendLine("*/");
            }

            if (enm.HasAttribute<FlagsEnumAttribute>())
                sb.AppendLine("[Flags]");

            if (!enm.IsNested)
                sb.Append("public ");
            else
                sb.Append(enm.ProtectionLevel.GetCLRProtectionName() + ": ");

            //if (inProtectedTypesProxy)
            //sb.Append("enum " + enm.Name + "\n");
            //else
            sb.AppendLine("enum class " + enm.CLRName);

            sb.AppendLine("{");
            sb.IncreaseIndent();
            for (int i = 0; i < enm.CLREnumValues.Length; i++)
            {
                if (!String.IsNullOrEmpty(enm.CLREnumSummaries[i]))
                {
                    sb.AppendLine("///<summary>{0}</summary>", CodeStyleDefinition.ToCamelCase(enm.CLREnumSummaries[i]));
                }

                string value = enm.NativeEnumValues[i];
                //if (inProtectedTypesProxy)
                //{
                //    value = enm.ParentFullNativeName + "::" + enm.CLREnumValues[i];
                //    sb.Append("PUBLIC_");
                //}

                sb.Append(enm.CLREnumValues[i] + " = " + value);
                if (i < enm.CLREnumValues.Length - 1)
                    sb.Append(",");
                sb.AppendEmptyLine();
            }
            sb.DecreaseIndent();
            sb.AppendLine("};\n");
        }

        private void IncAddSharedPtrType(AbstractTypeDefinition type, SourceCodeStringBuilder sb)
        {
            if (!type.Name.EndsWith("Ptr"))
                throw new Exception("SharedPtr class that doesn't have a name ending to 'Ptr'");

            string basename;
            if (type is ClassDefinition)
                basename = (type as ClassDefinition).BaseClassesNames[0];
            else
                basename = (type as TypedefDefinition).BaseTypeName;

            int s = basename.IndexOf("<");
            int e = basename.LastIndexOf(">");
            string baseClass = basename.Substring(s + 1, e - s - 1).Trim();
            //string nativeClass = _nativePrefix + "::" + baseClass;
            AbstractTypeDefinition baseType = type.DetermineType<AbstractTypeDefinition>(baseClass);
            string nativeClass = baseType.FullyQualifiedNativeName;

            string className = type.FullyQualifiedCLRName;
            if (className.Contains("::"))
                className = className.Substring(className.IndexOf("::") + 2);

            if (!type.IsNested)
            {
                _preDeclarations.Add("ref class " + type.Name + ";");
                sb.Append("public ");
            }
            else
            {
                sb.Append(type.ProtectionLevel.GetCLRProtectionName() + ": ");
            }

            sb.AppendLine("ref class " + type.Name + " : public " + baseClass);
            sb.AppendLine("{");
            sb.AppendLine("public protected:");
            sb.IncreaseIndent();
            sb.AppendLine("\t" + type.FullyQualifiedNativeName + "* _sharedPtr;");
            sb.AppendEmptyLine();
            sb.AppendLine(type.Name + "(" + type.FullyQualifiedNativeName + "& sharedPtr) : " + baseClass + "( sharedPtr.getPointer() )");
            sb.BeginBlock();
            sb.AppendLine("_sharedPtr = new " + type.FullyQualifiedNativeName + "(sharedPtr);");
            sb.EndBlock();
            sb.AppendEmptyLine();
            sb.AppendLine("!" + type.Name + "()");
            sb.BeginBlock();
            sb.AppendLine("if (_sharedPtr != 0)");
            sb.BeginBlock();
            sb.AppendLine("delete _sharedPtr;");
            sb.AppendLine("_sharedPtr = 0;");
            sb.EndBlock();
            sb.EndBlock();
            sb.AppendEmptyLine();
            sb.AppendLine("~" + type.Name + "()");
            sb.BeginBlock();
            sb.AppendLine("this->!" + type.Name + "();");
            sb.EndBlock();
            sb.AppendEmptyLine();
            sb.DecreaseIndent();
            sb.AppendLine("public:");
            sb.IncreaseIndent();

            sb.AppendLine("static operator {0}^ (const Ogre::{0}& ptr)", className);
            sb.BeginBlock();
            sb.AppendLine("if (ptr.isNull()) return nullptr;");
            sb.AppendLine("return gcnew {0}(const_cast<Ogre::{0}&>(ptr));", className);
            sb.EndBlock();
            sb.AppendEmptyLine();
            sb.AppendLine("static operator Ogre::{0}& ({0}^ t)", className);
            sb.BeginBlock();
            sb.AppendLine("if (CLR_NULL == t) return *((gcnew {0}(Ogre::{0}()))->_sharedPtr);", className);
            sb.AppendLine("return *(t->_sharedPtr);");
            sb.EndBlock();
            sb.AppendEmptyLine();
            sb.AppendLine("static operator Ogre::{0}* ({0}^ t)", className);
            sb.BeginBlock();
            sb.AppendLine("if (CLR_NULL == t) return (gcnew {0}(Ogre::{0}()))->_sharedPtr;", className);
            sb.AppendLine("return t->_sharedPtr;");
            sb.EndBlock();
            sb.AppendEmptyLine();

            if (type is ClassDefinition)
            {
                ClassDefinition realType = type.DetermineType<ClassDefinition>(baseClass, false);
                if (realType != null && realType.BaseClass != null && realType.BaseClass.Name == "Resource")
                {
                    // For Resource subclasses (Material etc.) allow implicit conversion of ResourcePtr (i.e ResourcePtr -> MaterialPtr)

                    AddTypeDependancy(realType.BaseClass);

                    sb.AppendLine("static " + type.Name + "^ FromResourcePtr( ResourcePtr^ ptr )");
                    sb.BeginBlock();
                    sb.AppendLine("return (" + type.Name + "^) ptr;");
                    sb.EndBlock();
                    sb.AppendEmptyLine();

                    sb.AppendLine("static operator " + type.Name + "^ ( ResourcePtr^ ptr )");
                    sb.BeginBlock();
                    sb.AppendLine("if (CLR_NULL == ptr) return nullptr;");
                    sb.AppendLine("void* castptr = dynamic_cast<" + nativeClass + "*>(ptr->_native);");
                    sb.AppendLine("if (castptr == 0) throw gcnew InvalidCastException(\"The underlying type of the ResourcePtr object is not of type " + baseClass + ".\");");
                    sb.AppendLine("return gcnew " + type.Name + "( (" + type.FullyQualifiedNativeName + ") *(ptr->_sharedPtr) );");
                    sb.EndBlock();
                    sb.AppendEmptyLine();
                }
            }

            //sb.AppendLine(type.Name + "() : " + baseClass + "( (" + nativeClass + "*) 0 )");
            //sb.AppendLine("{");
            //sb.AppendLine("\t_sharedPtr = new " + type.FullNativeName + "();");
            //sb.AppendLine("}");
            //sb.AppendLine();
            if (baseType is ClassDefinition && (baseType as ClassDefinition).IsInterface)
            {
                string proxyName = NativeProxyClassProducer.GetProxyName(baseType as ClassDefinition);
                sb.AppendLine(type.Name + "(" + baseType.CLRName + "^ obj) : " + baseClass + "( static_cast<" + proxyName + "*>( (" + nativeClass + "*)obj ) )");
                sb.BeginBlock();
                sb.AppendLine("_sharedPtr = new " + type.FullyQualifiedNativeName + "( static_cast<" + proxyName + "*>(obj->_native) );");
                sb.EndBlock();
                sb.AppendEmptyLine();
            }
            else
            {
                sb.AppendLine(type.Name + "(" + baseClass + "^ obj) : " + baseClass + "( obj->_native )");
                sb.BeginBlock();
                sb.AppendLine("_sharedPtr = new " + type.FullyQualifiedNativeName + "( static_cast<" + nativeClass + "*>(obj->_native) );");
                sb.EndBlock();
                sb.AppendEmptyLine();
            }
            //sb.AppendLine("void Bind(" + baseClass + "^ obj)");
            //sb.AppendLine("{");
            //sb.AppendLine("\t(*_sharedPtr).bind( static_cast<" + nativeClass + "*>(obj->_native) );");
            //sb.AppendLine("}");
            //sb.AppendLine();

            sb.AppendLine("virtual bool Equals(Object^ obj) override");
            sb.BeginBlock();
            sb.AppendLine(type.Name + "^ clr = dynamic_cast<" + type.Name + "^>(obj);");
            sb.AppendLine("if (clr == CLR_NULL)");
            sb.BeginBlock();
            sb.AppendLine("return false;");
            sb.EndBlock();
            sb.AppendEmptyLine();
            sb.AppendLine("return (_native == clr->_native);");
            sb.EndBlock();
            sb.AppendLine("bool Equals(" + type.Name + "^ obj)");
            sb.BeginBlock();
            sb.AppendLine("if (obj == CLR_NULL)");
            sb.BeginBlock();
            sb.AppendLine("return false;");
            sb.EndBlock();
            sb.AppendEmptyLine();
            sb.AppendLine("return (_native == obj->_native);");
            sb.EndBlock();
            sb.AppendEmptyLine();

            sb.AppendLine("static bool operator == (" + type.Name + "^ val1, " + type.Name + "^ val2)");
            sb.BeginBlock();
            sb.AppendLine("if ((Object^)val1 == (Object^)val2) return true;");
            sb.AppendLine("if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;");
            sb.AppendLine("return (val1->_native == val2->_native);");
            sb.EndBlock();
            sb.AppendEmptyLine();
            sb.AppendLine("static bool operator != (" + type.Name + "^ val1, " + type.Name + "^ val2)");
            sb.BeginBlock();
            sb.AppendLine("return !(val1 == val2);");
            sb.EndBlock();
            sb.AppendEmptyLine();

            sb.AppendLine("virtual int GetHashCode() override");
            sb.BeginBlock();
            sb.AppendLine("return reinterpret_cast<int>( _native );");
            sb.EndBlock();
            sb.AppendEmptyLine();

            sb.AppendLine("property IntPtr NativePtr");
            sb.BeginBlock();
            sb.AppendLine("IntPtr get() { return (IntPtr)_sharedPtr; }");
            sb.EndBlock();
            sb.AppendEmptyLine();

            sb.AppendLine("property bool Unique");
            sb.BeginBlock();
            sb.AppendLine("bool get()");
            sb.BeginBlock();
            sb.AppendLine("return (*_sharedPtr).unique();");
            sb.EndBlock();
            sb.EndBlock();
            sb.AppendEmptyLine();
            sb.AppendLine("property int UseCount");
            sb.BeginBlock();
            sb.AppendLine("int get()");
            sb.BeginBlock();
            sb.AppendLine("return (*_sharedPtr).useCount();");
            sb.EndBlock();
            sb.EndBlock();
            sb.AppendEmptyLine();
            //sb.AppendLine("void SetNull()");
            //sb.AppendLine("{");
            //sb.AppendLine("\t(*_sharedPtr).setNull();");
            //sb.AppendLine("\t_native = 0;");
            //sb.AppendLine("}");
            //sb.AppendLine();
            //sb.AppendLine("property bool IsNull");
            //sb.AppendLine("{");
            //sb.IncreaseIndent();
            //sb.AppendLine("bool get()");
            //sb.AppendLine("{");
            //sb.AppendLine("\treturn (*_sharedPtr).isNull();");
            //sb.AppendLine("}");
            //sb.DecreaseIndent();
            //sb.AppendLine("}");
            //sb.AppendLine();
            sb.AppendLine("property " + baseClass + "^ Target");
            sb.BeginBlock();
            sb.AppendLine(baseClass + "^ get()");
            sb.BeginBlock();
            sb.AppendLine("return static_cast<" + nativeClass + "*>(_native);");
            sb.EndBlock();
            sb.EndBlock();
            sb.DecreaseIndent();
            sb.AppendLine("};\n\n");
        }

        public void IncAddSTLContainer(TypedefDefinition t, SourceCodeStringBuilder sb)
        {
            if (t is DefStdPair)
            {
                //sb.AppendIndent("");
                //if (t.IsNested)
                //    sb.Append(Producer.GetProtectionString(t.ProtectionType) + ": ");
                //sb.Append("typedef " + t.FullSTLContainerTypeName + " " + t.CLRName + ";\n\n");
                return;
            }

            if (!t.IsNested)
            {
                AddPreDeclaration("ref class " + t.CLRName + ";");
                AddPreDeclaration("ref class Const_" + t.CLRName + ";");
            }

            if (t is DefTemplateOneType)
            {
                if (t.HasAttribute<STLListNoRemoveAndUniqueAttribute>())
                {
                    sb.AppendLine("#undef INC_STLLIST_DEFINE_REMOVE_AND_UNIQUE");
                    sb.AppendLine("#define INC_STLLIST_DEFINE_REMOVE_AND_UNIQUE(M)");
                    sb.AppendEmptyLine();
                }

                sb.AppendLine("#define STLDECL_MANAGEDTYPE " + t.TypeParams[0].MemberTypeCLRName);
                sb.AppendLine("#define STLDECL_NATIVETYPE " + t.TypeParams[0].MemberTypeNativeName);
                CheckTypeForDependancy(t.TypeParams[0].ParamType);
            }
            else if (t is DefTemplateTwoTypes)
            {
                sb.AppendLine("#define STLDECL_MANAGEDKEY " + t.TypeParams[0].MemberTypeCLRName);
                sb.AppendLine("#define STLDECL_MANAGEDVALUE " + t.TypeParams[1].MemberTypeCLRName);
                sb.AppendLine("#define STLDECL_NATIVEKEY " + t.TypeParams[0].MemberTypeNativeName);
                sb.AppendLine("#define STLDECL_NATIVEVALUE " + t.TypeParams[1].MemberTypeNativeName);
                CheckTypeForDependancy(t.TypeParams[0].ParamType);
                CheckTypeForDependancy(t.TypeParams[1].ParamType);
            }

            string publicprot, privateprot;
            if (!t.IsNested)
            {
                publicprot = "public";
                privateprot = "private";
            }
            else
            {
                publicprot = t.ProtectionLevel.GetCLRProtectionName() + ": ";
                privateprot = "private:";
                sb.Append(publicprot);
            }

            sb.Append("INC_DECLARE_STL" + t.STLContainer.ToUpper());

            if (t.IsReadOnly)
                sb.Append("_READONLY");

            sb.Append("( " + t.CLRName);

            if (t is DefTemplateOneType)
                sb.AppendLine(", STLDECL_MANAGEDTYPE, STLDECL_NATIVETYPE, " + publicprot + ", " + privateprot + " )");
            else if (t is DefTemplateTwoTypes)
                sb.AppendLine(", STLDECL_MANAGEDKEY, STLDECL_MANAGEDVALUE, STLDECL_NATIVEKEY, STLDECL_NATIVEVALUE, " + publicprot + ", " + privateprot + " )");
            else
                throw new Exception("Unexpected");

            if (t is DefTemplateOneType)
            {
                sb.AppendLine("#undef STLDECL_MANAGEDTYPE");
                sb.AppendLine("#undef STLDECL_NATIVETYPE");

                if (t.HasAttribute<STLListNoRemoveAndUniqueAttribute>())
                {
                    sb.AppendEmptyLine();
                    sb.AppendLine("#undef INC_STLLIST_DEFINE_REMOVE_AND_UNIQUE");
                    sb.AppendLine("#define INC_STLLIST_DEFINE_REMOVE_AND_UNIQUE(M)    INC_STLLIST_REMOVE_AND_UNIQUE_DEFINITIONS(M)");
                }
            }
            else if (t is DefTemplateTwoTypes)
            {
                sb.AppendLine("#undef STLDECL_MANAGEDKEY");
                sb.AppendLine("#undef STLDECL_MANAGEDVALUE");
                sb.AppendLine("#undef STLDECL_NATIVEKEY");
                sb.AppendLine("#undef STLDECL_NATIVEVALUE");
            }

            sb.AppendEmptyLine();
        }

        public void CppAddSTLContainer(TypedefDefinition t, SourceCodeStringBuilder sb)
        {
            if (t is DefStdPair)
                return;

            if (t is DefTemplateOneType)
            {
                if (t.HasAttribute<STLListNoRemoveAndUniqueAttribute>())
                {
                    sb.AppendLine("#undef CPP_STLLIST_DEFINE_REMOVE_AND_UNIQUE");
                    sb.AppendLine("#define CPP_STLLIST_DEFINE_REMOVE_AND_UNIQUE(PREFIX,CLASS_NAME,M,N)");
                    sb.AppendEmptyLine();
                }

                sb.AppendLine("#define STLDECL_MANAGEDTYPE " + t.TypeParams[0].MemberTypeCLRName);
                sb.AppendLine("#define STLDECL_NATIVETYPE " + t.TypeParams[0].MemberTypeNativeName);
                CppCheckTypeForDependancy(t.TypeParams[0].ParamType);
            }
            else if (t is DefTemplateTwoTypes)
            {
                sb.AppendLine("#define STLDECL_MANAGEDKEY " + t.TypeParams[0].MemberTypeCLRName);
                sb.AppendLine("#define STLDECL_MANAGEDVALUE " + t.TypeParams[1].MemberTypeCLRName);
                sb.AppendLine("#define STLDECL_NATIVEKEY " + t.TypeParams[0].MemberTypeNativeName);
                sb.AppendLine("#define STLDECL_NATIVEVALUE " + t.TypeParams[1].MemberTypeNativeName);
                CppCheckTypeForDependancy(t.TypeParams[0].ParamType);
                CppCheckTypeForDependancy(t.TypeParams[1].ParamType);
            }

            sb.Append("CPP_DECLARE_STL" + t.STLContainer.ToUpper());

            if (t.IsReadOnly)
                sb.Append("_READONLY");

            string prefix;
            if (!t.IsNested)
            {
                prefix = t.FullyQualifiedNativeName;
                prefix = prefix.Substring(0, prefix.LastIndexOf("::"));
            }
            else
            {
                prefix = t.SurroundingClass.FullyQualifiedNativeName;
            }

            if (prefix.Contains("::"))
                prefix = prefix.Substring(prefix.IndexOf("::") + 2) + "::";
            else
                prefix = "";

            sb.Append("( " + prefix + ", " + t.CLRName);

            if (t is DefTemplateOneType)
                sb.AppendLine(", STLDECL_MANAGEDTYPE, STLDECL_NATIVETYPE )");
            else if (t is DefTemplateTwoTypes)
                sb.AppendLine(", STLDECL_MANAGEDKEY, STLDECL_MANAGEDVALUE, STLDECL_NATIVEKEY, STLDECL_NATIVEVALUE )");
            else
                throw new Exception("Unexpected");

            if (t is DefTemplateOneType)
            {
                sb.AppendLine("#undef STLDECL_MANAGEDTYPE");
                sb.AppendLine("#undef STLDECL_NATIVETYPE");

                if (t.HasAttribute<STLListNoRemoveAndUniqueAttribute>())
                {
                    sb.AppendEmptyLine();
                    sb.AppendLine("#undef CPP_STLLIST_DEFINE_REMOVE_AND_UNIQUE");
                    sb.AppendLine("#define CPP_STLLIST_DEFINE_REMOVE_AND_UNIQUE(PREFIX,CLASS_NAME,M,N)    CPP_STLLIST_REMOVE_AND_UNIQUE_DEFINITIONS(PREFIX,CLASS_NAME,M,N)");
                }
            }
            else if (t is DefTemplateTwoTypes)
            {
                sb.AppendLine("#undef STLDECL_MANAGEDKEY");
                sb.AppendLine("#undef STLDECL_MANAGEDVALUE");
                sb.AppendLine("#undef STLDECL_NATIVEKEY");
                sb.AppendLine("#undef STLDECL_NATIVEVALUE");
            }

            sb.AppendEmptyLine();
        }

        public void IncAddIterator(DefIterator t, SourceCodeStringBuilder sb)
        {
            if (!t.IsNested)
                AddPreDeclaration("ref class " + t.CLRName + ";");

            CheckTypeForDependancy(t.IterationElementTypeMember.MemberType);

            if (t.IsMapIterator)
                CheckTypeForDependancy(t.IterationKeyTypeMember.MemberType);

            sb.Append(t.ProtectionLevel.GetCLRProtectionName());
            if (t.IsNested)
                sb.Append(":");

            if (t.IsMapIterator)
                sb.Append(" INC_DECLARE_MAP_ITERATOR");
            else
                sb.Append(" INC_DECLARE_ITERATOR");

            if (t.TypeParams[0].ParamType.ProtectionLevel == ProtectionLevel.Protected
                && !t.TypeParams[0].ParamType.SurroundingClass.AllowVirtuals)
            {
                // the container type will not be declared,
                // declare an iterator without a constructor that takes a container class
                sb.Append("_NOCONSTRUCTOR");
            }

            if (t.IsMapIterator)
                sb.AppendLine("( " + t.CLRName + ", " + t.FullyQualifiedNativeName + ", " + t.TypeParams[0].ParamType.FullyQualifiedCLRName + ", " + t.IterationElementTypeMember.MemberTypeCLRName + ", " + t.IterationElementTypeMember.MemberTypeNativeName + ", " + t.IterationKeyTypeMember.MemberTypeCLRName + ", " + t.IterationKeyTypeMember.MemberTypeNativeName + " )");
            else
                sb.AppendLine("( " + t.CLRName + ", " + t.FullyQualifiedNativeName + ", " + t.TypeParams[0].ParamType.FullyQualifiedCLRName + ", " + t.IterationElementTypeMember.MemberTypeCLRName + ", " + t.IterationElementTypeMember.MemberTypeNativeName + " )");

            sb.AppendEmptyLine();
        }

        public void CppAddIterator(DefIterator t, SourceCodeStringBuilder sb)
        {
            string prefix;
            if (!t.IsNested)
            {
                prefix = t.FullyQualifiedNativeName;
                prefix = prefix.Substring(0, prefix.LastIndexOf("::"));
            }
            else
                prefix = t.SurroundingClass.FullyQualifiedNativeName;

            if (prefix.Contains("::"))
                prefix = prefix.Substring(prefix.IndexOf("::") + 2) + "::";
            else
                prefix = "";

            if (t.IsMapIterator)
                sb.Append("CPP_DECLARE_MAP_ITERATOR");
            else
                sb.Append("CPP_DECLARE_ITERATOR");

            bool noConstructor = t.TypeParams[0].ParamType.ProtectionLevel == ProtectionLevel.Protected
                && !t.TypeParams[0].ParamType.SurroundingClass.AllowVirtuals;

            if (noConstructor)
            {
                // the container type will not be declared,
                // declare an iterator without a constructor that takes a container class
                sb.Append("_NOCONSTRUCTOR");
            }

            if (t.IsMapIterator)
                sb.Append("( " + prefix + ", " + t.CLRName + ", " + t.FullyQualifiedNativeName + ", " + t.TypeParams[0].ParamType.FullyQualifiedCLRName + ", " + t.IterationElementTypeMember.MemberTypeCLRName + ", " + t.IterationElementTypeMember.MemberTypeNativeName + ", " + t.IterationKeyTypeMember.MemberTypeCLRName + ", " + t.IterationKeyTypeMember.MemberTypeNativeName);
            else
                sb.Append("( " + prefix + ", " + t.CLRName + ", " + t.FullyQualifiedNativeName + ", " + t.TypeParams[0].ParamType.FullyQualifiedCLRName + ", " + t.IterationElementTypeMember.MemberTypeCLRName + ", " + t.IterationElementTypeMember.MemberTypeNativeName);

            if (!noConstructor)
            {
                if (t.IsConstIterator)
                    sb.Append(", const");
                else
                    sb.Append(", ");
            }

            sb.AppendLine(" )");

            AddTypeDependancy(t.TypeParams[0].ParamType);

            sb.AppendEmptyLine();
        }

        public void IncAddInternalTypeDef(TypedefDefinition t, SourceCodeStringBuilder sb)
        {
            if (t.IsNested)
                sb.Append(t.ProtectionLevel.GetCLRProtectionName() + ": ");
            sb.Append("typedef " + t.FullyQualifiedNativeName + " " + t.CLRName + ";\n\n");
        }

        public void IncAddValueTypeTypeDef(TypedefDefinition t, SourceCodeStringBuilder sb)
        {
            if (t.IsNested)
                sb.Append(t.ProtectionLevel.GetCLRProtectionName() + ": ");
            sb.Append("typedef " + t.BaseType.FullyQualifiedCLRName + " " + t.CLRName + ";\n\n");
        }

        private void IncAddIncludeFiles(string include, List<AbstractTypeDefinition> usedTypes, SourceCodeStringBuilder sb)
        {
            sb.AppendLine("#pragma warning(push, 0)");
            sb.AppendLine("#pragma managed(push, off)");
            sb.AppendLine("#include \"{0}\"", include);
            sb.AppendLine("#pragma managed(pop)");
            sb.AppendLine("#pragma warning(pop)");
            List<string> added = new List<string>();

            foreach (AbstractTypeDefinition type in usedTypes)
            {
                if (String.IsNullOrEmpty(type.IncludeFileName) || type.IncludeFileName == include)
                    continue;

                if (added.Contains(type.IncludeFileName))
                    continue;

                sb.AppendLine("#include \"" + GetCLRIncludeFileName(type.IncludeFileName) + "\"");
                added.Add(type.IncludeFileName);
            }

            sb.AppendEmptyLine();
        }

        private void CppAddIncludeFiles(string include, List<AbstractTypeDefinition> usedTypes, SourceCodeStringBuilder sb)
        {
            sb.AppendLine("#include \"MogreStableHeaders.h\"\n");
            sb.AppendLine("#include \"{0}\"", GetCLRIncludeFileName(include));
            List<string> added = new List<string>();

            foreach (AbstractTypeDefinition type in usedTypes)
            {
                if (String.IsNullOrEmpty(type.IncludeFileName) || type.IncludeFileName == include)
                    continue;

                if (added.Contains(type.IncludeFileName))
                    continue;

                sb.AppendLine("#include \"" + GetCLRIncludeFileName(type.IncludeFileName) + "\"");
                added.Add(type.IncludeFileName);
            }

            sb.AppendEmptyLine();
        }

        public virtual void AddTypeDependancy(AbstractTypeDefinition type)
        {
            if (!_usedTypes.Contains(type))
                _usedTypes.Add(type);
        }

        public virtual void AddPreDeclaration(string decl)
        {
            if (!_preDeclarations.Contains(decl))
                _preDeclarations.Add(decl);
        }

        public virtual void AddPragmaMakePublicForType(AbstractTypeDefinition type)
        {
            if (!_pragmaMakePublicTypes.Contains(type))
                _pragmaMakePublicTypes.Add(type);
        }

        public virtual void CheckTypeForDependancy(AbstractTypeDefinition type)
        {
            if (type is EnumDefinition
                || (!(type is IDefString) && type is TypedefDefinition && (type as TypedefDefinition).BaseType is DefInternal)
                || type.HasWrapType(WrapTypes.NativePtrValueType)
                || type.HasWrapType(WrapTypes.ValueType))
                AddTypeDependancy(type);
            else if (type.SurroundingClass != null)
                AddTypeDependancy(type.SurroundingClass);
            else if (type is TypedefDefinition)
                CheckTypeForDependancy((type as TypedefDefinition).BaseType);

            if (!type.IsNested && type is ClassDefinition)
                AddPragmaMakePublicForType(type);
        }

        public virtual void CppCheckTypeForDependancy(AbstractTypeDefinition type)
        {
            if (!(type is EnumDefinition)
                && !type.IsNested
                && !type.IsPureManagedClass
                && !(type is DefInternal)
                && !type.IsValueType)
            {
                AddTypeDependancy(type);
            }
        }
    }

    public class IncludeFileWrapEventArgs : EventArgs
    {
        private string _include;
        public string IncludeFile
        {
            get { return _include; }
        }

        public IncludeFileWrapEventArgs(string include)
        {
            _include = include;
        }
    }
}
