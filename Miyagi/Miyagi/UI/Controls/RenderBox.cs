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
namespace Miyagi.UI.Controls
{
    using System;
    using System.Collections.Generic;

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Resources;
    using Miyagi.Common.Serialization;
    using Miyagi.UI.Controls.Elements;

    /// <summary>
    /// A RenderBox.
    /// </summary>
    public class RenderBox : Control
    {
        #region Fields

        private Colour backgroundColour;
        private object camera;
        private TextureElement textureElement;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RenderBox class.
        /// </summary>
        public RenderBox()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RenderBox class.
        /// </summary>
        /// <param name="name">The name of the RenderBox.</param>
        public RenderBox(string name)
            : base(name)
        {
            this.TextureName = "Miyagi_RenderBoxTexture_" + this.Name + Guid.NewGuid();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the background colour.
        /// </summary>
        public Colour BackgroundColour
        {
            get
            {
                return this.backgroundColour;
            }

            set
            {
                if (this.backgroundColour != value)
                {
                    this.backgroundColour = value;
                    this.NeedsUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the camera.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [SerializerOptions(Ignore = true)]
        public object Camera
        {
            get
            {
                return this.camera;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                if (this.camera != value)
                {
                    this.camera = value;
                    this.NeedsUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the camera should be resized when the control is resized.
        /// </summary>
        public bool ResizeCamera
        {
            get;
            set;
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        /// <value>A list of elements.</value>
        protected override IList<IElement> Elements
        {
            get
            {
                var retValue = base.Elements;
                if (this.textureElement != null)
                {
                    retValue.Add(this.textureElement);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the RenderBox has to be updated.
        /// </summary>
        protected bool NeedsUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        protected TextureElement TextureElement
        {
            get
            {
                return this.textureElement ?? (this.textureElement = new TextureElement(this, () => this.ZOrder * 10));
            }
        }

        /// <summary>
        /// Gets the name of the texture.
        /// </summary>
        protected string TextureName
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Creates the render texture.
        /// </summary>
        protected virtual void CreateRenderTexture()
        {
            if (this.MiyagiSystem != null)
            {
                this.MiyagiSystem.Backend.CreateRenderTexture(this.TextureName, this.Size, this.backgroundColour, this.Camera);
            }
        }

        /// <summary>
        /// Disposes the RenderBox.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveTexture();
            }

            this.camera = null;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Resizes the control.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            base.DoResize(widthFactor, heightFactor);
            this.NeedsUpdate = true;
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnSizeChanged(ChangedValueEventArgs<Size> e)
        {
            base.OnSizeChanged(e);

            if (this.ResizeCamera)
            {
                this.NeedsUpdate = true;
            }
        }

        /// <summary>
        /// Removes the texture.
        /// </summary>
        protected virtual void RemoveTexture()
        {
            if (this.textureElement != null && this.textureElement.Style != null)
            {
                this.TextureElement.Texture = null;
            }

            if (this.MiyagiSystem != null)
            {
                this.MiyagiSystem.Backend.RemoveTexture(this.TextureName);
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();

            if (this.NeedsUpdate && this.camera != null)
            {
                this.RemoveTexture();
                this.CreateRenderTexture();

                this.NeedsUpdate = false;

                this.TextureElement.Texture = new Texture(this.TextureName);
            }

            this.TextureElement.Update(this.DeltaLocation, this.DeltaSize);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}