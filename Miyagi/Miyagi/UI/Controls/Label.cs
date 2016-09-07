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
    using System.ComponentModel;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// A Label control.
    /// </summary>
    public class Label : Control, ITextElementOwner, ILocalizable, IToolTipElementOwner
    {
        #region Fields

        private bool isLocaleResourceKeyDirty;
        private string localeResourceKey;
        private string text;
        private TextElement textElement;
        private Size textSize;
        private ToolTipElement toolTipElement;
        private string toolTipText;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Label class.
        /// </summary>
        public Label()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Label class.
        /// </summary>
        /// <param name="name">The name of the Label.</param>
        public Label(string name)
            : base(name)
        {
            this.text = string.Empty;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the <see cref="Styles.TextStyle.Font"/> property of the TextStyle of this control changes.
        /// </summary>
        public event EventHandler FontChanged;

        /// <summary>
        /// Occurs when the <see cref="Text"/> property of the TextStyle of this control changes.
        /// </summary>
        public event EventHandler<TextEventArgs> TextChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the displayed text.
        /// </summary>
        public virtual string DisplayedText
        {
            get
            {
                return this.Text;
            }
        }

        /// <summary>
        /// Gets or sets the locale resource key.
        /// </summary>
        /// <value>A string representing the locale resource key.</value>
        public string LocaleResourceKey
        {
            get
            {
                return this.localeResourceKey;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.localeResourceKey != value)
                {
                    this.localeResourceKey = value;
                    this.isLocaleResourceKeyDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        protected ToolTipElement ToolTipElement
        {
            get
            {
                return this.toolTipElement ?? (this.toolTipElement = new ToolTipElement(this, () => 1));
            }
        }


        /// <summary>
        /// Gets or sets the displayed text.
        /// </summary>
        /// <value>A string representing the displayed text.</value>
        [Localizable(true)]
        public virtual string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.text != value)
                {
                    this.SetText(value, true);
                }
            }
        }

        /// <summary>
        /// Gets a Rectangle representing the text area.
        /// </summary>
        public virtual Rectangle TextBounds
        {
            get
            {
                return this.DisplayRectangle;
            }
        }

        /// <summary>
        /// Gets or sets the style of the text.
        /// </summary>
        public virtual TextStyle TextStyle
        {
            get
            {
                return this.TextElement.Style;
            }

            set
            {
                this.ThrowIfDisposed();
                this.TextElement.Style = value;
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        protected override IList<IElement> Elements
        {
            get
            {
                var retValue = base.Elements;

                if (this.textElement != null)
                {
                    retValue.Add(this.textElement);
                }
                return retValue;
            }
        }

        /// <summary>
        /// Gets the TextElement.
        /// </summary>
        protected TextElement TextElement
        {
            get
            {
                return this.textElement ?? (this.textElement = new TextElement(this, () => (this.ZOrder * 10) + 9)
                                                               {
                                                                   FontChangedCallback = () => this.OnFontChanged(EventArgs.Empty)
                                                               });
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
                this.ThrowIfDisposed();
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
                this.ThrowIfDisposed();
                this.ToolTipElement.Style = value;
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the control.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (this.toolTipElement != null)
            {
                this.toolTipElement.Dispose();
            }

            if (disposing)
            {
                this.textElement = null;
                this.toolTipElement = null;
            }
        }

        /// <summary>
        /// Resizes the control.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            base.DoResize(widthFactor, heightFactor);
            this.TextStyle.Resize(widthFactor, heightFactor);
        }

        /// <summary>
        /// Method that calculates the required area for the control, so that the text in it fits appropiately.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>A Size representing the width and height of a rectangle.</returns>
        protected override Size GetPreferredSizeCore(Size proposedSize)
        {
            return this.SizeFromClientSize(this.textSize);
        }

        /// <summary>
        /// Raises the <see cref="Control.ClientSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            this.TextElement.UpdateType |= UpdateTypes.Text;
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseEnter"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.ToolTipElement.InjectMouseEnter(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.ToolTipElement.InjectMouseLeave(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (ToolTipElement.Visible)
            {
                ToolTipElement.Hide();
            }
        }

        /// <summary>
        /// Raises the <see cref="Label.FontChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFontChanged(EventArgs e)
        {
            this.RecalculateTextSize();

            if (this.FontChanged != null)
            {
                this.FontChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MaxSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnMaxSizeChanged(EventArgs e)
        {
            base.OnMaxSizeChanged(e);
            this.RecalculateTextSize();
        }

        /// <summary>
        /// Raises the <see cref="Label.TextChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/> instance containing the event data.</param>
        protected virtual void OnTextChanged(TextEventArgs e)
        {
            this.RecalculateTextSize();

            if (this.TextChanged != null)
            {
                this.TextChanged(this, e);
            }
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="newText">A string representing the new text.</param>
        /// <param name="fromProperty">Indicates whether this method is called by the <see cref="Text"/> Property.</param>
        protected void SetText(string newText, bool fromProperty)
        {
            this.text = newText;
            this.TextElement.UpdateType |= UpdateTypes.Text;

            this.OnTextChanged(new TextEventArgs(fromProperty));
            this.OnPropertyChanged("Text");
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();

            if (this.isLocaleResourceKeyDirty && !string.IsNullOrEmpty(this.localeResourceKey))
            {
                this.MiyagiSystem.LocaleManager.ApplyResourceKey(this, this.localeResourceKey);
                this.isLocaleResourceKeyDirty = false;
            }

            this.TextElement.Update(this.DeltaLocation, this.DeltaSize);
            if (this.toolTipElement != null
                && !String.IsNullOrEmpty(this.ToolTipText))
            {
                this.ToolTipElement.Update(this.DeltaLocation, this.DeltaSize);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void RecalculateTextSize()
        {
            if (this.TextStyle != null && this.TextStyle.Font != null)
            {
                this.textSize = this.TextStyle.Font.MeasureString(this.text, this.MaxSize, this.TextStyle.Multiline);
            }

            if (this.AutoSize)
            {
                this.SetBounds(this.Left, this.Top, this.PreferredSize.Width, this.PreferredSize.Height, BoundsSpecified.Size);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}