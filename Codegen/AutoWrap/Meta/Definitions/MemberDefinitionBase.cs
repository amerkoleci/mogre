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

namespace AutoWrap.Meta
{
    /// <summary>
    /// Describes a native class or struct member, i.e. a field (see <see cref="MemberFieldDefinition"/>) or a 
    /// method (<see cref="MemberMethodDefinition"/>). Note that only native (C++) members will be derived
    /// from this class; i.e. CLR properties aren't derived from this class.
    /// </summary>
    // NOTE: Don't call this class "AbstractMemberDefintion" to avoid confusion about whether the 
    //   described member is an "abstract" member (i.e. an abstract method).
    public abstract class MemberDefinitionBase : AttributeSet, ITypeMember, IRenamable
    {
        private readonly string _nativeName;
        /// <summary>
        /// The native (C++) name of this member.
        /// </summary>
        public virtual string NativeName
        {
            get { return _nativeName; }
        }

        /// <summary>
        /// The managed (C++/CLI) name of this member.
        /// </summary>
        public virtual string CLRName
        {
            get { return this.GetRenameName(); }
        }

        private readonly string _summary;
        public virtual string Summary
        {
            get
            {
                return _summary;
            }
        }

        public virtual bool IsIgnored
        {
            get
            {
                //TODO: Find a more general way to handle templates and get rid of this hacky way
                if (Definition.StartsWith("Controller<"))
                    return true;

                return (MemberType.IsIgnored || HasAttribute<IgnoreAttribute>());
            }
        }

        public readonly bool IsStatic;

        /// <summary>
        /// Indicates whether this member is C++ <c>const</c>.
        /// </summary>
        /// <remarks>Required by <see cref="ITypeMember"/>.</remarks>
        public abstract bool IsConst { get; }

        private AbstractTypeDefinition _memberType;
        public AbstractTypeDefinition MemberType
        {
            get
            {
                if (_memberType == null)
                {
                    // NOTE: This code can't be placed in the constructor as there may be circular references
                    //   which then would lead to "FindType()" failing (as the type hasn't been added yet).
                    if (_container != "")
                    {
                        _memberType = TypedefDefinition.CreateExplicitCollectionType(_containingClass, _container, _containerKey,
                            (_containerValue != "") ? _containerValue : _typeName);
                    } else
                        _memberType = ContainingClass.DetermineType<AbstractTypeDefinition>(_typeName, false);
                }

                return _memberType;
            }
        }

        private readonly string _typeName;
        public virtual string MemberTypeName
        {
            get { return _typeName; }
        }

        /// <summary>
        /// The fully qualified name of this member's CLR type (i.e. with CLR (dest) namespace).
        /// </summary>
        /// <remarks>Required by <see cref="ITypeMember"/>.</remarks>
        public virtual string MemberTypeCLRName
        {
            get { return MemberType.GetCLRTypeName(this);  }
        }

        /// <summary>
        /// The fully qualified name of this member's native type (i.e. with native (source) namespace).
        /// </summary>
        /// <remarks>Required by <see cref="ITypeMember"/>.</remarks>
        public virtual string MemberTypeNativeName
        {
            get { return (this as ITypeMember).MemberType.GetNativeTypeName(IsConst, (this as ITypeMember).PassedByType); }
        }

        private readonly ClassDefinition _containingClass;
        /// <summary>
        /// The class this member is contained in.
        /// </summary>
        /// <remarks>Required by <see cref="ITypeMember"/>.</remarks>
        public virtual ClassDefinition ContainingClass
        {
            get { return _containingClass; }
        }

        /// <summary>
        /// The native (C++) protection level of this member (e.g. "public", "protected", ...).
        /// </summary>
        public readonly ProtectionLevel ProtectionLevel;

        private readonly PassedByType _passedByType;
    
        /// <summary>
        /// Describes how this member is accessed (e.g. pointer or copy). The actual interpretation
        /// depends on whether this member is a method or a field.
        /// </summary>
        public PassedByType PassedByType {
            get { return _passedByType; }
        }

        private readonly string _container;
        private readonly string _containerKey;
        private readonly string _containerValue;

        public readonly string Definition;

        public MemberDefinitionBase(XmlElement elem, ClassDefinition containingClass)
            : base(containingClass.MetaDef)
        {
            _containingClass = containingClass;
            IsStatic = elem.GetAttribute("static") == "yes";
            ProtectionLevel = ProtectionLevelExtensions.ParseProtectionLevel(elem.GetAttribute("protection"));
            _passedByType = (PassedByType)Enum.Parse(typeof(PassedByType), elem.GetAttribute("passedBy"), true);

            foreach (XmlElement child in elem.ChildNodes)
            {
                switch (child.Name)
                {
                    case "name":
                        _nativeName = child.InnerText;
                        break;

                    case "type":
                        _typeName = child.InnerText;
                        _container = child.GetAttribute("container");
                        _containerKey = child.GetAttribute("containerKey");
                        _containerValue = child.GetAttribute("containerValue");
                        break;

                    case "definition":
                        Definition = child.InnerText;
                        break;

                    case "summary":
                        _summary = child.InnerXml.Trim();
                        break;

                    default:
                        // Let the subclass decide what to do with this.
                        InterpretChildElement(child);
                        break;
                }
            }
        }


        /// <summary>
        /// This method allows subclasses to interpret XML child elements other than
        /// "name", "type", and "definition" (which are already interpreted in the constructor).
        /// Note that not all fields of this class may have been initialized yet.
        /// </summary>
        /// <param name="child">the child element to be interpreted</param>
        protected virtual void InterpretChildElement(XmlElement child)
        {
            throw new Exception("Unsupported child element: '" + child.Name + "'");
        }
    }
}
