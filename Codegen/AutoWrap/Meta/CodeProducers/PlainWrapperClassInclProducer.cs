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
    class PlainWrapperClassInclProducer : ClassInclProducer
    {
        protected override string GetTopBaseClassName()
        {
            return null;
        }

        protected override bool RequiresCleanUp
        {
            get { return _classDefinition.BaseClass == null; }
        }

        protected override bool DoCleanupInFinalizer
        {
            get { return !_classDefinition.HasAttribute<NoFinalizerAttribute>(); }
        }

        protected override void GenerateCodeInternalDeclarations()
        {
            base.GenerateCodeInternalDeclarations();

            if (_classDefinition.BaseClass == null)
            {
                _codeBuilder.AppendLine(_classDefinition.FullyQualifiedNativeName + "* _native;");
                _codeBuilder.AppendLine("bool _createdByCLR;");
                _codeBuilder.AppendEmptyLine();
            }
        }

        protected override void GenerateCodePublicDeclarations()
        {
            base.GenerateCodePublicDeclarations();
            AddManagedNativeConversionsDefinition();
        }

        protected virtual void AddManagedNativeConversionsDefinition()
        {
            if (_classDefinition.Name == _classDefinition.CLRName)
            {
                string name = GetClassName();
                _codeBuilder.AppendLine("inline static operator {0}^ (const Ogre::{0}* t)", name);
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("if (t)");
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return gcnew {0}(const_cast<Ogre::{0}*>(t));", name);
                _codeBuilder.EndBlock();
                _codeBuilder.AppendLine("else");
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return nullptr;");
                _codeBuilder.EndBlock();
                _codeBuilder.EndBlock();

                _codeBuilder.AppendLine("inline static operator {0}^ (const Ogre::{0}& t)", name);
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return gcnew {0}(&const_cast<Ogre::{0}&>(t));", name);
                _codeBuilder.EndBlock();

                _codeBuilder.AppendLine("inline static operator Ogre::{0}* ({0}^ t)", name);
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return (t == CLR_NULL) ? 0 : static_cast<Ogre::{0}*>(t->_native);", name);
                _codeBuilder.EndBlock();

                _codeBuilder.AppendLine("inline static operator Ogre::{0}& ({0}^ t)", name);
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return *static_cast<Ogre::{0}*>(t->_native);", name);
                _codeBuilder.EndBlock();
            }
            else
            {
                string clrName = _classDefinition.FullyQualifiedCLRName.Substring(_classDefinition.FullyQualifiedCLRName.IndexOf("::") + 2);
                string nativeName = _classDefinition.FullyQualifiedNativeName.Substring(_classDefinition.FullyQualifiedNativeName.IndexOf("::") + 2);

                _codeBuilder.AppendLine("inline static operator {0}^ (const Ogre::{1}* t)", clrName, nativeName);
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("if (t)");
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return gcnew {0}(const_cast<Ogre::{1}*>(t));", clrName, nativeName);
                _codeBuilder.EndBlock();
                _codeBuilder.AppendLine("else");
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return nullptr;");
                _codeBuilder.EndBlock();
                _codeBuilder.EndBlock();

                _codeBuilder.AppendLine("inline static operator {0}^ (const Ogre::{1}& t)", clrName, nativeName);
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return gcnew {0}(&const_cast<Ogre::{1}&>(t));", clrName, nativeName);
                _codeBuilder.EndBlock();

                _codeBuilder.AppendLine("inline static operator Ogre::{1}* ({0}^ t)", clrName, nativeName);
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return (t == CLR_NULL) ? 0 : static_cast<Ogre::{1}*>(t->_native);", clrName, nativeName);
                _codeBuilder.EndBlock();

                _codeBuilder.AppendLine("inline static operator Ogre::{1}& ({0}^ t)", clrName, nativeName);
                _codeBuilder.BeginBlock();
                _codeBuilder.AppendLine("return *static_cast<Ogre::{1}*>(t->_native);", clrName, nativeName);
                _codeBuilder.EndBlock();
            }
        }

        protected override void AddInternalConstructors()
        {
            base.AddInternalConstructors();

            if (_classDefinition.BaseClass == null)
            {
                _codeBuilder.AppendLine("{0}( " + _classDefinition.FullyQualifiedNativeName + "* obj ) : _native(obj), _createdByCLR(false)", _classDefinition.CLRName);
            }
            else
            {
                ClassDefinition topclass = GetTopClass(_classDefinition);
                _codeBuilder.AppendLine("{0}( " + topclass.FullyQualifiedNativeName + "* obj ) : " + topclass.CLRName + "(obj)", _classDefinition.CLRName);
            }
            _codeBuilder.BeginBlock();

            //NOTE: SuppressFinalize should not be called when the class is 'wrapped' by a SharedPtr class, (i.e DataStreamPtr -> DataStream)
            //so that the SharedPtr class gets a chance to clean up. Look for a way to have SuppressFinalize without this kind of problems.
            //_sb.AppendLine("System::GC::SuppressFinalize(this);");

            base.GenerateCodeConstructorBody();
            _codeBuilder.EndBlock();
            _codeBuilder.AppendEmptyLine();
        }

        protected override void AddDisposerBody()
        {
            base.AddDisposerBody();
            _codeBuilder.AppendLine("if (_createdByCLR &&_native)");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("delete _native;");
            _codeBuilder.AppendLine("_native = 0;");
            _codeBuilder.EndBlock();
        }

        public PlainWrapperClassInclProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }
    }
}
