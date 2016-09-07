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
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// A TextBox control.
    /// </summary>
    public class TextBox : SkinnedControl, ITextBoxElementOwner
    {
        #region Fields

        private KeyEvent lastKeyEvent;
        private char passwordChar;
        private TextBoxElement textBoxElement;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextBox class.
        /// </summary>
        public TextBox()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TextBox class.
        /// </summary>
        /// <param name="name">The name of the TextBox.</param>
        public TextBox(string name)
            : base(name)
        {
            this.DefocusOnSubmit = true;
            this.AutoCompleteSource = new List<string>();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the enter is pressed and the control has the focus.
        /// </summary>
        /// <remarks>Submits the text.</remarks>
        public event EventHandler<ValueEventArgs<string>> Submit;

        #endregion Events

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
        /// Gets or sets the auto completion source.
        /// </summary>
        public ICollection<string> AutoCompleteSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the caret location.
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
        /// Gets or sets a value indicating whether the text should be cleared after submit.
        /// </summary>
        public bool ClearTextOnSubmit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TextBox should lose focus after submit.
        /// </summary>
        public bool DefocusOnSubmit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the displayed text.
        /// </summary>
        /// <value>The displayed text.</value>
        public override string DisplayedText
        {
            get
            {
                string retValue = this.PasswordChar == 0
                                      ? base.DisplayedText
                                      : new string(this.PasswordChar, base.DisplayedText.Length);

                return retValue.Length > this.TextBoxElement.TextScrollOffset
                           ? retValue.Substring(this.TextBoxElement.TextScrollOffset)
                           : string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a key is currently down.
        /// </summary>
        public bool IsKeyDown
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the character used to obfuscate the text of the TextBox.
        /// </summary>
        public char PasswordChar
        {
            get
            {
                return this.passwordChar;
            }

            set
            {
                if (this.passwordChar != value)
                {
                    this.ThrowIfDisposed();
                    this.passwordChar = value;
                    this.OnPasswordCharChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the selected text.
        /// </summary>
        public string SelectedText
        {
            get
            {
                return this.TextBoxElement.SelectedText;
            }
        }

        /// <summary>
        /// Gets or sets the TextBox style.
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

        /// <summary>
        /// Tries to insert a string into the TextBox.
        /// </summary>
        /// <param name="text">String to insert.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        public virtual bool InsertText(string text)
        {
            this.ThrowIfDisposed();
            return this.TextBoxElement.InsertText(text);
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
                if (e.KeyCode == ConsoleKey.Enter)
                {
                    this.OnSubmit(new ValueEventArgs<string>(this.Text));
                }
                else
                {
                    this.lastKeyEvent = e.KeyEvent;
                    this.IsKeyDown = true;
                }
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyHeld"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.KeyboardEventArgs"/> instance containing the event data.</param>
        protected override void OnKeyHeld(KeyboardEventArgs e)
        {
            base.OnKeyHeld(e);
            this.TextBoxElement.HandleKeyEvent(this.lastKeyEvent);
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected override void OnKeyUp(KeyboardEventArgs e)
        {
            base.OnKeyUp(e);

            this.IsKeyDown = false;
            this.lastKeyEvent = null;
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            this.DeselectText();
            this.TextBoxElement.InjectMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDrag"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            base.OnMouseDrag(e);
            this.TextBoxElement.InjectMouseDrag(e);
        }

        /// <summary>
        /// Handles PasswordChar changes.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnPasswordCharChanged(EventArgs e)
        {
            this.TextElement.UpdateType |= UpdateTypes.Text;
        }

        /// <summary>
        /// Raises the <see cref="TextBox.Submit"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual void OnSubmit(ValueEventArgs<string> e)
        {
            if (this.Submit != null)
            {
                this.Submit(this, e);
            }

            if (this.ClearTextOnSubmit)
            {
                this.Text = string.Empty;
                this.TextBoxElement.RemoveSprite();
            }

            if (this.DefocusOnSubmit)
            {
                this.Focused = false;
                this.MiyagiSystem.GUIManager.FocusedControl = null;
            }
        }

        /// <summary>
        /// Raises the <see cref="Label.TextChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/> instance containing the event data.</param>
        protected override void OnTextChanged(TextEventArgs e)
        {
            base.OnTextChanged(e);
            this.TextBoxElement.InjectTextChanged(e);
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();

            if (this.IsMouseOver)
            {
                if (this.MiyagiSystem.GUIManager.Cursor != null)
                {
                    this.MiyagiSystem.GUIManager.Cursor.ActiveMode = "TextInput";
                }
            }

            if (this.TextBoxStyle.UseCaret)
            {
                this.TextBoxElement.Update(this.DeltaLocation, this.DeltaSize);
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}