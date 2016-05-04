using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Represents a CLR property (inside of a CLR class).
    /// </summary>
    public class MemberPropertyDefinition : ITypeMember
    {
        /// <summary>
        /// The name of this property.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Returns one of the accessor functions to be used to identify certain attribues
        /// of this property (like protection level).
        /// </summary>
        private MemberMethodDefinition _accessorFunction
        {
            get { return (CanRead) ? GetterFunction : SetterFunction; }
        }

        public MemberMethodDefinition SetterFunction;
        public MemberMethodDefinition GetterFunction;

        /// <summary>
        /// Denotes whether this property can be read.
        /// </summary>
        public bool CanRead
        {
            get { return GetterFunction != null; }
        }

        /// <summary>
        /// Denotes whether this property can be set.
        /// </summary>
        public bool CanWrite
        {
            get { return SetterFunction != null; }
        }

        public ProtectionLevel ProtectionLevel
        {
            get { return _accessorFunction.ProtectionLevel; }
        }
    
        public bool IsVirtual
        {
            get { return _accessorFunction.IsVirtual; }
        }
    
        public bool IsAbstract
        {
            get { return _accessorFunction.IsAbstract; }
        }
    
        public bool IsStatic
        {
            get { return _accessorFunction.IsStatic; }
        }
    

        #region ITypeMember Implementations

        public AbstractTypeDefinition MemberType
        {
            get { return (CanRead) ? GetterFunction.MemberType : SetterFunction.Parameters[0].Type; }
        }

        /// <summary>
        /// The name of this member's type - valid for both C++ and C++/CLI. May be different from 
        /// <c>Type.Name</c> when the type is an inner type (like <c>HardwareBuffer::Usage</c>) or 
        /// when the type is a const type. No namepsace name will be included (as in 
        /// <see cref="MemberTypeCLRName"/> and <see cref="MemberTypeNativeName"/>).
        /// </summary>
        public string MemberTypeName { get; set; }

        /// <summary>
        /// The fully qualified name of this member's CLR type (i.e. with CLR (dest) namespace).
        /// </summary>
        public virtual string MemberTypeCLRName
        {
            get { return MemberType.GetCLRTypeName(this); }
        }

        /// <summary>
        /// The fully qualified name of this member's native type (i.e. with native (source) namespace).
        /// </summary>
        public string MemberTypeNativeName
        {
            get { return (this as ITypeMember).MemberType.GetNativeTypeName((this as ITypeMember).IsConst, (this as ITypeMember).PassedByType); }
        }
        
        /// <summary>
        /// Denotes how this member's type is passed (i.e. as copy or reference).
        /// </summary>
        public PassedByType PassedByType { get; set; }
        
        public ClassDefinition ContainingClass
        {
            get
            {
                return _accessorFunction.ContainingClass; 
                throw new InvalidOperationException("The containing class is ambiguous"); 
            }
        }

        public bool IsConst
        {
            get { return (CanRead) ? GetterFunction.IsConst : SetterFunction.Parameters[0].IsConst; }
        }

        public bool HasAttribute<T>() where T : AutoWrapAttribute
        {
            return _accessorFunction.HasAttribute<T>();
        }

        public T GetAttribute<T>() where T : AutoWrapAttribute
        {
            return _accessorFunction.GetAttribute<T>();
        }

        #endregion

        public MemberPropertyDefinition(string name)
        {
            Name = name;
        }

        public MemberPropertyDefinition Clone()
        {
            return (MemberPropertyDefinition)MemberwiseClone();
        }
    
        /// <summary>
        /// Checks whether this property is contained in the specified class or any of its base classes.
        /// </summary>
        /// <param name="clazz">the class to check</param>
        /// <param name="allowInheritedSignature">if this is <c>false</c> only the specified class will be
        /// checked for the property. Otherwise all base classes will be checked as well.</param>
        public bool IsContainedIn(ClassDefinition clazz, bool allowInBaseClass)
        {
            return clazz.ContainsFunctionSignature(_accessorFunction.Signature, allowInBaseClass);
        }
    
        public static string GetPropertyName(MemberMethodDefinition methodDef)
        {
            // property
            string name = methodDef.GetRenameName();

            if (name.StartsWith("get"))
                return name.Substring(3);

            if (name.StartsWith("set"))
                return name.Substring(3);
            
            if (!methodDef.MetaDef.CodeStyleDef.AllowIsInPropertyName && name.StartsWith("is"))
                return name.Substring(2);

            // For properties named like "hasEnabledAnimationState".
            return methodDef.MetaDef.CodeStyleDef.ConvertPropertyName(methodDef.CLRName, methodDef);
        }

        /// <summary>
        /// Converts the specified methods into CLR properties.
        /// </summary>
        /// <param name="methods">The methods to convert. Methods that are no accesors (<c>IsProperty == false</c>) will be ignored.</param>
        public static MemberPropertyDefinition[] GetPropertiesFromMethods(IEnumerable<MemberMethodDefinition> methods)
        {
            // NOTE: Use a sorted "list" here so that the order (and thereby the position) of thy properties in 
            //   the generated source code doesn't vary each time the sources are generated.
            SortedDictionary<string, MemberPropertyDefinition> props = new SortedDictionary<string, MemberPropertyDefinition>();

            foreach (MemberMethodDefinition f in methods)
            {
                if (!f.IsProperty || !f.IsDeclarableFunction)
                    continue;

                MemberPropertyDefinition p = null;
                string propName = GetPropertyName(f);
                CodeStyleDefinition codeStyle = f.MetaDef.CodeStyleDef;        

                if (props.ContainsKey(propName))
                {
                    p = props[propName];
                } else
                {
                    //
                    // Merge properties with "is" or "has" prefix as this prefix can only be determined
                    // from the get accessor but not from the set accessor.
                    //
                    if (f.IsPropertySetAccessor)
                    {
                        // Set accessor - check for existing properties prefixed with "is" or "has".
                        if (codeStyle.AllowIsInPropertyName && props.ContainsKey(codeStyle.CLRPropertyIsPrefix + propName))
                        {
                            propName = codeStyle.CLRPropertyIsPrefix + propName;
                            p = props[propName];
                        }
                        else if (props.ContainsKey(codeStyle.CLRPropertyHasPrefix + propName))
                        {
                            propName = codeStyle.CLRPropertyHasPrefix + propName;
                            p = props[propName];
                        }
                    } else
                    {
                        // Get accessor
                        string oldPropName = null;
                        if (codeStyle.AllowIsInPropertyName
                            && propName.StartsWith(codeStyle.CLRPropertyIsPrefix)
                            && props.ContainsKey(propName.Substring(codeStyle.CLRPropertyIsPrefix.Length)))
                        {
                            // is prefix
                            oldPropName = propName.Substring(codeStyle.CLRPropertyIsPrefix.Length);
                        }
                        else if (propName.StartsWith(codeStyle.CLRPropertyHasPrefix)
                            && props.ContainsKey(propName.Substring(codeStyle.CLRPropertyHasPrefix.Length)))
                        {
                            // has prefix
                            oldPropName = propName.Substring(codeStyle.CLRPropertyHasPrefix.Length);
                        }

                        if (oldPropName != null)
                        {
                            MemberPropertyDefinition oldProp = props[oldPropName];
                            // We need to check here whether the set accessor is actually
                            if (oldProp.MemberTypeName == f.MemberTypeName)
                            {
                                props.Remove(oldPropName);

                                p = new MemberPropertyDefinition(propName);
                                p.MemberTypeName = oldProp.MemberTypeName;
                                p.PassedByType = oldProp.PassedByType;
                                p.SetterFunction = oldProp.SetterFunction;

                                props.Add(p.Name, p);
                            }
                        }
                    }

                    if (p == null)
                    {
                        // New property found
                        p = new MemberPropertyDefinition(propName);
                        if (f.IsPropertyGetAccessor)
                        {
                            p.MemberTypeName = f.MemberTypeName;
                            p.PassedByType = f.PassedByType;
                        } else
                        {
                            p.MemberTypeName = f.Parameters[0].TypeName;
                            p.PassedByType = f.Parameters[0].PassedByType;
                        }

                        props.Add(p.Name, p);
                    }
                }

                if (f.IsPropertyGetAccessor)
                    p.GetterFunction = f;
                else if (f.IsPropertySetAccessor)
                    p.SetterFunction = f;
            }

            return props.Values.ToArray();
        }

        /// <summary>
        /// Checks whether this method is a get accessor for a CLR property.
        /// </summary>
        /// <seealso cref="IsPropertyGetAccessor"/>
        public static bool CheckForGetAccessor(MemberMethodDefinition methodDef)
        {
            //
            // IMPORTANT: Don't use any of the "IsProperty..." properties of MemberMethodDefinition
            //   in this method as those properties use this method to determine their values.
            //
            if (methodDef.IsOverriding)
            {
                // Check this before checking possible attributes
                return methodDef.BaseMethod.IsPropertyGetAccessor;
            }
            
            if (methodDef.IsConstructor || methodDef.MemberTypeName == "void" || methodDef.Parameters.Count != 0)
            {
                // Check this before checking possible attributes
                return false;
            }
            
            if (methodDef.HasAttribute<PropertyAttribute>())
            {
                return true;
            }
            
            if (methodDef.HasAttribute<MethodAttribute>())
            {
                return false;
            }
            
            if (methodDef.HasAttribute<CustomIncDeclarationAttribute>()
                 || methodDef.HasAttribute<CustomCppDeclarationAttribute>())
            {
                return false;
            }

            string name = methodDef.GetRenameName();

            if (methodDef.MemberTypeName == "bool"
                    && ((name.StartsWith("is") && Char.IsUpper(name[2]) && methodDef.MetaDef.CodeStyleDef.AllowIsInPropertyName)
                    || (name.StartsWith("has") && Char.IsUpper(name[3])))
                    && methodDef.Parameters.Count == 0)
                return true;

            if (!methodDef.MemberType.IsValueType
                    && (methodDef.MemberType.IsSharedPtr || methodDef.MemberType is DefTemplateOneType || methodDef.MemberType is DefTemplateTwoTypes))
                return false;

            if (methodDef.MemberType.HasAttribute<ReturnOnlyByMethodAttribute>())
            {
                // Invalid type for a property
                return false;
            }

            string propName;
            if (name.StartsWith("get") && Char.IsUpper(name[3]))
            {
                propName = name.Substring(3);
            } else if (name.StartsWith("is") && Char.IsUpper(name[2])) {
                propName = name.Substring(2);
            } else {
                // Not a valid getter prefix.
                return false;
            }

            // Check if the property's name collides with the name of a nested type. In this 
            // case we can't convert the method into a property.
            AbstractTypeDefinition type = methodDef.ContainingClass.GetNestedType(propName, false);
            if (type != null)
                return false;

            // Check if the property's name collides with the name of a method. In this 
            // case we can't convert the method into a property.
            MemberMethodDefinition method = methodDef.ContainingClass.GetMethodByCLRName(propName, true, false);

            // If there is no method == valid property name
            return (method == null);
        }

        /// <summary>
        /// Checks whether this method is a set accessor for a CLR property.
        /// </summary>
        /// <seealso cref="IsPropertyGetAccessor"/>
        public static bool CheckForSetAccessor(MemberMethodDefinition methodDef)
        {
            //
            // IMPORTANT: Don't use any of the "IsProperty..." properties of MemberMethodDefinition
            //   in this method as those properties use this method to determine their values.
            //
            if (methodDef.IsOverriding)
            {
                // Check this before checking possible attributes
                return methodDef.BaseMethod.IsPropertySetAccessor;
            }
            
            if (methodDef.IsConstructor || methodDef.MemberTypeName != "void" || methodDef.Parameters.Count != 1)
            {
                // Check this before checking possible attributes
                return false;
            }
            
            if (methodDef.HasAttribute<PropertyAttribute>())
                return true;
            
            if (methodDef.HasAttribute<MethodAttribute>())
                return false;
            
            if (methodDef.HasAttribute<CustomIncDeclarationAttribute>()
                    || methodDef.HasAttribute<CustomCppDeclarationAttribute>())
                return false;

            string name = methodDef.GetRenameName();

            if (!name.StartsWith("set") || !Char.IsUpper(name[3]))
                return false;

            // Check to see if there is a "get" function
            string propName = name.Substring(3);
            MemberMethodDefinition method;
            // TODO by manski: Allow the case that the getter and the setter come from different classes.
            //   Special care must be taken in this case as for example "Property.ContainingClass" can't
            //   be used anymore, since there are two classes involved. This could be solved by returning
            //   the "lowest" subclass (i.e. the class in which both accessors are defined then).
            //   Then the second argument should be changed to "true" here.
            method = methodDef.ContainingClass.GetMethodByNativeName("get" + propName, false, false);
            if (method == null)
            {
                method = methodDef.ContainingClass.GetMethodByNativeName("is" + propName, false, false);
                if (method == null) {
                    method = methodDef.ContainingClass.GetMethodByNativeName("has" + propName, false, false);
                }
            }

            // NOTE: Most checks done in "CheckForGetAccessor()" are represented in "method.IsPropertyGetAccessor".
            return (method != null && method.IsPropertyGetAccessor && method.MemberTypeCLRName == methodDef.Parameters[0].MemberTypeCLRName
                && (!methodDef.ContainingClass.AllowVirtuals
                || (method.IsVirtual == methodDef.IsVirtual && method.IsOverriding == methodDef.IsOverriding)));
        }
    }
}