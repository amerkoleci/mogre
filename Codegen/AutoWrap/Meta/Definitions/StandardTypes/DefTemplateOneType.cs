using System;
using System.Xml;

namespace AutoWrap.Meta
{
    public class DefTemplateOneType : TypedefDefinition
    {
        public override bool IsValueType
        {
            get { return false; }
        }

        public override string CLRName
        {
            get
            {
                if (IsUnnamedSTLContainer)
                    return "STL" + STLContainer + "_" + TypeParams[0].ParamType.Name;
                
                return base.CLRName;
            }
        }

        public override string FullyQualifiedCLRName
        {
            get
            {
                if (IsUnnamedSTLContainer)
                    return CLRName;

                return base.FullyQualifiedCLRName;
            }
        }

        public override void ProduceDefaultParamValueConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion, out AbstractTypeDefinition dependancyType)
        {
            preConversion = postConversion = "";
            dependancyType = null;
            switch (param.PassedByType)
            {
                case PassedByType.Pointer:
                    if (param.DefaultValue == "NULL" || param.DefaultValue == "0")
                    {
                        conversion = "nullptr";
                        return;
                    }
                    
                    throw new Exception("Unexpected");
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            switch (param.PassedByType)
            {
                case PassedByType.Reference:
                case PassedByType.Pointer:
                    if (param.IsConst)
                        return FullyQualifiedCLRName.Replace(CLRName, "Const_" + CLRName) + "^";

                    return FullyQualifiedCLRName + "^";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            newname = param.Name;
            return "";
        }

        public override string ProducePostCallParamConversionCleanupCode(ParamDefinition param)
        {
            return "";
        }

        public override string GetCLRTypeName(ITypeMember m)
        {
            switch (m.PassedByType)
            {
                case PassedByType.Reference:
                case PassedByType.Pointer:
                    if (m.IsConst)
                        return FullyQualifiedCLRName.Replace(CLRName, "Const_" + CLRName) + "^";

                    return FullyQualifiedCLRName + "^";
                case PassedByType.Value:
                    if (m.IsConst || IsReadOnly)
                        return FullyQualifiedCLRName.Replace(CLRName, "Const_" + CLRName) + "^";

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
                case PassedByType.Pointer:
                    return expr;
                case PassedByType.Value:
                    if (m.IsConst || IsReadOnly)
                        return FullyQualifiedCLRName + "::ByValue( " + expr + " )->ReadOnlyInstance";

                    return FullyQualifiedCLRName + "::ByValue( " + expr + " )";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string FullyQualifiedNativeName
        {
            get
            {
                if (Name.StartsWith("std::"))
                    return Name + "<" + TypeParams[0].MemberTypeNativeName + ">";

                if (ProtectionLevel == ProtectionLevel.Protected)
                    return NativeProtectedTypesProxy.GetProtectedTypesProxyName(SurroundingClass) + "::" + Name;

                return base.FullyQualifiedNativeName;
            }
        }

        public DefTemplateOneType(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}