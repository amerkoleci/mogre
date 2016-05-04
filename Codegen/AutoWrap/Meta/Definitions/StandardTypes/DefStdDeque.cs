using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefStdDeque : DefStdVector
    {
        public override string STLContainer
        {
            get { return "Deque"; }
        }

        public override string FullSTLContainerTypeName
        {
            get { return "STLDeque<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[0].MemberTypeNativeName + ">"; }
        }

        public DefStdDeque(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}