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
using System.Reflection;

namespace AutoWrap.Meta
{
    abstract class ClassInclProducer : ClassCodeProducer
    {
        public ClassInclProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
            AddPreDeclarations();

            if (_classDefinition.BaseClass != null)
                AddTypeDependancy(_classDefinition.BaseClass);

            if (AllowSubclassing)
            {
                _wrapper.AddPreClassProducer(new NativeProtectedTypesProxy(metaDef, _wrapper, _classDefinition, _codeBuilder));
                _wrapper.AddPostClassProducer(new NativeProtectedStaticsProxy(metaDef, _wrapper, _classDefinition, _codeBuilder));
                //_wrapper.PreClassProducers.Add(new IncNativeProtectedTypesProxy(_wrapper, _t, _code));
            }
        }

        protected override void AddTypeDependancy(AbstractTypeDefinition type)
        {
            base.AddTypeDependancy(type);
            _wrapper.AddTypeDependancy(type);
        }

        protected virtual void AddPreDeclarations()
        {
            if (!_classDefinition.IsNested)
            {
                _wrapper.AddPreDeclaration("ref class " + _classDefinition.CLRName + ";");
                _wrapper.AddPragmaMakePublicForType(_classDefinition);
            }
        }

        protected virtual void CheckTypeForDependancy(AbstractTypeDefinition type)
        {
            _wrapper.CheckTypeForDependancy(type);
        }

        protected virtual string GetCLRTypeName(ITypeMember m)
        {
            CheckTypeForDependancy(m.MemberType);
            return m.MemberTypeCLRName;
        }

        protected virtual string GetCLRParamTypeName(ParamDefinition param)
        {
            CheckTypeForDependancy(param.Type);
            return param.Type.GetCLRParamTypeName(param);
        }

        protected virtual void AddDefinition()
        {
            if (!_classDefinition.IsNested)
                _codeBuilder.Append("public ");
            else
                _codeBuilder.Append(_classDefinition.ProtectionLevel.GetCLRProtectionName() + ": ");
            string baseclass = GetBaseAndInterfaces();
            if (baseclass != "")
                _codeBuilder.AppendLine("ref class {0}{1} : {2}", _classDefinition.CLRName, (IsAbstractClass) ? " abstract" : "", baseclass);
            else
                _codeBuilder.AppendLine("ref class {0}{1}", _classDefinition.CLRName, (IsAbstractClass) ? " abstract" : "");
        }

        protected override void GenerateCodeInterfaceMethod(MemberMethodDefinition f)
        {
            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine(f.ProtectionLevel.GetCLRProtectionName() + ":");
            _codeBuilder.IncreaseIndent();
            base.GenerateCodeInterfaceMethod(f);
        }

        protected override void GenerateCodeInterfaceMethodsForField(MemberFieldDefinition field)
        {
            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine(field.ProtectionLevel.GetCLRProtectionName() + ":");
            _codeBuilder.IncreaseIndent();
            base.GenerateCodeInterfaceMethodsForField(field);
        }

        protected override void GenerateCodePreBody()
        {
            base.GenerateCodePreBody();

            AddComments();
            AddDefinition();

            _codeBuilder.AppendLine("{");
            _codeBuilder.IncreaseIndent();
        }

        protected override void GenerateCodePostBody()
        {
            base.GenerateCodePostBody();

            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine("};\n");
        }

        protected override void GenerateCodePrivateDeclarations()
        {
            _codeBuilder.DecreaseIndent();
            if (IsNativeClass)
                _codeBuilder.AppendLine("private:");
            else
                _codeBuilder.AppendLine("private protected:");
            _codeBuilder.IncreaseIndent();
            base.GenerateCodePrivateDeclarations();

            AddCachedFields();

            if (_listeners.Count > 0)
            {
                _codeBuilder.AppendLine("\n//Event and Listener fields");
                AddEventFields();
            }
        }

        protected override void GenerateCodeStaticConstructor()
        {
            if (_classDefinition.IsInterface)
                _codeBuilder.AppendLine("static " + _classDefinition.Name + "();");
            else
                _codeBuilder.AppendLine("static " + _classDefinition.CLRName + "();");
        }

        protected virtual void AddEventFields()
        {
            foreach (ClassDefinition cls in _listeners)
            {
                _codeBuilder.AppendLine(GetNativeDirectorName(cls) + "* " + NameToPrivate(cls.Name) + ";");
                foreach (MemberMethodDefinition f in cls.PublicMethods)
                {
                    if (f.IsDeclarableFunction)
                    {
                        _codeBuilder.AppendLine(cls.FullyQualifiedCLRName + "::" + f.CLRName + "Handler^ " + NameToPrivate(f.NativeName) + ";");
                    }
                    else
                        continue;

                    if (cls.HasAttribute<StopDelegationForReturnAttribute>())
                    {
                        _codeBuilder.AppendLine("array<Delegate^>^ " + NameToPrivate(f.NativeName) + "Delegates;");
                    }
                }

                _codeBuilder.AppendEmptyLine();
            }
        }

