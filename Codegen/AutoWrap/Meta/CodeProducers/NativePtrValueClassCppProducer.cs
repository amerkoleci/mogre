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
    class NativePtrValueClassCppProducer : ClassCppProducer
    {
        protected override string GetNativeInvokationTarget(bool isConst)
        {
            return "_native";
        }

        protected override string GetNativeInvokationTargetObject()
        {
            return "*_native";
        }

        protected override void GenerateCodePublicDeclarations()
        {
            base.GenerateCodePublicDeclarations();

            if (!IsReadOnly)
            {
                _codeBuilder.AppendEmptyLine();
                AddCreators();
            }
        }

        protected override void AddPublicConstructor(MemberMethodDefinition f)
        {
        }

        protected virtual void AddCreators()
        {
            if (!_classDefinition.IsNativeAbstractClass)
            {
                if (_classDefinition.Constructors.Length > 0)
                {
                    foreach (MemberMethodDefinition func in _classDefinition.Constructors)
                        if (func.ProtectionLevel == ProtectionLevel.Public)
                            AddCreator(func);
                }
                else
                    AddCreator(null);

                _codeBuilder.AppendEmptyLine();
            }
        }

        protected virtual void AddCreator(MemberMethodDefinition f)
        {
            if (f == null)
                AddCreatorOverload(f, 0);
            else
            {
                int defcount = 0;

                if (!f.HasAttribute<NoDefaultParamOverloadsAttribute>())
                {
                    foreach (ParamDefinition param in f.Parameters)
                        if (param.DefaultValue != null)
                            defcount++;
                }

                // The overloads (because of default values)
                for (int dc = 0; dc <= defcount; dc++)
                {
                    if (dc < defcount && f.HasAttribute<HideParamsWithDefaultValuesAttribute>())
                        continue;

                    AddCreatorOverload(f, f.Parameters.Count - dc);
                }

            }
        }

        protected virtual void AddCreatorOverload(MemberMethodDefinition f, int count)
        {
            _codeBuilder.Append(_classDefinition.FullyQualifiedCLRName + " " + GetClassName() + "::Create");
            if (f == null)
                _codeBuilder.Append("()");
            else
                AddMethodParameters(f, count);

            _codeBuilder.AppendEmptyLine();
            _codeBuilder.BeginBlock();

            string preCall = null, postCall = null;

            if (f != null)
            {
                preCall = GetMethodPreNativeCall(f, count);
                postCall = GetMethodPostNativeCall(f, count);

                if (!String.IsNullOrEmpty(preCall))
                    _codeBuilder.AppendLine(preCall);
            }

            _codeBuilder.AppendLine(_classDefinition.CLRName + " ptr;");
            _codeBuilder.Append("ptr._native = new " + _classDefinition.FullyQualifiedNativeName + "(");

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

            _codeBuilder.AppendLine("return ptr;");

            _codeBuilder.EndBlock();
        }

        public NativePtrValueClassCppProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }
    }
}
