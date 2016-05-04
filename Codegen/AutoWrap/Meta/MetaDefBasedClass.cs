namespace AutoWrap.Meta
{
    // TODO by manski: If this class is at the end only subclasses by AbstractCodeProducer, remove it.
    public abstract class MetaDefBasedClass
    {
        public readonly MetaDefinition MetaDef;

        protected MetaDefBasedClass(MetaDefinition metaDef)
        {
            MetaDef = metaDef;
        }
    }
}