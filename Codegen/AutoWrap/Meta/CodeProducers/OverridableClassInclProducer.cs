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
    class NativeProxyClassInclProducer : NativeProxyClassProducer
    {
        public NativeProxyClassInclProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }

        protected virtual void AddDefinition()
        {
            _codeBuilder.Append("class " + ProxyName + " : public " + _classDefinition.FullyQualifiedNativeName);
            if (_classDefinition.IsInterface)
                _codeBuilder.Append(", public CLRObject");
            _codeBuilder.AppendEmptyLine();
        }

        protected override void GenerateCodePreBody()
        {
            base.GenerateCodePreBody();

            AddDefinition();

            _codeBuilder.AppendLine("{");
            _codeBuilder.AppendLine("public:");
            _codeBuilder.IncreaseIndent();
        }

        protected override void GenerateCodePostBody()
        {
            base.GenerateCodePostBody();

            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine("};\n");
        }

        protected override void AddFields()
        {
            string className;
            if (_classDefinition.IsNested)
            {
                className = _classDefinition.SurroundingClass.FullyQualifiedCLRName + "::" + _classDefinition.Name;
            }
            else
            {
                className = this.MetaDef.ManagedNamespace + "::" + _classDefinition.Name;
            }

            _codeBuilder.AppendLine("friend ref class " + className + ";");

            //Because of a possible compiler bug, in order for properties to access
            //protected members of a native class, they must be explicitely declared
            //with 'friend' specifier
            foreach (MemberPropertyDefinition prop in _overridableProperties)
            {
                if ((prop.CanRead && prop.GetterFunction.ProtectionLevel == ProtectionLevel.Protected)
                    || (prop.CanWrite && prop.SetterFunction.ProtectionLevel == ProtectionLevel.Protected))
                    _codeBuilder.AppendLine("friend ref class " + className + "::" + prop.Name + ";");
            }

            if (_classDefinition.IsInterface)
            {
                foreach (MemberFieldDefinition field in _classDefinition.Fields)
                {
                    if (!field.IsIgnored
                        && field.ProtectionLevel == ProtectionLevel.Protected
                        && !field.IsStatic)
                        _codeBuilder.AppendLine("friend ref class " + className + "::" + field.NativeName + ";");
                }
            }

            _codeBuilder.AppendEmptyLine();
            _codeBuilder.AppendLine("bool* _overriden;");

            _codeBuilder.AppendEmptyLine();
            _codeBuilder.AppendLine("gcroot<" + className + "^> _managed;");

            //_sb.AppendLine();
            //foreach (DefField field in _protectedFields)
            //{
            //    _sb.AppendLine(field.NativeTypeName + "& ref_" + field.Name + ";");
            //}

            _codeBuilder.AppendEmptyLine();
            _codeBuilder.AppendLine("virtual void _Init_CLRObject() override { *static_cast<CLRObject*>(this) = _managed; }");
        }

        protected override void AddConstructor(MemberMethodDefinition f)
        {
            string className;
            if (_classDefinition.IsNested)
            {
                className = _classDefinition.SurroundingClass.FullyQualifiedCLRName + "::" + _classDefinition.Name;
            }
            else
            {
                className = this.MetaDef.ManagedNamespace + "::" + _classDefinition.Name;
            }
            _codeBuilder.Append(ProxyName + "( " + className + "^ managedObj");
            if (f != null)
            {
                foreach (ParamDefinition param in f.Parameters)
                    _codeBuilder.Append(", " + param.MemberTypeNativeName + " " + param.Name);
            }

            _codeBuilder.AppendLine(" ) :");
            _codeBuilder.IncreaseIndent();

            if (f != null)
            {
                _codeBuilder.Append(_classDefinition.FullyQualifiedNativeName + "(");
                for (int i = 0; i < f.Parameters.Count; i++)
                {
                    ParamDefinition param = f.Parameters[i];
                    _codeBuilder.Append(" " + param.Name);
                    if (i < f.Parameters.Count - 1)
                        _codeBuilder.Append(",");
                }
                _codeBuilder.AppendLine(" ),");
            }

            _codeBuilder.Append("_managed(managedObj)");

            //foreach (DefField field in _protectedFields)
            //{
            //    _sb.Append(",\n");
            //    _sb.AppendIndent("\tref_" + field.Name + "(" + field.Name + ")");
            //}
            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendEmptyLine();
            _codeBuilder.AppendLine("{");
            _codeBuilder.AppendLine("}");
        }

        protected override void AddOverridableFunction(MemberMethodDefinition f)
        {
            if (f.IsVirtual)
                _codeBuilder.Append("virtual ");
            _codeBuilder.Append(f.MemberTypeNativeName + " " + f.NativeName + "(");
            AddNativeMethodParams(f);
            _codeBuilder.Append(" ) ");
            if (f.IsConstMethod)
                _codeBuilder.Append("const ");
            _codeBuilder.AppendLine("override;");
        }

        //protected override void AddProtectedFunction(DefFunction f)
        //{
        //    _sb.AppendIndent("");
        //    _sb.Append(f.NativeTypeName + " base_" + f.Name + "(");
        //    AddNativeMethodParams(f);
        //    _sb.Append(" ) ");
        //    if (f.IsConstFunctionCall)
        //        _sb.Append("const ");
        //    _sb.Append(";\n");
        //}
    }

    class IncOverridableClassProducer : NonOverridableClassInclProducer
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
                _codeBuilder = tempsb;
                _codeBuilder.AppendLine(res);
            }
            else
                base.GenerateCode();
        }

        protected override void AddPreDeclarations()
        {
            if (!_classDefinition.IsNested)
            {
                _wrapper.AddPreDeclaration("ref class " + _classDefinition.Name + ";");
                _wrapper.AddPragmaMakePublicForType(_classDefinition);

            }
        }

        protected override void AddDefinition()
        {
            if (_classDefinition.IsInterface)
            {
                //put _t.Name instead of _t.CLRName
                if (!_classDefinition.IsNested)
                    _codeBuilder.Append("public ");
                else
                    _codeBuilder.Append(_classDefinition.ProtectionLevel.GetCLRProtectionName() + ": ");
                string baseclass = GetBaseAndInterfaces();
                if (baseclass != "")
                    _codeBuilder.AppendLine("ref class {0}{1} : {2}", _classDefinition.Name, (IsAbstractClass) ? " abstract" : "", baseclass);
                else
                    _codeBuilder.AppendLine("ref class {0}{1}", _classDefinition.Name, (IsAbstractClass) ? " abstract" : "");
            }
            else
                base.AddDefinition();
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

        protected override bool AllowMethodIndexAttributes
        {
            get
            {
                return true;
            }
        }

        public IncOverridableClassProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
            _wrapper.AddPostClassProducer(new NativeProxyClassInclProducer(metaDef, _wrapper, _classDefinition, _codeBuilder));
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
        //    string name = (f.ProtectionType == ProtectionType.Public) ? f.Name : "base_" + f.Name;
        //    return "static_cast<" + ProxyName + "*>(_native)->" + name;
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

        protected override void AddManagedNativeConversionsDefinition()
        {
        }

        protected override void AddInternalConstructors()
        {
            // Allow the internal constructor for interfaces too, so that they can be wrapped by a SharedPtr class
            //if (!_t.IsInterface)
                base.AddInternalConstructors();
        }

        protected override void AddDefaultImplementationClass()
        {
        }
    }

    class IncSubclassingClassProducer : IncOverridableClassProducer
    {
        protected ClassDefinition[] _additionalInterfaces;

        public IncSubclassingClassProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb, ClassDefinition[] additionalInterfaces)
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

        protected override bool RequiresCleanUp
        {
            get
            {
                return false;
            }
        }

        protected override bool AllowMethodOverloads
        {
            get
            {
                return false;
            }
        }

        protected override bool DeclareAsOverride(MemberMethodDefinition f)
        {
            if (f.ProtectionLevel == ProtectionLevel.Public)
                return true;
            else
                return false;
        }

        protected override void AddPreDeclarations()
        {
            if (!_classDefinition.IsNested)
                _wrapper.AddPragmaMakePublicForType(_classDefinition);
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

        protected override string GetBaseAndInterfaces()
        {
            return _classDefinition.FullyQualifiedCLRName;
        }

        protected override void GenerateCodePrivateDeclarations()
        {
            //_sb.DecreaseIndent();
            //_sb.AppendLine("private:");
            //_sb.IncreaseIndent();
        }

        protected override void GenerateCodePublicDeclarations()
        {
            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine("public:");
            _codeBuilder.IncreaseIndent();

            AddPublicConstructors();

            _codeBuilder.AppendEmptyLine();
            foreach (MemberPropertyDefinition prop in _overridableProperties)
            {
                if (!prop.IsAbstract)
                {
                    GenerateCodeProperty(prop);
                    _codeBuilder.AppendEmptyLine();
                }
            }

            foreach (MemberMethodDefinition func in _overridableFunctions)
            {
                if (!func.IsProperty && func.ProtectionLevel == ProtectionLevel.Public
                    && !func.IsAbstract)
                {
                    GenerateCodeMethod(func);
                    _codeBuilder.AppendEmptyLine();
                }
            }
        }

        protected override void GenerateCodeProtectedDeclarations()
        {
            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine("protected public:");
            _codeBuilder.IncreaseIndent();

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

