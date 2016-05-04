using System.Xml;
using AutoWrap.Meta;

namespace AutoWrap.Mogre
{
    internal class MogreNamespaceDefinition : NamespaceDefinition
    {
        public MogreNamespaceDefinition(MetaDefinition metaDef, XmlElement elem) : base(metaDef, elem)
        {
        }

        public override T DetermineType<T>(string name, bool raiseException = true)
        {
            if (name == "DisplayString")
                return (T)(object)new DefUtfString(this);

            return base.DetermineType<T>(name, raiseException);
        }
    }
}