        protected virtual bool DoCleanupInFinalizer
        {
            get { return _classDefinition.HasAttribute<DoCleanupInFinalizerAttribute>(); }
        }

        protected override void GenerateCodeInternalDeclarations()
        {
            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine("public protected:");
            _codeBuilder.IncreaseIndent();
            base.GenerateCodeInternalDeclarations();

            AddInternalConstructors();

            if (RequiresCleanUp)
            {
                if (DoCleanupInFinalizer)
                {
                    _codeBuilder.AppendLine("~" + _classDefinition.CLRName + "()");
                    _codeBuilder.BeginBlock();
                    _codeBuilder.AppendLine("this->!" + _classDefinition.CLRName + "();");
                    _codeBuilder.EndBlock();
                    _codeBuilder.AppendLine("!" + _classDefinition.CLRName + "()");
                }
                else
                    _codeBuilder.AppendLine("~" + _classDefinition.CLRName + "()");
                _codeBuilder.BeginBlock();
                AddDisposerBody();
                _codeBuilder.EndBlock();
                _codeBuilder.AppendEmptyLine();
            }

            foreach (ClassDefinition cls in _interfaces)
            {
                _codeBuilder.AppendLine("virtual " + cls.FullyQualifiedNativeName + "* _" + cls.CLRName + "_GetNativePtr() = " + cls.CLRName + "::_GetNativePtr;");
                _codeBuilder.AppendEmptyLine();
            }
        }

        protected virtual void AddInternalConstructors()
        {
        }

        protected virtual bool RequiresCleanUp
        {
            get { return _listeners.Count > 0; }
        }

        protected virtual void AddDisposerBody()
        {
            if (_classDefinition.HasAttribute<CustomDisposingAttribute>())
            {
                string text = _classDefinition.GetAttribute<CustomDisposingAttribute>().Text;
                _codeBuilder.AppendLine(text);
            }

            foreach (ClassDefinition cls in _listeners)
            {
                MemberMethodDefinition removerFunc = null;
                foreach (MemberMethodDefinition func in _classDefinition.PublicMethods)
                {
                    if (func.IsListenerRemover && func.Parameters[0].Type == cls)
                    {
                        removerFunc = func;
                        break;
                    }
                }
                if (removerFunc == null)
                    throw new Exception("Unexpected");

                string name = NameToPrivate(cls.Name);
                _codeBuilder.AppendLine("if (" + name + " != 0)");
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("if (_native != 0) " + GetNativeInvokationTarget(removerFunc) + "(" + name + ");");
                _codeBuilder.AppendLine("delete " + name + "; " + name + " = 0;");
                _codeBuilder.EndBlock();
            }
        }

        protected override void GenerateCodePublicDeclarations()
        {
            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine("public:");
            _codeBuilder.IncreaseIndent();

            AddPublicConstructors();

            _codeBuilder.AppendEmptyLine();
            AddPublicFields();
            _codeBuilder.AppendEmptyLine();

            if (_listeners.Count > 0)
            {
                AddEventMethods();
            }
            base.GenerateCodePublicDeclarations();
        }

        protected override void GenerateCodePreNestedTypes()
        {
            base.GenerateCodePreNestedTypes();

            if (_classDefinition.HasAttribute<CustomIncPreDeclarationAttribute>())
            {
                string txt = _classDefinition.GetAttribute<CustomIncPreDeclarationAttribute>().DeclarationText;
                txt = ReplaceCustomVariables(txt);
                _codeBuilder.AppendLine(txt);
                _codeBuilder.AppendEmptyLine();
            }
        }

        protected override void GenerateCodePostNestedTypes()
        {
            base.GenerateCodePostNestedTypes();

            if (_classDefinition.HasAttribute<CustomIncDeclarationAttribute>())
            {
                string txt = _classDefinition.GetAttribute<CustomIncDeclarationAttribute>().DeclarationText;
                txt = ReplaceCustomVariables(txt);
                _codeBuilder.AppendLine(txt);
                _codeBuilder.AppendEmptyLine();
            }
        }

