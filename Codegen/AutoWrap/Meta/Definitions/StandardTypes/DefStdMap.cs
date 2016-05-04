using System;
using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefStdMap : DefTemplateTwoTypes
    {
        public override bool IsUnnamedSTLContainer
        {
            get { return Name.StartsWith("std::"); }
        }

        public override string STLContainer
        {
            get { return "Map"; }
        }

        public override string FullSTLContainerTypeName
        {
            get { return "STLMap<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[1].MemberTypeCLRName + ", " + TypeParams[0].MemberTypeNativeName + ", " + TypeParams[1].MemberTypeNativeName + ">"; }
        }

        //public override void GetDefaultParamValueConversion(DefParam param, out string preConversion, out string conversion, out string postConversion)
        //{
        //    preConversion = postConversion = "";
        //    switch (param.PassedByType)
        //    {
        //        case PassedByType.Pointer:
        //            if (param.IsConst)
        //                conversion = "nullptr";
        //            else
        //                throw new Exception("Unexpected");
        //            break;
        //        default:
        //            throw new Exception("Unexpected");
        //    }
        //}

        public virtual string ConversionTypeName
        {
            get
            {
                if (TypeParams[0].ParamType is IDefString || TypeParams[1].ParamType is IDefString)
                {
                    if (TypeParams[0].ParamType is IDefString && TypeParams[1].ParamType is IDefString)
                        return "Collections::Specialized::NameValueCollection^";
                    
                    throw new Exception("Unexpected");
                }

                return "Collections::Generic::SortedList<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[1].MemberTypeCLRName + ">^";
            }
        }

        public virtual string PreCallConversionFunction
        {
            get
            {
                if (TypeParams[0].ParamType is IDefString || TypeParams[1].ParamType is IDefString)
                {
                    if (TypeParams[0].ParamType is IDefString && TypeParams[1].ParamType is IDefString)
                        return "FillMapFromNameValueCollection";
                    
                    throw new Exception("Unexpected");
                }

                return "FillMapFromSortedList<" + FullyQualifiedNativeName + ", " + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[1].MemberTypeCLRName + ">";
            }
        }

        public virtual string NativeCallConversionFunction
        {
            get
            {
                if (TypeParams[0].ParamType is IDefString || TypeParams[1].ParamType is IDefString)
                {
                    if (TypeParams[0].ParamType is IDefString && TypeParams[1].ParamType is IDefString)
                        return "GetNameValueCollectionFromMap";
                    
                    throw new Exception("Unexpected");
                }

                return "GetSortedListFromMap<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[1].MemberTypeCLRName + ", " + FullyQualifiedNativeName + ">";
            }
        }

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            if (!IsUnnamedSTLContainer)
                return base.GetCLRParamTypeName(param);

            switch (param.PassedByType)
            {
                case PassedByType.Reference:
                    return ConversionTypeName;
                case PassedByType.Pointer:
                    if (param.IsConst)
                        return ConversionTypeName;
                    
                        throw new Exception("Unexpected");
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            if (!IsUnnamedSTLContainer)
                return base.ProducePreCallParamConversionCode(param, out newname);

            string expr;
            switch (param.PassedByType)
            {
                case PassedByType.Reference:
                    expr = FullyQualifiedNativeName + " o_" + param.Name + ";\n";
                    expr += PreCallConversionFunction + "(o_" + param.Name + ", " + param.Name + ");\n";
                    newname = "o_" + param.Name;
                    return expr;
                case PassedByType.Pointer:
                    if (param.IsConst)
                    {
                        expr = FullyQualifiedNativeName + "* p_" + param.Name + " = 0;\n";
                        expr += FullyQualifiedNativeName + " o_" + param.Name + ";\n";
                        expr += "if (" + param.Name + " != CLR_NULL)\n{\n";
                        expr += "\t" + PreCallConversionFunction + "(o_" + param.Name + ", " + param.Name + ");\n";
                        expr += "\tp_" + param.Name + " = &o_" + param.Name + ";\n";
                        expr += "}\n";
                        newname = "p_" + param.Name;
                        return expr;
                    }
                    
                    throw new Exception("Unexpected");
                default:
                    throw new Exception("Unexpected");
            }
        }

        //public override string GetCLRTypeName(ITypeMember m)
        //{
        //    switch (m.PassedByType)
        //    {
        //        case PassedByType.Reference:
        //            return ConversionTypeName;
        //        case PassedByType.PointerPointer:
        //        case PassedByType.Value:
        //        case PassedByType.Pointer:
        //        default:
        //            throw new Exception("Unexpected");
        //    }
        //}

        //public override string GetNativeCallConversion(string expr, ITypeMember m)
        //{
        //    switch (m.PassedByType)
        //    {
        //        case PassedByType.Reference:
        //            return NativeCallConversionFunction + "( " + expr + ")";
        //        case PassedByType.PointerPointer:
        //        case PassedByType.Value:
        //        case PassedByType.Pointer:
        //        default:
        //            throw new Exception("Unexpected");
        //    }
        //}

        public DefStdMap(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}