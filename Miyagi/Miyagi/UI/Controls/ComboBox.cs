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
    using System.Collections.Generic;
    using System.Linq;

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Serialization;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// A ComboBox control.
    /// </summary>
    public class ComboBox : DropDownList, ITextBoxElementOwner
    {
        #region Fields

        private Point lastMouseHoverLocation;
        private TextBoxElement textBoxElement;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ComboBox class.
        /// </summary>
        public ComboBox()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ComboBox class.
        /// </summary>
        /// <param name="name">The name of the ComboBox.</param>
        public ComboBox(string name)
            : base(name)
        {
            this.AutoCompleteEnabled = true;
        }

        #endregion Constructors

        #region Properties

        #region Explicit Interface Properties

        TextElement IEditBoxElementOwner.TextElement
        {
            get
            {
                return this.TextElement;
            }
        }

        #endregion Explicit Interface Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether auto completion is enabled.
        /// </summary>
        public bool AutoCompleteEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the auto completion source.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public ICollection<string> AutoCompleteSource
        {
            get
            {
                return this.AutoCompleteEnabled ? (from i in this.Items select i.Text).ToArray() : null;
            }
        }

        /// <summary>
        /// Gets or sets the location of the caret.
        /// </summary>
        public int CaretLocation
        {
            get
            {
                return this.TextBoxElement.CaretLocation;
            }

            set
            {
                this.ThrowIfDisposed();
                this.TextBoxElement.CaretLocation = value;
            }
        }

        /// <summary>
        /// Gets the displayed text.
        /// </summary>
        public override string DisplayedText
        {
            get
            {
                string retValue = base.DisplayedText;
                return retValue.Length > this.TextBoxElement.TextScrollOffset
                           ? retValue.Substring(this.TextBoxElement.TextScrollOffset)
                           : string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a key is currently pressed.
        /// </summary>
        public bool IsKeyDown
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the style of the TextBox.
        /// </summary>
        public TextBoxStyle TextBoxStyle
        {
            get
            {
                return this.TextBoxElement.Style;
            }

            set
            {
                this.ThrowIfDisposed();
                this.TextBoxElement.Style = value;
            }
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets a value indicating whether arrow key movement is blocked.
        /// </summary>
        /// <value>
        /// <c>true</c> if arrow key movement is blocked; otherwise, <c>false</c>.
        /// </value>
        protected internal override bool IsArrowKeyMovementBlocked
        {
            get
            {
                return true;
            }
        }

        #endregion Protected Internal Properties

        #region Protected Properties

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        protected override IList<IElement> Elements
        {
            get
            {
                var retValue = base.Elements;
                if (this.textBoxElement != null)
                {
                    retValue.Add(this.textBoxElement);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets the TextBoxElement.
        /// </summary>
        protected TextBoxElement TextBoxElement
        {
            get
            {
                return this.textBoxElement ?? (this.textBoxElement = new TextBoxElement(this, () => this.ZOrder * 10));
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Explicit Interface Methods

        void IEditBoxElementOwner.SetText(string text)
        {
            this.SetText(text, false);
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Deselects the text.
        /// </summary>
        public void DeselectText()
        {
            this.ThrowIfDisposed();
            this.TextBoxElement.DeselectText();
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
                this.textBoxElement = null;
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
            this.TextBoxStyle.Resize(widthFactor, heightFactor);
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected override void OnKeyDown(KeyboardEventArgs e)
        {
            if (this.TextBoxElement.HandleKeyEvent(e.KeyEvent))
            {
                this.IsKeyDown = true;
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected override void OnKeyUp(KeyboardEventArgs e)
        {
            base.OnKeyUp(e);
            this.IsKeyDown = false;
        }

        /// <summary>
        /// Handles mouse press events.
        /// </summary>
        /// <param name="e">A MouseButtonEventArgs that contains the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (!this.IsMouseOverTextBox(e.MouseLocation))
            {
                this.DeselectText();
            }
            else
            {
                this.TextBoxElement.InjectMouseDown(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDrag"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            base.OnMouseDrag(e);

            Point derLoc = this.GetLocationInViewport();
            Point mouseLoc = e.NewValue;
            if (mouseLoc.X - derLoc.X <= this.Width - this.ListElement.ScrollBarElement.Style.Extent
                && mouseLoc.Y - derLoc.Y <= this.Height)
            {
                this.TextBoxElement.InjectMouseDrag(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHover"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            base.OnMouseHover(e);
            this.lastMouseHoverLocation = e.MouseLocation;
        }

        /// <summary>
        /// Raises the <see cref="Label.TextChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/> instance containing the event data.</param>
        protected override void OnTextChanged(TextEventArgs e)
        {
            base.OnTextChanged(e);

            this.SelectItems(this.Text);
            this.TextBoxElement.InjectTextChanged(e);
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();

            if (this.IsMouseOver && this.GUI.GUIManager.Cursor != null)
            {
                this.GUI.GUIManager.Cursor.ActiveMode = this.IsMouseOverTextBox(this.lastMouseHoverLocation)
                                                            ? "TextInput"
                                                            : "Main";
            }

            if (this.TextBoxStyle.UseCaret)
            {
                this.TextBoxElement.Update(this.DeltaLocation, this.DeltaSize);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private bool IsMouseOverTextBox(Point p)
        {
            var rec = new Rectangle(this.GetLocationInViewport(), this.Width - this.Height, this.Height);
            return rec.Contains(p);
        }

        #endregion Private Methods

        #endregion Methods
    }
}