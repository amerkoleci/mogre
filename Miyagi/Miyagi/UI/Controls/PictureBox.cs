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
    using System.Drawing;
    using System.IO;

    using SD = System.Drawing;

    /// <summary>
    /// A PictureBox.
    /// </summary>
    public class PictureBox : BitmapControl
    {
        #region Fields

        private SD.Image tempImage;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PictureBox class.
        /// </summary>
        public PictureBox()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PictureBox class.
        /// </summary>
        /// <param name="name">The name of the PictureBox.</param>
        public PictureBox(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Loads a image from the specified byte array.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public void Load(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                this.Load(ms);
            }
        }

        /// <summary>
        /// Loads a image from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Load(Stream stream)
        {
            this.ThrowIfDisposed();
            this.tempImage = Image.FromStream(stream);
            this.NeedsUpdate = true;
        }

        /// <summary>
        /// Loads a image from the specified file.
        /// </summary>
        /// <param name="fileName">A string representing the name of the file.</param>
        public void Load(string fileName)
        {
            this.ThrowIfDisposed();

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                this.Load(fs);
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
            base.Dispose(disposing);
            if (disposing)
            {
                if (this.tempImage != null)
                {
                    this.tempImage.Dispose();
                    this.tempImage = null;
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="Bitmap"/> changes.
        /// </summary>
        protected override void OnBitmapChanged()
        {
            base.OnBitmapChanged();
            if (this.tempImage != null)
            {
                this.tempImage.Dispose();
                this.tempImage = null;
            }
        }

        /// <summary>
        /// Updates the Bitmap
        /// </summary>
        /// <remarks>This is called <see cref="BitmapControl.NeedsUpdate"/> is true.</remarks>
        protected override void UpdateBitmap()
        {
            if (this.tempImage == null)
            {
                if (this.Bitmap == null)
                {
                    return;
                }

                this.tempImage = (Bitmap)this.Bitmap.Clone();
            }

            if (this.Bitmap == null || (this.Bitmap.Width != this.Width || this.Bitmap.Height != this.Height))
            {
                int width = this.Width;
                int height = this.Height;

                if (width > 0 && height > 0)
                {
                    this.SetBitmap(new SD.Bitmap(width, height));
                    using (var g = SD.Graphics.FromImage(this.Bitmap))
                    {
                        g.DrawImage(this.tempImage, 0, 0, width, height);
                    }
                }
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}