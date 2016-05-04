using System;

namespace AutoWrap.Meta
{
    public class StandardTypesFactory
    {
        public virtual AbstractTypeDefinition FindStandardType(TypedefDefinition typedef)
        {
            AbstractTypeDefinition expl = null;

            if (typedef.BaseTypeName.Contains("<") || typedef.BaseTypeName.Contains("std::") || Mogre17.IsCollection(typedef.BaseTypeName))
            {
                if (typedef.BaseTypeName == "std::vector" || typedef.BaseTypeName == "std::list")
                    expl = CreateTemplateOneTypeParamType(typedef);
                else
                {
                    switch (typedef.TypeParamNames.Length)
                    {
                        case 1:
                            expl = CreateTemplateOneTypeParamType(typedef);
                            break;
                        case 2:
                            expl = CreateTemplateTwoTypeParamsType(typedef);
                            break;
                        default:
                            throw new Exception("Unexpected");
                    }
                }
            }

            if (expl == null)
                throw new ArgumentException("Unsupported or unknown standard type: " + typedef.BaseTypeName);

            return expl;
        }

        protected virtual AbstractTypeDefinition CreateTemplateOneTypeParamType(TypedefDefinition typedef)
        {
            if (typedef.IsSharedPtr)
                return new DefSharedPtr(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);

            if (AbstractCodeProducer.IsIteratorWrapper(typedef))
                return new DefIterator(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);

            if (typedef.BaseTypeName.StartsWith("TRect"))
                return new DefTRect(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);

             return CreateSTLCollectionType(typedef);
        }

        protected virtual AbstractTypeDefinition CreateTemplateTwoTypeParamsType(TypedefDefinition typedef)
        {
            string baseTypeName = Mogre17.GetBaseType(typedef);

            switch (baseTypeName)
            {
                case "::std::hash_map":
                    return new DefStdHashMap(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                case "std::map":
                    return new DefStdMap(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                case "std::multimap":
                    return new DefStdMultiMap(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                case "std::pair":
                    return new DefStdPair(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                default:
                    throw new Exception("Unexpected");
            }
        }

        protected virtual AbstractTypeDefinition CreateSTLCollectionType(TypedefDefinition typedef)
        {
            string baseTypeName = Mogre17.GetBaseType(typedef);

            switch (baseTypeName)
            {
                case "std::vector":
                    return new DefStdVector(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                case "std::set":
                    return new DefStdSet(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                case "std::deque":
                    return new DefStdDeque(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                case "std::list":
                    return new DefStdList(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                case "HashedVector":
                    return new DefHashedVector(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                case "std::map":
                    return new DefStdMap(typedef.Namespace, typedef.SurroundingClass, typedef.DefiningXmlElement);
                default:
                    throw new Exception("Unexpected");
            }
        }
    }
}