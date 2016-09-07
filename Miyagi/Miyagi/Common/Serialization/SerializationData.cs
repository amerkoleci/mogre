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
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A dictionary a serializable data.
    /// </summary>
    public class SerializationData : IEnumerable<KeyValuePair<string, object>>
    {
        #region Fields

        private readonly Dictionary<string, object> dict;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationData"/> class.
        /// </summary>
        public SerializationData()
        {
            this.dict = new Dictionary<string, object>();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this instance has data.
        /// </summary>
        /// <value><c>true</c> if this instance has data; otherwise, <c>false</c>.</value>
        public bool HasData
        {
            get
            {
                return this.dict.Count > 0;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        public object this[string key]
        {
            get
            {
                return this.dict.ContainsKey(key) ? this.dict[key] : null;
            }
        }

        #endregion Indexers

        #region Methods

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dict.GetEnumerator();
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="obj">The obj.</param>
        public void Add(string key, object obj)
        {
            this.dict[key] = obj;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.dict.GetEnumerator();
        }

        #endregion Public Methods

        #endregion Methods
    }
}