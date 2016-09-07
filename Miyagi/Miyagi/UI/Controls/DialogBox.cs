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
    using System.Security.Permissions;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Resources;
    using Miyagi.Internals;

    /// <summary>
    /// A DialogBox.
    /// </summary>
    public sealed class DialogBox : ModalGUI
    {
        #region Fields

        private readonly Size buttonSize;
        private readonly Point buttonTextOffset;
        private readonly Point labelTextOffset;
        private readonly Point location;
        private readonly Size size;

        private Button buttonCancel;
        private Button buttonNo;
        private Button buttonOk;
        private Button buttonYes;
        private DialogResult dialogResult;
        private Panel mainPanel;

        #endregion Fields

        #region Constructors

        private DialogBox(DialogBoxSettings dialogBoxSettings, MiyagiSystem system)
        {
            this.GUIManager = system.GUIManager;
            this.SpriteRenderer = system.RenderManager.Create2DRenderer();
            system.LocaleManager.CurrentCultureChanged += this.LocaleManagerCultureChanged;

            if (dialogBoxSettings == null)
            {
                Size screenRes = system.RenderManager.MainViewport.Size;
                this.location = new Point(screenRes.Width / 3, screenRes.Height / 4);
                this.labelTextOffset = new Point(3, 5);
                this.buttonTextOffset = Point.Empty;
                this.size = new Size(screenRes.Width / 3, screenRes.Height / 4);
                this.buttonSize = new Size(this.size.Width / 5, this.size.Height / 8);
            }
            else
            {
                this.location = dialogBoxSettings.Location;
                this.labelTextOffset = dialogBoxSettings.LabelTextOffset;
                this.buttonTextOffset = dialogBoxSettings.ButtonTextOffset;
                this.size = dialogBoxSettings.Size;
                this.buttonSize = dialogBoxSettings.ButtonSize;
            }

            system.GUIManager.ReleaseFocusedAndGrabbedControl();

            // reset result
            this.dialogResult = DialogResult.None;

            // set this to the top of the ModalDialogs stack
            system.GUIManager.PushModalDialog(this);
            this.Visible = true;
        }

        #endregion Constructors

        #region Properties

        #region Public Static Properties

        /// <summary>
        /// Gets or sets the button skin.
        /// </summary>
        public static Skin ButtonSkin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the skin.
        /// </summary>
        public static Skin Skin
        {
            get;
            set;
        }

        #endregion Public Static Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Shows the DialogBox.
        /// </summary>
        /// <param name="text">The text of the DialogBox.</param>
        /// <param name="title">The title of the DialogBox.</param>
        /// <returns>A DialogResult enum representing the pressed button.</returns>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public static DialogResult Show(string text, string title)
        {
            return Show(text, title, DialogBoxButtons.Ok);
        }

        /// <summary>
        /// Shows the DialogBox.
        /// </summary>
        /// <param name="text">The text of the DialogBox.</param>
        /// <param name="title">The title of the DialogBox.</param>
        /// <param name="buttons">The buttons of the DialogBox.</param>
        /// <returns>A DialogResult enum representing the pressed button.</returns>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public static DialogResult Show(string text, string title, DialogBoxButtons buttons)
        {
            return Show(text, title, buttons, null);
        }

        /// <summary>
        /// Shows the DialogBox.
        /// </summary>
        /// <param name="text">The text of the DialogBox.</param>
        /// <param name="title">The title of the DialogBox.</param>
        /// <param name="buttons">The buttons of the DialogBox.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>
        /// A DialogResult enum representing the pressed button.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public static DialogResult Show(string text, string title, DialogBoxButtons buttons, DialogBoxSettings settings)
        {
            var dialogBox = new DialogBox(settings, MiyagiSystem.Latest);
            dialogBox.CreateControls(text, title, buttons);

            while (dialogBox.dialogResult == DialogResult.None)
            {
                if (dialogBox.MiyagiSystem == null || !dialogBox.MiyagiSystem.Backend.MessagePump())
                {
                    dialogBox.Close();
                    return DialogResult.None;
                }
            }

            dialogBox.Close();
            return dialogBox.dialogResult;
        }

        #endregion Public Static Methods

        #region Private Methods

        private void Close()
        {
            this.MiyagiSystem.GUIManager.PopModalDialog(this);
            this.Dispose();
        }

        [SecurityPermission(SecurityAction.LinkDemand)]
        private void CreateControls(string text, string title, DialogBoxButtons buttons)
        {
            Skin dialogBoxSkin = Skin ?? (Skin = Skin.CreateForDialogBox(this.MiyagiSystem));
            Skin dialogBoxButtonSkin = ButtonSkin ?? (ButtonSkin = Skin.CreateForDialogBoxButton(this.MiyagiSystem));

            // create main panel
            this.Controls.Add(
                this.mainPanel = new Panel
                                 {
                                     Movable = true,
                                     Size = this.size,
                                     Location = this.location,
                                     ResizeMode = ResizeModes.None,
                                     Skin = dialogBoxSkin
                                 });

            // title label
            this.mainPanel.Controls.Add(
                new Label
                {
                    Size = new Size(this.size.Width, this.size.Height / 4),
                    Location = Point.Empty,
                    Text = title,
                    TextStyle =
                        {
                            Alignment = Alignment.TopLeft,
                            Offset = this.labelTextOffset
                        }
                });

            // text label
            this.mainPanel.Controls.Add(
                new Label
                {
                    Size = new Size(this.size.Width, this.size.Height / 4),
                    Location = new Point(0, this.size.Height / 4),
                    Text = text,
                    TextStyle =
                        {
                            Alignment = Alignment.MiddleCenter,
                            Offset = this.labelTextOffset
                        }
                });

            // buttons
            if (buttons.IsFlagSet(DialogBoxButtons.Ok))
            {
                this.mainPanel.Controls.Add(
                    this.buttonOk = new Button
                                    {
                                        Size = this.buttonSize,
                                        TextStyle =
                                            {
                                                Alignment = Alignment.MiddleCenter,
                                                Offset = this.buttonTextOffset,
                                            },
                                        Skin = dialogBoxButtonSkin
                                    });

                this.buttonOk.MouseClick += this.DialogBoxMouseClick;
            }

            if (buttons.IsFlagSet(DialogBoxButtons.Yes))
            {
                this.mainPanel.Controls.Add(
                    this.buttonYes = new Button
                                     {
                                         Size = this.buttonSize,
                                         TextStyle =
                                             {
                                                 Alignment = Alignment.MiddleCenter,
                                                 Offset = this.buttonTextOffset,
                                             },
                                         Skin = dialogBoxButtonSkin
                                     });

                this.buttonYes.MouseClick += this.DialogBoxMouseClick;
            }

            if (buttons.IsFlagSet(DialogBoxButtons.No))
            {
                this.mainPanel.Controls.Add(
                    this.buttonNo = new Button
                                    {
                                        Size = this.buttonSize,
                                        TextStyle =
                                            {
                                                Alignment = Alignment.MiddleCenter,
                                                Offset = this.buttonTextOffset,
                                            },
                                        Skin = dialogBoxButtonSkin
                                    });

                this.buttonNo.MouseClick += this.DialogBoxMouseClick;
            }

            if (buttons.IsFlagSet(DialogBoxButtons.Cancel))
            {
                this.mainPanel.Controls.Add(
                    this.buttonCancel = new Button
                                        {
                                            Size = this.buttonSize,
                                            TextStyle =
                                                {
                                                    Alignment = Alignment.MiddleCenter,
                                                    Offset = this.buttonTextOffset,
                                                },
                                            Skin = dialogBoxButtonSkin
                                        });

                this.buttonCancel.MouseClick += this.DialogBoxMouseClick;
            }

            // align buttons
            int buttonCount = this.mainPanel.Controls.Count - 2;
            int totalWidth = (this.buttonSize.Width * buttonCount) + ((this.buttonSize.Width / 10) * (buttonCount - 1));
            int width = this.buttonSize.Width + (this.buttonSize.Width / 10);
            for (int i = 1; i <= buttonCount; i++)
            {
                int x = ((this.size.Width - totalWidth) / 2) + (width * (i - 1));
                int y = (this.size.Height * 3) / 4;
                this.mainPanel.Controls[i + 1].Location = new Point(x, y);
            }

            this.SetButtonText();
        }

        private void DialogBoxMouseClick(object sender, MouseButtonEventArgs e)
        {
            var b = (Button)sender;

            if (b == this.buttonCancel)
            {
                this.dialogResult = DialogResult.Cancel;
            }
            else if (b == this.buttonNo)
            {
                this.dialogResult = DialogResult.No;
            }
            else if (b == this.buttonOk)
            {
                this.dialogResult = DialogResult.Ok;
            }
            else if (b == this.buttonYes)
            {
                this.dialogResult = DialogResult.Yes;
            }
        }

        private void LocaleManagerCultureChanged(object sender, EventArgs e)
        {
            this.SetButtonText();
        }

        private void SetButtonText()
        {
            LocaleManager loc = this.MiyagiSystem.LocaleManager;

            if (this.buttonYes != null)
            {
                this.buttonYes.Text = loc.GetStringInternal("{DialogBox.Yes}");
            }

            if (this.buttonNo != null)
            {
                this.buttonNo.Text = loc.GetStringInternal("{DialogBox.No}");
            }

            if (this.buttonCancel != null)
            {
                this.buttonCancel.Text = loc.GetStringInternal("{DialogBox.Cancel}");
            }

            if (this.buttonOk != null)
            {
                this.buttonOk.Text = loc.GetStringInternal("{DialogBox.OK}");
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}