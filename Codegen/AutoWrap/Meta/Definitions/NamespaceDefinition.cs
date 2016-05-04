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
using System.Xml;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Contains the definition of a C++ namespace (i.e. its name and the contained types) along 
    /// with the name of its managed counter part.
    /// </summary>
    /// <remarks>As for now, only three levels of nested namespaces are supported 
    /// (like "Level1::Level2::Level3").</remarks>
    public class NamespaceDefinition
    {
        public readonly MetaDefinition MetaDef;

        /// <summary>
        /// The native (C++) name of this namespace.
        /// </summary>
        /// <seealso cref="CLRName"/>
        public readonly string NativeName;

        /// <summary>
        /// The managed (C++/CLI) name of this namespace. May differ from <see cref="NativeName"/>.
        /// </summary>
        public readonly string CLRName;

        /// <summary>
        /// The parent namespace of this namespace or <c>null</c>, if this namespace is a root
        /// namespace (i.e. has no parent).
        /// </summary>
        public readonly NamespaceDefinition ParentNamespace;
        
        private readonly List<NamespaceDefinition> _childNamespaces = new List<NamespaceDefinition>();
        /// <summary>
        /// The child namespaces of this namespace.
        /// </summary>
        /// <seealso cref="ChildNamespaceCount"/>
        public IEnumerable<NamespaceDefinition> ChildNamespaces
        {
            get
            {
                foreach (NamespaceDefinition def in _childNamespaces)
                {
                    yield return def;
                }
            }
        }
        
        /// <summary>
        /// The number of child namespaces in the namespace.
        /// </summary>
        /// <seealso cref="ChildNamespaces"/>
        public uint ChildNamespaceCount
        {
            get { return (uint)_childNamespaces.Count; }
        }
        
        private readonly List<AbstractTypeDefinition> _containedTypes = new List<AbstractTypeDefinition>();
        /// <summary>
        /// The type definitions (mostly class definitions) contained in this namespace. Classes contained in child 
        /// namespaces are not returned but can be obtained through use of <see cref="ChildNamespaces"/>.
        /// </summary>
        // TODO by manski: What happens when a subclass returns different types for types in this list (like
        //   MOGRE returning "DefUtfString" for "DisplayString")? Then this can't be used for search type or what?
        public IEnumerable<AbstractTypeDefinition> ContainedTypes
        {
            get
            {
                foreach (AbstractTypeDefinition def in _containedTypes)
                {
                    yield return def;
                }
            }
        }

        public NamespaceDefinition(MetaDefinition metaDef, XmlElement elem)
        {
            MetaDef = metaDef;

            // The child namespace names are stored in the attributes "second" and "third". Thus
            // we can only support up to three namespace levels (like "Level1::Level2::Level3").
            string second = elem.GetAttribute("second");
            string third = elem.GetAttribute("third");
            
            NativeName = elem.GetAttribute("name");
            CLRName = metaDef.ManagedNamespace;
            
            if (second != "")
            {
                NativeName += "::" + second;
                CLRName += "::" + second;
            }
            
            if (third != "")
            {
                NativeName += "::" + third;
                CLRName += "::" + third;
            }
            
            // If this is a child namespace, set parent and add itself to the parent.
            if (NativeName.Contains("::"))
            {
                // Is a child namespace.
                string parentNamespaceName = NativeName.Substring(0, NativeName.LastIndexOf("::"));
                ParentNamespace = metaDef.GetNameSpace(parentNamespaceName);

                ParentNamespace._childNamespaces.Add(this);
            }
            else
                ParentNamespace = null;
            
            //
            // Add types contained in this namespace.
            //
            foreach (XmlElement child in elem.ChildNodes)
            {
                AbstractTypeDefinition type = MetaDef.Factory.CreateType(this, null, child);
                if (type != null)
                    _containedTypes.Add(type);
            }
        }

        /// <summary>
        /// Finds a type (e.g. a class) within this namespace. This namespace and all parent namespaces
        /// will be searched (where this namespace will be searched first). Child namespace won't be searched.
        /// The type is searched for as if it would be used directly within a <c>namespace</c> block (or within a nested
        /// class directly inside a <c>namespace</c> block).
        /// </summary>
        /// <param name="name">the name of the type. Can be fully qualified (i.e. with namespace and surrounding
        /// class).</param>
        /// <param name="raiseException">indicates whether an exception is to be thrown when the type can't
        /// be found. If this is <c>false</c>, an instance of <see cref="DefInternal"/> will be returned when
        /// the type couldn't be found.</param>
        /// <returns></returns>
        public AbstractTypeDefinition DetermineType(string name, bool raiseException = true)
        {
            return DetermineType<AbstractTypeDefinition>(name, raiseException);
        }

        /// <summary>
        /// Finds a type (e.g. a class) within this namespace. This namespace and all parent namespaces
        /// will be searched (where this namespace will be searched first). Child namespace won't be searched.
        /// The type is searched for as if it would be used directly within a <c>namespace</c> block (or within a nested
        /// class directly inside a <c>namespace</c> block).
        /// </summary>
        /// <typeparam name="T">the type of the definition to be returned (e.g. <see cref="ClassDefinition"/>).</typeparam>
        /// <param name="name">the name of the type. The name must be specified without namespace. </param>
        /// <param name="raiseException">indicates whether an exception is to be thrown when the type can't
        /// be found. If this is <c>false</c>, an instance of <see cref="DefInternal"/> will be returned when
        /// the type couldn't be found.</param>
        /// <returns></returns>
        /// <seealso cref="AbstractTypeDefinition.DetermineType"/>
        public virtual T DetermineType<T>(string name, bool raiseException = true) where T : AbstractTypeDefinition
        {
            // Search this namespace
            AbstractTypeDefinition type = FindTypeInList<T>(name, _containedTypes);
            // Search parent namespace for the type.
            if (type == null)
            {
                if (ParentNamespace != null)
                    return ParentNamespace.DetermineType<T>(name, raiseException);

                if (raiseException)
                    throw new Exception("Could not find type");

                return (T)(object)new DefInternal(this, name);
            }

            if (type is AbstractTypeDefinition && type.IsIgnored)
            {
                // Short circuit out to handle OGRE 1.6 memory allocator types
                // TODO by manski: ??? Document this more clearly.
                return (T)type;
            }

            return (T)type.ResolveType();
        }

        /// <summary>
        /// Finds the specified type in a specified list.
        /// </summary>
        /// <returns>Returns the type definition or <c>null</c>, if it could not be found.</returns>
        private AbstractTypeDefinition FindTypeInList<T>(string name, List<AbstractTypeDefinition> types) 
        {
            List<AbstractTypeDefinition> list = new List<AbstractTypeDefinition>();

            string topname = name;
            string nextnames = null;

            if (name.Contains("::"))
            {
                topname = name.Substring(0, name.IndexOf("::"));
                nextnames = name.Substring(name.IndexOf("::") + 2);
            }

            foreach (AbstractTypeDefinition t in types)
            {
                if (t.Name == topname && t is T) 
                {
                    list.Add(t);
                }
            }

            if (list.Count == 0)
                return null;
            
            if (list.Count > 1)
                throw new Exception("Found more than one type");

            AbstractTypeDefinition type = list[0];

            if (nextnames != null)
                return FindTypeInList<T>(nextnames, ((ClassDefinition)type).NestedTypes);

            return type;
        }

        /// <summary>
        /// Returns the type definition for the specified type or throws a <see cref="KeyNotFoundException"/>
        /// if the type is not part of this namespace.
        /// </summary>
        /// <param name="name">the name of the type to look for. Must come without namespace prepended (i.e.
        /// just the class/type name).</param>
        public AbstractTypeDefinition FindTypeDefinition(string name)
        {
            AbstractTypeDefinition type = null;

            foreach (AbstractTypeDefinition t in _containedTypes)
            {
                if (t.Name == name)
                {
                    type = t;
                    break;
                }
            }

            if (type == null)
                throw new KeyNotFoundException(String.Format("DefType not found for '{0}'", name));

            return type;
        }

        public override string ToString()
        {
            return "Namespace: " + CLRName;
        }
    }
}