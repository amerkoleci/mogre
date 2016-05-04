using System;
using System.Xml;

namespace AutoWrap.Meta
{
    /// <summary>
    /// Describes a field (i.e. a variable or constant; either static or not) of a class. Note that
    /// this doesn't describe a CLR property. This is done with <see cref="MemberPropertyDefinition"/>.
    /// </summary>
    public class MemberFieldDefinition : MemberDefinitionBase
    {
        public override bool IsConst
        {
            get { return Definition.StartsWith("const "); }
        }

        private string _fullNativeName;

        public virtual string FullNativeName
        {
            get { return _fullNativeName; }
        }

        private string _arraySize;

        public virtual string ArraySize
        {
            get { return _arraySize; }
        }

        public virtual bool IsNativeArray
        {
            get { return (_arraySize != null); }
        }

        public MemberFieldDefinition(XmlElement elem, ClassDefinition containingClass)
            : base(elem, containingClass)
        {
            if (elem.Name != "variable")
                throw new Exception("Wrong element; expected 'variable'.");

            _fullNativeName = this.Definition.Substring(this.Definition.LastIndexOf(" ") + 1);
            if (_fullNativeName.Contains("["))
            {
                //It's native array
                int index = _fullNativeName.IndexOf("[");
                int last = _fullNativeName.IndexOf("]");
                string size = _fullNativeName.Substring(index + 1, last - index - 1);
                if (size != "")
                {
                    _arraySize = size;
                    _fullNativeName = _fullNativeName.Substring(0, index);
                }
            }
        }
    }
}