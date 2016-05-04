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
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Base class for all classes that generate some kind of source code.
    /// </summary>
    public abstract class AbstractCodeProducer : MetaDefBasedClass
    {
        protected AbstractCodeProducer(MetaDefinition metaDef)
            : base(metaDef)
        {
        }

        protected virtual string GetNativeDirectorReceiverInterfaceName(ClassDefinition type)
        {
            if (!type.HasWrapType(WrapTypes.NativeDirector))
                throw new Exception("Unexpected");

            string name = (type.IsNested) ? type.SurroundingClass.Name + "_" + type.Name : type.Name;
            return "I" + name + "_Receiver";
        }

        protected virtual string GetNativeDirectorName(ClassDefinition type)
        {
            if (!type.HasWrapType(WrapTypes.NativeDirector))
                throw new Exception("Unexpected");

            string name = (type.IsNested) ? type.SurroundingClass.Name + "_" + type.Name : type.Name;
            return name + "_Director";
        }

        /// <summary>
        /// Checks whether the specified property can be added to the generated source code.
        /// </summary>
        protected virtual bool IsPropertyAllowed(MemberPropertyDefinition p)
        {
            // If the property is ignored or the property is unhandled
            if (p.IsIgnored() || !p.IsTypeHandled())
                return false;
            
            if (p.ContainingClass.IsSingleton && (p.Name == "Singleton" || p.Name == "SingletonPtr"))
                return false;
            
            return true;
        }

        /// <summary>
        /// Checks whether the specified function can be added to the generated source code.
        /// </summary>
        protected virtual bool IsFunctionAllowed(MemberMethodDefinition f)
        {
            // If the function is ignored or the return value type is unhandled
            if (f.IsIgnored() || !f.IsTypeHandled())
                return false;
        
            // Check whether all parameter types are handled
            foreach (ParamDefinition param in f.Parameters)
            {
                if (!param.IsTypeHandled())
                    return false;
            }

            return true;
        }

        protected virtual string NameToPrivate(string name)
        {
            return "_" + Char.ToLower(name[0]) + name.Substring(1);
        }
        protected virtual string NameToPrivate(MemberDefinitionBase m)
        {
            string name = m.NativeName;
            if (m is MemberMethodDefinition
                && (m as MemberMethodDefinition).IsPropertyGetAccessor
                && name.StartsWith("get"))
                name = name.Substring(3);

            return NameToPrivate(name);
        }

        protected virtual void AddTypeDependancy(AbstractTypeDefinition type)
        {
        }

        public static bool IsIteratorWrapper(TypedefDefinition type)
        {
            string[] iters = new string[] { "MapIterator", "ConstMapIterator",
                            "VectorIterator", "ConstVectorIterator" };

            foreach (string it in iters)
                if (type.BaseTypeName.StartsWith(it))
                    return true;

            return false;
        }
    }
}
