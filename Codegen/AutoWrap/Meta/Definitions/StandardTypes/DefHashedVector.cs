using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefHashedVector : DefStdVector
    {
        public override string STLContainer
        {
            get { return "HashedVector"; }
        }

        public override string FullSTLContainerTypeName
        {
            get { return "STLHASHEDVECTOR<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[0].MemberTypeNativeName + ">"; }
        }

        public DefHashedVector(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}