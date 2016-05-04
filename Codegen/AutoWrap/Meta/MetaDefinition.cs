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
using System.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Holds the information from the <c>meta.xml</c> file, i.e. information about the 
    /// original C++ source code.
    /// </summary>
    public class MetaDefinition
    {
        private const string META_TAG = "meta";
        private const string NAMESPACE_TAG = "namespace";

        private readonly XmlDocument _doc = new XmlDocument();

        public readonly string NativeNamespace;
        public readonly string ManagedNamespace;

        /// <summary>
        /// The factory used to create all definitions.
        /// </summary>
        public readonly MetaConstructFactory Factory;

        public readonly CodeStyleDefinition CodeStyleDef;

        private readonly Dictionary<string, NamespaceDefinition> _namespaces = new Dictionary<string, NamespaceDefinition>();
        /// <summary>
        /// Contains all namespace definitions for the specified sources. Contains elements
        /// for root namespaces (like "Ogre") as well as elements for child namespaces
        /// (like "Ogre::OverlayElementCommands"). Note that the elements come in no specific
        /// order.
        /// </summary>
        public IEnumerable<NamespaceDefinition> Namespaces
        {
            get
            {
                foreach (NamespaceDefinition space in _namespaces.Values)
                {
                    yield return space;
                }
            }
        }

        public MetaDefinition(string file, string nativeNamespace, string managedNamespace, MetaConstructFactory factory, CodeStyleDefinition codeStyleDef)
        {
            _doc.Load(file);
            NativeNamespace = nativeNamespace;
            ManagedNamespace = managedNamespace;
            Factory = factory;
            CodeStyleDef = codeStyleDef;

            // Find the root tag - assumes we only have one meta tag.
            XmlElement root = (XmlElement)_doc.GetElementsByTagName(META_TAG)[0];
            
            // Iterate through all namespaces
            foreach (XmlNode nsNode in root.ChildNodes)
            {
                XmlElement nsElement = nsNode as XmlElement;
                if (nsElement == null || nsElement.LocalName != NAMESPACE_TAG)
                {
                    // Not an XML element or not an namespace element, but something else.
                    continue;
                }

                AddNamespace(nsElement);
            }
        }

        /// <summary>
        /// Adds an attributes files (i.e. "Attributes.xml" as described in the readme) to the meta 
        /// information.
        /// </summary>
        /// <param name="file">the path to the xml file</param>
        public void AddAttributes(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            // Find the root tag - assumes we only have one meta tag.
            XmlElement root = (XmlElement)doc.GetElementsByTagName(META_TAG)[0];

            List<KeyValuePair<AttributeSet, AutoWrapAttribute>> unprocessedAttributes
                = new List<KeyValuePair<AttributeSet, AutoWrapAttribute>>();

            // Iterate through all namespaces
            foreach (XmlNode nsNode in root.ChildNodes)
            {
                XmlElement nsElement = nsNode as XmlElement;
                if (nsElement == null || nsElement.LocalName != NAMESPACE_TAG)
                {
                    // Not an XML element or not an namespace element, but something else.
                    continue;
                }

                // Process types in the current namespace
                NamespaceDefinition nsDef = GetNameSpace(nsElement.GetAttribute("name"));
                foreach (XmlNode typeNode in nsElement.ChildNodes)
                {
                    XmlElement typeElement = typeNode as XmlElement;
                    if (typeElement == null)
                        continue;

                    AddAttributesForType(nsDef.FindTypeDefinition(typeElement.GetAttribute("name")), typeElement, unprocessedAttributes);
                }
            }

            // Post process all unprocessed attributes
            foreach (KeyValuePair<AttributeSet, AutoWrapAttribute> pair in unprocessedAttributes)
            {
                pair.Value.PostProcessAttributes(pair.Key);
            }
        }

        private void AddAttributesForType(AbstractTypeDefinition type, XmlElement elem, List<KeyValuePair<AttributeSet, AutoWrapAttribute>> unprocessedAttributes)
        {
            // Add
            foreach (XmlAttribute attr in elem.Attributes)
            {
                if (attr.Name != "name")
                    AddAttributeToSet(type, CreateAttributeFromXmlAttribute(attr), unprocessedAttributes);
            }

            foreach (XmlNode childNode in elem.ChildNodes)
            {
                XmlElement childElement = childNode as XmlElement;

                if (childElement == null)
                {
                    // Not an XML element
                    continue;
                }

                // Attached Attribute
                if (IsAttachedProperty(childElement))
                {
                    AddAttributeToSet(type, CreateAttributeFromAttachedAttribute(childElement), unprocessedAttributes);
                    continue;
                }

                // Not an attached attribute
                switch (childElement.Name)
                {
                    case "class":
                    case "struct":
                    case "enumeration":
                    case "typedef":
                        AddAttributesForType(((ClassDefinition)type).GetNestedType(childElement.GetAttribute("name")), childElement, unprocessedAttributes);
                        break;
                    case "function":
                    case "variable":
                        foreach (MemberDefinitionBase m in ((ClassDefinition)type).GetMembers(childElement.GetAttribute("name")))
                        {
                            AddAttributesInMember(m, childElement, unprocessedAttributes);
                        }
                        break;

                    default:
                        throw new Exception("Unexpected");
                }
            }
        }

        private void AddAttributesInMember(MemberDefinitionBase member, XmlElement elem, List<KeyValuePair<AttributeSet, AutoWrapAttribute>> unprocessedAttributes)
        {
            foreach (XmlAttribute attr in elem.Attributes)
            {
                if (attr.Name != "name")
                {
                    AddAttributeToSet(member, CreateAttributeFromXmlAttribute(attr), unprocessedAttributes);
                }
            }

            foreach (XmlNode childNode in elem.ChildNodes)
            {
                XmlElement childElement = childNode as XmlElement;
            
                if (childElement == null)
                {
                    // Not an XML element
                    continue;
                }
            
                // Attached property
                if (IsAttachedProperty(childElement))
                {
                    AddAttributeToSet(member, CreateAttributeFromAttachedAttribute(childElement), unprocessedAttributes);
                    continue;
                }
            
                switch (childElement.Name)
                {
                    case "param":
                        if (!(member is MemberMethodDefinition))
                            throw new Exception("Unexpected");
            
                        string name = childElement.GetAttribute("name");
                        ParamDefinition param = null;
                        foreach (ParamDefinition p in (member as MemberMethodDefinition).Parameters)
                        {
                            if (p.Name == name)
                            {
                                param = p;
                                break;
                            }
                        }
            
                        if (param == null)
                            return;
                        //throw new Exception("Wrong param name");
            
                        // Add all attributes (except for "name") to the set
                        foreach (XmlAttribute attr in childElement.Attributes)
                        {
                            if (attr.Name != "name")
                                AddAttributeToSet(param, CreateAttributeFromXmlAttribute(attr), unprocessedAttributes);
                        }
                        break;
            
                    default:
                        throw new Exception("Unexpected");
                }
            }
        }

        /// <summary>
        /// Create an <see cref="AutoWrapAttribute"/> instance from a so called attached attribute. An attached attribute is
        /// an XML child element of the element the attribute is defined for. See "readme-attributes.txt" for more information.
        /// </summary>
        /// <param name="elem">the attribute as attached attribute XML element</param>
        private static AutoWrapAttribute CreateAttributeFromAttachedAttribute(XmlElement elem)
        {
            // Strip underscore (indicator for attached attributes)
            string typename = elem.Name.Substring(1);
            Type type = AutoWrapAttribute.FindAttribute(typename);
            return (AutoWrapAttribute)type.GetMethod("FromElement").Invoke(null, new object[] { elem });
        }

        /// <summary>
        /// Create an <see cref="AutoWrapAttribute"/> instance from an XML attribute.
        /// </summary>
        /// <param name="attr">the XML attribute to create this instance from</param>
        /// <seealso cref="CreateAttributeFromAttachedAttribute"/>
        private static AutoWrapAttribute CreateAttributeFromXmlAttribute(XmlAttribute attr)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement elem = doc.CreateElement("_" + attr.Name);
            elem.InnerText = attr.Value;
            return CreateAttributeFromAttachedAttribute(elem);
        }

        /// <summary>
        /// Checks whether the specified XML element is an attached property.
        /// </summary>
        private static bool IsAttachedProperty(XmlElement elem)
        {
            return (elem.Name[0] == '_');
        }
    
        /// <summary>
        /// Adds an attribute to an attribute set and marks the attribute as "not yet processed" so that it can
        /// be processed later.
        /// </summary>
        /// <param name="attributeSet">the set the attribute is to be added to</param>
        /// <param name="attrib">the attribute to be added</param>
        /// <param name="unprocessedAttributes">the list of unprocessed attributes</param>
        private static void AddAttributeToSet(AttributeSet attributeSet, AutoWrapAttribute attrib, List<KeyValuePair<AttributeSet, AutoWrapAttribute>> unprocessedAttributes)
        {
            attributeSet.AddAttribute(attrib);
            unprocessedAttributes.Add(new KeyValuePair<AttributeSet, AutoWrapAttribute>(attributeSet, attrib));
        }


        /// <summary>
        /// Returns the namespace definition for the specified name.
        /// </summary>
        /// <param name="nativeNamespaceName">the native name of the namespace to be looked up; 
        /// if this name could not be found, a <see cref="KeyNotFoundException"/> will be thrown</param>
        public NamespaceDefinition GetNameSpace(string nativeNamespaceName)
        {
            return _namespaces[nativeNamespaceName];
        }
    
        /// <summary>
        /// Adds a namespace to the namespace list. IMPORTANT: Child namespaces must be added
        /// after their parent namespaces. Otherwise a <see cref="KeyNotFoundException"/> will
        /// be thrown.
        /// </summary>
        /// <param name="elem">the XML element holding the namespace definition</param>
        private void AddNamespace(XmlElement elem)
        {
            if (elem.Name != "namespace")
                throw new InvalidOperationException("Wrong element; expected 'namespace'.");

            NamespaceDefinition spc = Factory.CreateNamespace(this, elem);
            _namespaces[spc.NativeName] = spc;
        }
    }
}