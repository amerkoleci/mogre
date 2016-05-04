using System;
using System.Xml;

namespace AutoWrap.Meta
{
    public class StructDefinition
        : ClassDefinition
    {
        public StructDefinition(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
            if (elem.Name != "struct")
                throw new Exception("Not struct element");
        }
    }
}