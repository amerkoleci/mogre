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
    class NativeProtectedTypesProxy : ClassCodeProducer
    {
        public static string GetProtectedTypesProxyName(AbstractTypeDefinition type)
        {
            string name = type.FullyQualifiedNativeName;
            name = name.Substring(name.IndexOf("::") + 2);
            name = name.Replace("::", "_");
            name = "Mogre::" + name + "_ProtectedTypesProxy";
            return name;
        }

        public NativeProtectedTypesProxy(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }

        public override void GenerateCode()
        {
            Init();

            if (HasProtectedTypes() || HasProtectedStaticFields())
            {
                string className = GetProtectedTypesProxyName(_classDefinition);
                className = className.Substring(className.IndexOf("::") + 2);
                _codeBuilder.AppendLine("class " + className + " : public " + _classDefinition.FullyQualifiedNativeName);
                _codeBuilder.AppendLine("{");
                _codeBuilder.AppendLine("public:");
                _codeBuilder.IncreaseIndent();

                className = _classDefinition.FullyQualifiedCLRName;
                className = className.Substring(className.IndexOf("::") + 2);

                if (_classDefinition.IsInterface)
                {
                    className = className.Replace(_classDefinition.CLRName, _classDefinition.Name);
                }

                className = this.MetaDef.ManagedNamespace + "::" + className;

                _codeBuilder.AppendLine("friend ref class " + className + ";");

                foreach (AbstractTypeDefinition nested in _classDefinition.NestedTypes)
                {
                    AbstractTypeDefinition type = nested.DetermineType<AbstractTypeDefinition>(nested.Name);

                    if (type.ProtectionLevel == ProtectionLevel.Protected
                        && type.IsSTLContainer && Wrapper.IsTypeWrappable(type))
                    {
                        GenerateCodeNestedType(type);
                    }
                }

                _codeBuilder.DecreaseIndent();
                _codeBuilder.AppendLine("};\n");
            }
        }

        protected virtual bool HasProtectedTypes()
        {
            foreach (AbstractTypeDefinition nested in _classDefinition.NestedTypes)
            {
                AbstractTypeDefinition type = nested.DetermineType<AbstractTypeDefinition>(nested.Name);

                if (type.ProtectionLevel == ProtectionLevel.Protected
                    && type.IsSTLContainer && Wrapper.IsTypeWrappable(type) )
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual bool HasProtectedStaticFields()
        {
            foreach (MemberFieldDefinition field in _classDefinition.Fields)
            {
                if (field.ProtectionLevel == ProtectionLevel.Protected
                    && field.IsStatic
                    && !field.IsIgnored)
                {
                    return true;
                }
            }

            return false;
        }

        protected override void GenerateCodeNestedType(AbstractTypeDefinition nested)
        {
            if (nested.IsSTLContainer)
            {
                _codeBuilder.AppendLine("typedef " + _classDefinition.FullyQualifiedNativeName + "::" + nested.Name + " " + nested.CLRName + ";");
            }
            else
                throw new Exception("Unexpected");
        }

    //    protected override void AddBody()
    //    {
    //        AddAllNestedTypes();

    //        foreach (DefField field in _t.ProtectedFields)
    //        {
    //            if (!field.HasAttribute<IgnoreAttribute>()
    //                && field.IsStatic)
    //            {
    //                AddStaticField(field);
    //                _sb.AppendLine();
    //                continue;
    //            }
    //        }

    //        foreach (DefFunction f in _t.ProtectedMethods)
    //        {
    //            if (f.IsDeclarableFunction && f.IsStatic)
    //            {
    //                AddMethod(f);
    //                _sb.AppendLine();
    //            }
    //        }
    //    }

    //    protected override void AddAllNestedTypes()
    //    {
    //        foreach (DefType nested in _t.NestedTypes)
    //        {
    //            DefType type = (nested.IsNested) ? nested.ParentClass.FindType<DefType>(nested.Name) : nested.NameSpace.FindType<DefType>(nested.Name);

    //            if (type.ProtectionType == ProtectionType.Protected
    //                && (type is DefEnum || (type.IsSTLContainer && _wrapper.TypeIsWrappable(type))))
    //            {
    //                type.ProtectionType = ProtectionType.Public;
    //                AddNestedType(type);
    //                type.ProtectionType = ProtectionType.Protected;
    //            }
    //        }
    //    }

    //    protected virtual void AddNativeMethodParams(DefFunction f)
    //    {
    //        for (int i = 0; i < f.Parameters.Count; i++)
    //        {
    //            DefParam param = f.Parameters[i];
    //            _sb.Append(" ");

    //            _sb.Append(param.NativeTypeName);
    //            _sb.Append(" " + param.Name);

    //            if (i < f.Parameters.Count - 1) _sb.Append(",");
    //        }
    //    }
    }

    class NativeProtectedStaticsProxy : ClassCodeProducer
    {
        public static string GetProtectedStaticsProxyName(AbstractTypeDefinition type)
        {
            string name = type.FullyQualifiedNativeName;
            name = name.Substring(name.IndexOf("::") + 2);
            name = name.Replace("::", "_");
            name = "Mogre::" + name + "_ProtectedStaticsProxy";
            return name;
        }

        public NativeProtectedStaticsProxy(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }

        public override void GenerateCode()
        {
            Init();

            if (HasProtectedStatics())
            {
                string className = GetProtectedStaticsProxyName(_classDefinition);
                className = className.Substring(className.IndexOf("::") + 2);
                _codeBuilder.AppendLine("class " + className + " : public " + _classDefinition.FullyQualifiedNativeName);
                _codeBuilder.AppendLine("{");
                _codeBuilder.AppendLine("public:");
                _codeBuilder.IncreaseIndent();

                className = _classDefinition.FullyQualifiedCLRName;
                className = className.Substring(className.IndexOf("::") + 2);

                if (_classDefinition.IsInterface)
                {
                    className = className.Replace(_classDefinition.CLRName, _classDefinition.Name);
                }

                className = this.MetaDef.ManagedNamespace + "::" + className;

                _codeBuilder.AppendLine("friend ref class " + className + ";");

                AddFriends(className, _classDefinition);

                foreach (ClassDefinition iface in _interfaces)
                {
                    if (iface == _classDefinition)
                        continue;

                    AddFriends(className, iface);
                }

                _codeBuilder.DecreaseIndent();
                _codeBuilder.AppendLine("};\n");
            }
        }

        protected virtual void AddFriends(string className, ClassDefinition type)
        {
            foreach (MemberFieldDefinition field in type.ProtectedFields)
            {
                if (!field.IsIgnored
                    && !(field.IsStatic && type != _classDefinition) )
                {
                    _codeBuilder.AppendLine("friend ref class " + className + "::" + field.NativeName + ";");
                }
            }

            foreach (MemberMethodDefinition func in type.Methods)
            {
                if (func.IsDeclarableFunction
                    && func.ProtectionLevel == ProtectionLevel.Protected
                    && !(func.IsStatic && type != _classDefinition)
                    && func.IsProperty
                    && !func.IsVirtual)
                {
                    _codeBuilder.AppendLine("friend ref class " + className + "::" + func.CLRName + ";");
                }
            }
        }

        protected virtual bool HasProtectedStatics()
        {
            if (HasProtectedStatics(_classDefinition))
                return true;

            foreach (ClassDefinition iface in _interfaces)
            {
                if (iface == _classDefinition)
                    continue;

                if (HasProtectedStatics(iface))
                    return true;
            }

            return false;
        }

        protected virtual bool HasProtectedStatics(ClassDefinition type)
        {
            foreach (MemberFieldDefinition field in type.ProtectedFields)
            {
                if (!field.IsIgnored
                    && !(field.IsStatic && type != _classDefinition))
                {
                    return true;
                }
            }

            foreach (MemberMethodDefinition func in type.Methods)
            {
                if (func.IsDeclarableFunction
                    && func.ProtectionLevel == ProtectionLevel.Protected
                    && !(func.IsStatic && type != _classDefinition)
                    && !func.IsVirtual)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

