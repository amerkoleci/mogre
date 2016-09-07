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
namespace Miyagi.UI.Controls.Elements
{
    using System;
    using System.Linq;

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a textbox.
    /// </summary>
    public sealed class TextBoxElement : EditBoxElement<ITextBoxElementOwner, TextBoxStyle>
    {
        #region Fields

        private int lastAutoCompleteIndex = -1;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxElement"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public TextBoxElement(ITextBoxElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
        }

        #endregion Constructors

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Checks the text restrictions.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns><c>true</c> if the text does not violates the restrictions; otherwise <c>false</c></returns>
        protected override bool CheckRestrictions(string text)
        {
            return (!this.Style.DigitOnly || CheckTextRestriction(char.IsDigit, this.Owner.Text))
                   && (!this.Style.LetterOrDigitOnly || CheckTextRestriction(char.IsLetterOrDigit, this.Owner.Text));
        }

        /// <summary>
        /// Handles a KeyEvent.
        /// </summary>
        /// <param name="arg">The KeyEvent to handle.</param>
        /// <param name="deselectText">if set to <c>true</c> the text should be deselected.</param>
        /// <param name="handled">if set to <c>true</c> the keyevent has been handled.</param>
        protected override void HandleKeyEventCore(KeyEvent arg, ref bool deselectText, ref bool handled)
        {
            base.HandleKeyEventCore(arg, ref deselectText, ref handled);

            if (!this.Style.ReadOnly)
            {
                switch (arg.Key)
                {
                    case ConsoleKey.UpArrow:
                        deselectText = this.GetAutoComplete(++this.lastAutoCompleteIndex);
                        if (deselectText)
                        {
                            this.RemoveSelectedText();
                        }

                        handled = true;
                        break;
                    case ConsoleKey.DownArrow:
                        deselectText = this.GetAutoComplete(--this.lastAutoCompleteIndex);
                        if (deselectText)
                        {
                            this.RemoveSelectedText();
                        }

                        handled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Called when text is inserted.
        /// </summary>
        /// <param name="deselectText">if set to <c>true</c> the text should be deselected.</param>
        protected override void OnTextInserted(ref bool deselectText)
        {
            // autocomplete
            if (this.Owner.AutoCompleteSource != null && this.Owner.AutoCompleteSource.Count > 0)
            {
                deselectText = this.GetAutoComplete(0);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private bool GetAutoComplete(int index)
        {
            if (this.Owner.AutoCompleteSource == null || this.Owner.AutoCompleteSource.Count == 0)
            {
                return false;
            }

            string text = this.Owner.Text;

            var autoCompleteList = this.Owner.AutoCompleteSource
                .Where(s => s.StartsWith(text) && this.GetTextLength(s) > this.GetTextLength(text))
                .OrderBy(s => s.Length)
                .Select(s => s);

            index = Math.Max(0, Math.Min(autoCompleteList.Count() - 1, index));
            string autoComplete = autoCompleteList.ElementAtOrDefault(index);

            this.lastAutoCompleteIndex = index;

            if (!string.IsNullOrEmpty(autoComplete) && autoComplete != text)
            {
                var selectedTextRange = new Range(this.GetTextLength(this.Owner.Text), autoComplete.Length - 1);
                this.Owner.SetText(autoComplete);
                this.SelectedTextRange = selectedTextRange;
                return false;
            }

            return true;
        }

        #endregion Private Methods

        #endregion Methods
    }
}