        protected virtual void AddPublicConstructors()
        {
            if (_classDefinition.IsNativeAbstractClass && !_classDefinition.IsInterface)
                return;

            if (_classDefinition.Constructors.Length > 0)
            {
                foreach (MemberMethodDefinition func in _classDefinition.Constructors)
                {
                    if (func.ProtectionLevel == ProtectionLevel.Public &&
                        !func.HasAttribute<IgnoreAttribute>())
                    {
                        AddPublicConstructor(func);
                    }
                }
            }
            else
            {
                AddPublicConstructor(null);
            }
        }

        protected virtual void AddPublicConstructor(MemberMethodDefinition function)
        {
            string className = (_classDefinition.IsInterface) ? _classDefinition.Name : _classDefinition.CLRName;

            if (function == null)
            {
                _codeBuilder.AppendLine(className + "();");
            }
            else
            {
                int defcount = 0;

                if (!function.HasAttribute<NoDefaultParamOverloadsAttribute>())
                {
                    foreach (ParamDefinition param in function.Parameters)
                        if (param.DefaultValue != null)
                            defcount++;
                }

                bool hideParams = function.HasAttribute<HideParamsWithDefaultValuesAttribute>();
                // The overloads (because of default values)
                for (int dc = 0; dc <= defcount; dc++)
                {
                    if (dc < defcount && hideParams)
                        continue;

                    AddComments(function, function.Parameters.Count - dc);
                    _codeBuilder.Append(className);
                    AddMethodParameters(function, function.Parameters.Count - dc);
                    _codeBuilder.AppendLine(";");
                }
            }
        }

        protected virtual void AddPublicFields()
        {
        }

        protected virtual void AddEventInvokers()
        {
            foreach (ClassDefinition cls in _listeners)
            {
                foreach (MemberMethodDefinition f in cls.PublicMethods)
                {
                    if (f.IsDeclarableFunction)
                    {
                        _codeBuilder.Append("virtual " + GetCLRTypeName(f) + " On" + f.CLRName);
                        AddMethodParameters(f);
                        _codeBuilder.AppendLine(" = " + GetNativeDirectorReceiverInterfaceName(cls) + "::" + f.CLRName);
                        _codeBuilder.BeginBlock();
                        if (f.MemberTypeName != "void")
                            _codeBuilder.Append("return ");
                        _codeBuilder.Append(f.CLRName + "(");
                        for (int i = 0; i < f.Parameters.Count; i++)
                        {
                            ParamDefinition param = f.Parameters[i];
                            _codeBuilder.Append(" " + param.Name);
                            if (i < f.Parameters.Count - 1)
                                _codeBuilder.Append(",");
                        }
                        _codeBuilder.AppendLine(" );");
                        _codeBuilder.EndBlock();
                        _codeBuilder.AppendEmptyLine();
                    }
                }

                _codeBuilder.AppendEmptyLine();
            }
        }

