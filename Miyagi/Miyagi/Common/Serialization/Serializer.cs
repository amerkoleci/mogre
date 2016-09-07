/*
 Miyagi v1.2.1
 Copyright (c) 2008 - 2012 Tobias Bohnen

 Permission is hereby granted, free of charge, to any person obtaining a copy of this
 software and associated documentation files (the "Software"), to deal in the Software
 without restriction, including without limitation the rights to use, copy, modify, merge,
 publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 to whom the Software is furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all copies or
 substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 DEALINGS IN THE SOFTWARE.
 */
namespace Miyagi.Common.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    using Miyagi.Internals;

    /// <summary>
    /// The base class for serializers.
    /// </summary>
    public abstract class Serializer
    {
        #region Fields

        private static readonly Dictionary<Type, List<PropertyDescriptor>> PropertyCache = new Dictionary<Type, List<PropertyDescriptor>>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Serializer"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        protected Serializer(SerializationManager manager)
        {
            this.SerializationManager = manager;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the list of supported file extensions.
        /// </summary>
        public abstract IList<string> FileExtensions
        {
            get;
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the miyagi system.
        /// </summary>
        /// <value>The miyagi system.</value>
        protected MiyagiSystem MiyagiSystem
        {
            get
            {
                return this.SerializationManager.MiyagiSystem;
            }
        }

        /// <summary>
        /// Gets the serialization manager.
        /// </summary>
        protected SerializationManager SerializationManager
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Exports to a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="serializationData">The SerializationData.</param>
        public abstract void ExportToStream(Stream stream, IDictionary<IManager, SerializationData> serializationData);

        /// <summary>
        /// Imports from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public abstract void ImportFromStream(Stream stream);

        #endregion Public Methods

        #region Protected Static Methods

        /// <summary>
        /// Convert an invariant string to an object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="s">The invariant string.</param>
        /// <returns></returns>
        protected static object ConvertFromInvariantString(Type type, string s)
        {
            return TypeDescriptor.GetConverter(type).ConvertFromInvariantString(s);
        }

        /// <summary>
        /// Convert an object to an invariant string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        protected static string ConvertToInvariantString(object obj)
        {
            return obj as string ?? TypeDescriptor.GetConverter(obj).ConvertToInvariantString(obj);
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected static IList<PropertyDescriptor> GetProperties(Type type)
        {
            var retValue = new List<PropertyDescriptor>();

            if (PropertyCache.ContainsKey(type))
            {
                retValue = PropertyCache[type];
            }
            else
            {
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(type);
                pdc = pdc.Sort();
                var loadLast = new List<PropertyDescriptor>();
                foreach (PropertyDescriptor prop in pdc)
                {
                    var att = prop.GetAttribute<SerializerOptionsAttribute>();

                    if ((!prop.IsReadOnly || (prop.PropertyType.GetInterface(typeof(IEnumerable).FullName) != null && prop.PropertyType != typeof(string)))
                        && !(att != null && att.Ignore)
                        && prop.Attributes[typeof(ObsoleteAttribute)] == null)
                    {
                        if (att != null && att.LoadLast)
                        {
                            loadLast.Add(prop);
                        }
                        else
                        {
                            retValue.Add(prop);
                        }
                    }
                }

                if (loadLast.Count > 0)
                {
                    retValue.AddRange(loadLast);
                }

                PropertyCache[type] = retValue;
            }

            return retValue;
        }

        #endregion Protected Static Methods

        #endregion Methods
    }
}