using System;
using System.Xml;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Describes a C++ <c>typedef</c>.
    /// </summary>
    public class TypedefDefinition : AbstractTypeDefinition
    {
        public override void ProduceDefaultParamValueConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion, out AbstractTypeDefinition dependancyType)
        {
            preConversion = postConversion = "";
            dependancyType = null;
            if (!(this is DefTemplateOneType || this is DefTemplateTwoTypes) && BaseType is DefInternal)
                conversion = param.DefaultValue;
            else
                throw new Exception("Unexpected");
        }

        public override void ProduceNativeParamConversionCode(ParamDefinition param, out string preConversion, out string conversion, out string postConversion)
        {
            if (!(this is DefTemplateOneType || this is DefTemplateTwoTypes) && BaseType is DefInternal)
                BaseType.ProduceNativeParamConversionCode(param, out preConversion, out conversion, out postConversion);
            else
                base.ProduceNativeParamConversionCode(param, out preConversion, out conversion, out postConversion);
        }

        public override string GetCLRParamTypeName(ParamDefinition param)
        {
            string s = BaseType.GetCLRParamTypeName(param).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName);
            if (s.Contains("Mogre::int32"))
                return s;

            return BaseType.GetCLRParamTypeName(param).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName).Replace(BaseType.FullyQualifiedNativeName, FullyQualifiedNativeName);
        }

        public override string ProducePreCallParamConversionCode(ParamDefinition param, out string newname)
        {
            if (!(this is DefTemplateOneType || this is DefTemplateTwoTypes) && BaseType is DefInternal)
            {
                string s = BaseType.ProducePreCallParamConversionCode(param, out newname).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName);
                if (s.Contains("Mogre::int32"))
                    return s;

                return BaseType.ProducePreCallParamConversionCode(param, out newname).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName).Replace(BaseType.FullyQualifiedNativeName, FullyQualifiedNativeName);
            }

            return base.ProducePreCallParamConversionCode(param, out newname);
        }

        public override string ProducePostCallParamConversionCleanupCode(ParamDefinition param)
        {
            if (!(this is DefTemplateOneType || this is DefTemplateTwoTypes) && BaseType is DefInternal)
            {
                string s = BaseType.ProducePostCallParamConversionCleanupCode(param).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName);
                if (s.Contains("Mogre::int32"))
                    return s;
                
                return BaseType.ProducePostCallParamConversionCleanupCode(param).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName).Replace(BaseType.FullyQualifiedNativeName, FullyQualifiedNativeName);
            }

            return base.ProducePostCallParamConversionCleanupCode(param);
        }

        public override string GetCLRTypeName(ITypeMember m)
        {
            string s = BaseType.GetCLRTypeName(m).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName);
            if (s.Contains("Mogre::int32"))
                return s;
            return BaseType.GetCLRTypeName(m).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName).Replace(BaseType.FullyQualifiedNativeName, FullyQualifiedNativeName);
        }

        public override string ProduceNativeCallConversionCode(string expr, ITypeMember m)
        {
            if (!(this is DefTemplateOneType || this is DefTemplateTwoTypes) && BaseType is DefInternal)
            {
                string s = BaseType.ProduceNativeCallConversionCode(expr, m).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName);
                if (s.Contains("Mogre::int32"))
                    return s;
                return
                    BaseType.ProduceNativeCallConversionCode(expr, m).Replace(BaseType.FullyQualifiedCLRName, FullyQualifiedCLRName).Replace(BaseType.FullyQualifiedNativeName, FullyQualifiedNativeName);
            }

            return base.ProduceNativeCallConversionCode(expr, m);
        }

        /// <summary>
        /// Resolves this type definition to its actual type. This has two applications:
        /// 
        /// 1. For a type that is replaced by another type the other type will be returned.
        /// 2. For a <c>typedef</c> this returns the actual underlying type of the typedef.
        /// </summary>
        public override AbstractTypeDefinition ResolveType()
        {
            if (ReplaceByType != null) {
                return ReplaceByType;
            }

            AbstractTypeDefinition expl = null;

            if (BaseTypeName.Contains("<") || BaseTypeName.Contains("std::") || Mogre17.IsCollection(BaseTypeName))
            {
                // Standard types
                expl = MetaDef.Factory.StandardTypesFactory.FindStandardType(this);
            } else if (Name == "String")
            {
                // Typdef "String", which is (obviously) a string.
                expl = new DefStringTypeDef(Namespace, SurroundingClass, DefiningXmlElement);
            }

            if (expl == null)
                return this;

            expl.LinkAttributes(this);
            return expl;
        }

        /// <summary>
        /// Creates a type definition for a collection type (e.g. a list or a map).
        /// </summary>
        public static AbstractTypeDefinition CreateExplicitCollectionType(ClassDefinition surroundingClass, string container, string key, string val)
        {
            string stdcont = "std::" + container;
            XmlDocument doc = new XmlDocument();

            XmlElement elem = doc.CreateElement("typedef");
            elem.SetAttribute("basetype", stdcont);
            elem.SetAttribute("name", stdcont);

            XmlElement te = doc.CreateElement("type");
            te.InnerText = val;
            elem.AppendChild(te);

            if (key != "")
            {
                te = doc.CreateElement("type");
                te.InnerText = key;
                elem.InsertAfter(te, null);
            }

            TypedefDefinition newTypedef = surroundingClass.Namespace.MetaDef.Factory.CreateTypedef(surroundingClass.Namespace, surroundingClass, elem);
            return newTypedef.ResolveType();
        }

        public override bool IsIgnored
        {
            get
            {
                if (base.IsIgnored)
                    return true;

                foreach (ITypeMember m in TypeParams)
                    if (m.MemberType.IsIgnored || m.MemberType.HasWrapType(WrapTypes.NativeDirector))
                        return true;

                return false;
            }
        }

        public override bool IsValueType
        {
            get { return BaseType.IsValueType; }
        }

        private AbstractTypeDefinition _baseType;
        public virtual AbstractTypeDefinition BaseType
        {
            get
            {
                if (_baseType == null)
                {
                    string basename = BaseTypeName;
                    if (basename.Contains("<"))
                        basename = basename.Substring(0, basename.IndexOf("<")).Trim();

                    _baseType = DetermineType<AbstractTypeDefinition>(basename, false);
                }

                return _baseType;
            }
        }

        public bool IsTypedefOfInternalType
        {
            get
            {
                return BaseType is DefInternal || (BaseType is TypedefDefinition && (BaseType as TypedefDefinition).IsTypedefOfInternalType);
            }
        }

        public string BaseTypeName;
        public string[] TypeParamNames;

        protected PassedByType[] _passed;

        private TypeParamDefinition[] _typeParams;

        public virtual TypeParamDefinition[] TypeParams
        {
            get
            {
                if (_typeParams == null)
                {
                    _typeParams = new TypeParamDefinition[TypeParamNames.Length];
                    for (int i = 0; i < TypeParamNames.Length; i++)
                    {
                        bool isConst = false;
                        string name = TypeParamNames[i];
                        if (name.StartsWith("const "))
                        {
                            isConst = true;
                            name = name.Substring("const ".Length);
                        }
                        _typeParams[i] = new TypeParamDefinition(DetermineType<AbstractTypeDefinition>(name, false), _passed[i], isConst);
                    }
                }

                return _typeParams;
            }
        }

        public override bool IsSharedPtr
        {
            get { return BaseTypeName.StartsWith("SharedPtr"); }
        }

        public TypedefDefinition(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
            : base(nsDef, surroundingClass, elem)
        {
            if (elem.Name != "typedef")
                throw new Exception("Not typedef element");

            BaseTypeName = elem.GetAttribute("basetype");
            string type = elem.GetAttribute("type");
            if (type == "")
            {
                TypeParamNames = new string[elem.ChildNodes.Count];
                _passed = new PassedByType[elem.ChildNodes.Count];
                for (int i = 0; i < elem.ChildNodes.Count; i++)
                {
                    TypeParamNames[i] = elem.ChildNodes[i].InnerText.Trim();
                    string pass = (elem.ChildNodes[i] as XmlElement).GetAttribute("passedBy");
                    _passed[i] = (pass == "") ? PassedByType.Value : (PassedByType) Enum.Parse(typeof (PassedByType), pass, true);
                }
            }
            else
            {
                TypeParamNames = new string[1];
                TypeParamNames[0] = type.Trim();
                _passed = new PassedByType[1];
                string pass = elem.GetAttribute("passedBy");
                _passed[0] = (pass == "") ? PassedByType.Value : (PassedByType) Enum.Parse(typeof (PassedByType), pass, true);
            }
        }
    }
}