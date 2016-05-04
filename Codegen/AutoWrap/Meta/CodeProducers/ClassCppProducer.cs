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
    abstract class ClassCppProducer : ClassCodeProducer
    {
        public ClassCppProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
            //if (AllowSubclassing)
            //{
            //    _wrapper.PreClassProducers.Add(new CppNativeProtectedTypesProxy(_wrapper, _t, _sb));
            //}
        }

        protected override void AddTypeDependancy(AbstractTypeDefinition type)
        {
            _wrapper.CppCheckTypeForDependancy(type);
        }

        protected virtual string GetCLRTypeName(ITypeMember m)
        {
            AddTypeDependancy(m.MemberType);
            if (m.MemberType.IsUnnamedSTLContainer)
                return GetClassName() + "::" + m.MemberTypeCLRName;
            else
                return m.MemberTypeCLRName;
        }

        protected virtual string GetCLRParamTypeName(ParamDefinition param)
        {
            AddTypeDependancy(param.Type);
            return param.Type.GetCLRParamTypeName(param);
        }

        protected override void GenerateCodePostBody()
        {
            base.GenerateCodePostBody();
            _codeBuilder.AppendEmptyLine();

            if (_classDefinition.HasAttribute<CLRObjectAttribute>(true))
            {
                _codeBuilder.AppendLine("__declspec(dllexport) " + _wrapper.GetInitCLRObjectFuncSignature(_classDefinition));
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("*pClrObj = gcnew " + _classDefinition.FullyQualifiedCLRName + "(pClrObj);");
                _codeBuilder.EndBlock();
            }

            _codeBuilder.AppendEmptyLine();
        }

        protected override void GenerateCodeInternalDeclarations()
        {
            base.GenerateCodeInternalDeclarations();

            foreach (ClassDefinition cls in _interfaces)
            {
                _codeBuilder.AppendLine(cls.FullyQualifiedNativeName + "* " + GetClassName() + "::_" + cls.CLRName + "_GetNativePtr()");
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return static_cast<" + cls.FullyQualifiedNativeName + "*>( " + GetNativeInvokationTarget() + " );");
                _codeBuilder.EndBlock();
                _codeBuilder.AppendEmptyLine();
            }
        }

        protected override void GenerateCodePublicDeclarations()
        {
            if (!_classDefinition.IsNativeAbstractClass || _classDefinition.IsInterface)
            {
                if (_classDefinition.Constructors.Length > 0)
                {
                    foreach (MemberMethodDefinition function in _classDefinition.Constructors)
                    {
                        if (function.ProtectionLevel == ProtectionLevel.Public &&
                            !function.HasAttribute<IgnoreAttribute>())
                        {
                            AddPublicConstructor(function);
                        }
                    }
                }
                else
                {
                    AddPublicConstructor(null);
                }

                _codeBuilder.AppendEmptyLine();
            }

            base.GenerateCodePublicDeclarations();
        }

        protected virtual void AddPublicConstructor(MemberMethodDefinition function)
        {
            if (function == null)
            {
                AddPublicConstructorOverload(function, 0);
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
                    if (dc < defcount && function.HasAttribute<HideParamsWithDefaultValuesAttribute>())
                        continue;
                    AddPublicConstructorOverload(function, function.Parameters.Count - dc);
                }

            }
        }

        protected virtual void AddPublicConstructorOverload(MemberMethodDefinition f, int count)
        {
            _codeBuilder.Append(GetClassName() + "::" + _classDefinition.CLRName);
            if (f == null)
                _codeBuilder.Append("()");
            else
                AddMethodParameters(f, count);

            string nativeType = GetTopClass(_classDefinition).FullyQualifiedNativeName;
            if (GetTopBaseClassName() == "Wrapper")
                nativeType = "CLRObject";

            if (GetBaseClassName() != null)
                _codeBuilder.Append(" : " + GetBaseClassName() + "((" + nativeType + "*) 0)");

            _codeBuilder.AppendEmptyLine();
            _codeBuilder.BeginBlock();

            if (!_classDefinition.IsInterface)
                _codeBuilder.AppendLine("_createdByCLR = true;");

            string preCall = null, postCall = null;

            if (f != null)
            {
                preCall = GetMethodPreNativeCall(f, count);
                postCall = GetMethodPostNativeCall(f, count);

                if (!String.IsNullOrEmpty(preCall))
                    _codeBuilder.AppendLine(preCall);
            }

            _codeBuilder.Append("_native = new " + _classDefinition.FullyQualifiedNativeName + "(");

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    ParamDefinition p = f.Parameters[i];
                    string newname;
                    p.Type.ProducePreCallParamConversionCode(p, out newname);
                    _codeBuilder.Append(" " + newname);
                    if (i < count - 1) _codeBuilder.Append(",");
                }
            }

            _codeBuilder.AppendLine(");");

            if (!String.IsNullOrEmpty(postCall))
            {
                _codeBuilder.AppendEmptyLine();
                _codeBuilder.AppendLine(postCall);
                _codeBuilder.AppendEmptyLine();
            }

            GenerateCodeConstructorBody();

            _codeBuilder.EndBlock();
        }

        protected override void GenerateCodeStaticConstructor()
        {
            if (_classDefinition.IsInterface)
                _codeBuilder.AppendLine("static " + _classDefinition.Name + "::" + _classDefinition.Name + "()");
            else
                _codeBuilder.AppendLine("static " + _classDefinition.CLRName + "::" + _classDefinition.CLRName + "()");

            _codeBuilder.BeginBlock();
            foreach (MemberDefinitionBase m in _cachedMembers)
            {
                if (m.IsStatic)
                {
                    _codeBuilder.Append(NameToPrivate(m) + " = ");
                    if (m.ProtectionLevel == ProtectionLevel.Protected)
                    {
                        _codeBuilder.Append(NativeProtectedTypesProxy.GetProtectedTypesProxyName(m.ContainingClass));
                        _codeBuilder.AppendLine("::" + m.NativeName + ";");
                    }
                    else
                    {
                        _codeBuilder.Append(m.ContainingClass.FullyQualifiedNativeName);
                        _codeBuilder.AppendLine("::" + m.NativeName + ";");
                    }
                }
            }
            _codeBuilder.EndBlock();
        }

        protected override void GenerateCodePostNestedTypes()
        {
            base.GenerateCodePostNestedTypes();

            if (_classDefinition.HasAttribute<CustomCppDeclarationAttribute>())
            {
                string txt = _classDefinition.GetAttribute<CustomCppDeclarationAttribute>().DeclarationText;
                txt = ReplaceCustomVariables(txt);
                _codeBuilder.AppendLine(txt);
                _codeBuilder.AppendEmptyLine();
            }
        }

        protected override void GenerateCodeNestedTypeBeforeMainType(AbstractTypeDefinition nested)
        {
            base.GenerateCodeNestedType(nested);
            _wrapper.CppAddType(nested, _codeBuilder);
        }

        protected override void GenerateCodeNestedType(AbstractTypeDefinition nested)
        {
            if (nested.HasWrapType(WrapTypes.NativeDirector))
            {
                //Interface and native director are already declared before the declaration of this class.
                return;
            }

            base.GenerateCodeNestedType(nested);
            _wrapper.CppAddType(nested, _codeBuilder);
        }

        protected override void GenerateCodeMethod(MemberMethodDefinition f)
        {
            if (f.HasAttribute<CustomCppDeclarationAttribute>())
            {
                if (f.IsAbstract && AllowSubclassing)
                {
                    return;
                }
                else
                {
                    string txt = f.GetAttribute<CustomCppDeclarationAttribute>().DeclarationText;
                    txt = ReplaceCustomVariables(txt, f);
                    _codeBuilder.AppendLine(txt);
                    _codeBuilder.AppendEmptyLine();
                    return;
                }
            }

            int defcount = 0;

            if (!f.HasAttribute<NoDefaultParamOverloadsAttribute>())
            {
                foreach (ParamDefinition param in f.Parameters)
                    if (param.DefaultValue != null)
                        defcount++;
            }

            bool methodIsVirtual = DeclareAsVirtual(f);

            for (int dc = 0; dc <= defcount; dc++)
            {
                if (dc == 0 && f.IsAbstract && AllowSubclassing)
                {
                    //It's abstract, no body definition
                    continue;
                }

                if (!AllowMethodOverloads && dc > 0)
                    continue;

                if (dc < defcount && f.HasAttribute<HideParamsWithDefaultValuesAttribute>())
                    continue;

                _codeBuilder.Append(GetCLRTypeName(f) + " " + GetClassName() + "::" + f.CLRName);
                AddMethodParameters(f, f.Parameters.Count - dc);
                _codeBuilder.AppendEmptyLine();
                _codeBuilder.BeginBlock();

                bool isVirtualOverload = dc > 0 && methodIsVirtual && AllowVirtualMethods;

                if (isVirtualOverload)
                {
                    // Overloads (because of default values)
                    // main method is virtual, call it with CLR default values if _isOverriden=true,
                    // else do a normal native call

                    _codeBuilder.AppendLine("if (_isOverriden)");
                    _codeBuilder.BeginBlock();

                    bool hasPostConversions = false;
                    for (int i = f.Parameters.Count - dc; i < f.Parameters.Count; i++)
                    {
                        ParamDefinition p = f.Parameters[i];
                        if (!String.IsNullOrEmpty(p.CLRDefaultValuePreConversion))
                            _codeBuilder.AppendLine(p.CLRDefaultValuePreConversion);
                        if (!String.IsNullOrEmpty(p.CLRDefaultValuePostConversion))
                            hasPostConversions = true;

                        string n1, n2, n3;
                        AbstractTypeDefinition dependancy;
                        p.Type.ProduceDefaultParamValueConversionCode(p, out n1, out n2, out n3, out dependancy);
                        if (dependancy != null)
                            AddTypeDependancy(dependancy);
                    }

                    if (!f.HasReturnValue)
                    {
                        if (hasPostConversions)
                        {
                            _codeBuilder.Append(GetCLRTypeName(f) + " mp_return = ");
                        }
                        else
                        {
                            _codeBuilder.Append("return ");
                        }
                    }

                    _codeBuilder.Append(f.CLRName + "(");
                    for (int i = 0; i < f.Parameters.Count; i++)
                    {
                        ParamDefinition p = f.Parameters[i];
                        _codeBuilder.Append(" ");
                        if (i < f.Parameters.Count - dc)
                            _codeBuilder.Append(p.Name);
                        else
                        {
                            _codeBuilder.Append(p.CLRDefaultValue);
                        }
                        if (i < f.Parameters.Count - 1) _codeBuilder.Append(",");
                    }
                    _codeBuilder.AppendLine(" );");

                    for (int i = f.Parameters.Count - dc; i < f.Parameters.Count; i++)
                    {
                        ParamDefinition p = f.Parameters[i];
                        if (!String.IsNullOrEmpty(p.CLRDefaultValuePostConversion))
                            _codeBuilder.AppendLine(p.CLRDefaultValuePostConversion);
                    }

                    if (!f.HasReturnValue && hasPostConversions)
                    {
                        _codeBuilder.AppendLine("return mp_return;");
                    }

                    _codeBuilder.EndBlock();
                    _codeBuilder.AppendLine("else");
                    _codeBuilder.BeginBlock();
                }

                AddMethodBody(f, f.Parameters.Count - dc);

                if (isVirtualOverload)
                {
                    _codeBuilder.EndBlock();
                }

                _codeBuilder.EndBlock();
            }
        }

        protected virtual void AddMethodParameters(MemberMethodDefinition f, int count)
        {
            _codeBuilder.Append("(");
            for (int i = 0; i < count; i++)
            {
                ParamDefinition p = f.Parameters[i];
                _codeBuilder.Append(" " + GetCLRParamTypeName(p) + " " + p.Name);
                if (i < count - 1) _codeBuilder.Append(",");
            }
            _codeBuilder.Append(" )");
        }
        protected void AddMethodParameters(MemberMethodDefinition f)
        {
            AddMethodParameters(f, f.Parameters.Count);
        }

        protected virtual void AddMethodBody(MemberMethodDefinition f, int count)
        {
            string preCall = GetMethodPreNativeCall(f, count);
            string nativeCall = GetMethodNativeCall(f, count);
            string postCall = GetMethodPostNativeCall(f, count);

            if (!String.IsNullOrEmpty(preCall))
                _codeBuilder.AppendLine(preCall);

            if (f.HasReturnValue)
            {
                _codeBuilder.AppendLine(nativeCall + ";");
                if (!String.IsNullOrEmpty(postCall))
                    _codeBuilder.AppendLine(postCall);
            }
            else
            {
                if (String.IsNullOrEmpty(postCall))
                {
                    _codeBuilder.AppendLine("return " + nativeCall + ";");
                }
                else
                {
                    _codeBuilder.AppendLine(GetCLRTypeName(f) + " retres = " + nativeCall + ";");
                    _codeBuilder.AppendLine(postCall);
                    _codeBuilder.AppendLine("return retres;");
                }
            }
        }

        protected virtual string GetMethodPreNativeCall(MemberMethodDefinition f, int paramCount)
        {
            string res = String.Empty;

            for (int i = 0; i < paramCount; i++)
            {
                ParamDefinition p = f.Parameters[i];
                string newname;
                res += p.Type.ProducePreCallParamConversionCode(p, out newname);
            }

            return res;
        }

        protected virtual string GetMethodNativeCall(MemberMethodDefinition f, int paramCount)
        {
            string invoke;
            if (f.IsStatic)
            {
                if (f.ProtectionLevel == ProtectionLevel.Protected)
                {
                    string classname = NativeProtectedStaticsProxy.GetProtectedStaticsProxyName(_classDefinition);
                    invoke = classname + "::" + f.NativeName + "(";
                }
                else
                {
                    invoke = _classDefinition.FullyQualifiedNativeName + "::" + f.NativeName + "(";
                }
            }
            else
            {
                invoke = GetNativeInvokationTarget(f) + "(";
            }

            for (int i = 0; i < paramCount; i++)
            {
                ParamDefinition p = f.Parameters[i];
                string newname;
                p.Type.ProducePreCallParamConversionCode(p, out newname);
                invoke += " " + newname;
                if (i < paramCount - 1) invoke += ",";
            }

            invoke += " )";

            if (f.HasReturnValue)
                return invoke;
            else
                return f.MemberType.ProduceNativeCallConversionCode(invoke, f);
        }

        protected virtual string GetMethodPostNativeCall(MemberMethodDefinition f, int paramCount)
        {
            string res = String.Empty;

            for (int i = 0; i < paramCount; i++)
            {
                ParamDefinition p = f.Parameters[i];
                res += p.Type.ProducePostCallParamConversionCleanupCode(p);
            }

            return res;
        }

        protected string AddParameterConversion(ParamDefinition param)
        {
            string newname, expr, postcall;
            expr = param.Type.ProducePreCallParamConversionCode(param, out newname);
            postcall = param.Type.ProducePostCallParamConversionCleanupCode(param);
            if (!String.IsNullOrEmpty(postcall))
                throw new Exception("Unexpected");

            if (!String.IsNullOrEmpty(expr))
                _codeBuilder.AppendLine(expr);

            return newname;
        }

        protected override void GenerateCodeProperty(MemberPropertyDefinition p)
        {
            string ptype = GetCLRTypeName(p);
            string pname = GetClassName() + "::" + p.Name;
            if (p.CanRead)
            {
                if (!(p.GetterFunction.IsAbstract && AllowSubclassing))
                {
                    if (AllowProtectedMembers || p.GetterFunction.ProtectionLevel != ProtectionLevel.Protected)
                    {
                        string managedType = GetMethodNativeCall(p.GetterFunction, 0);

                        _codeBuilder.AppendLine(ptype + " " + pname + "::get()");
                        _codeBuilder.BeginBlock();
                        if (_cachedMembers.Contains(p.GetterFunction))
                        {
                            string priv = NameToPrivate(p.Name);
                            _codeBuilder.AppendLine("return ( CLR_NULL == " + priv + " ) ? (" + priv + " = " + managedType + ") : " + priv + ";");
                        }
                        else
                        {
                            _codeBuilder.AppendLine("return " + managedType + ";");
                        }
                        _codeBuilder.EndBlock();
                    }
                }
            }

            if (p.CanWrite)
            {
                if (!(p.SetterFunction.IsAbstract && AllowSubclassing))
                {
                    if (AllowProtectedMembers || p.SetterFunction.ProtectionLevel != ProtectionLevel.Protected)
                    {
                        _codeBuilder.AppendLine("void " + pname + "::set( " + ptype + " " + p.SetterFunction.Parameters[0].Name + " )");
                        _codeBuilder.BeginBlock();

                        string preCall = GetMethodPreNativeCall(p.SetterFunction, 1);
                        string nativeCall = GetMethodNativeCall(p.SetterFunction, 1);
                        string postCall = GetMethodPostNativeCall(p.SetterFunction, 1);

                        if (!String.IsNullOrEmpty(preCall))
                            _codeBuilder.AppendLine(preCall);

                        _codeBuilder.AppendLine(nativeCall + ";");

                        if (!String.IsNullOrEmpty(postCall))
                            _codeBuilder.AppendLine(postCall);

                        _codeBuilder.EndBlock();
                    }
                }
            }
        }

        protected override void GenerateCodePropertyField(MemberFieldDefinition field)
        {
            string ptype = GetCLRTypeName(field);
            string pname = GetClassName() + "::" + (field.HasAttribute<RenameAttribute>() ? field.GetAttribute<RenameAttribute>().Name : field.NativeName);

            if (field.IsNativeArray)
            {
                if (field.MemberType.HasAttribute<NativeValueContainerAttribute>()
                    || (field.MemberType.IsValueType && !field.MemberType.HasWrapType(WrapTypes.NativePtrValueType)))
                {
                    ParamDefinition tmpParam = new ParamDefinition(this.MetaDef, field, field.NativeName + "_array");
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

                    ptype = GetCLRTypeName(tmpParam);
                    string managedType = field.MemberType.ProduceNativeCallConversionCode(GetNativeInvokationTarget(field), tmpParam);

                    _codeBuilder.AppendLine(ptype + " " + pname + "::get()");
                    _codeBuilder.BeginBlock();
                    _codeBuilder.AppendLine("return " + managedType + ";");
                    _codeBuilder.EndBlock();
                }
                else
                {
                    string managedType = field.MemberType.ProduceNativeCallConversionCode(GetNativeInvokationTarget(field) + "[index]", field);

                    _codeBuilder.AppendLine(ptype + " " + pname + "::get(int index)");
                    _codeBuilder.BeginBlock();
                    _codeBuilder.AppendLine("if (index < 0 || index >= " + field.ArraySize + ") throw gcnew IndexOutOfRangeException();");
                    _codeBuilder.AppendLine("return " + managedType + ";");
                    _codeBuilder.EndBlock();
                    _codeBuilder.AppendLine("void " + pname + "::set(int index, " + ptype + " value )");
                    _codeBuilder.BeginBlock();
                    _codeBuilder.AppendLine("if (index < 0 || index >= " + field.ArraySize + ") throw gcnew IndexOutOfRangeException();");
                    string param = AddParameterConversion(new ParamDefinition(this.MetaDef, field, "value"));
                    _codeBuilder.AppendLine(GetNativeInvokationTarget(field) + "[index] = " + param + ";");
                    _codeBuilder.EndBlock();
                }
            }
            else if (_cachedMembers.Contains(field))
            {
                string managedType;
                if (field.MemberType.IsSTLContainer)
                {
                    managedType = GetNativeInvokationTarget(field);
                }
                else
                {
                    managedType = field.MemberType.ProduceNativeCallConversionCode(GetNativeInvokationTarget(field), field);
                }
                string priv = NameToPrivate(field);

                _codeBuilder.AppendLine(ptype + " " + pname + "::get()");
                _codeBuilder.BeginBlock();
                if (!field.IsStatic)
                    _codeBuilder.AppendLine("return ( CLR_NULL == " + priv + " ) ? (" + priv + " = " + managedType + ") : " + priv + ";");
                else
                    _codeBuilder.AppendLine("return " + priv + ";");
                _codeBuilder.EndBlock();
            }
            else
            {
                string managedType = field.MemberType.ProduceNativeCallConversionCode(GetNativeInvokationTarget(field), field);

                _codeBuilder.AppendLine(ptype + " " + pname + "::get()");
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return " + managedType + ";");
                _codeBuilder.EndBlock();

                if ( // SharedPtrs can be copied by value. Let all be copied by value just to be sure (field.PassedByType == PassedByType.Pointer || field.Type.IsValueType)
                    !IsReadOnly && !field.MemberType.HasAttribute<ReadOnlyForFieldsAttribute>()
                    && !field.IsConst)
                {
                    _codeBuilder.AppendLine("void " + pname + "::set( " + ptype + " value )");
                    _codeBuilder.BeginBlock();
                    string param = AddParameterConversion(new ParamDefinition(this.MetaDef, field, "value"));
                    _codeBuilder.AppendLine(GetNativeInvokationTarget(field) + " = " + param + ";");
                    _codeBuilder.EndBlock();
                }
            }
        }

        protected override void GenerateCodeMethodsForField(MemberFieldDefinition field)
        {
            string managedType = field.MemberType.ProduceNativeCallConversionCode(GetNativeInvokationTarget(field), field);

            _codeBuilder.AppendLine(GetCLRTypeName(field) + " " + GetClassName() + "::get_" + field.NativeName + "()");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("return " + managedType + ";");
            _codeBuilder.EndBlock();

            ParamDefinition param = new ParamDefinition(this.MetaDef, field, "value");
            _codeBuilder.AppendLine("void " + GetClassName() + "::set_" + field.NativeName + "(" + param.Type.GetCLRParamTypeName(param) + " value)");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine(GetNativeInvokationTarget(field) + " = " + AddParameterConversion(param) + ";");
            _codeBuilder.EndBlock();
        }

        protected override void GenerateCodePredefinedMethods(PredefinedMethods pm)
        {
            string cls = GetClassName();
            switch (pm)
            {
                case PredefinedMethods.Equals:
                    _codeBuilder.AppendLine("bool " + cls + "::Equals(Object^ obj)");
                    _codeBuilder.BeginBlock();
                    _codeBuilder.AppendLine(cls + "^ clr = dynamic_cast<" + cls + "^>(obj);");
                    _codeBuilder.AppendLine("if (clr == CLR_NULL)");
                    _codeBuilder.BeginBlock();
                    _codeBuilder.AppendLine("return false;");
                    _codeBuilder.EndBlock();
                    _codeBuilder.AppendEmptyLine();
                    _codeBuilder.AppendLine("if (_native == NULL) throw gcnew Exception(\"The underlying native object for the caller is null.\");");
                    _codeBuilder.AppendLine("if (clr->_native == NULL) throw gcnew ArgumentException(\"The underlying native object for parameter 'obj' is null.\");");
                    _codeBuilder.AppendEmptyLine();
                    _codeBuilder.AppendLine("return " + GetNativeInvokationTargetObject() + " == *(static_cast<" + _classDefinition.FullyQualifiedNativeName + "*>(clr->_native));");
                    _codeBuilder.EndBlock();
                    _codeBuilder.AppendEmptyLine();

                    if (!_classDefinition.HasWrapType(WrapTypes.NativePtrValueType))
                    {
                        _codeBuilder.AppendLine("bool " + cls + "::Equals(" + cls + "^ obj)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("if (obj == CLR_NULL)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("return false;");
                        _codeBuilder.EndBlock();
                        _codeBuilder.AppendEmptyLine();
                        _codeBuilder.AppendLine("if (_native == NULL) throw gcnew Exception(\"The underlying native object for the caller is null.\");");
                        _codeBuilder.AppendLine("if (obj->_native == NULL) throw gcnew ArgumentException(\"The underlying native object for parameter 'obj' is null.\");");
                        _codeBuilder.AppendEmptyLine();
                        _codeBuilder.AppendLine("return " + GetNativeInvokationTargetObject() + " == *(static_cast<" + this.MetaDef.NativeNamespace + "::" + cls + "*>(obj->_native));");
                        _codeBuilder.EndBlock();

                        _codeBuilder.AppendEmptyLine();
                        _codeBuilder.AppendLine("bool " + cls + "::operator ==(" + cls + "^ obj1, " + cls + "^ obj2)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("if ((Object^)obj1 == (Object^)obj2) return true;");
                        _codeBuilder.AppendLine("if ((Object^)obj1 == nullptr || (Object^)obj2 == nullptr) return false;");
                        _codeBuilder.AppendEmptyLine();
                        _codeBuilder.AppendLine("return obj1->Equals(obj2);");
                        _codeBuilder.EndBlock();

                        _codeBuilder.AppendEmptyLine();
                        _codeBuilder.AppendLine("bool " + cls + "::operator !=(" + cls + "^ obj1, " + cls + "^ obj2)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("return !(obj1 == obj2);");
                        _codeBuilder.EndBlock();
                    }
                    else
                    {
                        _codeBuilder.AppendLine("bool " + cls + "::Equals(" + cls + " obj)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("if (_native == NULL) throw gcnew Exception(\"The underlying native object for the caller is null.\");");
                        _codeBuilder.AppendLine("if (obj._native == NULL) throw gcnew ArgumentException(\"The underlying native object for parameter 'obj' is null.\");");
                        _codeBuilder.AppendEmptyLine();
                        _codeBuilder.AppendLine("return *_native == *obj._native;");
                        _codeBuilder.EndBlock();

                        _codeBuilder.AppendEmptyLine();
                        _codeBuilder.AppendLine("bool " + cls + "::operator ==(" + cls + " obj1, " + cls + " obj2)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("return obj1.Equals(obj2);");
                        _codeBuilder.EndBlock();

                        _codeBuilder.AppendEmptyLine();
                        _codeBuilder.AppendLine("bool " + cls + "::operator !=(" + cls + " obj1, " + cls + " obj2)");
                        _codeBuilder.BeginBlock();
                        _codeBuilder.AppendLine("return !obj1.Equals(obj2);");
                        _codeBuilder.EndBlock();
                    }
                    break;
            }
        }
    }
}
