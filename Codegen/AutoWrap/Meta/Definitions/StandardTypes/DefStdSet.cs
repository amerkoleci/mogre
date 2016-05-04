using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefStdSet : DefStdList
    {
        public override string STLContainer
        {
            get { return "Set"; }
        }

        public override string FullSTLContainerTypeName
        {
            get { return "STLSet<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[0].MemberTypeNativeName + ">"; }
        }

        public DefStdSet(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}