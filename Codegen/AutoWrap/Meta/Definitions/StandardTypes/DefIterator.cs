using System;
using System.Xml;

namespace AutoWrap.Meta
{
    public class DefIterator : DefTemplateOneType
    {
        public bool IsConstIterator
        {
            get { return Name.StartsWith("Const") && Char.IsUpper(Name["Const".Length]); }
        }

        public bool IsMapIterator
        {
            get { return TypeParams[0].ParamType is DefTemplateTwoTypes; }
        }

        private ITypeMember _iterationElementType;
        public virtual ITypeMember IterationElementTypeMember
        {
            get
            {
                if (_iterationElementType == null)
                {
                    if (TypeParams[0].ParamType is DefTemplateOneType)
                        _iterationElementType = (TypeParams[0].ParamType as DefTemplateOneType).TypeParams[0];
                    else if (TypeParams[0].ParamType is DefTemplateTwoTypes)
                        _iterationElementType = (TypeParams[0].ParamType as DefTemplateTwoTypes).TypeParams[1];
                    else
                        throw new Exception("Unexpected");
                }

                return _iterationElementType;
            }
        }

        private ITypeMember _iterationKeyType;
        public virtual ITypeMember IterationKeyTypeMember
        {
            get
            {
                if (_iterationKeyType == null)
                {
                    if (TypeParams[0].ParamType is DefTemplateTwoTypes)
                        _iterationKeyType = (TypeParams[0].ParamType as DefTemplateTwoTypes).TypeParams[0];
                }

                return _iterationKeyType;
            }
        }

        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            newname = param.Name;
            return String.Empty;
        }

        public override string ProducePostCallParamConversionCleanupCode(ParamDefinition param)
        {
            return String.Empty;
        }

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            switch (param.PassedByType)
            {
                case PassedByType.Value:
                    return FullyQualifiedCLRName + "^";
                default:
                    throw new Exception("Unexpected");
            }
        }

        public override string GetCLRTypeName(ITypeMember m)
        {
            switch (m.PassedByType)
            {
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
                case PassedByType.Value:
                    return expr;
                default:
                    throw new Exception("Unexpected");
            }
        }

        public DefIterator(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
        }
    }
}