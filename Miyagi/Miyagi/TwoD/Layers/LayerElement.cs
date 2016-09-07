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
namespace Miyagi.TwoD.Layers
{
    using System;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Internals;

    /// <summary>
    /// The abstract base class for layer elements.
    /// </summary>
    public abstract class LayerElement : IDisposable, INamable
    {
        #region Fields

        private static readonly NameGenerator NameGenerator = new NameGenerator();

        private Layer layer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Element class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        protected LayerElement(string name)
        {
            NameGenerator.NextWhenNullOrEmpty(this, name);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the Layer.
        /// </summary>
        public Layer Layer
        {
            get
            {
                return this.layer;
            }

            internal set
            {
                if (this.layer != value)
                {
                    Layer oldLayer = this.layer;
                    this.layer = value;

                    if (this.layer != null && !this.layer.Elements.Contains(this))
                    {
                        this.layer.Elements.Add(this);
                    }

                    if (oldLayer != null)
                    {
                        this.DestroyElement();
                        if (oldLayer.Elements.Contains(this))
                        {
                            oldLayer.Elements.Remove(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the size of the viewport.
        /// </summary>
        protected Size ViewportSize
        {
            get
            {
                return this.layer != null ? this.layer.SpriteRenderer.Viewport.Size : Size.Empty;
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Disposes the Element.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Updates the Element.
        /// </summary>
        public abstract void Update();

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Destroys the Element.
        /// </summary>
        protected abstract void DestroyElement();

        /// <summary>
        /// Disposes the Element.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion Protected Methods

        #endregion Methods
    }
}