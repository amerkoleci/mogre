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
    class NativeDirectorClassCppProducer : ClassCppProducer
    {
        public override bool IsNativeClass
        {
            get { return true; }
        }

        public NativeDirectorClassCppProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }

        protected override void AddPublicConstructor(MemberMethodDefinition f)
        {
        }

        protected override string GetClassName()
        {
            string full = _classDefinition.FullyQualifiedCLRName;
            int index = full.IndexOf("::");
            string name = full.Substring(index + 2);

            index = name.LastIndexOf("::");
            if (index == -1)
                return GetNativeDirectorName(_classDefinition);

            if (!_classDefinition.IsNested)
            {
                return name.Substring(0, index + 2) + GetNativeDirectorName(_classDefinition);
            }
            else
            {
                name = name.Substring(0, index);
                index = name.LastIndexOf("::");
                if (index == -1)
                    return GetNativeDirectorName(_classDefinition);
                else
                    return name.Substring(0, index + 2) + GetNativeDirectorName(_classDefinition);
            }
        }

        protected override void GenerateCodeMethod(MemberMethodDefinition f)
        {
            string def = f.Definition.Replace(f.ContainingClass.FullyQualifiedNativeName, GetClassName()) + "(";
            if (def.StartsWith("virtual "))
                def = def.Substring("virtual ".Length);
            _codeBuilder.Append(def);
            for (int i = 0; i < f.Parameters.Count; i++)
            {
                ParamDefinition param = f.Parameters[i];
                _codeBuilder.Append(" ");
                AddNativeMethodParam(param);
                if (i < f.Parameters.Count - 1) _codeBuilder.Append(",");
            }
            _codeBuilder.AppendLine(" )");
            _codeBuilder.BeginBlock();

            _codeBuilder.AppendLine("if (doCallFor" + f.CLRName + ")");
            _codeBuilder.BeginBlock();

            NativeProxyClassCppProducer.AddNativeProxyMethodBody(f, "_receiver", _codeBuilder);

            _codeBuilder.EndBlock();
            if (!f.HasReturnValue)
            {
                if (!f.HasAttribute<DefaultReturnValueAttribute>())
                    throw new Exception("Default return value not set.");

                _codeBuilder.AppendLine("else");
                _codeBuilder.AppendLine("\treturn " + f.GetAttribute<DefaultReturnValueAttribute>().Name + ";");
            }

            _codeBuilder.EndBlock();
        }

        protected virtual void AddNativeMethodParam(ParamDefinition param)
        {
            _codeBuilder.Append(param.MemberTypeNativeName + " " + param.Name);
        }
    }
}
