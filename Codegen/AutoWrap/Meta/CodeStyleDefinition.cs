using System;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Describes how the generated code should look like. Subclass this class to adjust the
    /// settings. You can, however, use this class directly as well, if the default settings
    /// are satisfying.
    /// </summary>
    public class CodeStyleDefinition
    {
        /// <summary>
        /// Denotes how one indention level in the code should look like. Possible values are
        /// tabulators or spaces.
        /// </summary>
        public virtual string IndentionLevelString
        {
            get { return "  "; }
        }

        /// <summary>
        /// Returns the new line characters to be used for each new line.
        /// </summary>
        public virtual string NewLineCharacters
        {
            get { return "\r\n"; }
        }

        /// <summary>
        /// Denotes whether "is" is allowed as prefix in a property name. If this
        /// is <c>true</c> a property could be named "IsEnabled" while if this is
        /// <c>false</c> the property would be named "Enabled". Note that get accessor
        /// methods whose names start with "is" are always considered a property, 
        /// regardless of the value of this (i.e. <c>AllowIsInPropertyName</c>) property.
        /// </summary>
        public virtual bool AllowIsInPropertyName
        {
            get { return false; }
        }

        /// <summary>
        /// The prefix "is" for a CLR property name (e.g. "Is" in "IsEnabled").
        /// </summary>
        public virtual string CLRPropertyIsPrefix
        {
            get { return "Is"; }
        }
    
        /// <summary>
        /// The prefix "has" for a CLR property name (e.g. "Has" in "HasSubMeshes").
        /// </summary>
        public virtual string CLRPropertyHasPrefix
        {
            get { return "Has"; }
        }
    
        /// <summary>
        /// Converts the native method name into an CLR method name. <b>Be careful:</b> Don't call 
        /// <see cref="MemberMethodDefinition.CLRName"/> from this method!
        /// </summary>
        /// <param name="name">the native name of this method</param>
        /// <param name="methodDef">the definition of this method</param>
        /// <returns>the CLR name of this method</returns>
        public virtual string ConvertMethodName(string name, MemberMethodDefinition methodDef)
        {
            return ToCamelCase(name);
        }

        /// <summary>
        /// Converts the name into a CLR property name. <b>Be careful:</b> Don't call 
        /// <see cref="MemberMethodDefinition.CLRName"/> from this method!
        /// </summary>
        /// <param name="name">the name of one of the accessor methods with the prefixes
        /// "get", "set", and possibly "is" (see <see cref="RemoveIsFromPropertyName"/>) removed.</param>
        /// <param name="methodDef">the definition of one of the accessor methods for this property</param>
        /// <returns>the CLR name of this property</returns>
        /// <seealso cref="CLRPropertyIsPrefix"/>
        /// <seealso cref="CLRPropertyHasPrefix"/>
        public virtual string ConvertPropertyName(string name, MemberMethodDefinition methodDef)
        {
            return ToCamelCase(name);
        }

        /// <summary>
        /// Converts the name into upper camel case, meaning the first character will be
        /// made upper-case. Note that the remainder of the name must already be in camel
        /// case. Can be used for converting method names and property names to C# coding style.
        /// </summary>
        public static string ToCamelCase(string name)
        {
            return Char.ToUpper(name[0]) + name.Substring(1);
        }
    }
}