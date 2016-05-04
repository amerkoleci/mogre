using System;
using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefTRect : DefTemplateOneType
    {
        public override bool IsValueType
        {
            get { return true; }
        }

        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            if (param.PassedByType == PassedByType.Pointer)
                newname = "(" + param.Type.FullyQualifiedNativeName + "*) " + param.Name;
            else
                newname = param.Name;
            return string.Empty;
        }

        public override string ProducePostCallParamConversionCleanupCode(ParamDefinition param)
        {
            return string.Empty;
        }

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            switch (param.PassedByType)
            {
                case PassedByType.Reference:
                case PassedByType.Value:
                    if (param.PassedByType == PassedByType.Reference && !param.IsConst)
                        throw new Exception("Unexpected");
                    return FullyQualifiedCLRName;
                case PassedByType.Pointer:
                    return FullyQualifiedCLRName + "*";
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
                    return FullyQualifiedCLRName;
                case PassedByType.Pointer:
                    return FullyQualifiedCLRName + "*";
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
                case PassedByType.Pointer:
                    return "(" + m.MemberTypeCLRName + ") " + expr;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public DefTRect(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}