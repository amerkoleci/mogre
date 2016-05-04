using System;

namespace AutoWrap.Meta
{
    class DefInternal : AbstractTypeDefinition 
    {
        public override void ProduceNativeParamConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion)
        {
            preConversion = postConversion = null;
            conversion = param.Name;

            switch (param.PassedByType)
            {
                case PassedByType.Value:
                case PassedByType.Reference:
                    break;
                case PassedByType.Pointer:
                    if (!IsVoid)
                    {
                        if (!param.IsConst)
                        {
                            conversion = "*" + param.Name;
                        }
                        else
                        {
                            //Treat it as array
                            throw new Exception("Unexpected");
                        }
                    }
                    break;
                case PassedByType.PointerPointer:
                    throw new Exception("Unexpected");
            }
        }

        public override void ProduceDefaultParamValueConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion, out AbstractTypeDefinition dependancyType)
        {
            preConversion = postConversion = "";
            dependancyType = null;
            if (IsVoid)
            {
                conversion = param.DefaultValue;
                return;
            }

            switch (param.PassedByType)
            {
                case PassedByType.Pointer:
                    if (!param.IsConst)
                    {
                        preConversion = FullyQualifiedCLRName + " out_" + param.Name + ";";
                        conversion = "out_" + param.Name;
                        return;
                    }

                    throw new Exception("Unexpected");
                default:
                    conversion = param.DefaultValue;
                    break;
            }
        }

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            if (IsVoid)
            {
                string name = "void";
                if (param.IsConst)
                    name = "const " + name;

                switch (param.PassedByType)
                {
                    case PassedByType.Value:
                        break;
                    case PassedByType.Pointer:
                        name += "*";
                        break;
                    case PassedByType.PointerPointer:
                        name += "*%";
                        break;
                    case PassedByType.Reference:
                        throw new Exception("Unexpected");
                }
                return name;
            }

            switch (param.PassedByType)
            {
                case PassedByType.Value:
                    return Name;
                case PassedByType.Reference:
                case PassedByType.Pointer:
                    if (!param.IsConst && !param.HasAttribute<RawPointerParamAttribute>())
                    {
                        return "[Out] " + Name + "%";
                    }

                    //Treat it as array
                    //return "array<" + FullCLRName + ">^";
                    string name = Name + "*";
                    if (param.IsConst)
                        name = "const " + name;
                    return name;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            if (IsVoid)
            {
                newname = param.Name;
                return "";
            }

            switch (param.PassedByType)
            {
                case PassedByType.Reference:
                    newname = "*p_" + param.Name;
                    return "pin_ptr<" + FullyQualifiedCLRName + "> p_" + param.Name + " = &" + param.Name + ";\n";
                case PassedByType.Pointer:
                    if (!param.IsConst && !param.HasAttribute<RawPointerParamAttribute>())
                    {
                        newname = "p_" + param.Name;
                        return "pin_ptr<" + FullyQualifiedCLRName + "> p_" + param.Name + " = &" + param.Name + ";\n";
                    }

                    //Treat it as array
                    //newname = "arr_" + param.Name;
                    //string expr = FullNativeName + "* arr_" + param.Name + " = new " + FullNativeName + "[" + param.Name + "->Length];\n";
                    //expr += "pin_ptr<" + FullCLRName + "> src_" + param.Name + " = &" + param.Name + "[0];\n";
                    //expr += "memcpy(arr_" + param.Name + ", src_" + param.Name + ", " + param.Name + "->Length * sizeof(" + FullNativeName + "));\n";
                    //return expr;
                    newname = param.Name;
                    return "";
                default:
                    return base.ProducePreCallParamConversionCode(param, out newname);
            }
        }

        public override string ProducePostCallParamConversionCleanupCode(ParamDefinition param)
        {
            if (IsVoid)
                return "";

            switch (param.PassedByType)
            {
                case PassedByType.Pointer:
                    if (param.IsConst)
                    {
                        //Treat it as array
                        //return "delete[] arr_" + param.Name + ";";
                        return "";
                    }

                    return base.ProducePostCallParamConversionCleanupCode(param);
                default:
                    return base.ProducePostCallParamConversionCleanupCode(param);
            }
        }

        public override string GetCLRTypeName(ITypeMember m)
        {
            switch (m.PassedByType)
            {
                case PassedByType.Value:
                    return Name;
                case PassedByType.Pointer:
                    if (IsVoid)
                        return "void*";
                    
                    if (m.HasAttribute<ArrayTypeAttribute>())
                        return "array<" + FullyQualifiedCLRName + ">^";
                    
                    string name = Name + "*";
                    if (m.IsConst)
                        name = "const " + name;
                    return name;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string ProduceNativeCallConversionCode(string expr, ITypeMember m)
        {
            switch (m.PassedByType)
            {
                case PassedByType.Reference:
                    //Could be called from NativeClass producer
                case PassedByType.Value:
                    return expr;
                case PassedByType.Pointer:
                    if (IsVoid)
                        return expr;
                    
                    if (m.HasAttribute<ArrayTypeAttribute>())
                    {
                        int len = m.GetAttribute<ArrayTypeAttribute>().Length;
                        return "GetValueArrayFromNativeArray<" + FullyQualifiedCLRName + ", " + FullyQualifiedNativeName + ">( " + expr + " , " + len + " )";
                    }
                    
                    return expr;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public virtual bool IsVoid
        {
            get { return (Name == "void"); }
        }

        private string _name;

        public override string Name
        {
            get { return _name; }
        }

        public override string FullyQualifiedCLRName
        {
            get { return Name; }
        }

        public override string FullyQualifiedNativeName
        {
            get { return Name; }
        }

        public override bool IsValueType
        {
            get { return true; }
        }

        public DefInternal(NamespaceDefinition nsDef, string name)
            : base(nsDef)
        {
            if (name.StartsWith("const "))
                name = name.Substring("const ".Length);

            _name = name;
        }
    }
}