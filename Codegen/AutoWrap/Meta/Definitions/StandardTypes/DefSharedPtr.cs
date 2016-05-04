using System;
using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefSharedPtr : DefTemplateOneType
    {
        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            newname = "(" + param.MemberTypeNativeName + ")" + param.Name;
            return String.Empty;
        }

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            return GetCLRTypeName(param);
        }

        public override string GetCLRTypeName(ITypeMember m)
        {
            switch (m.PassedByType)
            {
                case PassedByType.Reference:
                case PassedByType.Value:
                    return FullyQualifiedCLRName + "^";
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
                    return expr;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public DefSharedPtr(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}