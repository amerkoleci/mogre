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
    using System.ComponentModel;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Common.Serialization;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// A item of a ListControl.
    /// </summary>
    [SerializableType]
    public class ListItem : INamable, IToolTipElementOwner, IDisposable
    {
        #region Fields

        private bool isMouseOver;
        private ListItemElement listItemElement;
        private bool selected;
        private ToolTipElement toolTipElement;
        private string toolTipText;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListItem"/> class.
        /// </summary>
        public ListItem()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListItem"/> class.
        /// </summary>
        /// <param name="text">The <see cref="Text"/> and <see cref="Name"/> of the <see cref="ListItem"/>.</param>
        public ListItem(string text)
        {
            this.Name = this.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListItem"/> class.
        /// </summary>
        /// <param name="text">The <see cref="Text"/> and <see cref="Name"/> of the <see cref="ListItem"/>.</param>
        /// <param name="texture">The filename of the <see cref="Texture"/>.</param>
        public ListItem(string text, string texture)
            : this(text)
        {
            this.Style.Texture = new Texture(texture);
        }

        #endregion Constructors

        #region Properties

        #region Explicit Interface Properties

        Rectangle IElementOwner.DisplayRectangle
        {
            get
            {
                return new Rectangle(Point.Empty, this.Size);
            }
        }

        Rectangle ITextElementOwner.TextBounds
        {
            get
            {
                return new Rectangle(Point.Empty, this.Size);
            }
        }

        #endregion Explicit Interface Properties

        #region Public Properties

        /// <summary>
        /// Gets the displayed text.
        /// </summary>
        public string DisplayedText
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is over the item.
        /// </summary>
        /// <value><c>true</c> if the mouse is over the item; otherwise, <c>false</c>.</value>
        public bool IsMouseOver
        {
            get
            {
                return this.isMouseOver;
            }

            internal set
            {
                if (this.isMouseOver != value)
                {
                    this.isMouseOver = value;
                    this.OnIsMouseOverChanged();
                }
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        public Point Location
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the <see cref="MiyagiSystem"/>.
        /// </summary>
        public MiyagiSystem MiyagiSystem
        {
            get
            {
                return this.Owner == null ? null : this.Owner.MiyagiSystem;
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
        /// Gets the opacity.
        /// </summary>
        /// <value>The opacity, ranging between 0 and 1.</value>
        public float Opacity
        {
            get
            {
                return this.Owner.Opacity;
            }
        }

        /// <summary>
        /// Gets the owning <see cref="ListControl"/>.
        /// </summary>
        public ListControl Owner
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is selected.
        /// </summary>
        public bool Selected
        {
            get
            {
                return this.selected;
            }

            set
            {
                if (this.selected != value)
                {
                    if (this.Owner != null)
                    {
                        if (value)
                        {
                            this.Owner.SelectItems(this);
                        }
                        else
                        {
                            this.Owner.DeselectItems(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public Size Size
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the <see cref="ISpriteRenderer"/>.
        /// </summary>
        public ISpriteRenderer SpriteRenderer
        {
            get
            {
                return this.Owner.SpriteRenderer;
            }
        }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public ListItemStyle Style
        {
            get
            {
                return this.ListItemElement.Style;
            }

            set
            {
                this.ListItemElement.Style = value;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get
            {
                return this.DisplayedText;
            }

            set
            {
                if (this.DisplayedText != value)
                {
                    this.DisplayedText = value;
                    this.ListItemElement.TextElement.UpdateType |= UpdateTypes.Text;
                }
            }
        }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        public Texture Texture
        {
            get
            {
                return this.Style.Texture;
            }

            set
            {
                this.Style.Texture = value;
            }
        }

        /// <summary>
        /// Gets the texture filtering.
        /// </summary>
        /// <value>A TextureFiltering enum representing the texture filtering.</value>
        public TextureFiltering TextureFiltering
        {
            get
            {
                return this.Owner.TextureFiltering;
            }
        }

        /// <summary>
        /// Gets or sets the style of the tooltip.
        /// </summary>
        public ToolTipStyle ToolTipStyle
        {
            get
            {
                return this.ToolTipElement.Style;
            }

            set
            {
                this.ToolTipElement.Style = value;
            }
        }

        /// <summary>
        /// Gets or sets the text of the tooltip.
        /// </summary>
        [Localizable(true)]
        public string ToolTipText
        {
            get
            {
                return this.toolTipText;
            }

            set
            {
                if (value != null)
                {
                    value = " " + value + " ";
                    value = value.Replace(Environment.NewLine, " " + Environment.NewLine + " ");
                }
                if (this.toolTipText != value)
                {
                    this.toolTipText = value;
                    this.ToolTipElement.UpdateText();                    

                    if (string.IsNullOrEmpty(value))
                    {
                        this.ToolTipElement.Hide();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets an object which contains data about the item.
        /// </summary>
        /// <remarks>This is ignored by the serializer.</remarks>
        [SerializerOptions(Ignore = true)]
        public object UserData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the owner is visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                if (this.Owner != null)
                {
                    return this.Owner.Visible;
                }

                return false;
            }
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets the list item element.
        /// </summary>
        protected internal ListItemElement ListItemElement
        {
            get
            {
                return this.listItemElement ?? (this.listItemElement = new ListItemElement(this, () => this.Owner.ZOrder * 10));
            }
        }

        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        protected internal ToolTipElement ToolTipElement
        {
            get
            {
                return this.toolTipElement ?? (this.toolTipElement = new ToolTipElement(this, () => 1));
            }
        }

        #endregion Protected Internal Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Disposes the ListBoxItem.
        /// </summary>
        public void Dispose()
        {
            this.ListItemElement.Dispose();
            this.ToolTipElement.Dispose();
            this.Owner = null;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the location relative to its viewport origin.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        public Point GetLocationInViewport()
        {
            Point p = this.Owner.GetLocationInViewport();
            return new Point(
                p.X + this.Location.X,
                p.Y + this.Location.Y);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void SetSelected(bool value)
        {
            this.selected = value;
        }

        internal void Update()
        {
            if (!string.IsNullOrEmpty(this.ToolTipText) && this.IsMouseOver && this.Visible)
            {
                this.ToolTipElement.Update();
            }
        }

        #endregion Internal Methods

        #region Private Methods

        private void OnIsMouseOverChanged()
        {
            if (!this.isMouseOver)
            {
                this.ToolTipElement.InjectMouseLeave(new MouseEventArgs(Point.Empty));
            }
            else
            {
                this.ToolTipElement.InjectMouseEnter(new MouseEventArgs(Point.Empty));
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}