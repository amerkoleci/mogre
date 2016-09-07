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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;

    using Miyagi.Common.Events;
    using Miyagi.Internals;
    using Miyagi.Internals.ThirdParty;

    /// <summary>
    /// Keyboard wrapper.
    /// </summary>
    public static class Keyboard
    {
        #region Fields

        private static int currentKeyboardLayoutId;
        private static XDocument currentXmlDoc;

        #endregion Fields

        #region Properties

        #region Public Static Properties

        /// <summary>
        /// Gets or sets the folder that contains additional keyboard layouts.
        /// </summary>
        public static string AdditionalLayoutsFolder
        {
            get;
            set;
        }

        #endregion Public Static Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Gets a string representation of a KeyEvent.
        /// </summary>
        /// <param name="evt">The KeyEvent.</param>
        /// <returns>A string representation of a KeyEvent.</returns>
        public static string GetString(KeyEvent evt)
        {
            CheckKeyboardLayout();

            string sc = evt.Key.ToString().ToUpperInvariant();

            var node = currentXmlDoc.Descendants("Key")
                .FirstOrDefault(key => key.Element("VirtualKey").Value.ToUpperInvariant() == sc);

            if (node != null)
            {
                string modElement = "Char";
                ConsoleModifiers mod = evt.Modifiers;
                if (mod != 0)
                {
                    if (mod.IsFlagSet(ConsoleModifiers.Shift))
                    {
                        modElement = mod.IsFlagSet(ConsoleModifiers.Control) ? "ShiftCtrl" : "Shift";
                    }
                    else if (mod.IsFlagSet(ConsoleModifiers.Control))
                    {
                        modElement = mod.IsFlagSet(ConsoleModifiers.Alt) ? "CtrlAlt" : "Ctrl";
                    }
                }

                XElement keyNode = node.Element(modElement);
                if (keyNode != null)
                {
                    return keyNode.Value;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Converts a scancode to a ConsoleKey enum.
        /// </summary>
        /// <param name="scanCode">An <see cref="Int32"/> representing the scancode.</param>
        /// <param name="numpad">Indicates whether the scancode represents a numpad key.</param>
        /// <returns>A ConsoleKey representing the scancode.</returns>
        public static ConsoleKey ScanCodeToConsoleKey(int scanCode, bool numpad)
        {
            ConsoleKey retValue = ConsoleKey.NoName;

            if (scanCode > 128)
            {
                scanCode -= 128;
            }

            CheckKeyboardLayout();

            string sc = scanCode.ToString();
            var node = currentXmlDoc.Descendants("Key")
                .FirstOrDefault(key => key.Element("ScanCode").Value == sc);

            if (node != null)
            {
                retValue = node.Element("VirtualKey").Value.ParseEnum<ConsoleKey>();
            }

            // handle double-assigned scancodes
            if (numpad)
            {
                if (Console.NumberLock)
                {
                    switch (retValue)
                    {
                        case ConsoleKey.Insert:
                            retValue = ConsoleKey.NumPad0;
                            break;
                        case ConsoleKey.End:
                            retValue = ConsoleKey.NumPad1;
                            break;
                        case ConsoleKey.DownArrow:
                            retValue = ConsoleKey.NumPad2;
                            break;
                        case ConsoleKey.PageDown:
                            retValue = ConsoleKey.NumPad3;
                            break;
                        case ConsoleKey.LeftArrow:
                            retValue = ConsoleKey.NumPad4;
                            break;
                        case ConsoleKey.Clear:
                            retValue = ConsoleKey.NumPad5;
                            break;
                        case ConsoleKey.RightArrow:
                            retValue = ConsoleKey.NumPad6;
                            break;
                        case ConsoleKey.Home:
                            retValue = ConsoleKey.NumPad7;
                            break;
                        case ConsoleKey.UpArrow:
                            retValue = ConsoleKey.NumPad8;
                            break;
                        case ConsoleKey.PageUp:
                            retValue = ConsoleKey.NumPad9;
                            break;
                        case ConsoleKey.Delete:
                            retValue = ConsoleKey.Decimal;
                            break;
                    }
                }
                else
                {
                    switch (retValue)
                    {
                        case ConsoleKey.NumPad0:
                            retValue = ConsoleKey.Insert;
                            break;
                        case ConsoleKey.NumPad1:
                            retValue = ConsoleKey.End;
                            break;
                        case ConsoleKey.NumPad2:
                            retValue = ConsoleKey.DownArrow;
                            break;
                        case ConsoleKey.NumPad3:
                            retValue = ConsoleKey.PageDown;
                            break;
                        case ConsoleKey.NumPad4:
                            retValue = ConsoleKey.LeftArrow;
                            break;
                        case ConsoleKey.NumPad5:
                            retValue = ConsoleKey.Clear;
                            break;
                        case ConsoleKey.NumPad6:
                            retValue = ConsoleKey.RightArrow;
                            break;
                        case ConsoleKey.NumPad7:
                            retValue = ConsoleKey.Home;
                            break;
                        case ConsoleKey.NumPad8:
                            retValue = ConsoleKey.UpArrow;
                            break;
                        case ConsoleKey.NumPad9:
                            retValue = ConsoleKey.PageUp;
                            break;
                        case ConsoleKey.Decimal:
                            retValue = ConsoleKey.Delete;
                            break;
                    }
                }
            }

            return retValue;
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static void CheckKeyboardLayout()
        {
            if (currentKeyboardLayoutId != CultureInfo.CurrentCulture.KeyboardLayoutId)
            {
                currentKeyboardLayoutId = CultureInfo.CurrentCulture.KeyboardLayoutId;
                LoadKeyboardLayout();
            }
        }

        private static Stream GetStream()
        {
            string fileName = currentKeyboardLayoutId + ".xml";

            // search AdditionalLayoutsFolder
            if (!string.IsNullOrEmpty(AdditionalLayoutsFolder) && Directory.Exists(AdditionalLayoutsFolder))
            {
                string[] files = Directory.GetFiles(AdditionalLayoutsFolder, fileName, SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    if (Path.GetFileName(file) == fileName)
                    {
                        return new FileStream(file, FileMode.Open);
                    }
                }
            }

            // search Miyagi.KeyboardLayouts.zip
            if (File.Exists("Miyagi.KeyboardLayouts.zip"))
            {
                using (var zip = ZipStorer.Open("Miyagi.KeyboardLayouts.zip", FileAccess.Read))
                {
                    var files = zip.ReadCentralDir();
                    foreach (var entry in files)
                    {
                        if (entry.FilenameInZip == fileName)
                        {
                            var ms = new MemoryStream();
                            zip.ExtractFile(entry, ms);
                            ms.Position = 0;
                            return ms;
                        }
                    }
                }
            }

            // search embedded resources
            var asm = Assembly.GetExecutingAssembly();
            fileName = string.Format(CultureInfo.InvariantCulture, "Miyagi.Internals.Resources.KeyboardLayouts.{0}.xml", currentKeyboardLayoutId);
            var res = asm.GetManifestResourceNames();

            return res.Any(t => t == fileName) ? asm.GetManifestResourceStream(fileName) : asm.GetManifestResourceStream("Miyagi.Internals.Resources.KeyboardLayouts.1033.xml");
        }

        private static void LoadKeyboardLayout()
        {
            using (var layoutStream = GetStream())
            {
                using (var xtr = XmlReader.Create(layoutStream))
                {
                    currentXmlDoc = XDocument.Load(xtr);
                }
            }
        }

        #endregion Private Static Methods

        #endregion Methods
    }
}