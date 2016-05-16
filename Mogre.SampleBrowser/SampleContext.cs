using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mogre.SampleBrowser
{
    public class SampleContext
    {
        //static readonly Dictionary<TypeInfo, Func<Sample>> _samples = new Dictionary<TypeInfo, Func<Sample>>();
        static readonly List<SampleInfo> _samples = new List<SampleInfo>();

        public IEnumerable<SampleInfo> Samples
        {
            get { return _samples; }
        }

        static SampleContext()
        {
            var sampleTypeInfo = typeof(Sample).GetTypeInfo();

            foreach (var typeInfo in typeof(SampleContext).GetTypeInfo().Assembly.DefinedTypes)
            {
                if (typeInfo.IsAbstract == false &&
                    sampleTypeInfo.IsAssignableFrom(typeInfo))
                {
                    var displayNameAttribute = typeInfo.GetCustomAttribute<DisplayNameAttribute>();
                    var descriptionAttribute = typeInfo.GetCustomAttribute<DescriptionAttribute>();

                    _samples.Add(new SampleInfo(
                        displayNameAttribute != null ? displayNameAttribute.DisplayName : typeInfo.Name,
                        descriptionAttribute != null ? descriptionAttribute.Description : typeInfo.Name,
                        () => (Sample)Activator.CreateInstance(typeInfo.AsType())
                        )
                       );
                   // _samples.Add(typeInfo, () => (Sample)Activator.CreateInstance(typeInfo.AsType()));
                }
            }
        }

    }

    public sealed class SampleInfo
    {
        readonly Func<Sample> _factory;

        public string Name { get; private set; }
        public string Description { get; private set; }

        public SampleInfo(string name, string description, Func<Sample> factory)
        {
            Name = name;
            Description = description;
            _factory = factory;
        }

        public Sample Create()
        {
            return _factory();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
