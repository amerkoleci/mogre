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
    class InterfaceClassInclProducer : ClassInclProducer
    {
        protected override string GetTopBaseClassName()
        {
            return null;
        }

        protected override bool AllowMethodOverloads
        {
            get
            {
                return false;
            }
        }

        protected override void AddPreDeclarations()
        {
            if (!_classDefinition.IsNested)
            {
                _wrapper.AddPreDeclaration("interface class " + _classDefinition.CLRName + ";");
                _wrapper.AddPragmaMakePublicForType(_classDefinition);
            }
        }

        public InterfaceClassInclProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }

        protected override void GenerateCodePublicDeclarations()
        {
            _codeBuilder.AppendLine("static operator {0}^ (const {1}* t)", _classDefinition.CLRName, _classDefinition.FullyQualifiedNativeName);
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("if (0 == (const_cast<{0}*>(t))) return nullptr;", _classDefinition.FullyQualifiedNativeName);
            _codeBuilder.AppendLine("CLRObject* clr_obj = dynamic_cast<CLRObject*>((const_cast<{0}*>(t)));", _classDefinition.FullyQualifiedNativeName);
            _codeBuilder.AppendLine("if (0 == clr_obj)");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("throw gcnew System::Exception(\"The native class that implements {0} isn't a subclass of CLRObject. Cannot create the CLR wrapper object.\");", _classDefinition.FullyQualifiedNativeName);
            _codeBuilder.EndBlock();
            _codeBuilder.AppendLine("Object^ clr = *clr_obj;");
            _codeBuilder.AppendLine("if (nullptr == clr)");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("clr_obj->_Init_CLRObject();");
            _codeBuilder.AppendLine("clr = *clr_obj;");
            _codeBuilder.EndBlock();
            _codeBuilder.AppendLine("return static_cast<{0}^>(clr);", _classDefinition.CLRName);
            _codeBuilder.EndBlock();

            _codeBuilder.AppendLine("inline static operator {1}* ({0}^ t)", _classDefinition.CLRName, _classDefinition.FullyQualifiedNativeName);
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("return (t == CLR_NULL) ? 0 : t->_GetNativePtr();");
            _codeBuilder.EndBlock();

            _codeBuilder.AppendLine("virtual " + _classDefinition.FullyQualifiedNativeName + "* _GetNativePtr();\n");
            base.GenerateCodePublicDeclarations();
        }

        protected override void AddPublicConstructors()
        {
        }

        protected override void GenerateCodePrivateDeclarations()
        {
        }

        protected override void GenerateCodeInternalDeclarations()
        {
        }

        protected override void AddDefinition()
        {
            if (!_classDefinition.IsNested)
                _codeBuilder.Append("public ");
            else
                _codeBuilder.Append(_classDefinition.ProtectionLevel.GetCLRProtectionName() + ": ");
            _codeBuilder.AppendLine("interface class " + _classDefinition.CLRName);
        }

        protected override void GenerateCodeProtectedDeclarations()
        {
        }

        protected override void GenerateCodeMethod(MemberMethodDefinition f)
        {
            if (f.IsVirtual)
                base.GenerateCodeMethod(f);
        }

        protected override void GenerateCodeProperty(MemberPropertyDefinition p)
        {
            if (p.IsVirtual)
                base.GenerateCodeProperty(p);
        }
    }
}
