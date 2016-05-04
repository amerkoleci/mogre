using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoWrap.Meta
{
    class NativeProxyClassCppProducer : NativeProxyClassProducer
    {
        private static bool IsCachedFunction(MemberMethodDefinition f)
        {
            return (f.PassedByType == PassedByType.Reference
                && (f.MemberType.IsValueType || f.MemberType.IsPureManagedClass));
        }

        public static void AddNativeProxyMethodBody(MemberMethodDefinition f, string managedTarget, SourceCodeStringBuilder sb)
        {
            string managedCall;
            string fullPostConv = null;

            if (f.IsPropertyGetAccessor)
            {
                sb.AppendLine(f.MemberTypeCLRName + " mp_return = " + managedTarget + "->" + MemberPropertyDefinition.GetPropertyName(f) + ";");
                managedCall = "mp_return";
            }
            else if (f.IsPropertySetAccessor)
            {
                ParamDefinition param = f.Parameters[0];
                managedCall = managedTarget + "->" + MemberPropertyDefinition.GetPropertyName(f) + " = " + param.Type.ProduceNativeCallConversionCode(param.Name, param);
            }
            else
            {
                string pre, post, conv;

                foreach (ParamDefinition param in f.Parameters)
                {
                    param.Type.ProduceNativeParamConversionCode(param, out pre, out conv, out post);
                    if (!String.IsNullOrEmpty(pre))
                        sb.AppendLine(pre);

                    if (!String.IsNullOrEmpty(post))
                        fullPostConv += post + "\n";
                }

                bool explicitCast = f.HasAttribute<ExplicitCastingForParamsAttribute>();

                if (!f.HasReturnValue)
                {
                    sb.Append(f.MemberTypeCLRName + " mp_return = " + managedTarget + "->" + f.CLRName + "(");
                    for (int i = 0; i < f.Parameters.Count; i++)
                    {
                        ParamDefinition param = f.Parameters[i];
                        param.Type.ProduceNativeParamConversionCode(param, out pre, out conv, out post);
                        sb.Append(" ");
                        if (explicitCast) sb.Append("(" + param.MemberTypeCLRName + ")");
                        sb.Append(conv);
                        if (i < f.Parameters.Count - 1) sb.Append(",");
                    }
                    sb.AppendLine(" );");
                    managedCall = "mp_return";

                    if (!String.IsNullOrEmpty(fullPostConv))
                        sb.AppendLine(fullPostConv);
                }
                else
                {
                    managedCall = managedTarget + "->" + f.CLRName + "(";
                    for (int i = 0; i < f.Parameters.Count; i++)
                    {
                        ParamDefinition param = f.Parameters[i];
                        param.Type.ProduceNativeParamConversionCode(param, out pre, out conv, out post);
                        managedCall += " ";
                        if (explicitCast) managedCall += "(" + param.MemberTypeCLRName + ")";
                        managedCall += conv;
                        if (i < f.Parameters.Count - 1) managedCall += ",";
                    }
                    managedCall += " )";
                }
            }

            if (!f.HasReturnValue)
            {
                if (f.MemberType is IDefString)
                {
                    sb.AppendLine("SET_NATIVE_STRING( Mogre::Implementation::cachedReturnString, " + managedCall + " )");
                    sb.AppendLine("return Mogre::Implementation::cachedReturnString;");
                }
                else
                {
                    string returnExpr;
                    string newname, expr, postcall;
                    ParamDefinition param = new ParamDefinition(f.MetaDef, f, managedCall);
                    expr = f.MemberType.ProducePreCallParamConversionCode(param, out newname);
                    postcall = f.MemberType.ProducePostCallParamConversionCleanupCode(param);
                    if (!String.IsNullOrEmpty(expr))
                    {
                        sb.AppendLine(expr);
                        if (String.IsNullOrEmpty(postcall))
                            returnExpr = newname;
                        else
                        {
                            throw new Exception("Unexpected");
                        }
                    }
                    else
                    {
                        returnExpr = newname;
                    }

                    if (IsCachedFunction(f))
                    {
                        sb.AppendLine("STATIC_ASSERT( sizeof(" + f.MemberType.FullyQualifiedNativeName + ") <= CACHED_RETURN_SIZE )");
                        sb.AppendLine("memcpy( Mogre::Implementation::cachedReturn, &" + returnExpr + ", sizeof(" + f.MemberType.FullyQualifiedNativeName + ") );");
                        sb.AppendLine("return *reinterpret_cast<" + f.MemberType.FullyQualifiedNativeName + "*>(Mogre::Implementation::cachedReturn);");
                    }
                    else
                    {
                        sb.AppendLine("return " + returnExpr + ";");
                    }
                }
            }
            else
            {
                sb.AppendLine(managedCall + ";");

                if (!String.IsNullOrEmpty(fullPostConv))
                    sb.AppendLine(fullPostConv);
            }
        }

        public NativeProxyClassCppProducer(MetaDefinition metaDef, Wrapper wrapper, ClassDefinition t, SourceCodeStringBuilder sb)
            : base(metaDef, wrapper, t, sb)
        {
        }

        protected override void AddOverridableFunction(MemberMethodDefinition f)
        {
            _wrapper.CppCheckTypeForDependancy(f.MemberType);
            foreach (ParamDefinition param in f.Parameters)
                _wrapper.CppCheckTypeForDependancy(param.Type);

            _codeBuilder.Append(f.MemberTypeNativeName + " " + ProxyName + "::" + f.NativeName + "(");
            AddNativeMethodParams(f);
            _codeBuilder.Append(" )");
            if (f.IsConstMethod)
                _codeBuilder.Append(" const");
            _codeBuilder.AppendEmptyLine();
            _codeBuilder.BeginBlock();

            if (!f.IsAbstract)
            {
                _codeBuilder.AppendLine("if (_overriden[ " + _methodIndices[f] + " ])");
                _codeBuilder.BeginBlock();
            }

            if (f.HasAttribute<CustomNativeProxyDeclarationAttribute>())
            {
                string txt = f.GetAttribute<CustomNativeProxyDeclarationAttribute>().DeclarationText;
                txt = ReplaceCustomVariables(txt, f).Replace("@MANAGED@", "_managed");
                _codeBuilder.AppendLine(txt);
            }
            else
            {
                AddNativeProxyMethodBody(f, "_managed", _codeBuilder);
            }

            if (!f.IsAbstract)
            {
                _codeBuilder.EndBlock();
                _codeBuilder.AppendLine("else");
                _codeBuilder.Append("\t");
                if (!f.HasReturnValue) _codeBuilder.Append("return ");
                _codeBuilder.Append(f.ContainingClass.Name + "::" + f.NativeName + "(");
                for (int i = 0; i < f.Parameters.Count; i++)
                {
                    ParamDefinition param = f.Parameters[i];
                    _codeBuilder.Append(" " + param.Name);
                    if (i < f.Parameters.Count - 1) _codeBuilder.Append(",");
                }
                _codeBuilder.AppendLine(" );");
            }

            _codeBuilder.EndBlock();
        }

        //protected override void AddProtectedFunction(DefFunction f)
        //{
        //    _sb.AppendIndent("");
        //    _sb.Append(f.NativeTypeName + " " + ProxyName + "::base_" + f.Name + "(");
        //    AddNativeMethodParams(f);
        //    _sb.Append(" )");
        //    if (f.IsConstFunctionCall)
        //        _sb.Append(" const");
        //    _sb.AppendEmptyLine();
        //    _sb.AppendLine("{");
        //    _sb.IncreaseIndent();

        //    _sb.AppendIndent("");
        //    if (!f.IsVoid)
        //        _sb.Append("return ");
        //    _sb.Append(_t.FullNativeName + "::" + f.Name + "(");

        //    for (int i = 0; i < f.Parameters.Count; i++)
        //    {
        //        DefParam param = f.Parameters[i];
        //        _sb.Append(" " + param.Name);
        //        if (i < f.Parameters.Count - 1) _sb.Append(",");
        //    }
        //    _sb.Append(" );\n");

        //    _sb.DecreaseIndent();
        //    _sb.AppendLine("}");
        //}
    }
}
