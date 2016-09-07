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
namespace Miyagi.UI
{
    using System.Threading;

    /// <summary>
    /// Provides methods to place data on and retrieve data from the clipboard.
    /// </summary>
    public static class Clipboard
    {
        #region Fields

        private static string text = string.Empty;
        private static bool useSystemClipboard = true;

        #endregion Fields

        #region Properties

        #region Public Static Properties

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public static string Text
        {
            get
            {
                if (UseSystemClipboard)
                {
                    return System.Windows.Forms.Clipboard.ContainsText() ? System.Windows.Forms.Clipboard.GetText() : string.Empty;
                }

                return text;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (UseSystemClipboard)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        System.Windows.Forms.Clipboard.Clear();
                    }
                    else
                    {
                        System.Windows.Forms.Clipboard.SetText(value);
                    }
                }
                else
                {
                    text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the system clipboard should be used.
        /// </summary>
        public static bool UseSystemClipboard
        {
            get
            {
                return useSystemClipboard && Thread.CurrentThread.GetApartmentState() == ApartmentState.STA;
            }

            set
            {
                useSystemClipboard = value;
            }
        }

        #endregion Public Static Properties

        #endregion Properties
    }
}