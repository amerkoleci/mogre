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

    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;

    /// <summary>
    /// A frame of a texture.
    /// </summary>
    public sealed class TextureFrame : IDeepCopiable<TextureFrame>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextureFrame class.
        /// </summary>
        /// <param name="fileName">The filename of the source of frame.</param>
        /// <param name="uvCoordinates">A RectangleF representing the uv coordinates.</param>
        /// <param name="duration">A TimeSpan representing the duration the frame will be displayed.</param>
        public TextureFrame(string fileName, RectangleF uvCoordinates, TimeSpan duration)
        {
            this.FileName = Backend.NormalizeFilePath(fileName);
            this.UV = uvCoordinates;
            this.Duration = duration;
        }

        /// <summary>
        /// Initializes a new instance of the TextureFrame class.
        /// </summary>
        /// <param name="fileName">The filename of the source of frame.</param>
        /// <param name="uvCoordinates">A RectangleF representing the uv coordinates.</param>
        /// <param name="duration">The duration the frame will be displayed in milliseconds.</param>
        public TextureFrame(string fileName, RectangleF uvCoordinates, int duration)
            : this(fileName, uvCoordinates, TimeSpan.FromMilliseconds(duration))
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the time the frame will be displayed
        /// </summary>
        public TimeSpan Duration
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the filename of the source of the frame.
        /// </summary>
        public string FileName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the uv coordinates of the frame.
        /// </summary>
        public RectangleF UV
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Creates a deep copy of the TextureFrame.
        /// </summary>
        /// <returns>A deep copy of the TextureFrame.</returns>
        public TextureFrame CreateDeepCopy()
        {
            return (TextureFrame)this.MemberwiseClone();
        }

        #endregion Public Methods

        #endregion Methods
    }
}