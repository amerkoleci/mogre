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
    class OverridableClassCppProducer : NonOverridableClassCppProducer
    {
        public override void GenerateCode()
        {
            if (_classDefinition.IsInterface)
            {
                SourceCodeStringBuilder tempsb = _codeBuilder;
                _codeBuilder = new SourceCodeStringBuilder(this.MetaDef.CodeStyleDef);
                base.GenerateCode();
                string fname = _classDefinition.FullyQualifiedCLRName.Replace(_classDefinition.CLRName, _classDefinition.Name);
                string res = _codeBuilder.ToString().Replace(_classDefinition.FullyQualifiedCLRName + "::", fname + "::");
                fname = GetClassName().Replace(_classDefinition.CLRName, _classDefinition.Name);
                res = res.Replace(GetClassName() + "::", fname + "::");

                _codeBuilder = tempsb;
                _codeBuilder.AppendLine(res);
            }
            else
                base.GenerateCode();
        }

        protected override bool AllowProtectedMembers
        {
            get
            {
                return true;
            }
        }

        protected override bool AllowSubclassing
        {
            get
            {
                return true;
            }
        }

        public OverridableClassCppProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
            _wrapper.AddPostClassProducer(new NativeProxyClassCppProducer(metaDef, _wrapper, _classDefinition, _codeBuilder));
        }

        private string _proxyName;
        protected virtual string ProxyName
        {
            get
            {
                if (_proxyName == null)
                    _proxyName = NativeProxyClassProducer.GetProxyName(_classDefinition);

                return _proxyName;
            }
        }

        public override string ClassFullNativeName
        {
            get
            {
                return ProxyName;
            }
        }

        protected override string GetNativeInvokationTarget(MemberMethodDefinition f)
        {
            return "static_cast<" + ProxyName + "*>(_native)->" + f.ContainingClass.Name + "::" + f.NativeName;
        }

        protected override string GetNativeInvokationTarget(MemberFieldDefinition field)
        {
            return "static_cast<" + ProxyName + "*>(_native)->" + _classDefinition.FullyQualifiedNativeName + "::" + field.NativeName;
        }

        //protected override string GetNativeInvokationTarget(DefFunction f)
        //{
        //    if (f.ProtectionType == ProtectionType.Public)
        //        return "static_cast<" + ProxyName + "*>(_native)->" + _t.FullNativeName + "::" + f.Name;
        //    else
        //        return "static_cast<" + ProxyName + "*>(_native)->" + "base_" + f.Name;
        //}
        //protected override string GetNativeInvokationTarget(DefField field)
        //{
        //    string name = (field.ProtectionType == ProtectionType.Public) ? field.Name : "ref_" + field.Name;
        //    return "static_cast<" + ProxyName + "*>(_native)->" + name;
        //}
        //protected override string GetNativeInvokationTarget(bool isConst)
        //{
        //    string ret = "static_cast<";
        //    if (isConst)
        //        ret += "const ";
        //    return ret + ProxyName + "*>(_native)";
        //}

        protected override void AddDefaultImplementationClass()
        {
        }

        protected override void AddPublicConstructor(MemberMethodDefinition f)
        {
            _codeBuilder.Append(GetClassName() + "::" + _classDefinition.Name);
            if (f == null)
                _codeBuilder.Append("()");
            else
                AddMethodParameters(f);
            _codeBuilder.Append(" : " + GetBaseClassName() + "( (CLRObject*)0 )");
            _codeBuilder.AppendEmptyLine();
            _codeBuilder.BeginBlock();

            _codeBuilder.AppendLine("_createdByCLR = true;");
            _codeBuilder.AppendLine("Type^ thisType = this->GetType();");

            if (!IsAbstractClass && !_classDefinition.IsInterface)
                _codeBuilder.AppendLine("_isOverriden = (thisType != " + _classDefinition.CLRName + "::typeid);");
            else
                _codeBuilder.AppendLine("_isOverriden = true;  //it's abstract or interface so it must be overriden");

            int count = 0;
            string preCall = null, postCall = null;

            if (f != null)
            {
                count = f.Parameters.Count;
                preCall = GetMethodPreNativeCall(f, count);
                postCall = GetMethodPostNativeCall(f, count);

                if (!String.IsNullOrEmpty(preCall))
                    _codeBuilder.AppendLine(preCall);
            }

            if (!IsAbstractClass && !_classDefinition.IsInterface)
            {
                _codeBuilder.AppendLine("if (_isOverriden)");
                _codeBuilder.BeginBlock();
            }

            string proxyName = NativeProxyClassProducer.GetProxyName(_classDefinition);
            _codeBuilder.Append(proxyName + "* proxy = new " + proxyName + "(this");

            if (count > 0)
            {
                _codeBuilder.Append(",");
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

            _codeBuilder.AppendLine("proxy->_overriden = Implementation::SubclassingManager::Instance->GetOverridenMethodsArrayPointer(thisType, " + _classDefinition.Name + "::typeid, " + _methodIndicesCount + ");");
            _codeBuilder.AppendLine("_native = proxy;");

            if (!IsAbstractClass && !_classDefinition.IsInterface)
            {
                _codeBuilder.EndBlock();
                _codeBuilder.AppendLine("else");
                _codeBuilder.Append("\t_native = new " + _classDefinition.FullyQualifiedNativeName + "(");

                if (count > 0)
                {
                    _codeBuilder.Append(",");
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
            }

            if (!String.IsNullOrEmpty(postCall))
            {
                _codeBuilder.AppendEmptyLine();
                _codeBuilder.AppendLine(postCall);
                _codeBuilder.AppendEmptyLine();
            }

            _codeBuilder.AppendEmptyLine();
            GenerateCodeConstructorBody();

            _codeBuilder.EndBlock();
        }
    }

    class CppSubclassingClassProducer : OverridableClassCppProducer
    {
        protected ClassDefinition[] _additionalInterfaces;

        public CppSubclassingClassProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb, ClassDefinition[] additionalInterfaces)
            : base(metaDef, wrapper, t, sb)
        {
            this._additionalInterfaces = additionalInterfaces;
        }

        protected override void Init()
        {
            if (_additionalInterfaces != null)
                _interfaces.AddRange(_additionalInterfaces);

            base.Init();
        }

        protected override bool AllowMethodOverloads
        {
            get
            {
                return false;
            }
        }

        protected override void GenerateCodeAllNestedTypes()
        {
        }

        protected override void GenerateCodePreNestedTypes()
        {
        }

        protected override void GenerateCodePostNestedTypes()
        {
        }

        protected override void GenerateCodePublicDeclarations()
        {
            if (_classDefinition.Constructors.Length > 0)
            {
                foreach (MemberMethodDefinition func in _classDefinition.Constructors)
                    AddPublicConstructor(func);
            }
            else
                AddPublicConstructor(null);

            _codeBuilder.AppendEmptyLine();
            foreach (MemberPropertyDefinition prop in _overridableProperties)
            {
                GenerateCodeProperty(prop);
                _codeBuilder.AppendEmptyLine();
            }

            foreach (MemberMethodDefinition func in _overridableFunctions)
            {
                if (!func.IsProperty && func.ProtectionLevel == ProtectionLevel.Public)
                {
                    GenerateCodeMethod(func);
                    _codeBuilder.AppendEmptyLine();
                }
            }
        }

        protected override void GenerateCodeProtectedDeclarations()
        {
            foreach (MemberMethodDefinition func in _overridableFunctions)
            {
                if (!func.IsProperty && func.ProtectionLevel == ProtectionLevel.Protected)
                {
                    GenerateCodeMethod(func);
                    _codeBuilder.AppendEmptyLine();
                }
            }
        }
    }
}
