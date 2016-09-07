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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Linq;

    using Miyagi.Common.Data;
    using Miyagi.Common.Serialization;
    using Miyagi.Internals;
    using Miyagi.Internals.ThirdParty;

    /// <summary>
    /// A Font represents a font.
    /// </summary>
    [SerializableType]
    public abstract class Font : INamable, IXmlWritable
    {
        #region Fields

        private static readonly NameGenerator NameGenerator = new NameGenerator();

        private readonly FastSmartWeakEvent<PropertyChangedEventHandler> propertyChanged = new FastSmartWeakEvent<PropertyChangedEventHandler>();

        private bool isLeadingUserSet;
        private bool isSpaceWidthUserSet;
        private int leading;
        private int spaceWidth;
        private int tabSize;
        private int tracking;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Font class.
        /// </summary>
        /// <param name="name">The name of the Font.</param>
        protected Font(string name)
        {
            this.TabSize = 3;
            NameGenerator.NextWhenNullOrEmpty(this, name);
            this.GlyphCoordinates = new Dictionary<char, RectangleF>();

            if (Default == null)
            {
                Default = this;
            }
        }

        #endregion Constructors

        #region Events

        internal event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                this.propertyChanged.Add(value);
            }

            remove
            {
                this.propertyChanged.Remove(value);
            }
        }

        #endregion Events

        #region Properties

        #region Public Static Properties

        /// <summary>
        /// Gets or sets the default font.
        /// </summary>
        public static Font Default
        {
            get;
            set;
        }

        #endregion Public Static Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the filename of the font.
        /// </summary>
        /// <value>The filename of the font.</value>
        public string FileName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the vertical spacing.
        /// </summary>
        public int Leading
        {
            get
            {
                return this.isLeadingUserSet ? this.leading : (int)(this.GetUVCoordinates('l').Height * this.TextureSize.Height);
            }

            set
            {
                if (this.leading != value)
                {
                    this.leading = value;
                    this.isLeadingUserSet = true;
                    this.OnPropertyChanged("Leading");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the Font.
        /// </summary>
        /// <value>The name of the Font.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the space character.
        /// </summary>
        /// <value>The width of the space character.</value>
        public int SpaceWidth
        {
            get
            {
                return this.isSpaceWidthUserSet ? this.spaceWidth : (int)(this.GetUVCoordinates(' ').Width * this.TextureSize.Width);
            }

            set
            {
                if (this.spaceWidth != value)
                {
                    this.spaceWidth = value;
                    this.isSpaceWidthUserSet = true;
                    this.OnPropertyChanged("SpaceWidth");
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of spaces that a tab represents.
        /// </summary>
        /// <value>An <see cref="int"/> representing the number of spaces that a tab represents.</value>
        public int TabSize
        {
            get
            {
                return this.tabSize;
            }

            set
            {
                if (this.tabSize != value && value > 1)
                {
                    this.tabSize = value;
                    this.OnPropertyChanged("TabSize");
                }
            }
        }

        /// <summary>
        /// Gets the name of the texture of the font.
        /// </summary>
        public abstract string TextureName
        {
            get;
        }

        /// <summary>
        /// Gets or sets the horizontal spacing.
        /// </summary>
        public int Tracking
        {
            get
            {
                return this.tracking;
            }

            set
            {
                if (this.tracking != value)
                {
                    this.tracking = value;
                    this.OnPropertyChanged("Tracking");
                }
            }
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets or sets the size of the font texture.
        /// </summary>
        protected internal Size TextureSize
        {
            get;
            protected set;
        }

        #endregion Protected Internal Properties

        #region Protected Properties

        /// <summary>
        /// Gets or sets the glyph coordinates.
        /// </summary>
        protected IDictionary<char, RectangleF> GlyphCoordinates
        {
            get;
            set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the size required to fit the given text, including line breaks.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>A size where the text can fit.</returns>
        public Size MeasureString(string text)
        {
            return this.MeasureString(text, Size.Empty, false);
        }

        /// <summary>
        /// Gets the size required to fit the given text, including line breaks.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns>A size where the text can fit.</returns>
        public Size MeasureString(string text, Size maxSize, bool multiline)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Size.Empty;
            }

            int width = 0;
            int maxLineWidth = 0;            
            int height = this.Leading;

            if (!multiline
                && this is TrueTypeFont)
                height = ((TrueTypeFont)this).Size;

            int tempLeading = height;

            var uvRects = this.GetUVCoordinates(text);
            for (int i = 0; i < text.Length; i++)
            {
                bool isReturn = false;
                int charWidth = (int)Math.Ceiling(this.GetCharWidth(text[i], uvRects[i], ref isReturn));
                if (!char.IsWhiteSpace(text[i]))
                {
                    charWidth += this.Tracking;
                }

                if (!maxSize.IsEmpty && width + charWidth > maxSize.Width)
                {
                    isReturn = true;
                }

                if (!isReturn)
                {
                    width += charWidth;
                }
                else
                {
                    if (!maxSize.IsEmpty && height + tempLeading > maxSize.Height)
                    {
                        break;
                    }

                    height += tempLeading;
                    maxLineWidth = Math.Max(maxLineWidth, width);
                    width = 0;
                }
            }

            maxLineWidth = Math.Max(maxLineWidth, width);

            return new Size(maxLineWidth, height);
        }

        /// <summary>
        /// Converts the font to an XElement.
        /// </summary>
        /// <returns>An <see cref="XElement"/> representing the font.</returns>
        public abstract XElement ToXElement();

        #endregion Public Methods

        #region Internal Methods

        internal float GetCharWidth(char c, out RectangleF uvRect)
        {
            bool buf = false;
            uvRect = this.GetUVCoordinates(c);
            return this.GetCharWidth(c, uvRect, ref buf);
        }

        #endregion Internal Methods

        #region Protected Methods

        /// <summary>
        /// Creates the font.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        protected abstract void CreateFont(MiyagiSystem system);

        /// <summary>
        /// Handles property changes.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.propertyChanged != null)
            {
                this.propertyChanged.Raise(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private float GetCharWidth(char c, RectangleF uvRect, ref bool carriageReturn)
        {
            float retValue;

            switch (c)
            {
                case ' ':
                    retValue = this.SpaceWidth;
                    break;

                case '\t':
                    retValue = this.SpaceWidth * this.TabSize;
                    break;

                case '\n':
                    retValue = 0;
                    carriageReturn = true;
                    break;

                default:
                    retValue = uvRect.Width * this.TextureSize.Width;
                    break;
            }

            return retValue;
        }

        private RectangleF[] GetUVCoordinates(string text)
        {
            var uvrects = new RectangleF[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                uvrects[i] = this.GetUVCoordinates(text[i]);
            }

            return uvrects;
        }

        private RectangleF GetUVCoordinates(char c)
        {
            return this.GlyphCoordinates.ContainsKey(c) ? this.GlyphCoordinates[c] : RectangleF.Empty;
        }

        #endregion Private Methods

        #endregion Methods
    }
}