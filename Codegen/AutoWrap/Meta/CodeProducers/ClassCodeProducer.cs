#region GPL license
/*
 * This source file is part of the AutoWrap code generator of the
 * MOGRE project (http://mogre.sourceforge.net).
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
using System.Collections.Generic;
using System.Text;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Base class for all classes producing code for classes.
    /// </summary>
    public abstract class ClassCodeProducer : AbstractCodeProducer
    {
        protected readonly Wrapper _wrapper;
        protected readonly ClassDefinition _classDefinition;
        protected SourceCodeStringBuilder _codeBuilder;
        protected readonly List<ClassDefinition> _listeners = new List<ClassDefinition>();
        protected readonly List<MemberPropertyDefinition> _interfaceProperties = new List<MemberPropertyDefinition>();
        protected readonly List<MemberMethodDefinition> _abstractFunctions = new List<MemberMethodDefinition>();
        protected readonly List<MemberPropertyDefinition> _abstractProperties = new List<MemberPropertyDefinition>();
        protected readonly List<ClassDefinition> _interfaces = new List<ClassDefinition>();

        protected readonly List<MemberMethodDefinition> _overridableFunctions = new List<MemberMethodDefinition>();
        protected MemberPropertyDefinition[] _overridableProperties;
        //protected List<DefField> _protectedFields = new List<DefField>();
        protected readonly Dictionary<MemberMethodDefinition, int> _methodIndices = new Dictionary<MemberMethodDefinition, int>();
        protected int _methodIndicesCount = 0;

        protected readonly List<MemberDefinitionBase> _cachedMembers = new List<MemberDefinitionBase>();

        private bool _initCalled;

        public virtual string ClassFullNativeName
        {
            get { return _classDefinition.FullyQualifiedNativeName; }
        }

        public virtual bool IsNativeClass
        {
            get { return false; }
        }

        private bool _isAbstractClass;
        public bool IsAbstractClass
        {
            get { return _isAbstractClass; }
        }

        protected virtual bool AllowVirtualMethods
        {
            get { return _classDefinition.AllowSubClassing; }
        }

        protected virtual bool AllowProtectedMembers
        {
            get { return false; }
        }

        protected virtual bool AllowSubclassing
        {
            get { return _classDefinition.AllowSubClassing; }
        }

        protected virtual bool AllowMethodOverloads
        {
            get { return true; }
        }

        protected virtual bool AllowMethodIndexAttributes
        {
            get { return false; }
        }

        protected virtual bool AllowCachedMemberFields
        {
            get { return !_classDefinition.HasWrapType(WrapTypes.NativePtrValueType) && !_classDefinition.HasWrapType(WrapTypes.ValueType); }
        }

        protected virtual bool IsReadOnly
        {
            get { return _classDefinition.HasAttribute<ReadOnlyAttribute>(); }
        }

        public ClassCodeProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef)
        {
            this._wrapper = wrapper;
            _classDefinition = t;
            _codeBuilder = sb;

            foreach (ClassDefinition iface in _classDefinition.GetInterfaces())
            {
                AddTypeDependancy(iface);
                _interfaces.Add(iface);
            }

            if (_classDefinition.IsInterface)
            {
                // Declaring an overridable class for interface
                _interfaces.Add(_classDefinition);
            }
        }

        protected virtual void Init()
        {
            if (_initCalled)
                return;

            _initCalled = true;

            foreach (MemberMethodDefinition f in _classDefinition.PublicMethods)
            {
                if (f.IsListenerAdder && !f.IsIgnored)
                {
                    _listeners.Add((ClassDefinition)f.Parameters[0].Type);
                }
            }

            foreach (ClassDefinition iface in _classDefinition.GetInterfaces())
            {
                // Add attributes of interface methods from the interface classes
                foreach (MemberMethodDefinition f in iface.Methods)
                {
                    MemberMethodDefinition tf = _classDefinition.GetMethodWithSignature(f.Signature);
                    if (tf != null)
                        tf.AddAttributes(f.Attributes);
                }

                //Store properties of interface classes. They have precedence over type's properties.
                foreach (MemberPropertyDefinition ip in iface.GetProperties())
                {
                    if (IsPropertyAllowed(ip) &&
                        (ip.ProtectionLevel == ProtectionLevel.Public
                          || (AllowProtectedMembers && ip.ProtectionLevel == ProtectionLevel.Protected)))
                    {
                        _interfaceProperties.Add(ip);
                    }
                }
            }

            foreach (MemberFieldDefinition field in _classDefinition.Fields)
            {
                if (!field.IsIgnored && field.MemberType.IsSTLContainer)
                {
                    if (field.ProtectionLevel == ProtectionLevel.Public
                        || ((AllowSubclassing || AllowProtectedMembers) && field.ProtectionLevel == ProtectionLevel.Protected))
                        MarkCachedMember(field);
                }
            }

            foreach (ClassDefinition iface in _interfaces)
            {
                if (iface == _classDefinition)
                    continue;

                foreach (MemberFieldDefinition field in iface.Fields)
                {
                    if (!field.IsIgnored && field.MemberType.IsSTLContainer
                        && !field.IsStatic)
                    {
                        if (field.ProtectionLevel == ProtectionLevel.Public
                            || ((AllowSubclassing || AllowProtectedMembers) && field.ProtectionLevel == ProtectionLevel.Protected))
                            MarkCachedMember(field);
                    }
                }
            }

            foreach (MemberMethodDefinition func in _classDefinition.AbstractFunctions)
            {
                if (func.ProtectionLevel == ProtectionLevel.Public
                        || (AllowProtectedMembers && func.ProtectionLevel == ProtectionLevel.Protected))
                {
                    if ((func.ContainingClass.AllowSubClassing || (func.ContainingClass == _classDefinition && AllowSubclassing)) && !func.IsProperty)
                    {
                        _isAbstractClass = true;
                        _abstractFunctions.Add(func);
                    }
                }
            }

            foreach (MemberPropertyDefinition prop in _classDefinition.AbstractProperties)
            {
                if (IsPropertyAllowed(prop) && (prop.ContainingClass.AllowSubClassing
                    || (prop.ContainingClass == _classDefinition && AllowSubclassing)))
                {
                    if (prop.ProtectionLevel == ProtectionLevel.Public
                        || (AllowProtectedMembers && prop.ProtectionLevel == ProtectionLevel.Protected))
                    {

                        _isAbstractClass = true;
                        _abstractProperties.Add(prop);
                    }
                }
            }

            SearchOverridableFunctions(_classDefinition);
            //SearchProtectedFields(_t);

            foreach (ClassDefinition iface in _interfaces)
            {
                SearchOverridableFunctions(iface);
                //SearchProtectedFields(iface);
            }

            _overridableProperties = MemberPropertyDefinition.GetPropertiesFromMethods(_overridableFunctions);

            //Find cached members

            foreach (MemberDefinitionBase m in _classDefinition.Members)
            {
                if (m.HasAttribute<CachedGetAccessorAttribute>())
                    MarkCachedMember(m);
            }

            foreach (ClassDefinition iface in _interfaces)
            {
                if (iface == _classDefinition)
                    continue;

                foreach (MemberDefinitionBase m in iface.Members)
                {
                    if (m.HasAttribute<CachedGetAccessorAttribute>())
                        MarkCachedMember(m);
                }
            }
        }

        protected virtual bool DeclareAsVirtual(MemberMethodDefinition f)
        {
            return (f.IsVirtual && AllowVirtualMethods) || f.IsVirtualInterfaceMethod
                || (f.IsVirtual && f.BaseMethod != null && f.BaseMethod.ContainingClass.AllowVirtuals);
        }

        protected virtual bool DeclareAsOverride(MemberMethodDefinition f)
        {
            return (f.IsOverriding && DeclareAsVirtual(f))
                || (f.IsVirtualInterfaceMethod && !f.ContainingClass.IsInterface && !f.ContainingClass.ContainsInterfaceFunctionSignature(f.Signature, false));
        }

        protected ClassDefinition GetTopClass(ClassDefinition type)
        {
            if (type.BaseClass == null)
                return type;
            else
                return GetTopClass(type.BaseClass);
        }

        protected virtual void MarkCachedMember(MemberDefinitionBase m)
        {
            if (m.IsStatic || AllowCachedMemberFields)
                _cachedMembers.Add(m);
        }

        protected virtual string GetNativeInvokationTarget(bool isConst)
        {
            string ret = "static_cast<";
            if (isConst)
                ret += "const ";
            return ret + ClassFullNativeName + "*>(_native)";
        }
        protected string GetNativeInvokationTarget()
        {
            return GetNativeInvokationTarget(false);
        }

        protected virtual string GetNativeInvokationTarget(MemberMethodDefinition f)
        {
            if (!f.IsStatic)
            {
                if (f.ProtectionLevel == ProtectionLevel.Public)
                {
                    return GetNativeInvokationTarget(f.IsConstMethod) + "->" + f.NativeName;
                }
                else if (f.ProtectionLevel == ProtectionLevel.Protected)
                {
                    if (!f.IsVirtual)
                    {
                        string proxyName = NativeProtectedStaticsProxy.GetProtectedStaticsProxyName(_classDefinition);
                        return "static_cast<" + proxyName + "*>(_native)->" + f.NativeName;
                    }
                    else
                        throw new Exception("Unexpected");
                }
                else
                    throw new Exception("Unexpected");
            }
            else
            {
                if (f.ProtectionLevel == ProtectionLevel.Public)
                    return f.ContainingClass.FullyQualifiedNativeName + "::" + f.NativeName;
                else
                    return NativeProtectedStaticsProxy.GetProtectedStaticsProxyName(f.ContainingClass) + "::" + f.NativeName;
            }
        }
        protected virtual string GetNativeInvokationTarget(MemberFieldDefinition field)
        {
            if (!field.IsStatic)
            {
                if (field.ProtectionLevel == ProtectionLevel.Public)
                {
                    return GetNativeInvokationTarget() + "->" + field.NativeName;
                }
                else if (field.ProtectionLevel == ProtectionLevel.Protected)
                {
                    string proxyName = NativeProtectedStaticsProxy.GetProtectedStaticsProxyName(_classDefinition);
                    return "static_cast<" + proxyName + "*>(_native)->" + field.NativeName;
                }
                else
                    throw new Exception("Unexpected");
            }
            else
            {
                if (field.ProtectionLevel == ProtectionLevel.Public)
                    return field.ContainingClass.FullyQualifiedNativeName + "::" + field.NativeName;
                else
                    return NativeProtectedStaticsProxy.GetProtectedStaticsProxyName(field.ContainingClass) + "::" + field.NativeName;
            }
        }

        protected virtual string GetNativeInvokationTargetObject()
        {
            return "*(static_cast<" + ClassFullNativeName + "*>(_native))";
        }

        protected virtual void SearchOverridableFunctions(ClassDefinition type)
        {
            foreach (MemberMethodDefinition func in type.Methods)
            {
                if (func.IsDeclarableFunction && func.IsVirtual)
                {
                    if (!ContainsFunction(func, _overridableFunctions))
                    {
                        _overridableFunctions.Add(func);

                        if (!func.IsAbstract)
                        {
                            _methodIndices.Add(func, _methodIndicesCount);
                            _methodIndicesCount++;
                        }
                    }
                }
            }

            foreach (ClassDefinition iface in type.GetInterfaces())
                SearchOverridableFunctions(iface);

            if (type.BaseClass != null)
                SearchOverridableFunctions(type.BaseClass);
        }

        protected bool ContainsFunction(MemberMethodDefinition func, List<MemberMethodDefinition> list)
        {
            foreach (MemberMethodDefinition lf in list)
            {
                if (lf.Signature == func.Signature)
                    return true;
            }

            return false;
        }

        //protected virtual void SearchProtectedFields(DefClass type)
        //{
        //    foreach (DefField field in type.Fields)
        //    {
        //        if (!field.IsIgnored
        //            && field.ProtectionType == ProtectionType.Protected
        //            && !field.IsStatic
        //            && !ContainsField(field, _protectedFields))
        //        {
        //            _protectedFields.Add(field);
        //        }
        //    }

        //    foreach (DefClass iface in type.GetInterfaces())
        //        SearchProtectedFields(iface);

        //    if (type.BaseClass != null)
        //        SearchProtectedFields(type.BaseClass);
        //}

        //protected bool ContainsField(DefField field, List<DefField> list)
        //{
        //    foreach (DefField lf in list)
        //    {
        //        if (lf.Name == field.Name)
        //            return true;
        //    }

        //    return false;
        //}

        /// <summary>
        /// Generates the classes' code and inserts it at end of the already generated code.
        /// </summary>
        /// <seealso cref="GenerateCodeAtBeginning"/>
        public virtual void GenerateCode()
        {
            Init();

            GenerateCodePreBody();
            GenerateCodeBody();
            GenerateCodePostBody();
        }

        /// <summary>
        /// Generates the classes' code and inserts it before all other code already generated.
        /// </summary>
        /// <seealso cref="GenerateCode"/>
        public virtual void GenerateCodeAtBeginning()
        {
            SourceCodeStringBuilder orig = _codeBuilder;
            _codeBuilder = new SourceCodeStringBuilder(MetaDef.CodeStyleDef);

            GenerateCode();

            orig.InsertAt(0, _codeBuilder.ToString());
            _codeBuilder = orig;
        }

        protected virtual string ReplaceCustomVariables(string txt)
        {
            string targ = "static_cast<" + ClassFullNativeName + "*>(_native)";
            txt = txt.Replace("@NATIVE@", targ);
            txt = txt.Replace("@CLASS@", GetClassName());
            return txt;
        }

        protected virtual string ReplaceCustomVariables(string txt, MemberMethodDefinition func)
        {
            txt = ReplaceCustomVariables(txt);
            string replace;
            if (DeclareAsOverride(func))
                replace = "override";
            else if (func.IsAbstract && AllowSubclassing)
                replace = "abstract";
            else
                replace = "";
            txt = txt.Replace("@OVERRIDE@", replace);

            txt = txt.Replace("@NATIVE_INVOKATION_TARGET_FOR_FUNCTION@", GetNativeInvokationTarget(func));
            return txt;
        }

        protected virtual string GetClassName()
        {
            string full = _classDefinition.FullyQualifiedCLRName;
            int index = full.IndexOf("::");
            return full.Substring(index + 2);
        }

        protected virtual string GetTopBaseClassName()
        {
            return null;
        }

        protected virtual string GetBaseClassName()
        {
            return (_classDefinition.BaseClass == null || _classDefinition.BaseClass.HasWrapType(WrapTypes.NativeDirector)) ? GetTopBaseClassName() : _classDefinition.BaseClass.Name;
        }

        protected bool HasStaticCachedFields()
        {
            foreach (MemberDefinitionBase m in _cachedMembers)
            {
                if (m.IsStatic)
                    return true;
            }

            return false;
        }

        protected virtual void GenerateCodePreBody()
        {
            // If there are nested NativeDirectors, declare them before the declaration
            // of this class
            foreach (AbstractTypeDefinition nested in _classDefinition.NestedTypes)
            {
                if (nested.ProtectionLevel == ProtectionLevel.Public
                    || ((AllowProtectedMembers || AllowSubclassing) && nested.ProtectionLevel == ProtectionLevel.Protected))
                {
                    if (nested.HasWrapType(WrapTypes.NativeDirector))
                        GenerateCodeNestedTypeBeforeMainType(nested);
                }
            }

            _codeBuilder.AppendLine("//################################################################");
            _codeBuilder.AppendLine("//" + _classDefinition.CLRName);
            _codeBuilder.AppendLine("//################################################################\n");
        }

        protected virtual void GenerateCodeNestedTypeBeforeMainType(AbstractTypeDefinition nested)
        {
        }

        /// <summary>
        /// Generates the main part of the classes' code.
        /// </summary>
        protected virtual void GenerateCodeBody()
        {
            GenerateCodePreNestedTypes();
            _codeBuilder.AppendLine("//Nested Types");
            GenerateCodeAllNestedTypes();
            GenerateCodePostNestedTypes();

            _codeBuilder.AppendLine("//Private Declarations");
            GenerateCodePrivateDeclarations();
            _codeBuilder.AppendEmptyLine();

            _codeBuilder.AppendLine("//Internal Declarations");
            GenerateCodeInternalDeclarations();
            _codeBuilder.AppendEmptyLine();

            _codeBuilder.AppendLine("//Public Declarations");
            GenerateCodePublicDeclarations();
            _codeBuilder.AppendEmptyLine();

            _codeBuilder.AppendLine("//Protected Declarations");
            GenerateCodeProtectedDeclarations();
            _codeBuilder.AppendEmptyLine();
        }

        protected virtual void GenerateCodePostBody()
        {
        }

        protected virtual bool IsConstructorBodyEmpty
        {
            get { return (_interfaces.Count == 0); }
        }

        protected virtual void GenerateCodeConstructorBody()
        {
        }

        protected virtual void GenerateCodeStaticConstructor()
        {
        }

        /// <summary>
        /// Generates code that goes before the code for nested types.
        /// </summary>
        protected virtual void GenerateCodePreNestedTypes()
        {
        }

        /// <summary>
        /// Generates the code for all types (enums, classes, typedefs, ...) that are nested within the current class.
        /// </summary>
        /// <seealso cref="GenerateCodePreNestedTypes"/>
        /// <seealso cref="GenerateCodePostNestedTypes"/>
        protected virtual void GenerateCodeAllNestedTypes()
        {
            List<AbstractTypeDefinition> enums = new List<AbstractTypeDefinition>();
            List<AbstractTypeDefinition> nativePtrClasses = new List<AbstractTypeDefinition>();
            List<AbstractTypeDefinition> typedefs = new List<AbstractTypeDefinition>();
            List<AbstractTypeDefinition> otherTypes = new List<AbstractTypeDefinition>();

            // Only output nested types on interfaces if we are the abstract class
            if (_classDefinition.IsInterface && !((this is IncOverridableClassProducer) || (this is OverridableClassCppProducer)))
            {
                return;
            }

            foreach (AbstractTypeDefinition nested in _classDefinition.NestedTypes)
            {
                if (nested.ProtectionLevel == ProtectionLevel.Public
                    || ((AllowProtectedMembers || AllowSubclassing)
                    && nested.ProtectionLevel == ProtectionLevel.Protected))
                {
                    if (nested is EnumDefinition)
                        enums.Add(nested);
                    else if (Wrapper.IsTypeWrappable(nested))
                    {
                        if (nested.HasWrapType(WrapTypes.NativePtrValueType))
                        {
                            if (nested.HasAttribute<DefinitionIndexAttribute>())
                                nativePtrClasses.Insert(nested.GetAttribute<DefinitionIndexAttribute>().Index, nested);
                            else
                                nativePtrClasses.Add(nested);
                        }
                        else if (nested is TypedefDefinition)
                        {
                            typedefs.Add(nested);
                        }
                        else
                            otherTypes.Add(nested);
                    }
                }
            }

            // Add typedefs for value types
            foreach (TypedefDefinition nested in typedefs)
            {
                if (nested.BaseType.Name == "uint32" || nested.BaseType.HasAttribute<ValueTypeAttribute>())
                {
                    GenerateCodeNestedType(nested);
                }
            }

            foreach (AbstractTypeDefinition nested in enums)
            {
                GenerateCodeNestedType(nested);
            }

            foreach (AbstractTypeDefinition nested in nativePtrClasses)
            {
                GenerateCodeNestedType(nested);
            }

            foreach (AbstractTypeDefinition nested in otherTypes)
            {
                GenerateCodeNestedType(nested);
            }

            List<AbstractTypeDefinition> iterators = new List<AbstractTypeDefinition>();

            //Add typedefs after class declarations
            foreach (TypedefDefinition nested in typedefs)
            {
                if (nested.BaseType.Name == "uint32" || nested.BaseType.HasAttribute<ValueTypeAttribute>())
                    continue;

                if (AbstractCodeProducer.IsIteratorWrapper((TypedefDefinition)nested))
                    iterators.Add(nested);
                else
                    GenerateCodeNestedType(nested);
            }

            //Add iterators last
            foreach (AbstractTypeDefinition nested in iterators)
            {
                GenerateCodeNestedType(nested);
            }

            // Exit out here if this is CPP
            if ((this is OverridableClassCppProducer))
            {
                return;
            }

            //Add STL wrappers for fields that doesn't have proper typedefs

            List<string> stls = new List<string>();

            foreach (MemberDefinitionBase m in _classDefinition.Members)
            {
                if ((m is MemberFieldDefinition || (m is MemberMethodDefinition && (m as MemberMethodDefinition).IsDeclarableFunction))
                    && !m.IsIgnored
                    && (m.ProtectionLevel == ProtectionLevel.Public
                        || ((AllowSubclassing || AllowProtectedMembers) && m.ProtectionLevel == ProtectionLevel.Protected)))
                {
                    if (m.MemberType.IsUnnamedSTLContainer
                        && !stls.Contains(m.MemberType.CLRName))
                    {
                        GenerateCodeNestedType(m.MemberType);
                        stls.Add(m.MemberType.CLRName);
                    }
                }
            }

            foreach (ClassDefinition iface in _interfaces)
            {
                if (iface == _classDefinition)
                    continue;

                foreach (MemberDefinitionBase m in iface.Members)
                {
                    if ((m is MemberFieldDefinition || (m is MemberMethodDefinition && (m as MemberMethodDefinition).IsDeclarableFunction))
                        && !m.IsIgnored
                        && (m.ProtectionLevel == ProtectionLevel.Public
                            || ((AllowSubclassing || AllowProtectedMembers) && m.ProtectionLevel == ProtectionLevel.Protected)))
                    {
                        if (m.MemberType.IsUnnamedSTLContainer
                            && !stls.Contains(m.MemberType.CLRName))
                        {
                            GenerateCodeNestedType(m.MemberType);
                            stls.Add(m.MemberType.CLRName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates the code that goes after the code for nested types (but before any declarations of the class itself).
        /// </summary>
        protected virtual void GenerateCodePostNestedTypes()
        {
        }

        /// <summary>
        /// Generates the code for declarations (fields, methods, property) with CLR protection level <c>private</c>.
        /// </summary>
        protected virtual void GenerateCodePrivateDeclarations()
        {
            if (HasStaticCachedFields())
            {
                GenerateCodeStaticConstructor();
                _codeBuilder.AppendEmptyLine();
            }
        }

        /// <summary>
        /// Generates the code for declarations (fields, methods, property) with CLR protection level <c>internal</c>.
        /// </summary>
        protected virtual void GenerateCodeInternalDeclarations()
        {
        }

        /// <summary>
        /// Generates the code for declarations (fields, methods, property) with CLR protection level <c>protected</c>.
        /// </summary>
        protected virtual void GenerateCodeProtectedDeclarations()
        {
            if (AllowSubclassing)
            {
                foreach (MemberFieldDefinition field in _classDefinition.ProtectedFields)
                {
                    if (!field.IsIgnored)
                    {
                        //if (CheckTypeMemberForGetProperty(field) == false)
                        //    AddMethodsForField(field);
                        //else
                        GenerateCodePropertyField(field);

                        _codeBuilder.AppendEmptyLine();
                    }
                }

                foreach (MemberMethodDefinition f in _classDefinition.ProtectedMethods)
                {
                    if (f.IsDeclarableFunction && (AllowProtectedMembers || f.IsStatic || !f.IsVirtual))
                    {
                        GenerateCodeMethod(f);
                        _codeBuilder.AppendEmptyLine();
                    }
                }
            }
        }

        /// <summary>
        /// Generates the code for declarations (fields, methods, property) with CLR protection level <c>public</c>.
        /// </summary>
        protected virtual void GenerateCodePublicDeclarations()
        {
            foreach (MemberFieldDefinition field in _classDefinition.PublicFields)
            {
                if (!field.IsIgnored)
                {
                    //if (CheckTypeMemberForGetProperty(field) == false)
                    //    AddMethodsForField(field);
                    //else
                    GenerateCodePropertyField(field);

                    _codeBuilder.AppendEmptyLine();
                }
            }

            foreach (MemberPropertyDefinition p in _classDefinition.GetProperties())
            {
                if (IsPropertyAllowed(p) &&
                    (p.ProtectionLevel == ProtectionLevel.Public
                     || (AllowSubclassing && (p.IsStatic || !p.IsVirtual))
                     || (AllowProtectedMembers && p.ProtectionLevel == ProtectionLevel.Protected)))
                {
                    GenerateCodeProperty(EnhanceProperty(p));
                    _codeBuilder.AppendEmptyLine();
                }
            }

            foreach (MemberMethodDefinition f in _classDefinition.PublicMethods)
            {
                if (f.IsOperatorOverload)
                {
                    if (f.NativeName.EndsWith("=="))
                    {
                        if (!f.IsIgnored)
                            GenerateCodePredefinedMethods(PredefinedMethods.Equals);
                        _codeBuilder.AppendEmptyLine();
                    }
                    else if (f.NativeName.EndsWith("="))
                    {
                        if (!f.IsIgnored)
                            GenerateCodePredefinedMethods(PredefinedMethods.CopyTo);
                        _codeBuilder.AppendEmptyLine();
                    }
                }
                else if (f.IsDeclarableFunction)
                {
                    GenerateCodeMethod(f);
                    _codeBuilder.AppendEmptyLine();
                }
            }

            if (_classDefinition.HasAttribute<IncludePredefinedMethodAttribute>())
            {
                GenerateCodePredefinedMethods(_classDefinition.GetAttribute<IncludePredefinedMethodAttribute>().Methods);
                _codeBuilder.AppendEmptyLine();
            }

            foreach (ClassDefinition cls in _interfaces)
            {
                if (cls == _classDefinition)
                    continue;

                GenerateCodeInterfaceImplementation(cls);
            }
        }

        protected virtual void GenerateCodeInterfaceImplementation(ClassDefinition iface)
        {
            _codeBuilder.AppendLine("//------------------------------------------------------------");
            _codeBuilder.AppendLine("// Implementation for " + iface.CLRName);
            _codeBuilder.AppendLine("//------------------------------------------------------------\n");

            foreach (MemberPropertyDefinition ip in iface.GetProperties())
            {
                if (ip.IsStatic)
                    continue;

                if (ip.ProtectionLevel == ProtectionLevel.Public
                    || (AllowSubclassing && !ip.IsVirtual)
                    || (AllowProtectedMembers && ip.ProtectionLevel == ProtectionLevel.Protected))
                {
                    if (!ip.IsContainedIn(_classDefinition, true))
                    {
                        GenerateCodeInterfaceProperty(ip);
                        _codeBuilder.AppendEmptyLine();
                    }
                }
            }

            foreach (MemberMethodDefinition inf in iface.DeclarableMethods)
            {
                if (inf.IsStatic)
                    continue;

                if (inf.ProtectionLevel == ProtectionLevel.Public
                    || (AllowSubclassing && !inf.IsVirtual)
                    || (AllowProtectedMembers && inf.ProtectionLevel == ProtectionLevel.Protected))
                {
                    if (!_classDefinition.ContainsFunctionSignature(inf.Signature, false))
                    {
                        GenerateCodeInterfaceMethod(inf);
                        _codeBuilder.AppendEmptyLine();
                    }
                }
            }

            foreach (MemberFieldDefinition field in iface.Fields)
            {
                if (!field.HasAttribute<IgnoreAttribute>())
                {
                    if (field.IsStatic)
                        continue;

                    if (field.ProtectionLevel == ProtectionLevel.Public
                        || AllowSubclassing
                        || (AllowProtectedMembers && field.ProtectionLevel == ProtectionLevel.Protected))
                    {
                        //if (CheckTypeMemberForGetProperty(field) == false)
                        //    AddInterfaceMethodsForField(field);
                        //else
                        GenerateCodeInterfacePropertyField(field);

                        _codeBuilder.AppendEmptyLine();
                    }
                }
            }
        }

        protected virtual void GenerateCodeInterfaceProperty(MemberPropertyDefinition prop)
        {
            GenerateCodeProperty(prop);
        }

        protected virtual void GenerateCodeInterfaceMethod(MemberMethodDefinition f)
        {
            GenerateCodeMethod(f);
        }

        protected virtual void GenerateCodeInterfacePropertyField(MemberFieldDefinition field)
        {
            GenerateCodePropertyField(field);
        }

        protected virtual void GenerateCodeInterfaceMethodsForField(MemberFieldDefinition field)
        {
            GenerateCodeMethodsForField(field);
        }

        protected MemberPropertyDefinition EnhanceProperty(MemberPropertyDefinition property)
        {
            MemberPropertyDefinition prop = property.Clone();
            if (_classDefinition.BaseClass != null)
            {
                if (!prop.CanWrite)
                {
                    // There's a chance the class overrides only the get function. If the
                    // property is not declared virtual then it will be read-only, so if
                    // it's not virtual check if a base class contains the set property
                    // and if it does, include it.
                    // If it's virtual and its base function will be declared virtual, this
                    // function will be declared virtual too, so it's not necessary to add
                    // the set function.

                    if (!DeclareAsVirtual(prop.GetterFunction))
                    {
                        MemberPropertyDefinition bp = _classDefinition.BaseClass.GetProperty(prop.Name, true);
                        if (bp != null && bp.CanWrite)
                        {
                            prop.SetterFunction = bp.SetterFunction;
                        }
                    }
                }

                if (!prop.CanRead)
                {
                    // There's a chance the class overrides only the set function. If the
                    // property is not declared virtual then it will be write-only, so if
                    // it's not virtual check if a base class contains the set property
                    // and if it does, include it.
                    // If it's virtual and its base function will be declared virtual, this
                    // function will be declared virtual too, so it's not necessary to add
                    // the get function.

                    if (!DeclareAsVirtual(prop.SetterFunction))
                    {
                        MemberPropertyDefinition bp = _classDefinition.BaseClass.GetProperty(prop.Name, true);
                        if (bp != null && bp.CanRead)
                        {
                            prop.GetterFunction = bp.GetterFunction;
                        }
                    }
                }
            }

            return prop;
        }

        protected virtual void GenerateCodeStaticField(MemberFieldDefinition field)
        {
        }

        protected virtual void GenerateCodeMethodsForField(MemberFieldDefinition field)
        {
        }

        protected virtual void GenerateCodePropertyField(MemberFieldDefinition field)
        {
        }

        protected virtual void GenerateCodeNestedType(AbstractTypeDefinition nested)
        {
        }

        protected virtual void GenerateCodeMethod(MemberMethodDefinition func)
        {
        }

        protected virtual void GenerateCodeProperty(MemberPropertyDefinition prop)
        {
        }

        protected virtual void GenerateCodePredefinedMethods(PredefinedMethods pm)
        {
        }
    }
}
