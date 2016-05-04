using System;
using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefStdPair : DefTemplateTwoTypes
    {
        public override string STLContainer
        {
            get { return "Pair"; }
        }

        public override string FullSTLContainerTypeName
        {
            get { return ConversionTypeName; }
        }

        public virtual string ConversionTypeName
        {
            get { return "Pair<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[1].MemberTypeCLRName + ">"; }
        }

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            switch (param.PassedByType)
            {
                case PassedByType.Reference:
                case PassedByType.Value:
                    return ConversionTypeName;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            switch (param.PassedByType)
            {
                case PassedByType.Reference:
                case PassedByType.Value:
                    newname = "ToNative<" + ConversionTypeName + ", " + FullyQualifiedNativeName + ">( " + param.Name + ")";
                    return "";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string GetCLRTypeName(ITypeMember m)
        {
            switch (m.PassedByType)
            {
                case PassedByType.Reference:
                case PassedByType.Value:
                    return ConversionTypeName;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string ProduceNativeCallConversionCode(string expr, ITypeMember m)
        {
            switch (m.PassedByType)
            {
                case PassedByType.Reference:
                case PassedByType.Value:
                    return "ToManaged<" + ConversionTypeName + ", " + FullyQualifiedNativeName + ">( " + expr + " )";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public DefStdPair(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}