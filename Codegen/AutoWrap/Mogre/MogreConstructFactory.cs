using AutoWrap.Meta;
using System.Xml;

namespace AutoWrap.Mogre
{
    internal class MogreConstructFactory : MetaConstructFactory
    {
        public override NamespaceDefinition CreateNamespace(MetaDefinition metaDef, XmlElement elem)
        {
            return new MogreNamespaceDefinition(metaDef, elem);
        }
    }
}