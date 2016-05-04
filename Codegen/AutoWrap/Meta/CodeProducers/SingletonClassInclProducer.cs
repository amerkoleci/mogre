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
    class SingletonClassInclProducer : ClassInclProducer
    {
        protected override string GetTopBaseClassName()
        {
            return null;
        }

        protected override bool DoCleanupInFinalizer
        {
            get
            {
                // Normally object destruction would happen in the finalizer (!Root()) which runs in the garbage collection thread.
                // But Ogre Root must always be disposed in the main thread of the application so we need to have a Dispose() method.
                if (_classDefinition.FullyQualifiedNativeName == "Ogre::Root")
                    return false;

                return true;
            }
        }

        protected override bool IsPropertyAllowed(MemberPropertyDefinition p)
        {
            if (base.IsPropertyAllowed(p))
            {
                if (p.Name == "Singleton" || p.Name == "SingletonPtr")
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        protected override void AddDisposerBody()
        {
            _codeBuilder.AppendLine("_native = " + _classDefinition.FullyQualifiedNativeName + "::getSingletonPtr();");

            base.AddDisposerBody();

            _codeBuilder.AppendLine("if (_createdByCLR && _native) { delete _native; _native = 0; }");
            _codeBuilder.AppendLine("_singleton = nullptr;");
        }

        protected override void GenerateCodePrivateDeclarations()
        {
            base.GenerateCodePrivateDeclarations();
            _codeBuilder.AppendLine("static " + _classDefinition.CLRName + "^ _singleton;");

            if (_classDefinition.BaseClass == null)
            {
                _codeBuilder.AppendLine(_classDefinition.FullyQualifiedNativeName + "* _native;");
                _codeBuilder.AppendLine("bool _createdByCLR;");
            }
        }

        protected override void AddInternalConstructors()
        {
            base.AddInternalConstructors();

            if (_classDefinition.BaseClass == null)
                _codeBuilder.AppendLine(_classDefinition.CLRName + "( " + _classDefinition.FullyQualifiedNativeName + "* obj ) : _native(obj)");
            else
                _codeBuilder.AppendLine(_classDefinition.CLRName + "( " + _classDefinition.FullyQualifiedNativeName + "* obj ) : " + _classDefinition.BaseClass.CLRName + "(obj)");

            _codeBuilder.BeginBlock();
            base.GenerateCodeConstructorBody();
            _codeBuilder.EndBlock();
            _codeBuilder.AppendEmptyLine();
        }

        protected override void AddPublicFields()
        {
            _codeBuilder.AppendLine("/**");
            _codeBuilder.AppendLine("<summary>Gets the global instance of {0}.</summary>", _classDefinition.CLRName);
            _codeBuilder.AppendLine("*/");

            _codeBuilder.AppendLine("static property " + _classDefinition.CLRName + "^ Singleton");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine(_classDefinition.CLRName + "^ get()");
            _codeBuilder.BeginBlock();

            _codeBuilder.AppendLine(_classDefinition.FullyQualifiedNativeName + "* ptr = " + _classDefinition.FullyQualifiedNativeName + "::getSingletonPtr();");
            _codeBuilder.AppendLine("if (_singleton == CLR_NULL || _singleton->_native != ptr)");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("if (_singleton != CLR_NULL)");
            _codeBuilder.BeginBlock();
            _codeBuilder.AppendLine("_singleton->_native = 0;");
            _codeBuilder.AppendLine("_singleton = nullptr;");
            _codeBuilder.EndBlock();

            _codeBuilder.AppendLine("if ( ptr ) _singleton = gcnew " + _classDefinition.CLRName + "( ptr );");
            _codeBuilder.EndBlock();
            _codeBuilder.AppendLine("return _singleton;");
            _codeBuilder.EndBlock();
            _codeBuilder.EndBlock();

            base.AddPublicFields();
        }

        public SingletonClassInclProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }
    }
}
