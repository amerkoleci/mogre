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
    using System.Linq;

    using Miyagi.Common;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Internals;

    /// <summary>
    /// A 2D layer that can display multiple 2D elements.
    /// </summary>
    public class Layer : IDisposable, INamable
    {
        #region Fields

        private static readonly NameGenerator NameGenerator = new NameGenerator();

        private readonly MiyagiCollection<LayerElement> layerElements;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Layer class.
        /// </summary>
        public Layer()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Layer class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        public Layer(string name)
        {
            NameGenerator.NextWhenNullOrEmpty(this, name);
            this.layerElements = new MiyagiCollection<LayerElement>();
            this.layerElements.ItemAdded += this.ElementAdded;
            this.layerElements.ItemInserted += this.ElementAdded;
            this.layerElements.ItemRemoved += ElementRemoved;
        }

        /// <summary>
        /// Finalizes an instance of the Layer class.
        /// </summary>
        ~Layer()
        {
            this.Dispose(false);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the list of elements.
        /// </summary>
        public MiyagiCollection<LayerElement> Elements
        {
            get
            {
                return this.layerElements;
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

        /// <summary>
        /// Gets the SpriteRenderer.
        /// </summary>
        public ISpriteRenderer SpriteRenderer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the TwoDManager.
        /// </summary>
        public TwoDManager TwoDManager
        {
            get;
            internal set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Destroys the renderer.
        /// </summary>
        public void DestroyRenderer()
        {
            if (this.TwoDManager.MiyagiSystem.HasManager("Render") && this.SpriteRenderer != null)
            {
                this.TwoDManager.MiyagiSystem.RenderManager.DestroyRenderer(this.SpriteRenderer);
                this.SpriteRenderer = null;
            }
        }

        /// <summary>
        /// Disposes the Layer.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets an element of the specified type by name.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <returns>The first element of that name and type if it exists, otherwise null.</returns>
        /// <typeparam name="T">The type of the element.</typeparam>
        public T GetElement<T>(string name)
            where T : LayerElement
        {
            return this.Elements.Where(element => element.Name == name && element.GetType() == typeof(T)).Cast<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets a element by name.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <returns>If it exists the first element with that name, otherwise null.</returns>
        public LayerElement GetElement(string name)
        {
            return this.Elements.FirstOrDefault(element => element.Name == name);
        }

        /// <summary>
        /// Updates the Layer.
        /// </summary>
        public void Update()
        {
            if (this.layerElements.Count > 0)
            {
                if (this.SpriteRenderer == null)
                {
                    this.SpriteRenderer = this.TwoDManager.MiyagiSystem.RenderManager.Create2DRenderer();
                }

                foreach (LayerElement element in this.layerElements)
                {
                    element.Update();
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the Layer.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            foreach (LayerElement element in this.layerElements)
            {
                element.Dispose();
            }

            this.DestroyRenderer();
            this.layerElements.Clear();
        }

        #endregion Protected Methods

        #region Private Static Methods

        private static void ElementRemoved(object sender, CollectionEventArgs<LayerElement> e)
        {
            e.Item.Layer = null;
        }

        #endregion Private Static Methods

        #region Private Methods

        private void ElementAdded(object sender, CollectionEventArgs<LayerElement> e)
        {
            e.Item.Layer = this;
        }

        #endregion Private Methods

        #endregion Methods
    }
}