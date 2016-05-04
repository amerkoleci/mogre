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
using System.Collections.Generic;

namespace AutoWrap.Meta
{
    public class InterfacesForOverridableAttribute : AutoWrapAttribute
    {
        public List<ClassDefinition[]> Interfaces = new List<ClassDefinition[]>();

        private List<string[]> _interfaceNames = new List<string[]>();

        public override void PostProcessAttributes(AttributeSet holder)
        {
            ClassDefinition type = (ClassDefinition) holder;

            foreach (string[] names in _interfaceNames)
            {
                List<ClassDefinition> ifaces = new List<ClassDefinition>();
                foreach (string ifacename in names)
                {
                    ifaces.Add(type.DetermineType<ClassDefinition>(ifacename));
                }
                Interfaces.Add(ifaces.ToArray());
            }
        }

        public InterfacesForOverridableAttribute(string interfaces)
        {
            string[] lists = interfaces.Split('|');
            foreach (string list in lists)
            {
                string[] names = list.Split(',');
                for (int i = 0; i < names.Length; i++)
                    names[i] = names[i].Trim();

                _interfaceNames.Add(names);
            }
        }

        public static InterfacesForOverridableAttribute FromElement(XmlElement elem)
        {
            return new InterfacesForOverridableAttribute(elem.InnerText);
        }
    }
}