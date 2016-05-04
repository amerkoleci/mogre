using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefStdVector : DefStdList
    {
        public override string STLContainer
        {
            get { return "Vector"; }
        }

        public override string FullSTLContainerTypeName
        {
            get { return "STLVector<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[0].MemberTypeNativeName + ">"; }
        }

        public override string NativeCallConversionFunction
        {
            get { return "GetArrayFromVector"; }
        }

        public DefStdVector(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}