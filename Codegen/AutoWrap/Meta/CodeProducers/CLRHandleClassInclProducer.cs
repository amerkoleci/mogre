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
    class CLRHandleClassInclProducer : PlainWrapperClassInclProducer
    {
        protected override string GetTopBaseClassName()
        {
            return "INativePointer";
        }

        protected override bool DoCleanupInFinalizer
        {
            get { return _classDefinition.HasAttribute<DoCleanupInFinalizerAttribute>(); }
        }

        protected override void GenerateCodePrivateDeclarations()
        {
            base.GenerateCodePrivateDeclarations();
            _codeBuilder.AppendEmptyLine();
            _codeBuilder.AppendLine("virtual void ClearNativePtr() = INativePointer::ClearNativePtr");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("_native = 0;");
            _codeBuilder.EndBlock();
        }

        protected override void AddManagedNativeConversionsDefinition()
        {
            string name = GetClassName();
            _codeBuilder.AppendLine("static operator {0}^ (const Ogre::{0}* ct)", name);
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("Ogre::{0}* t = const_cast<Ogre::{0}*>(ct);", name);
            _codeBuilder.AppendLine("if (t)");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("Object^ clr = t->_CLRHandle;");
            _codeBuilder.AppendLine("if (CLR_NULL == clr)");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("clr = gcnew {0}(t);", name);
            _codeBuilder.AppendLine("t->_CLRHandle = clr;");
            _codeBuilder.EndBlock();
            _codeBuilder.AppendLine("return static_cast<{0}^>(clr);", name);
            _codeBuilder.EndBlock();
            _codeBuilder.AppendLine("else");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("return nullptr;");
            _codeBuilder.EndBlock();
            _codeBuilder.EndBlock();

            _codeBuilder.AppendLine("static operator {0}^ (const Ogre::{0}& ct)", name);
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("Ogre::{0}* t = &const_cast<Ogre::{0}&>(ct);", name);
            _codeBuilder.AppendLine("Object^ clr = t->_CLRHandle;");
            _codeBuilder.AppendLine("if (CLR_NULL == clr)");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("clr = gcnew {0}(t);", name);
            _codeBuilder.AppendLine("t->_CLRHandle = clr;");
            _codeBuilder.EndBlock();
            _codeBuilder.AppendLine("return static_cast<{0}^>(clr);", name);
            _codeBuilder.EndBlock();

            _codeBuilder.AppendLine("inline static operator Ogre::{0}* ({0}^ t)", name);
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("return (t == CLR_NULL) ? 0 : t->_native;");
            _codeBuilder.EndBlock();

            _codeBuilder.AppendLine("inline static operator Ogre::{0}& ({0}^ t)", name);
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("return *t->_native;");
            _codeBuilder.EndBlock();
        }

        public CLRHandleClassInclProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }
    }
}
