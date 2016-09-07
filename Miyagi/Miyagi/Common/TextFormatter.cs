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
    using System;
    using System.Collections.Generic;

    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;

    /// <summary>
    /// Converts a string to a quad array.
    /// </summary>
    public sealed class TextFormatter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatter"/> class.
        /// </summary>
        /// <param name="textFormatterSettings">The text formatter settings.</param>
        public TextFormatter(TextFormatterSettings textFormatterSettings)
        {
            this.TextFormatterSettings = textFormatterSettings;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the text formatter settings.
        /// </summary>
        public TextFormatterSettings TextFormatterSettings
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Creates the text quads.
        /// </summary>
        /// <param name="rect">A <see cref="Rectangle"/> representing the bounds of the text.</param>
        /// <param name="text">The text.</param>
        /// <returns>The text quads.</returns>
        public Quad[] CreateTextQuads(Rectangle rect, string text)
        {
            // return a dummy if the text is empty
            if (string.IsNullOrEmpty(text))
            {
                return new[] { Quad.CreateDummy(rect.ToScreenCoordinates(this.TextFormatterSettings.ViewportSize)) };
            }

            var retValue = new List<Quad>(text.Length);

            var colour = this.TextFormatterSettings.ColourDefinition;
            var alignment = this.TextFormatterSettings.Alignment;
            bool multiline = this.TextFormatterSettings.Multiline;
            bool wordWrap = this.TextFormatterSettings.WordWrap && multiline;

            var font = this.TextFormatterSettings.Font;
            int lineSpacing = font.Leading;
            if (!multiline
                && font is TrueTypeFont)
                lineSpacing = ((TrueTypeFont)font).Size;

            int availableHeight = rect.Height;
            int cursorY = rect.Top;

            // split the text into lines
            var lineNodes = new LinkedList<string>(text.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            var lineNode = lineNodes.First;

            while (lineNode != null)
            {
                availableHeight -= lineSpacing;

                if (availableHeight >= 0)
                {
                    float availableWidth = rect.Width;
                    float cursorX = rect.Left;

                    string currentLine = lineNode.Value;
                    int lastWordStartIndex = 0;
                    float wordWidth = 0;

                    var lineQuads = new List<Quad>();

                    // split the line into chars
                    for (int currentCharIndex = 0; currentCharIndex < currentLine.Length; currentCharIndex++)
                    {
                        char c = currentLine[currentCharIndex];
                        bool isWhiteSpace = char.IsWhiteSpace(c);
                        RectangleF uvRect;
                        float charWidth = font.GetCharWidth(c, out uvRect);

                        if (wordWrap)
                        {
                            // buffer start index of latest word
                            if (!char.IsWhiteSpace(c) && currentCharIndex > 0 && char.IsWhiteSpace(currentLine[currentCharIndex - 1]))
                            {
                                lastWordStartIndex = currentCharIndex;
                                wordWidth = 0;
                            }

                            // ...and its width
                            wordWidth += charWidth;
                        }

                        // check whether we're running out of horizontal space
                        availableWidth -= charWidth;
                        if (availableWidth < 0)
                        {
                            if (wordWrap && lastWordStartIndex > 0)
                            {
                                // remove last word from quads and add as next line
                                lineQuads.RemoveRange(lastWordStartIndex, lineQuads.Count - lastWordStartIndex);
                                lineNodes.AddAfter(lineNode, currentLine.Substring(lastWordStartIndex));
                                availableWidth += wordWidth;
                            }
                            else
                            {
                                // add rest of line as new line
                                lineNodes.AddAfter(lineNode, currentLine.Substring(currentCharIndex));
                                availableWidth += charWidth;
                            }

                            break;
                        }

                        // create the position rectangle
                        var posRect = new RectangleF(cursorX, cursorY, charWidth, uvRect.Height * font.TextureSize.Height);

                        var quad = this.CreateQuad(isWhiteSpace, colour, posRect, uvRect);
                        lineQuads.Add(quad);

                        cursorX += charWidth;

                        // add additional spacing
                        cursorX += font.Tracking;
                        availableWidth -= font.Tracking;
                    }

                    this.AlignQuadsHorizontally(lineQuads, alignment, rect, rect.Width - availableWidth);
                    retValue.AddRange(lineQuads);

                    cursorY += lineSpacing;
                }
                else
                {
                    availableHeight += lineSpacing;
                    break;
                }

                // take only the first line if we are not multiline
                if (!multiline)
                {
                    break;
                }

                lineNode = lineNode.Next;
            }

            this.AlignQuadsVertically(retValue, alignment, rect, rect.Height - availableHeight);
            return retValue.ToArray();
        }

        #endregion Public Methods

        #region Private Methods

        private void AlignQuadsHorizontally(IEnumerable<Quad> quads, Alignment alignment, Rectangle rect, float textWidth)
        {
            if (textWidth > rect.Width)
            {
                textWidth = rect.Width;
            }

            float offset = 0;

            switch (alignment)
            {
                case Alignment.MiddleLeft:
                case Alignment.BottomLeft:
                case Alignment.TopLeft:
                    return;

                case Alignment.MiddleCenter:
                case Alignment.BottomCenter:
                case Alignment.TopCenter:
                    offset = (rect.Width - textWidth) / 2;
                    break;

                case Alignment.MiddleRight:
                case Alignment.BottomRight:
                case Alignment.TopRight:
                    offset = rect.Width - textWidth;
                    break;
            }

            foreach (Quad q in quads)
            {
                float offsetf = (offset / this.TextFormatterSettings.ViewportSize.Width) * 2;
                q.Move(new PointF(offsetf, 0));
            }
        }

        private void AlignQuadsVertically(IEnumerable<Quad> quads, Alignment alignment, Rectangle rect, int textHeight)
        {
            if (textHeight > rect.Height)
            {
                textHeight = rect.Height;
            }

            int offset = 0;

            switch (alignment)
            {
                case Alignment.TopLeft:
                case Alignment.TopCenter:
                case Alignment.TopRight:
                    return;

                case Alignment.MiddleLeft:
                case Alignment.MiddleCenter:
                case Alignment.MiddleRight:
                    offset = (rect.Height - textHeight) / 2;
                    break;

                case Alignment.BottomLeft:
                case Alignment.BottomCenter:
                case Alignment.BottomRight:
                    offset = rect.Height - textHeight;
                    break;
            }

            foreach (Quad q in quads)
            {
                float offsetf = -((offset / (float)this.TextFormatterSettings.ViewportSize.Height) * 2);
                q.Move(new PointF(0, offsetf));
            }
        }

        private Quad CreateQuad(bool isEmpty, ColourDefinition color, RectangleF rect, RectangleF uvrect)
        {
            rect = rect.ToScreenCoordinates(this.TextFormatterSettings.ViewportSize);
            return isEmpty
                       ? Quad.CreateDummy(rect)
                       : new Quad(color, rect, uvrect);
        }

        #endregion Private Methods

        #endregion Methods
    }
}