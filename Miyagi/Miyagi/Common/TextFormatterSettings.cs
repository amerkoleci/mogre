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
namespace Miyagi.Common
{
    using Miyagi.Common.Data;
    using Miyagi.Common.Resources;

    /// <summary>
    /// Represents settings for the <see cref="TextFormatter"/>.
    /// </summary>
    public sealed class TextFormatterSettings
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatterSettings"/> class.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <param name="colourDefinition">The colour definition.</param>
        /// <param name="font">The font.</param>
        /// <param name="multiline">if set to <c>true</c> the text can extend more than one line.</param>
        /// <param name="wordWrap">if set to <c>true</c> words should be wrapped to the next line when necessary.</param>
        /// <param name="viewportSize">Size of the viewport.</param>
        public TextFormatterSettings(Alignment alignment, ColourDefinition colourDefinition, Font font, bool multiline, bool wordWrap, Size viewportSize)
        {
            this.Alignment = alignment;
            this.ColourDefinition = colourDefinition;
            this.Font = font;
            this.Multiline = multiline;
            this.WordWrap = wordWrap;
            this.ViewportSize = viewportSize;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the alignment.
        /// </summary>
        public Alignment Alignment
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the colour definition.
        /// </summary>
        public ColourDefinition ColourDefinition
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the font.
        /// </summary>
        public Font Font
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the text can extend more than one line.
        /// </summary>
        public bool Multiline
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the size of the viewport.
        /// </summary>
        public Size ViewportSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether words should be wrapped to the next line when necessary.
        /// </summary>
        public bool WordWrap
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties
    }
}