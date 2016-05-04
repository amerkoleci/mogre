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
    class NativeDirectorClassInclProducer : ClassInclProducer
    {
        public override bool IsNativeClass
        {
            get { return true; }
        }

        protected override string GetTopBaseClassName()
        {
            return null;
        }

        protected override bool AllowMethodOverloads
        {
            get { return false; }
        }

        public NativeDirectorClassInclProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }

        protected override void GenerateCodeInternalDeclarations()
        {
        }

        protected virtual string ReceiverInterfaceName
        {
            get
            {
                return GetNativeDirectorReceiverInterfaceName(_classDefinition);
            }
        }

        protected virtual string DirectorName
        {
            get
            {
                return GetNativeDirectorName(_classDefinition);
            }
        }

        protected override void GenerateCodePreBody()
        {
            _codeBuilder.AppendLine("interface class " + ReceiverInterfaceName + "\n{");
            _codeBuilder.IncreaseIndent();
            foreach (MemberMethodDefinition f in _classDefinition.PublicMethods)
            {
                if (f.IsDeclarableFunction && f.IsVirtual)
                {
                    base.GenerateCodeMethod(f);
                    _codeBuilder.AppendEmptyLine();
                }
            }
            _codeBuilder.DecreaseIndent();
            _codeBuilder.AppendLine("};\n");

            if (!_classDefinition.IsNested)
            {
                AddMethodHandlersClass(_classDefinition, _codeBuilder);
            }

            base.GenerateCodePreBody();
        }

        public static void AddMethodHandlersClass(ClassDefinition type, SourceCodeStringBuilder sb)
        {
            if (!type.HasWrapType(WrapTypes.NativeDirector))
                throw new Exception("Unexpected");

            if (type.IsNested)
                sb.Append("public: ");
            else
                sb.Append("public ");

            sb.AppendLine("ref class " + type.Name + " abstract sealed");
            sb.AppendLine("{");
            sb.AppendLine("public:");
            sb.IncreaseIndent();

            foreach (MemberMethodDefinition f in type.PublicMethods)
            {
                if (f.IsDeclarableFunction && f.IsVirtual)
                {
                    //if (f.Parameters.Count > 0)
                    //{
                    //    AddEventArgsClass(f, sb);
                    //}

                    sb.Append("delegate static " + f.MemberTypeCLRName + " " + f.CLRName + "Handler(");
                    for (int i = 0; i < f.Parameters.Count; i++)
                    {
                        ParamDefinition param = f.Parameters[i];
                        sb.Append(" " + param.Type.GetCLRParamTypeName(param) + " " + param.Name);
                        if (i < f.Parameters.Count - 1) sb.Append(",");
                    }
                    sb.AppendLine(" );");
                }
            }

            sb.DecreaseIndent();
            sb.AppendLine("};");
            sb.AppendEmptyLine();
        }

        //protected static void AddEventArgsClass(DefFunction func, IndentStringBuilder sb)
        //{
        //    string className = func.CLRName + "EventArgs";
        //    sb.AppendLine("ref class " + className + " : EventArgs");
        //    sb.AppendLine("{");
        //    sb.AppendLine("public:");
        //    sb.IncreaseIndent();

        //    foreach (DefParam param in func.Parameters)
        //        sb.AppendLine(param.Type.GetCLRParamTypeName(param) + " " + param.Name + ";");

        //    sb.AppendLine();
        //    sb.AppendIndent(className + "(");
        //    for (int i = 0; i < func.Parameters.Count; i++)
        //    {
        //        DefParam param = func.Parameters[i];
        //        sb.Append(" " + param.Type.GetCLRParamTypeName(param) + " " + param.Name);
        //        if (i < func.Parameters.Count - 1) sb.Append(",");
        //    }
        //    sb.Append(" )\n");

        //    sb.AppendLine("{");
        //    sb.IncreaseIndent();
        //    foreach (DefParam param in func.Parameters)
        //        sb.AppendLine("this->" + param.Name + ";");

        //}

        protected override void GenerateCodePrivateDeclarations()
        {
            base.GenerateCodePrivateDeclarations();
            _codeBuilder.AppendLine("gcroot<" + ReceiverInterfaceName + "^> _receiver;");
        }

        protected override void AddPublicConstructors()
        {
            _codeBuilder.AppendLine(DirectorName + "( " + ReceiverInterfaceName + "^ recv )");
            _codeBuilder.Append("\t: _receiver(recv)");
            foreach (MemberMethodDefinition f in _classDefinition.PublicMethods)
            {
                if (f.IsDeclarableFunction && f.IsVirtual)
                {
                    _codeBuilder.Append(", doCallFor" + f.CLRName + "(false)");
                }
            }
            _codeBuilder.AppendEmptyLine();
            _codeBuilder.AppendLine("{");
            _codeBuilder.AppendLine("}");
        }

        protected override void AddPublicFields()
        {
            base.AddPublicFields();
            foreach (MemberMethodDefinition f in _classDefinition.PublicMethods)
            {
                if (f.IsDeclarableFunction && f.IsVirtual)
                {
                    _codeBuilder.AppendLine("bool doCallFor" + f.CLRName + ";");
                }
            }
        }

        protected override void GenerateCodeMethod(MemberMethodDefinition f)
        {
            _codeBuilder.Append(f.Definition.Replace(f.ContainingClass.FullyQualifiedNativeName + "::", "") + "(");
            for (int i = 0; i < f.Parameters.Count; i++)
            {
                ParamDefinition param = f.Parameters[i];
                _codeBuilder.Append(" ");
                AddNativeMethodParam(param);
                if (i < f.Parameters.Count - 1) _codeBuilder.Append(",");
            }
            _codeBuilder.AppendLine(" ) override;");
        }

        protected virtual void AddNativeMethodParam(ParamDefinition param)
        {
            _codeBuilder.Append(param.MemberTypeNativeName + " " + param.Name);
        }

        protected override void AddDefinition()
        {
            _codeBuilder.AppendLine("class " + DirectorName + " : public " + _classDefinition.FullyQualifiedNativeName);
        }

        protected override void GenerateCodeProtectedDeclarations()
        {
        }
    }
}
