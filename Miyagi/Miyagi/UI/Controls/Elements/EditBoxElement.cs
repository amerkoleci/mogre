namespace Miyagi.UI.Controls.Elements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a textbox.
    /// </summary>
    public abstract class EditBoxElement<TOwner, TStyle> : Element<TOwner, TStyle>, ICaretElementOwner
        where TOwner : class, IEditBoxElementOwner
        where TStyle : EditBoxStyle, new()
    {
        #region Fields

        private CaretElement caretElement;
        private int caretLocation;
        private int firstClickedQuad;
        private List<int> newLineIndices;
        private Range selectedTextRange;
        private int textScrollOffset;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextBoxElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        protected EditBoxElement(TOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the caret of the TextBox.
        /// </summary>
        /// <value>A CaretElement representing the caret of the TextBox.</value>
        public CaretElement CaretElement
        {
            get
            {
                return this.caretElement ?? (this.caretElement = new CaretElement(this, () => this.GetZOrder() + 3));
            }
        }

        /// <summary>
        /// Gets or sets the position of the caret.
        /// </summary>
        public int CaretLocation
        {
            get
            {
                return this.caretLocation;
            }

            set
            {
                if (this.Style.UseCaret)
                {
                    string text = this.Owner.Text ?? string.Empty;
                    int length = this.GetTextLength(text);
                    value = value.Clamp(0, length);

                    if (!this.TextElement.Style.Multiline)
                    {
                        int i = this.GetVisibleCharsCount();
                        if (value > i + this.TextScrollOffset)
                        {
                            this.TextScrollOffset += 3;
                        }

                        if (value < this.TextScrollOffset)
                        {
                            this.TextScrollOffset -= 3;
                        }
                    }

                    if (this.caretLocation != value)
                    {
                        this.caretLocation = value;
                        this.CaretElement.UpdateType |= UpdateTypes.Location;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the caret is on the beginning of a new line.
        /// </summary>
        public bool IsCaretOnNewLine
        {
            get
            {
                return this.IsOnNewLine(this.CaretLocation);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a key is currently down.
        /// </summary>
        public bool IsKeyDown
        {
            get
            {
                return this.Owner.IsKeyDown;
            }
        }

        /// <summary>
        /// Gets the selected text.
        /// </summary>
        public string SelectedText
        {
            get
            {
                string text = this.Owner.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    int first = this.selectedTextRange.First;
                    int length = this.selectedTextRange.Last - first + 1;
                    if (first != -1)
                    {
                        return text.Substring(first, length);
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the first and last index of the selected text.
        /// </summary>
        public Range SelectedTextRange
        {
            get
            {
                return this.selectedTextRange;
            }

            set
            {
                if (this.Style.UseCaret)
                {
                    if (value.First == -1)
                    {
                        this.selectedTextRange = value;
                    }
                    else
                    {
                        this.selectedTextRange = new Range(
                            Math.Max(Math.Min(value.First, value.Last), this.textScrollOffset),
                            Math.Max(value.First, value.Last));
                    }

                    int min = this.selectedTextRange.First;
                    int max = this.selectedTextRange.Last;
                    min -= this.GetNumberOfNewLinesBefore(min);
                    max -= this.GetNumberOfNewLinesBefore(max);
                    this.TextElement.SetSelectedQuads(new Range(min, max), this.textScrollOffset);
                }
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public override Size Size
        {
            get
            {
                return this.Owner.Size;
            }
        }

        /// <summary>
        /// Gets a collection of subelement.
        /// </summary>
        /// <value>A collection of IElements representing the subelements.</value>
        public override IEnumerable<IElement> SubElements
        {
            get
            {
                if (this.caretElement != null)
                {
                    yield return this.caretElement;
                }
            }
        }

        /// <summary>
        /// Gets the owner's TextElement.
        /// </summary>
        public TextElement TextElement
        {
            get
            {
                return this.Owner.TextElement;
            }
        }

        /// <summary>
        /// Gets or sets the scroll offset.
        /// </summary>
        public int TextScrollOffset
        {
            get
            {
                return this.textScrollOffset;
            }

            set
            {
                if (this.textScrollOffset != value)
                {
                    if (value < 0)
                    {
                        value = 0;
                    }

                    this.textScrollOffset = value;
                    this.TextElement.UpdateType |= UpdateTypes.Text;
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Deselects the text.
        /// </summary>
        public void DeselectText()
        {
            this.SelectedTextRange = new Range(-1, -1);
        }

        /// <summary>
        /// Gets the derived location.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        public Point GetLocationInViewport()
        {
            return this.Owner.GetLocationInViewport();
        }

        /// <summary>
        /// Gets the number of new lines before the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The number of new lines before the specified index.</returns>
        public int GetNumberOfNewLinesBefore(int index)
        {
            if (this.newLineIndices == null)
            {
                this.SetNewLineIndices();
            }

            return this.newLineIndices.Count(i => i < index);
        }

        /// <summary>
        /// Handles a KeyEvent.
        /// </summary>
        /// <param name="arg">The KeyEvent to handle.</param>
        /// <returns><c>true</c> if the KeyEvent could be handled; otherwise, <c>false</c>.</returns>
        public bool HandleKeyEvent(KeyEvent arg)
        {
            if (arg.Key == ConsoleKey.NoName)
            {
                return false;
            }

            bool handled = false;
            bool deselectText = true;

            // inject the key into the active textArea
            if (!this.HandleClipboard(arg, ref deselectText, ref handled))
            {
                this.HandleKeyEventCore(arg, ref deselectText, ref handled);
            }

            if (deselectText)
            {
                this.DeselectText();
            }

            return handled;
        }

        /// <summary>
        /// Injects changed text.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/> instance containing the event data.</param>
        public void InjectTextChanged(TextEventArgs e)
        {
            this.SetNewLineIndices();
            if (e.FromProperty)
            {
                this.DeselectText();
                this.CaretLocation = this.GetTextLength(this.Owner.Text);
                this.TextScrollOffset = 0;
            }
        }

        /// <summary>
        /// Tries to insert a string into the TextBox.
        /// </summary>
        /// <param name="newText">String to insert.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        public bool InsertText(string newText)
        {
            bool bla = false;
            bool retValue = false;
            this.InsertTextCore(newText, ref bla, ref retValue);
            return retValue;
        }

        #endregion Public Methods

        #region Internal Static Methods

        internal static bool CheckTextRestriction(Func<string, int, bool> pred, string s)
        {
            return !s.Where((t, i) => !pred(s, i)).Any();
        }

        #endregion Internal Static Methods

        #region Protected Methods

        /// <summary>
        /// Checks the text restrictions.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns><c>true</c> if the text does not violates the restrictions; otherwise <c>false</c></returns>
        protected abstract bool CheckRestrictions(string text);

        /// <summary>
        /// Gets the length of the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The length of the text.</returns>
        protected int GetTextLength(string text)
        {
            var s = text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (!this.TextElement.Style.Multiline || s.Length < 2)
            {
                return text.Length;
            }

            return s.Sum(t => t.Length) + s.Length - 1;
        }

        /// <summary>
        /// Handles a KeyEvent.
        /// </summary>
        /// <param name="arg">The KeyEvent to handle.</param>
        /// <param name="deselectText">if set to <c>true</c> the text should be deselected.</param>
        /// <param name="handled">if set to <c>true</c> the keyevent has been handled.</param>
        protected virtual void HandleKeyEventCore(KeyEvent arg, ref bool deselectText, ref bool handled)
        {
            if (!this.Style.ReadOnly)
            {
                switch (arg.Key)
                {
                    case ConsoleKey.Enter:
                        handled = true;
                        break;
                    case ConsoleKey.Backspace:
                        handled = this.RemoveText(-1);
                        break;
                    case ConsoleKey.Tab:
                        deselectText = false;
                        break;
                    case ConsoleKey.Delete:
                        handled = this.RemoveText(1);
                        break;
                    default:
                        string newText = Keyboard.GetString(arg);
                        this.CleanupText(ref newText);
                        if (!string.IsNullOrEmpty(newText))
                        {
                            this.InsertTextCore(newText, ref deselectText, ref handled);
                        }

                        break;
                }
            }

            switch (arg.Key)
            {
                case ConsoleKey.LeftArrow:
                    this.HandleLeftArrow(arg, ref deselectText);
                    handled = true;
                    break;
                case ConsoleKey.RightArrow:
                    this.HandleRightArrow(arg, ref deselectText);
                    handled = true;
                    break;
            }
        }

        /// <summary>
        /// Inject a pressed mouse button.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> to inject.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            this.SetCaretToPoint(e.MouseLocation);
            this.firstClickedQuad = this.caretLocation;
        }

        /// <summary>
        /// Inject a dragged mouse.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> to inject.</param>
        protected override void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            this.SetCaretToPoint(e.NewValue);
            var index = this.caretLocation;
            if (this.firstClickedQuad == index)
            {
                this.DeselectText();
            }
            else if (index > this.firstClickedQuad)
            {
                this.SelectedTextRange = new Range(index - 1, this.firstClickedQuad);
            }
            else if (index < this.firstClickedQuad)
            {
                this.SelectedTextRange = new Range(index, this.firstClickedQuad - 1);
            }
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "MaxCharacterLimit":
                    string text = this.Owner.Text;
                    if (!string.IsNullOrEmpty(text)
                        && this.GetTextLength(text) > this.Style.MaxCharacterLimit)
                    {
                        this.DeselectText();
                        this.RemoveText(this.Style.MaxCharacterLimit - this.GetTextLength(text)); // truncate.
                    }

                    break;

                case "UseCaret":
                    if (!this.Style.UseCaret)
                    {
                        this.CaretElement.RemoveSprite();
                        this.DeselectText();
                    }

                    break;
            }
        }

        /// <summary>
        /// Called when text is inserted.
        /// </summary>
        /// <param name="deselectText">if set to <c>true</c> the text should be deselected.</param>
        protected virtual void OnTextInserted(ref bool deselectText)
        {
        }

        /// <summary>
        /// Removes the selected text.
        /// </summary>
        protected void RemoveSelectedText()
        {
            int first = this.selectedTextRange.First;
            int length = this.selectedTextRange.Last - first + 1;
            if (first >= 0 && first + length <= this.GetTextLength(this.Owner.Text))
            {
                this.Owner.SetText(this.Owner.Text.Remove(first, length));
                this.CaretLocation = first;
            }
        }

        /// <summary>
        /// Sets the style of the sub elements.
        /// </summary>
        protected override void SetSubElementStyles()
        {
            if (this.Style != null)
            {
                this.CaretElement.Style = this.Style.CaretStyle;
            }
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        protected override void UpdateCore()
        {
            if (!this.Owner.Visible || !this.Owner.Focused)
            {
                if (this.CaretElement.Sprite != null)
                {
                    this.RemoveSprite();
                }

                return;
            }

            this.ForEachSubElement(ele => ele.Update());
        }

        #endregion Protected Methods

        #region Private Methods

        private bool CheckSpace(string text)
        {
            TextStyle style = this.Owner.TextElement.Style;
            Font font = style.Font;
            Size size = this.Owner.TextBounds.Size;
            Size textSize = font.MeasureString(text);

            if (textSize.Width > size.Width)
            {
                if (style.Multiline)
                {
                    if (size.Height - textSize.Height < font.Leading)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void CleanupText(ref string text)
        {
            Font font = this.Owner.TextElement.Style.Font;
            for (int i = text.Length - 1; i >= 0; i--)
            {
                Size charSize = font.MeasureString(text[i].ToString());
                if (charSize.Width == 0)
                {
                    text = text.Remove(i, 1);
                }
            }
        }

        private int GetCaretTextIndex()
        {
            return this.caretLocation + (this.GetNumberOfNewLinesBefore(this.caretLocation) * (Environment.NewLine.Length - 1));
        }

        private int GetClickedQuad(Point pos, out bool rightSide)
        {
            rightSide = true;

            var spr = this.TextElement.Sprite;
            if (spr != null && !string.IsNullOrEmpty(this.Owner.Text))
            {
                PointF pf = pos.ToScreenCoordinates(this.ViewportSize);
                float x = pf.X;
                float y = pf.Y;
                int count = spr.PrimitiveCount;

                for (int i = 0; i < count - 1; i++)
                {
                    Quad q = spr.GetQuad(i);
                    if (q.HitTest(x, y))
                    {
                        if (y <= q.Top && y >= q.Bottom)
                        {
                            if (x >= q.Left && x <= q.Center.X)
                            {
                                rightSide = false;
                            }
                        }

                        return i;
                    }
                }

                return spr.PrimitiveCount;
            }

            return 0;
        }

        private int GetVisibleCharsCount()
        {
            Font font = this.Owner.TextElement.Style.Font;
            int width = this.Owner.TextBounds.Size.Width;

            string text = this.Owner.DisplayedText;
            for (int i = this.GetTextLength(text); i >= 0; i--)
            {
                if (font.MeasureString(text.Substring(0, i)).Width < width)
                {
                    return i;
                }
            }

            return 0;
        }

        private bool HandleClipboard(KeyEvent arg, ref bool deselectText, ref bool handled)
        {
            if ((arg.Modifiers == ConsoleModifiers.Control && arg.Key == ConsoleKey.C)
                || (arg.Modifiers == ConsoleModifiers.Control && arg.Key == ConsoleKey.X && this.Style.ReadOnly))
            {
                // copy to clipboard
                Clipboard.Text = this.SelectedText;
                deselectText = false;
                return true;
            }

            if (!this.Style.ReadOnly)
            {
                if (arg.Modifiers == ConsoleModifiers.Control && arg.Key == ConsoleKey.V)
                {
                    // paste from clipboard
                    handled = this.InsertText(Clipboard.Text);
                    return true;
                }

                if (arg.Modifiers == ConsoleModifiers.Control && arg.Key == ConsoleKey.X)
                {
                    // cut to clipboard
                    Clipboard.Text = this.SelectedText;
                    this.RemoveSelectedText();
                    deselectText = true;
                    return true;
                }
            }

            return false;
        }

        private void HandleLeftArrow(KeyEvent arg, ref bool deselectText)
        {
            this.CaretLocation--;
            if (arg.Modifiers == ConsoleModifiers.Shift)
            {
                int loc = this.caretLocation;
                int min = this.selectedTextRange.First == -1 ? loc : this.selectedTextRange.First;
                int max = this.selectedTextRange.Last == -1 ? loc : this.selectedTextRange.Last;

                if (min == max
                    && min == loc
                    && this.selectedTextRange.First != -1
                    && this.selectedTextRange.Last != -1)
                {
                    min = max = -1;
                }
                else if (loc < min)
                {
                    min--;
                }
                else if (loc <= max && loc != min)
                {
                    max--;
                }

                this.SelectedTextRange = new Range(min, max);
                deselectText = false;
            }
            else
            {
                deselectText = true;
            }
        }

        private void HandleRightArrow(KeyEvent arg, ref bool deselectText)
        {
            this.CaretLocation++;
            if (arg.Modifiers == ConsoleModifiers.Shift)
            {
                int loc = this.caretLocation;
                int min = this.selectedTextRange.First == -1 ? loc - 1 : this.selectedTextRange.First;
                int max = this.selectedTextRange.Last == -1 ? loc - 1 : this.selectedTextRange.Last;

                if (min == max
                    && min == loc - 1
                    && this.selectedTextRange.First != -1
                    && this.selectedTextRange.Last != -1)
                {
                    min = max = -1;
                }
                else if (loc - 1 > max)
                {
                    max++;
                }
                else if (loc - 1 >= min && loc - 1 != max)
                {
                    min++;
                }

                this.SelectedTextRange = new Range(min, max);
                deselectText = false;
            }
            else
            {
                deselectText = true;
            }
        }

        private void InsertTextCore(string newText, ref bool deselectText, ref bool handled)
        {
            if (newText == null)
            {
                newText = string.Empty;
            }

            if (this.Owner.Text == null)
            {
                this.Owner.SetText(string.Empty);
            }

            if (!this.CheckRestrictions(newText))
            {
                handled = false;
                return;
            }

            // check available space
            if (this.Owner.TextElement.Style.Multiline && !this.CheckSpace(this.Owner.Text + newText))
            {
                handled = false;
                return;
            }

            if (this.selectedTextRange.First != -1)
            {
                this.RemoveSelectedText();
            }

            // check character limit
            if (this.Style.MaxCharacterLimit != 0 && this.GetTextLength(this.Owner.Text) + this.GetTextLength(newText) > this.Style.MaxCharacterLimit)
            {
                handled = false;
                return;
            }

            var index = this.GetCaretTextIndex();
            if (this.IsCaretOnNewLine)
            {
                index++;
            }

            this.Owner.SetText(this.Owner.Text.Insert(index, newText));
            this.CaretLocation += this.GetTextLength(newText);

            this.OnTextInserted(ref deselectText);
            handled = true;
            return;
        }

        private bool IsOnNewLine(int index)
        {
            if (this.newLineIndices == null)
            {
                this.SetNewLineIndices();
            }

            return this.newLineIndices.Contains(index);
        }

        private bool RemoveText(int count)
        {
            if (count == 0 || string.IsNullOrEmpty(this.Owner.Text))
            {
                return false;
            }

            if (this.selectedTextRange.First != -1)
            {
                this.RemoveSelectedText();
                return true;
            }

            var index = this.GetCaretTextIndex();
            if (count < 0)
            {
                if (index + count >= 0)
                {
                    bool isOnNewLine = this.IsCaretOnNewLine;
                    if (isOnNewLine && -count < Environment.NewLine.Length && Environment.NewLine.Length > 1)
                    {
                        count -= Environment.NewLine.Length - 1;
                    }
                    else
                    {
                        isOnNewLine = false;
                    }

                    this.Owner.SetText(this.Owner.Text.Remove(index - 1, -count));

                    if (isOnNewLine)
                    {
                        count += Environment.NewLine.Length - 1;
                    }

                    this.CaretLocation += count;
                    return true;
                }
            }
            else
            {
                if (index + count < this.GetTextLength(this.Owner.Text) + 1)
                {
                    if (this.IsOnNewLine(this.caretLocation + count))
                    {
                        count++;
                    }

                    this.Owner.SetText(this.Owner.Text.Remove(index, count));
                    return true;
                }
            }

            return false;
        }

        private void SetCaretToPoint(Point point)
        {
            bool right;
            int quad = this.GetClickedQuad(point, out right) + this.textScrollOffset;
            this.CaretLocation = right ? quad + 1 : quad;
            this.CaretLocation = this.GetCaretTextIndex();
        }

        private void SetNewLineIndices()
        {
            this.newLineIndices = new List<int>();
            string text = this.Owner.Text ?? string.Empty;
            int pos, offset = 0;
            while ((pos = text.IndexOf(Environment.NewLine)) > 0)
            {
                text = text.Substring(pos + Environment.NewLine.Length);
                offset += pos;
                this.newLineIndices.Add(offset + this.newLineIndices.Count + 1);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}