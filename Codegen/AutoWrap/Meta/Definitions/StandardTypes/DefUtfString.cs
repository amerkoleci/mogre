using System;

namespace AutoWrap.Meta
{
    class DefUtfString : AbstractTypeDefinition, IDefString
    {
        public override string FullyQualifiedNativeName
        {
            get { return "DisplayString"; }
        }

        public override bool IsValueType
        {
            get { return true; }
        }

        public override void ProduceDefaultParamValueConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion, out AbstractTypeDefinition dependancyType)
        {
            preConversion = postConversion = "";
            dependancyType = null;
            switch (param.PassedByType)
            {
                case PassedByType.Reference:
                case PassedByType.Value:
                    conversion = param.DefaultValue.Trim();
                    if (!conversion.StartsWith("\"") && conversion.Contains("::"))
                    {
                        //It's a static string of a class

                        if (conversion == "StringUtil::BLANK")
                        {
                            //Manually translate "StringUtil::BLANK" so that there's no need to wrap the StringUtil class
                            conversion = "String::Empty";
                            return;
                        }

                        string name = conversion.Substring(0, conversion.LastIndexOf("::"));
                        dependancyType = DetermineType<AbstractTypeDefinition>(name);
                    }
                    break;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            switch (param.PassedByType)
            {
                case PassedByType.Value:
                case PassedByType.Reference:
                    return "String^";
                case PassedByType.Pointer:
                    return "array<String^>^";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            string name = param.Name;
            switch (param.PassedByType)
            {
                case PassedByType.Value:
                case PassedByType.Reference:
                    newname = "o_" + name;
                    return "DECLARE_NATIVE_UTFSTRING( o_" + name + ", " + name + " )\n";
                case PassedByType.Pointer:
                    string expr = FullyQualifiedNativeName + "* arr_" + name + " = new " + FullyQualifiedNativeName + "[" + name + "->Length];\n";
                    expr += "for (int i=0; i < " + name + "->Length; i++)\n";
                    expr += "{\n";
                    expr += "\tSET_NATIVE_UTFSTRING( arr_" + name + "[i], " + name + "[i] )\n";
                    expr += "}\n";
                    newname = "arr_" + name;
                    return expr;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string ProducePostCallParamConversionCleanupCode(ParamDefinition param)
        {
            switch (param.PassedByType)
            {
                case PassedByType.Value:
                case PassedByType.Reference:
                    return "";
                case PassedByType.Pointer:
                    return "delete[] arr_" + param.Name + ";\n";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string GetCLRTypeName(ITypeMember m)
        {
            switch (m.PassedByType)
            {
                case PassedByType.Value:
                case PassedByType.Reference:
                    return "String^";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string ProduceNativeCallConversionCode(string expr, ITypeMember m)
        {
            switch (m.PassedByType)
            {
                case PassedByType.Value:
                case PassedByType.Reference:
                    return "UTF_TO_CLR_STRING( " + expr + " )";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public DefUtfString(NamespaceDefinition nsDef)
            : base(nsDef)
        {
        }
    }
}