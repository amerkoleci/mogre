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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AutoWrap.Meta
{
    /// <summary>
    /// This abstract class describes a C++ type. The following constructs are supported: 
    /// classes, structs, typedefs, and enumerations. This class has the following child
    /// classes: <see cref="ClassDefinition"/>, <see cref="EnumDefinition"/>, <see cref="DefInternal"/>,
    /// <see cref="TypedefDefinition"/>, <see cref="DefString"/>, and <see cref="DefUtfString"/>.
    /// </summary>
    public abstract class AbstractTypeDefinition : AttributeSet
    {
        public virtual string STLContainer
        {
            get { return null; }
        }

        public virtual string FullSTLContainerTypeName
        {
            get { return null; }
        }

        public bool IsSTLContainer
        {
            get { return STLContainer != null; }
        }
        
        public virtual bool IsUnnamedSTLContainer
        {
            get { return false; }
        }
        
        public bool IsTemplate {
            get { return DefiningXmlElement.GetAttribute("template") == "true"; }
        }

        /// <summary>
        /// Indicates whether this type is ignored.
        /// </summary>
        public virtual bool IsIgnored
        {
            get
            {
                if (SurroundingClass != null && SurroundingClass.IsIgnored)
                    return true;

                return this.HasAttribute<IgnoreAttribute>();
            }
        }

        public virtual bool IsPureManagedClass
        {
            get { return HasAttribute<PureManagedClassAttribute>(); }
        }

        public virtual bool IsValueType
        {
            get { return HasAttribute<ValueTypeAttribute>(); }
        }

        private AbstractTypeDefinition _replaceByType;
        public virtual AbstractTypeDefinition ReplaceByType
        {
            get
            {
                if (_replaceByType == null && HasAttribute<ReplaceByAttribute>())
                {
                    string name = GetAttribute<ReplaceByAttribute>().Name;
                    if (SurroundingClass != null)
                        _replaceByType = SurroundingClass.DetermineType(name, false);
                    else
                        _replaceByType = Namespace.DetermineType(name, false);
                }

                return _replaceByType;
            }
        }

        /// <summary>
        /// The XML element that defines this type. Is <c>null</c> for standard types like strings (that are not
        /// defined in the meta.xml file).
        /// </summary>
        public readonly XmlElement DefiningXmlElement;

        /// <summary>
        /// The native .h file in which this type is defined. Is <c>null</c> for standard C/STL/CLR types.
        /// </summary>
        public readonly string IncludeFileName;

        public virtual bool IsSharedPtr
        {
            get { return false; }
        }

        public virtual bool IsReadOnly
        {
            get { return HasAttribute<ReadOnlyAttribute>(); }
        }

        public virtual string Name
        {
            get { return DefiningXmlElement.GetAttribute("name"); }
        }

        public virtual string CLRName
        {
            get
            {
                if (HasAttribute<RenameAttribute>())
                    return GetAttribute<RenameAttribute>().Name;
                
                return Name;
            }
        }

        public virtual string Summary
        {
            get
            {
                XmlNode node = DefiningXmlElement["summary"];
                if (node != null)
                {
                    return node.InnerXml.Trim();
                }
                return null;
            }
        }

        /// <summary>
        /// The namespace this type is defined in. Is never <c>null</c>.
        /// </summary>
        public readonly NamespaceDefinition Namespace;
        
        public ProtectionLevel ProtectionLevel;

        /// <summary>
        /// The class this type is nested within or <c>null</c> if this type is not nested.
        /// </summary>
        /// <seealso cref="IsNested"/>
        public readonly ClassDefinition SurroundingClass;

        /// <summary>
        /// Denotes whether this type is nested within a surrounding class.
        /// </summary>
        /// <seealso cref="SurroundingClass"/>
        public virtual bool IsNested
        {
            get { return SurroundingClass != null; }
        }

        /// <summary>
        /// The fully qualified name of this type (i.e. prefixed with the FQN of the surrounding class or namespace).
        /// </summary>
        public virtual string FullyQualifiedNativeName
        {
            get
            {
                if (SurroundingClass != null)
                    return SurroundingClass.FullyQualifiedNativeName + "::" + Name;

                return Namespace.NativeName + "::" + Name;
            }
        }

        public virtual string FullyQualifiedCLRName
        {
            get
            {
                if (SurroundingClass != null)
                    return SurroundingClass.FullyQualifiedCLRName + "::" + CLRName;

                return Namespace.CLRName + "::" + CLRName;
            }
        }

        public bool IsInternalTypeDef
        {
            get
            {
                if (!(this is TypedefDefinition))
                {
                    return false;
                }
                
                if (IsSharedPtr)
                {
                    return false;
                }
                
                TypedefDefinition explicitType = this.IsNested ? this.SurroundingClass.DetermineType<TypedefDefinition>(this.Name)
                                                                : this.Namespace.DetermineType<TypedefDefinition>(this.Name);
                if (explicitType.IsSTLContainer)
                {
                    return false;
                }
                
                return (explicitType.BaseType is DefInternal);
            }
        }

        /// <summary>
        /// Used for standard types (like <see cref="DefInternal"/>, <see cref="DefUtfString"/>, and <see cref="DefString"/>)
        /// that are not created from XML elements in the meta.xml file.
        /// </summary>
        /// <param name="nsDef">the namespace in which this type is defined. Must not be <c>null</c>.</param>
        protected AbstractTypeDefinition(NamespaceDefinition nsDef) : base(nsDef.MetaDef)
        {
            // If the namespace is "null", then "nsDef.MetaDef" will throw a NullPointerException
            Namespace = nsDef;
            DefiningXmlElement = null;
            IncludeFileName = null;
        }

        /// <summary>
        /// Used for types defined in the meta.xml file.
        /// </summary>
        /// <param name="nsDef">the namespace in which this type is defined. Must not be <c>null</c>.</param>
        /// <param name="surroundingClass">The class this type definition is nested in or <c>null</c> if it's not nested.
        /// In the former case the namespace of the class must be identical to the namespace passed as argument.</param>
        /// <param name="elem">the XML element describing the type; must not be <c>null</c></param>
        protected AbstractTypeDefinition(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef.MetaDef)
        {
            DefiningXmlElement = elem;
            IncludeFileName = elem.GetAttribute("includeFile");

            Namespace = nsDef;
            if (surroundingClass != null && surroundingClass.Namespace != nsDef) {
                throw new ArgumentException("Namespaces don't match.");
            }
            SurroundingClass = surroundingClass;

            ProtectionLevel = ProtectionLevelExtensions.ParseProtectionLevel(elem.GetAttribute("protection"));
        }

        /// <summary>
        /// Denotes whether this type definition has the specified wrap type. This is it has the <see cref="WrapTypeAttribute"/>
        /// defined with the specified value.
        /// </summary>
        /// <param name="wrapType">the expected wrap type</param>
        public virtual bool HasWrapType(WrapTypes wrapType)
        {
            return HasAttribute<WrapTypeAttribute>() && GetAttribute<WrapTypeAttribute>().WrapType == wrapType;
        }

        /// <summary>
        /// Resolves this type definition to its actual type. This has two applications:
        /// 
        /// 1. For a type that is replaced by another type the other type will be returned.
        /// 2. For a <c>typedef</c> this returns the actual underlying type of the typedef.
        /// </summary>
        public virtual AbstractTypeDefinition ResolveType()
        {
            if (this.ReplaceByType != null)
            {
                return this.ReplaceByType;
            }

            return this;
        }

        public virtual string GetNativeTypeName(bool isConst, PassedByType passed)
        {
            string postfix = null;
            switch (passed)
            {
                case PassedByType.Pointer:
                    postfix = "*";
                    break;
                case PassedByType.PointerPointer:
                    postfix = "**";
                    break;
                case PassedByType.Reference:
                    postfix = "&";
                    break;
                case PassedByType.Value:
                    postfix = "";
                    break;
                default:
                    throw new Exception("Unexpected");
            }
            return (isConst ? "const " : "") + FullyQualifiedNativeName + postfix;
        }

        public abstract string GetCLRTypeName(ITypeMember m);
        public abstract string GetCLRParamTypeName(ParamDefinition param);

        #region Code Generation Methods

        public virtual string ProduceNativeCallConversionCode(string expr, ITypeMember m)
        {
            return expr;
        }

        public virtual void ProduceNativeParamConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion)
        {
            preConversion = postConversion = null;
            conversion = param.Type.ProduceNativeCallConversionCode(param.Name, param);
        }

        public virtual void ProduceDefaultParamValueConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion, out AbstractTypeDefinition dependancyType)
        {
            throw new Exception("Unexpected");
        }

        /// <summary>
        /// Produces the code to convert a single parameter from a CLR type to its native
        /// counterpart. This code then will be inserted before the call to the method that
        /// uses this parameter.
        /// </summary>
        /// <param name="param">the parameter to pass</param>
        /// <param name="newname">the name of the converted (i.e. native) parameter; should
        /// be <c>param.Name</c> if no conversion is done.</param>
        /// <returns>the conversion code; should be an empty string, if no conversion is done</returns>
        public virtual string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            newname = param.Name;
            return "";
        }

        /// <summary>
        /// Produces the code to convert a single parameter from a native type to its CLR
        /// counterpart. This code then will be inserted after the call to the method that
        /// uses this parameter. The code may also contain cleanup code.
        /// </summary>
        /// <param name="param">the parameter that was passed</param>
        /// <returns>the conversion code; should be an empty string, if no conversion is done</returns>
        public virtual string ProducePostCallParamConversionCleanupCode(ParamDefinition param)
        {
            return "";
        }

        #endregion

        /// <summary>
        /// Finds a type (e.g. class, enum, typedef) as if it was used directly at a certain position in the code
        /// (i.e. from the type definition's perspective). See the other overload for more information.
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
        /// Determines a type (e.g. class, enum, typedef) as if it was used directly at a certain position in the code
        /// (i.e. from the type definition's perspective). Consider the following C# code example:
        /// 
        /// <code>
        /// namespace MyNamespace {
        ///   public class MyClass {
        ///   }
        ///   
        ///   public class MySecondClass {
        ///     public MyClass MyVar; // Using MyClass
        ///   }
        ///   
        ///   public class MyThirdClass {
        ///     public MyClass MyVar; // Using MyClass
        ///     
        ///     public class MyClass {
        ///     }
        ///   }
        /// }
        /// </code>
        /// 
        /// The type returned for <c>MyClass</c> will be different depending on where the type is used.
        /// If it's used by <c>MySecondClass</c> it's the first class <c>MyClass</c>. If it's used
        /// by <c>MyThirdClass</c> it's the inner class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">the name of the type. Can be fully qualified (i.e. with namespace and surrounding
        /// class).</param>
        /// <param name="raiseException">indicates whether an exception is to be thrown when the type can't
        /// be found. If this is <c>false</c>, an instance of <see cref="DefInternal"/> will be returned when
        /// the type couldn't be found.</param>
        /// <returns></returns>
        public virtual T DetermineType<T>(string name, bool raiseException = true) where T : AbstractTypeDefinition
        {
            if (name.StartsWith(this.MetaDef.NativeNamespace + "::"))
            {
                name = name.Substring(name.IndexOf("::") + 2);
                return Namespace.DetermineType<T>(name, raiseException);
            }

            return (IsNested) ? SurroundingClass.DetermineType<T>(name, raiseException) : Namespace.DetermineType<T>(name, raiseException);
        }

        public override string ToString()
        {
            if (GetType() == typeof(AbstractTypeDefinition))
                return "Unknown construct: " + Name;
 
            return GetType().Name.Replace("Definition", "") + ": " + Name;
        }
    }
}