        protected virtual void AddEventMethods()
        {
            foreach (ClassDefinition cls in _listeners)
            {
                MemberMethodDefinition adderFunc = null;
                foreach (MemberMethodDefinition func in _classDefinition.PublicMethods)
                {
                    if (func.IsListenerAdder && func.Parameters[0].Type == cls)
                    {
                        adderFunc = func;
                        break;
                    }
                }
                if (adderFunc == null)
                    throw new Exception("Unexpected");

                foreach (MemberMethodDefinition f in cls.PublicMethods)
                {
                    if (f.IsDeclarableFunction)
                    {
                        AddComments(f, 0);
                        string handler = cls.FullyQualifiedCLRName + "::" + f.CLRName + "Handler^";
                        string privField = NameToPrivate(f.NativeName);
                        string listener = NameToPrivate(cls.Name);
                        _codeBuilder.AppendLine("event " + handler + " " + f.CLRName);
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("void add(" + handler + " hnd)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("if (" + privField + " == CLR_NULL)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("if (" + listener + " == 0)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine(listener + " = new " + GetNativeDirectorName(cls) + "(this);");
                        _codeBuilder.AppendLine(GetNativeInvokationTarget(adderFunc) + "(" + listener + ");");
                        _codeBuilder.EndBlock();
                        _codeBuilder.AppendLine(listener + "->doCallFor" + f.CLRName + " = true;");
                        _codeBuilder.EndBlock();
                        _codeBuilder.AppendLine(privField + " += hnd;");

                        if (cls.HasAttribute<StopDelegationForReturnAttribute>())
                        {
                            _codeBuilder.AppendLine(privField + "Delegates = " + privField + "->GetInvocationList();");
                        }

                        _codeBuilder.EndBlock();
                        _codeBuilder.AppendLine("void remove(" + handler + " hnd)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine(privField + " -= hnd;");
                        _codeBuilder.AppendLine("if (" + privField + " == CLR_NULL) " + listener + "->doCallFor" + f.CLRName + " = false;");

                        if (cls.HasAttribute<StopDelegationForReturnAttribute>())
                        {
                            _codeBuilder.AppendLine("if (" + privField + " == CLR_NULL) " + privField + "Delegates = nullptr; else " + privField + "Delegates = " + privField + "->GetInvocationList();");
                        }

                        _codeBuilder.EndBlock();
                        _codeBuilder.DecreaseIndent();
                        _codeBuilder.AppendLine("private:");
                        _codeBuilder.IncreaseIndent();
                        _codeBuilder.Append(GetCLRTypeName(f) + " raise");
                        AddMethodParameters(f);
                        _codeBuilder.AppendEmptyLine();
                        _codeBuilder.BeginBlock();

                        if (cls.HasAttribute<StopDelegationForReturnAttribute>())
                        {
                            _codeBuilder.AppendLine("if (" + privField + ")");
                            _codeBuilder.BeginBlock();
                            string list = privField + "Delegates";
                            string stopret = cls.GetAttribute<StopDelegationForReturnAttribute>().Return;
                            _codeBuilder.AppendLine(f.MemberType.FullyQualifiedCLRName + " mp_return;");
                            _codeBuilder.AppendLine("for (int i=0; i < " + list + "->Length; i++)");
                            _codeBuilder.BeginBlock();
                            _codeBuilder.Append("mp_return = " + "static_cast<" + handler + ">(" + list + "[i])(");
                            for (int i = 0; i < f.Parameters.Count; i++)
                            {
                                ParamDefinition param = f.Parameters[i];
                                _codeBuilder.Append(" " + param.Name);
                                if (i < f.Parameters.Count - 1)
                                    _codeBuilder.Append(",");
                            }
                            _codeBuilder.AppendLine(" );");
                            _codeBuilder.AppendLine("if (mp_return == " + stopret + ") break;");
                            _codeBuilder.EndBlock();
                            _codeBuilder.AppendLine("return mp_return;");
                            _codeBuilder.EndBlock();
                        }
                        else
                        {
                            _codeBuilder.AppendLine("if (" + privField + ")");
                            _codeBuilder.Append("\t");
                            if (f.MemberTypeName != "void")
                                _codeBuilder.Append("return ");
                            _codeBuilder.Append(privField + "->Invoke(");
                            for (int i = 0; i < f.Parameters.Count; i++)
                            {
                                ParamDefinition param = f.Parameters[i];
                                _codeBuilder.Append(" " + param.Name);
                                if (i < f.Parameters.Count - 1)
                                    _codeBuilder.Append(",");
                            }
                            _codeBuilder.AppendLine(" );");
                        }

                        _codeBuilder.EndBlock();
                        _codeBuilder.EndBlock();
                        _codeBuilder.AppendEmptyLine();
                    }
                }
                _codeBuilder.AppendEmptyLine();
            }
        }

        protected override void GenerateCodeProtectedDeclarations()
        {
            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine("protected public:");
            _codeBuilder.IncreaseIndent();
            base.GenerateCodeProtectedDeclarations();

            if (_listeners.Count > 0)
            {
                AddEventInvokers();
            }
        }

        protected override void GenerateCodeStaticField(MemberFieldDefinition field)
        {
            base.GenerateCodeStaticField(field);
            if (field.IsConst)
                _codeBuilder.Append("const ");
            if (field.IsStatic)
                _codeBuilder.Append("static ");
            _codeBuilder.Append(GetCLRTypeName(field) + " " + field.NativeName + " = " + field.MemberType.ProduceNativeCallConversionCode(field.FullNativeName, field) + ";\n\n");
        }

        protected override void GenerateCodeNestedTypeBeforeMainType(AbstractTypeDefinition nested)
        {
            base.GenerateCodeNestedType(nested);
            _wrapper.IncAddType(nested, _codeBuilder);
        }

        protected override void GenerateCodeAllNestedTypes()
        {
            //Predeclare all nested classes in case there are classes referencing their "siblings"
            foreach (AbstractTypeDefinition nested in _classDefinition.NestedTypes)
            {
                if (nested.ProtectionLevel == ProtectionLevel.Public
                    || ((AllowProtectedMembers || AllowSubclassing) && nested.ProtectionLevel == ProtectionLevel.Protected))
                {
                    AbstractTypeDefinition expl = _classDefinition.DetermineType<AbstractTypeDefinition>(nested.Name);

                    if ((expl.IsSTLContainer
                        || (!nested.IsValueType && nested is ClassDefinition && !(nested as ClassDefinition).IsInterface)) && Wrapper.IsTypeWrappable(nested))
                    {
                        _codeBuilder.AppendLine(nested.ProtectionLevel.GetCLRProtectionName() + ": ref class " + nested.CLRName + ";");
                    }
                }
            }

            _codeBuilder.AppendEmptyLine();

            base.GenerateCodeAllNestedTypes();
        }

        protected override void GenerateCodeNestedType(AbstractTypeDefinition nested)
        {
            if (nested.HasWrapType(WrapTypes.NativeDirector))
            {
                //Interface and native director are already declared before the declaration of this class.
                //Just declare the method handlers of the class.
                NativeDirectorClassInclProducer.AddMethodHandlersClass((ClassDefinition)nested, _codeBuilder);
                return;
            }

            base.GenerateCodeNestedType(nested);
            _wrapper.IncAddType(nested, _codeBuilder);
        }

        protected virtual void AddCachedFields()
        {
            if (_cachedMembers.Count > 0)
            {
                _codeBuilder.AppendLine("//Cached fields");
                foreach (MemberDefinitionBase m in _cachedMembers)
                {
                    if (m.IsStatic)
                    {
                        _codeBuilder.Append("static ");
                        _wrapper.AddUsedType(m.MemberType);
                    }

                    _codeBuilder.AppendLine(m.MemberTypeCLRName + " " + NameToPrivate(m) + ";");
                }
            }
        }

        protected virtual string GetBaseAndInterfaces()
        {
            string baseclass = "";
            if (GetBaseClassName() != null)
                baseclass = "public " + GetBaseClassName();

            if (_interfaces.Count > 0)
            {
                if (baseclass != "")
                    baseclass += ", ";

                foreach (ClassDefinition it in _interfaces)
                {
                    AddTypeDependancy(it);
                    string itname = it.CLRName;
                    if (it.IsNested)
                        itname = it.SurroundingClass.FullyQualifiedCLRName + "::" + itname;
                    baseclass += "public " + itname + ", ";
                }
                baseclass = baseclass.Substring(0, baseclass.Length - ", ".Length);
            }

            if (_listeners.Count > 0)
            {
                if (baseclass != "")
                    baseclass += ", ";

                foreach (ClassDefinition it in _listeners)
                {
                    AddTypeDependancy(it);
                    baseclass += "public " + GetNativeDirectorReceiverInterfaceName(it) + ", ";
                }
                baseclass = baseclass.Substring(0, baseclass.Length - ", ".Length);
            }

            return baseclass;
        }

        protected override void GenerateCodeMethod(MemberMethodDefinition f)
        {
            if (f.HasAttribute<CustomIncDeclarationAttribute>())
            {
                string txt = f.GetAttribute<CustomIncDeclarationAttribute>().DeclarationText;
                txt = ReplaceCustomVariables(txt, f);
                _codeBuilder.AppendLine(txt);
                _codeBuilder.AppendEmptyLine();
                return;
            }

            int defcount = 0;

            if (!f.HasAttribute<NoDefaultParamOverloadsAttribute>())
            {
                foreach (ParamDefinition param in f.Parameters)
                    if (param.DefaultValue != null)
                        defcount++;
            }

            bool methodIsVirtual = DeclareAsVirtual(f);

            // The main method
            AddComments(f, f.Parameters.Count);

            if (AllowMethodIndexAttributes && f.IsVirtual && !f.IsAbstract)
                AddMethodIndexAttribute(f);

            if (f.IsStatic)
                _codeBuilder.Append("static ");
            if (methodIsVirtual)
                _codeBuilder.Append("virtual ");
            _codeBuilder.Append(GetCLRTypeName(f) + " " + f.CLRName);
            AddMethodParameters(f, f.Parameters.Count);
            if (DeclareAsOverride(f))
            {
                _codeBuilder.Append(" override");
            }
            else if (f.IsAbstract && AllowSubclassing)
            {
                _codeBuilder.Append(" abstract");
            }

            _codeBuilder.AppendLine(";");

            if (AllowMethodOverloads)
            {
                // The overloads (because of default values)
                for (int dc = 1; dc <= defcount; dc++)
                {
                    if (dc < defcount && f.HasAttribute<HideParamsWithDefaultValuesAttribute>())
                        continue;

                    AddComments(f, f.Parameters.Count - dc);
                    if (f.IsStatic)
                        _codeBuilder.Append("static ");
                    _codeBuilder.Append(GetCLRTypeName(f) + " " + f.CLRName);
                    AddMethodParameters(f, f.Parameters.Count - dc);
                    _codeBuilder.AppendLine(";");
                }
            }
        }

        protected virtual void AddMethodIndexAttribute(MemberMethodDefinition f)
        {
            _codeBuilder.AppendLine("[Implementation::MethodIndex( " + _methodIndices[f] + " )]");
        }

        protected virtual void AddMethodParameters(MemberMethodDefinition f, int count)
        {
            _codeBuilder.Append("(");
            for (int i = 0; i < count; i++)
            {
                ParamDefinition param = f.Parameters[i];

                _codeBuilder.Append(" " + GetCLRParamTypeName(param));
                _codeBuilder.Append(" " + param.Name);
                if (i < count - 1)
                    _codeBuilder.Append(",");
            }
            _codeBuilder.Append(" )");
        }

        protected void AddMethodParameters(MemberMethodDefinition f)
        {
            AddMethodParameters(f, f.Parameters.Count);
        }

        protected override void GenerateCodeProperty(MemberPropertyDefinition p)
        {
            AddComments(p);
            string ptype = GetCLRTypeName(p);
            _codeBuilder.AppendLine("property {0} {1}", ptype, p.Name);
            _codeBuilder.BeginBlock();
            if (p.CanRead)
            {
                MemberMethodDefinition f = p.GetterFunction;
                bool methodIsVirtual = DeclareAsVirtual(f);

                if (p.GetterFunction.ProtectionLevel == ProtectionLevel.Public || (AllowProtectedMembers && p.GetterFunction.ProtectionLevel == ProtectionLevel.Protected))
                {
                    _codeBuilder.DecreaseIndent();
                    _codeBuilder.AppendLine(p.GetterFunction.ProtectionLevel.GetCLRProtectionName() + ":");
                    _codeBuilder.IncreaseIndent();

                    if (AllowMethodIndexAttributes && f.IsVirtual && !f.IsAbstract)
                    {
                        AddMethodIndexAttribute(f);
                    }

                    if (p.GetterFunction.IsStatic)
                        _codeBuilder.Append("static ");
                    if (methodIsVirtual)
                        _codeBuilder.Append("virtual ");
                    _codeBuilder.Append(ptype + " get()");
                    if (DeclareAsOverride(p.GetterFunction))
                    {
                        _codeBuilder.Append(" override");
                    }
                    else if (f.IsAbstract && AllowSubclassing)
                    {
                        _codeBuilder.Append(" abstract");
                    }
                    _codeBuilder.AppendLine(";");
                }
            }
            if (p.CanWrite)
            {
                MemberMethodDefinition f = p.SetterFunction;
                bool methodIsVirtual = DeclareAsVirtual(f);

                if (p.SetterFunction.ProtectionLevel == ProtectionLevel.Public || (AllowProtectedMembers && p.SetterFunction.ProtectionLevel == ProtectionLevel.Protected))
                {
                    _codeBuilder.DecreaseIndent();
                    _codeBuilder.AppendLine(p.SetterFunction.ProtectionLevel.GetCLRProtectionName() + ":");
                    _codeBuilder.IncreaseIndent();

                    if (AllowMethodIndexAttributes && f.IsVirtual && !f.IsAbstract)
                    {
                        AddMethodIndexAttribute(f);
                    }

                    if (p.SetterFunction.IsStatic)
                        _codeBuilder.Append("static ");
                    if (methodIsVirtual)
                        _codeBuilder.Append("virtual ");
                    _codeBuilder.Append("void set(" + ptype + " " + p.SetterFunction.Parameters[0].Name + ")");
                    if (DeclareAsOverride(p.SetterFunction))
                    {
                        _codeBuilder.Append(" override");
                    }
                    else if (f.IsAbstract && AllowSubclassing)
                    {
                        _codeBuilder.Append(" abstract");
                    }
                    _codeBuilder.AppendLine(";");
                }
            }
            _codeBuilder.EndBlock();
        }

        protected override void GenerateCodePropertyField(MemberFieldDefinition field)
        {
            AddComments(field);

            string ptype;

            if (field.IsNativeArray)
            {
                if (field.MemberType.HasAttribute<NativeValueContainerAttribute>()
                    || (field.MemberType.IsValueType && !field.MemberType.HasWrapType(WrapTypes.NativePtrValueType)))
                {
                    ParamDefinition tmpParam = new ParamDefinition(this.MetaDef, field, field.NativeName);
                    switch (field.PassedByType)
                    {
                        case PassedByType.Value:
                            tmpParam.PassedByType = PassedByType.Pointer;
                            break;
                        case PassedByType.Pointer:
                            tmpParam.PassedByType = PassedByType.PointerPointer;
                            break;
                        default:
                            throw new Exception("Unexpected");
                    }

                    ptype = tmpParam.MemberTypeCLRName;
                    if (field.IsStatic)
                        _codeBuilder.Append("static ");
                    _codeBuilder.AppendLine("property {0} {1}", ptype, field.NativeName);
                    _codeBuilder.BeginBlock();

                    _codeBuilder.DecreaseIndent();
                    _codeBuilder.AppendLine(field.ProtectionLevel.GetCLRProtectionName() + ":");
                    _codeBuilder.IncreaseIndent();
                    _codeBuilder.AppendLine(ptype + " get();");

                    _codeBuilder.EndBlock();
                }
                else
                {
                    ptype = field.MemberTypeCLRName;
                    if (field.IsStatic)
                        _codeBuilder.Append("static ");
                    _codeBuilder.AppendLine("property {0} {1}[int]", ptype, field.NativeName);
                    _codeBuilder.BeginBlock();

                    _codeBuilder.DecreaseIndent();
                    _codeBuilder.AppendLine(field.ProtectionLevel.GetCLRProtectionName() + ":");
                    _codeBuilder.IncreaseIndent();
                    _codeBuilder.AppendLine(ptype + " get(int index);");
                    _codeBuilder.AppendLine("void set(int index, " + ptype + " value);");

                    _codeBuilder.EndBlock();
                }
            }
            else if (_cachedMembers.Contains(field))
            {
                ptype = field.MemberTypeCLRName;
                if (field.IsStatic)
                    _codeBuilder.Append("static ");
                _codeBuilder.AppendLine("property {0} {1}", ptype, field.NativeName);
                _codeBuilder.BeginBlock();

                _codeBuilder.DecreaseIndent();
                _codeBuilder.AppendLine(field.ProtectionLevel.GetCLRProtectionName() + ":");
                _codeBuilder.IncreaseIndent();
                _codeBuilder.AppendLine(ptype + " get();");

                _codeBuilder.EndBlock();
            }
            else
            {
                ptype = GetCLRTypeName(field);
                if (field.IsStatic)
                    _codeBuilder.Append("static ");
                if (field.HasAttribute<RenameAttribute>())
                {
                    _codeBuilder.AppendLine("property {0} {1}", ptype, field.GetAttribute<RenameAttribute>().Name);
                }
                else
                {
                    _codeBuilder.AppendLine("property {0} {1}", ptype, field.NativeName);
                }
                _codeBuilder.BeginBlock();

                _codeBuilder.DecreaseIndent();
                _codeBuilder.AppendLine(field.ProtectionLevel.GetCLRProtectionName() + ":");
                _codeBuilder.IncreaseIndent();
                _codeBuilder.AppendLine(ptype + " get();");

                if ( // SharedPtrs can be copied by value. Let all be copied by value just to be sure (field.PassedByType == PassedByType.Pointer || field.Type.IsValueType)
                   !IsReadOnly && !field.MemberType.HasAttribute<ReadOnlyForFieldsAttribute>()
                    && !field.IsConst)
                {
                    _codeBuilder.DecreaseIndent();
                    _codeBuilder.AppendLine(field.ProtectionLevel.GetCLRProtectionName() + ":");
                    _codeBuilder.IncreaseIndent();
                    _codeBuilder.AppendLine("void set(" + ptype + " value);");
                }

                _codeBuilder.EndBlock();
            }
        }

        protected override void GenerateCodeMethodsForField(MemberFieldDefinition field)
        {
            _codeBuilder.AppendLine(GetCLRTypeName(field) + " get_" + field.NativeName + "();");
            ParamDefinition param = new ParamDefinition(this.MetaDef, field, "value");
            _codeBuilder.AppendLine("void set_" + field.NativeName + "(" + param.Type.GetCLRParamTypeName(param) + " value);");
        }

        protected virtual void AddComments()
        {
            string summary = _classDefinition.Summary;
            if (!String.IsNullOrEmpty(summary))
            {
                _codeBuilder.AppendLine("/**");
                _codeBuilder.AppendLine("<summary>{0}</summary>", CodeStyleDefinition.ToCamelCase(summary));
                _codeBuilder.AppendLine("*/");
            }
        }

        protected virtual void AddComments(MemberMethodDefinition f, int count)
        {
            string summary = f.Summary;
            if (String.IsNullOrEmpty(summary) || f.Summary.StartsWith("overrid", StringComparison.OrdinalIgnoreCase)) //catches both overrides and overridden
            {
                MemberMethodDefinition m = f;
                while (m.BaseMethod != null)
                {
                    m = m.BaseMethod;
                }
                summary = m.Summary;
            }
            if (!String.IsNullOrEmpty(summary))
            {
                _codeBuilder.AppendLine("/**");
                _codeBuilder.AppendLine("<summary>{0}</summary>", CodeStyleDefinition.ToCamelCase(summary));
                for (int i = 0; i < count; i++)
                {
                    ParamDefinition param = f.Parameters[i];
                    if (!String.IsNullOrEmpty(param.Summary))
                    {
                        _codeBuilder.AppendLine("<param name=\"{0}\">{1}</param>", param.Name, param.Summary);
                    }
                }
                _codeBuilder.AppendLine("*/");
            }
        }

        protected virtual void AddComments(MemberPropertyDefinition p)
        {
            if (p.CanRead && !String.IsNullOrEmpty(p.GetterFunction.Summary))
            {
                string getsummary = p.GetterFunction.Summary;
                if (p.GetterFunction.Summary.StartsWith("overrid", StringComparison.OrdinalIgnoreCase)) //catches both overrides and overridden
                {
                    MemberMethodDefinition m = p.GetterFunction;
                    while (m.BaseMethod != null)
                    {
                        m = m.BaseMethod;
                    }
                    getsummary = m.Summary;
                }

                _codeBuilder.AppendLine("/**");
                _codeBuilder.Append("<summary>");
                if (p.CanWrite)
                {
                    string setsummary = p.SetterFunction.Summary;

                    if (getsummary.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
                    {
                        _codeBuilder.Append("Gets or Sets");
                        _codeBuilder.Append(getsummary.Substring(getsummary.IndexOf(' ')));
                    }
                    else if (setsummary != null && setsummary.StartsWith("Set", StringComparison.OrdinalIgnoreCase))
                    {
                        _codeBuilder.Append("Gets or Sets");
                        _codeBuilder.Append(setsummary.Substring(setsummary.IndexOf(' ')));
                    }
                    else if (getsummary.StartsWith("Returns true ", StringComparison.OrdinalIgnoreCase))
                    {
                        _codeBuilder.Append("Gets or Sets");
                        _codeBuilder.Append(getsummary.Substring(12));
                    }
                    else if (getsummary.StartsWith("Retrieve", StringComparison.OrdinalIgnoreCase) ||
                        getsummary.StartsWith("Return", StringComparison.OrdinalIgnoreCase) ||
                        getsummary.StartsWith("Determine", StringComparison.OrdinalIgnoreCase))
                    {
                        _codeBuilder.Append("Gets or Sets");
                        _codeBuilder.Append(getsummary.Substring(getsummary.IndexOf(' ')));
                    }
                    else
                    {
                        _codeBuilder.Append(CodeStyleDefinition.ToCamelCase(getsummary));
                    }
                }
                else
                {
                    _codeBuilder.Append(CodeStyleDefinition.ToCamelCase(getsummary));
                }
                _codeBuilder.AppendLine("</summary>");
                _codeBuilder.AppendLine("*/");
            }
        }

        protected virtual void AddComments(MemberFieldDefinition f)
        {
            if (!String.IsNullOrEmpty(f.Summary))
            {
                _codeBuilder.AppendLine("/**");
                _codeBuilder.AppendLine("<summary>{0}</summary>", CodeStyleDefinition.ToCamelCase(f.Summary));
                _codeBuilder.AppendLine("*/");
            }
        }

        protected override void GenerateCodePredefinedMethods(PredefinedMethods pm)
        {
            string clrType = _classDefinition.CLRName + (_classDefinition.IsValueType ? "" : "^");

            // For operator ==
            if ((PredefinedMethods.Equals & pm) != 0)
            {
                _codeBuilder.AppendLine("virtual bool Equals(Object^ obj) override;");
                _codeBuilder.AppendLine("bool Equals(" + clrType + " obj);");
                _codeBuilder.AppendLine("static bool operator == (" + clrType + " obj1, " + clrType + " obj2);");
                _codeBuilder.AppendLine("static bool operator != (" + clrType + " obj1, " + clrType + " obj2);");
            }

            // For operator =
            if ((PredefinedMethods.CopyTo & pm) != 0)
            {
                _codeBuilder.AppendLine("void CopyTo(" + clrType + " dest)");
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("if (_native == NULL) throw gcnew Exception(\"The underlying native object for the caller is null.\");");
                _codeBuilder.AppendLine("if (dest" + (_classDefinition.IsValueType ? "." : "->") + "_native == NULL) throw gcnew ArgumentException(\"The underlying native object for parameter 'dest' is null.\");");
                _codeBuilder.AppendEmptyLine();
                _codeBuilder.AppendLine("*(dest" + (_classDefinition.IsValueType ? "." : "->") + "_native) = *_native;");
                _codeBuilder.EndBlock();
            }
        }
    }
}
