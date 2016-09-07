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
namespace Miyagi.Common.Resources
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A keyed collection of textures.
    /// </summary>
    public class TextureCollection : IEnumerable<KeyValuePair<string, Texture>>
    {
        #region Fields

        private readonly SortedList<string, Texture> items = new SortedList<string, Texture>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextureCollection class.
        /// </summary>
        public TextureCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TextureCollection class.
        /// </summary>
        /// <param name="textures">The textures.</param>
        internal TextureCollection(IEnumerable<KeyValuePair<string, Texture>> textures)
        {
            foreach (KeyValuePair<string, Texture> kvp in textures)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when a texture has been changed.
        /// </summary>
        public event EventHandler TextureChanged;

        #endregion Events

        #region Indexers

        /// <summary>
        /// Gets or sets a texture by skin name.
        /// </summary>
        /// <param name="key">The key of the texture.</param>
        public Texture this[string key]
        {
            get
            {
                Texture retValue;
                this.items.TryGetValue(key, out retValue);
                return retValue;
            }

            set
            {
                if (this[key] == value)
                {
                    return;
                }

                this.items[key] = value;

                if (this.TextureChanged != null)
                {
                    this.TextureChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion Indexers

        #region Methods

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Determinates whether the TextureCollection contains a texture of the specified key.
        /// </summary>
        /// <param name="key">The key of the texture.</param>
        /// <returns><c>true</c> if the TextureCollection contains a texture of the specified key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(string key)
        {
            return this.items.ContainsKey(key);
        }

        /// <summary>
        /// Returns the enumerator of the collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<KeyValuePair<string, Texture>> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <summary>
        /// Removes the texture.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RemoveTexture(string key)
        {
            this.items.Remove(key);
            if (this.TextureChanged != null)
            {
                this.TextureChanged(this, EventArgs.Empty);
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}