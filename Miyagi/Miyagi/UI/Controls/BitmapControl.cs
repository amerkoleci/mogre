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
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Security.Permissions;

    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Common.Serialization;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Elements;

    using Size = Miyagi.Common.Data.Size;

    /// <summary>
    /// A base class for Bitmap controls.
    /// </summary>
    public abstract class BitmapControl : Control
    {
        #region Fields

        private Bitmap bitmap;
        private TextureElement textureElement;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BitmapControl class.
        /// </summary>
        /// <param name="name">The name of the BitmapControl.</param>
        protected BitmapControl(string name)
            : base(name)
        {
            this.TextureName = "Miyagi_BitmapControlTexture_" + this.Name + Guid.NewGuid();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the used Bitmap.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public Bitmap Bitmap
        {
            get
            {
                return this.bitmap;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.bitmap != value)
                {
                    this.SetBitmap(value);
                    this.OnBitmapChanged();
                }
            }
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
        /// Gets or sets a value indicating whether the Bitmap needs to be written to the texture.
        /// </summary>
        protected bool NeedsUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement
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

        #region Public Methods

        /// <summary>
        /// Saves the Bitmap to the specified file.
        /// </summary>
        /// <param name="fileName">A string representing the name of the file.</param>
        /// <param name="format">An BitmapFormat representing the format.</param>
        public void Save(string fileName, ImageFormat format)
        {
            this.ThrowIfDisposed();
            if (this.bitmap != null)
            {
                this.bitmap.Save(fileName, format);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the control.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveTexture();
            }

            if (this.bitmap != null)
            {
                this.bitmap.Dispose();
            }

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
        /// Called when the <see cref="Bitmap"/> changes.
        /// </summary>
        protected virtual void OnBitmapChanged()
        {
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnSizeChanged(ChangedValueEventArgs<Size> e)
        {
            base.OnSizeChanged(e);

            if (this.textureElement != null)
            {
                this.TextureElement.Size = e.NewValue;
            }

            this.RemoveTexture();
            this.NeedsUpdate = true;
        }

        /// <summary>
        /// Removes the texture.
        /// </summary>
        protected void RemoveTexture()
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
        /// Sets the bitmap.
        /// </summary>
        protected void SetBitmap(Bitmap bmp)
        {
            if (this.bitmap != null)
            {
                this.bitmap.Dispose();
            }

            this.bitmap = bmp;
            this.NeedsUpdate = true;
        }

        /// <summary>
        /// Updates the <see cref="Bitmap"/>.
        /// </summary>
        /// <remarks>This is called <see cref="NeedsUpdate"/> is true.</remarks>
        protected abstract void UpdateBitmap();

        /// <summary>
        /// Updates the control.
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand)]
        protected override void UpdateCore()
        {
            base.UpdateCore();

            if (this.NeedsUpdate)
            {
                this.UpdateBitmap();
                this.ConvertBitmap();
                this.NeedsUpdate = false;

                this.TextureElement.Texture = this.Bitmap != null
                                                  ? new Texture(this.TextureName)
                                                  : null;
            }

            if (this.bitmap != null)
            {
                this.TextureElement.Update(this.DeltaLocation, this.DeltaSize);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        [SecurityPermission(SecurityAction.LinkDemand)]
        private void ConvertBitmap()
        {
            if (this.Bitmap != null)
            {
                Backend backend = this.MiyagiSystem.Backend;
                var nativeTextureHandle = backend.CreateTexture(this.TextureName, new Size(this.Bitmap.Size.Width, this.Bitmap.Size.Height));
                backend.WriteToTexture(this.Bitmap.ToByteArray(), nativeTextureHandle);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}