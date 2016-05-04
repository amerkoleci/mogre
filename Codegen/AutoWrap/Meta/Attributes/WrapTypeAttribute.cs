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
    /// The wrap types used for the <see cref="WrapTypeAttribute"/>. Note that each type definition
    /// can only have one of these types.
    /// </summary>
    public enum WrapTypes
    {
        /// <summary>
        /// Denotes that the class can't be subclassed (i.e. that it's <c>sealed</c>).
        /// </summary>
        NonOverridable, 
        /// <summary>
        /// Denotes that the class can be subclassed (i.e. inherited from).
        /// </summary>
        Overridable,
        /// <summary>
        /// ???
        /// </summary>
        NativeDirector,
        /// <summary>
        /// Denotes that the type is an CLR interface. A C++ class must be abstract and none of its methods
        /// must be implemented.
        /// </summary>
        Interface,
        /// <summary>
        /// Denotes a singleton. Generating classes: <see cref="SingletonClassInclProducer"/> and <see cref="SingletonClassCppProducer"/>
        /// </summary>
        Singleton, 
        SharedPtr, 
        ReadOnlyStruct, 
        ValueType, 
        NativePtrValueType, 
        CLRHandle, 
        PlainWrapper
    }

    /// <summary>
    /// This attribute specifies how the specified type is wrapped. More precisly: Specifies which code producer
    /// class to be used (see <see cref="Wrapper.IncAddType"/>).
    /// </summary>
    public class WrapTypeAttribute : AutoWrapAttribute
    {
        public WrapTypes WrapType;

        public WrapTypeAttribute(WrapTypes type)
        {
            WrapType = type;
        }

        public static WrapTypeAttribute FromElement(XmlElement elem)
        {
            WrapTypes wt = (WrapTypes) Enum.Parse(typeof (WrapTypes), elem.InnerText, true);
            return new WrapTypeAttribute(wt);
        }

        public override void PostProcessAttributes(AttributeSet holder)
        {
            switch (WrapType)
            {
                case WrapTypes.NativePtrValueType:
                case WrapTypes.ValueType:
                    if (!holder.HasAttribute<ValueTypeAttribute>())
                        holder.AddAttribute(new ValueTypeAttribute());
                    break;
            }
        }
    }
}