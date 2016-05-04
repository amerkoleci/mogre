using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefStdHashMap : DefStdMap
    {
        public override string STLContainer
        {
            get { return "HashMap"; }
        }

        public override string FullSTLContainerTypeName
        {
            get { return "STLHashMap<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[1].MemberTypeCLRName + ", " + TypeParams[0].MemberTypeNativeName + ", " + TypeParams[1].MemberTypeNativeName + ">"; }
        }

        public DefStdHashMap(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}