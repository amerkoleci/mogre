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

namespace AutoWrap.Meta
{
    /// <summary>
    /// Base class for all auto wrap attributes.
    /// </summary>
    public abstract class AutoWrapAttribute
    {
        private static readonly Type THIS_CLASS = typeof(AutoWrapAttribute);
        private static readonly string ATTRIBUTES_NAMESPACE = THIS_CLASS.Namespace;

        /// <summary>
        /// Called for each new attribute that has been added to the set.
        /// </summary>
        /// <param name="attributes">the set to which the attribute has been added</param>
        public virtual void PostProcessAttributes(AttributeSet attributes)
        {
        }

        /// <summary>
        /// Finds the type for the specified attribute. If the attribute could not be found, a <see cref="InvalidAttributeException"/>
        /// will be thrown.
        /// </summary>
        /// <param name="attribName">the attribute to look for</param>
        public static Type FindAttribute(string attribName)
        {
            try
            {
                Type type = Assembly.GetExecutingAssembly().GetType(ATTRIBUTES_NAMESPACE + "." + attribName + "Attribute", true, true);
                if (!THIS_CLASS.IsAssignableFrom(type))
                {
                    throw new InvalidAttributeException("The attribute \"" + attribName + "\" isn't derived from " + THIS_CLASS.Name);
                }
                return type;
            }
            catch (TypeLoadException)
            {
                throw new InvalidAttributeException("Could not find class for attribute: " + attribName);
            }
        }
    }

    /// <summary>
    /// Thrown when an auto wrap attribute couldn't be found.
    /// </summary>
    public class InvalidAttributeException : Exception
    {
        public InvalidAttributeException(string msg) : base(msg)
        {
        }
    }
}