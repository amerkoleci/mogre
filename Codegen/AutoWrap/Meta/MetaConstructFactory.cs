using System;
using System.Xml;

namespace AutoWrap.Meta
{
    /// <summary>
    /// This factory produces all the constructs (e.g. classes, namespaces, ...) used by the wrapping process.
    /// Custom implementations can be implemented by subclassing this class. Otherwise it's completely legitimate
    /// to use this class on its own.
    /// </summary>
    public class MetaConstructFactory
    {
        private readonly StandardTypesFactory _factory;
        public StandardTypesFactory StandardTypesFactory
        {
            get { return _factory; }
        }

        public MetaConstructFactory() : this(new StandardTypesFactory())
        {
        }

        public MetaConstructFactory(StandardTypesFactory factory)
        {
            _factory = factory;
        }

        public virtual NamespaceDefinition CreateNamespace(MetaDefinition metaDef, XmlElement elem)
        {
            return new NamespaceDefinition(metaDef, elem);
        }
  
        public virtual ClassDefinition CreateClass(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
        {
            return new ClassDefinition(nsDef, surroundingClass, elem);
        }

        public virtual StructDefinition CreateStruct(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
        {
            return new StructDefinition(nsDef, surroundingClass, elem);
        }

        public virtual TypedefDefinition CreateTypedef(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
        {
            return new TypedefDefinition(nsDef, surroundingClass, elem);
        }

        public virtual EnumDefinition CreateEnum(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
        {
            return new EnumDefinition(nsDef, surroundingClass, elem);
        }

        /// <summary>
        /// Creates an instance of this class from the specified xml element. This method will
        /// create an instance from an apropriate subclass (e.g. <see cref="ClassDefinition"/> for a class).
        /// </summary>
        /// <returns>Returns the new instance or "null" in case of a global variable.</returns>
        public AbstractTypeDefinition CreateType(NamespaceDefinition nsDef, ClassDefinition surroundingClass, XmlElement elem)
        {
            switch (elem.Name)
            {
                case "class":
                    return CreateClass(nsDef, surroundingClass, elem);
                case "struct":
                    return CreateStruct(nsDef, surroundingClass, elem);
                case "typedef":
                    return CreateTypedef(nsDef, surroundingClass, elem);
                case "enumeration":
                    return CreateEnum(nsDef, surroundingClass, elem);
                case "variable":
                    //It's global variables, ignore them
                    return null;
                default:
                    throw new InvalidOperationException("Type unknown: '" + elem.Name + "'");
            }
        }
    }
}