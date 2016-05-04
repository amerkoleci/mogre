using System;
using System.Xml;

namespace AutoWrap.Meta
{
    internal class DefStdList : DefTemplateOneType
    {
        public override bool IsUnnamedSTLContainer
        {
            get { return Name.StartsWith("std::"); }
        }

        public override string STLContainer
        {
            get { return "List"; }
        }

        public override string FullSTLContainerTypeName
        {
            get { return "STLList<" + TypeParams[0].MemberTypeCLRName + ", " + TypeParams[0].MemberTypeNativeName + ">"; }
        }

        public virtual string NativeCallConversionFunction
        {
            get { return "GetArrayFromList"; }
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
        //            {
        //                preConversion = "array<" + TypeMembers[0].CLRTypeName + ">^ out_" + param.Name + ";";
        //                conversion = "out_" + param.Name;
        //            }
        //            break;
        //        default:
        //            base.GetDefaultParamValueConversion(param, out preConversion, out conversion, out postConversion);
        //            break;
        //    }
        //}

        //public override string GetCLRParamTypeName(DefParam param)
        //{
        //    switch (param.PassedByType)
        //    {
        //        case PassedByType.Reference:
        //        case PassedByType.Pointer:
        //            if (param.IsConst)
        //                return "Collections::Generic::List<" + TypeMembers[0].CLRTypeName + ">^";
        //            else
        //                return "[Out] array<" + TypeMembers[0].CLRTypeName + ">^%";
        //        case PassedByType.Value:
        //            return "Collections::Generic::List<" + TypeMembers[0].CLRTypeName + ">^";
        //        case PassedByType.PointerPointer:
        //        default:
        //            throw new Exception("Unexpected");
        //    }
        //}

        //public override string GetPreCallParamConversion(DefParam param, out string newname)
        //{
        //    string expr;
        //    switch (param.PassedByType)
        //    {
        //        case PassedByType.Reference:
        //            if (param.IsConst)
        //            {
        //                expr = FullNativeName + " o_" + param.Name + ";\n";
        //                expr += "FillListFromGenericList<" + FullNativeName + ", " + TypeMembers[0].CLRTypeName + ">(o_" + param.Name + ", " + param.Name + ");\n";
        //                newname = "o_" + param.Name;
        //                return expr;
        //            }
        //            else
        //            {
        //                newname = "o_" + param.Name;
        //                return FullNativeName + " o_" + param.Name + ";\n";
        //            }
        //        case PassedByType.Pointer:
        //            if (param.IsConst)
        //            {
        //                expr = FullNativeName + "* p_" + param.Name + " = 0;\n";
        //                expr += FullNativeName + " o_" + param.Name + ";\n";
        //                expr += "if (" + param.Name + " != CLR_NULL)\n{\n";
        //                expr += "\tFillListFromGenericList<" + FullNativeName + ", " + TypeMembers[0].CLRTypeName + ">(o_" + param.Name + ", " + param.Name + ");\n";
        //                expr += "\tp_" + param.Name + " = &o_" + param.Name + ";\n";
        //                expr += "}\n";
        //                newname = "p_" + param.Name;
        //                return expr;
        //            }
        //            else
        //            {
        //                newname = "&o_" + param.Name;
        //                return FullNativeName + " o_" + param.Name + ";\n";
        //            }
        //        case PassedByType.Value:
        //            expr = FullNativeName + " o_" + param.Name + ";\n";
        //            expr += "FillListFromGenericList<" + FullNativeName + ", " + TypeMembers[0].CLRTypeName + ">(o_" + param.Name + ", " + param.Name + ");\n";
        //            newname = "o_" + param.Name;
        //            return expr;
        //        case PassedByType.PointerPointer:
        //        default:
        //            throw new Exception("Unexpected");
        //    }
        //}

        //public override string GetPostCallParamConversionCleanup(DefParam param)
        //{
        //    switch (param.PassedByType)
        //    {
        //        case PassedByType.Reference:
        //        case PassedByType.Pointer:
        //            if (param.IsConst)
        //            {
        //                return base.GetPostCallParamConversionCleanup(param);
        //            }
        //            else
        //            {
        //                return param.Name + " = " + NativeCallConversionFunction + "<" + TypeMembers[0].CLRTypeName + ", " + FullNativeName + ">(o_" + param.Name + ");\n";
        //            }
        //        default:
        //            return base.GetPostCallParamConversionCleanup(param);
        //    }
        //}

        //public override string GetCLRTypeName(ITypeMember m)
        //{
        //    if (IsUnnamedSTLContainer)
        //    {
        //        switch (m.PassedByType)
        //        {
        //            case PassedByType.Value:
        //            case PassedByType.Pointer:
        //            case PassedByType.Reference:
        //                return "array<" + TypeMembers[0].CLRTypeName + ">^";
        //            case PassedByType.PointerPointer:
        //            default:
        //                throw new Exception("Unexpected");
        //        }
        //    }
        //    else
        //        return base.GetCLRTypeName(m);
        //}

        //public override string GetNativeCallConversion(string expr, ITypeMember m)
        //{
        //    if (IsUnnamedSTLContainer)
        //    {
        //        switch (m.PassedByType)
        //        {
        //            case PassedByType.Value:
        //            case PassedByType.Reference:
        //                return NativeCallConversionFunction + "<" + TypeMembers[0].CLRTypeName + ", " + FullNativeName + ">( " + expr + " )";
        //            case PassedByType.Pointer:
        //                return NativeCallConversionFunction + "<" + TypeMembers[0].CLRTypeName + ", " + FullNativeName + ">( *(" + expr + ") )";
        //            case PassedByType.PointerPointer:
        //            default:
        //                throw new Exception("Unexpected");
        //        }
        //    }
        //    else
        //        return base.GetNativeCallConversion(expr, m);
        //}

        public DefStdList(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}