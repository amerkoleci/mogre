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

using System.Xml;

namespace AutoWrap.Meta
{
    /// <summary>
    /// If a class has this attribute, the value of this attribute will be used as class definition in
    /// the classes' CLR .h file. No code will be generated automatically for this class.
    /// </summary>
    public class CustomClassInclCodeAttribute : AutoWrapAttribute
    {
        public string Text;

        public CustomClassInclCodeAttribute(string decl)
        {
            Text = decl;
        }

        public static CustomClassInclCodeAttribute FromElement(XmlElement elem)
        {
            return new CustomClassInclCodeAttribute(elem.InnerText);
        }
    }
}