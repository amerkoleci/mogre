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
    using Miyagi.Common.Resources;
    using Miyagi.UI.Controls.Elements;

    /// <summary>
    /// A DropDownList control.
    /// </summary>
    public class DropDownList : ListControl
    {
        #region Fields

        private ButtonElement buttonElement;
        private Size dropDownSize;
        private TextureElement dropDownTexture;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DropDownList class.
        /// </summary>
        public DropDownList()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DropDownList class.
        /// </summary>
        /// <param name="name">The name of the DropDownList.</param>
        public DropDownList(string name)
            : base(name)
        {
            this.ListElement.Hide();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the size of the drop-down list.
        /// </summary>
        public Size DropDownSize
        {
            get
            {
                return this.dropDownSize;
            }

            set
            {
                this.ThrowIfDisposed();

                if (this.dropDownSize != value)
                {
                    this.HideDropDownList();
                    this.dropDownSize = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the items should be cropped.
        /// </summary>
        public override bool ListBoxCroppingDisabled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the total height available for items.
        /// </summary>
        /// <value>An <see cref="int"/> representing the total height available for items.</value>
        public override int ListBoxHeight
        {
            get
            {
                return this.DropDownSize.Height;
            }
        }

        /// <summary>
        /// Gets the fixed vertical item offset.
        /// </summary>
        /// <value>An <see cref="int"/> representing the fixed vertical item offset.</value>
        public override int ListBoxVerticalOffset
        {
            get
            {
                return this.Height;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scrollbar should be displayed.
        /// </summary>
        public override bool ShouldShowScrollBar
        {
            get
            {
                return this.Items.Count > this.ListStyle.MaxVisibleItems && this.DropDownVisible;
            }
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets a value indicating whether the control exceeds its parent.
        /// </summary>
        protected internal override bool IsExceedingParent
        {
            get
            {
                return this.DropDownVisible && ((this.Left + this.dropDownSize.Width > this.Parent.DisplayRectangle.Width) || (this.Bottom + this.dropDownSize.Height > this.Parent.DisplayRectangle.Height));
            }
        }

        #endregion Protected Internal Properties

        #region Protected Properties

        /// <summary>
        /// Gets the button element.
        /// </summary>
        /// <value>The button element.</value>
        protected ButtonElement ButtonElement
        {
            get
            {
                if (this.buttonElement == null)
                {
                    this.buttonElement = new ButtonElement(this, () => (this.ZOrder * 10) + 1, "Button");
                    this.buttonElement.MouseDown += this.ButtonElementMouseDown;
                }

                return this.buttonElement;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the drop down list is visible.
        /// </summary>
        protected bool DropDownVisible
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        /// <value>A list of elements.</value>
        protected override IList<IElement> Elements
        {
            get
            {
                var retValue = base.Elements;
                if (this.DropDownVisible && this.dropDownTexture != null)
                {
                    retValue.Add(this.dropDownTexture);
                }

                if (this.buttonElement != null)
                {
                    retValue.Add(this.buttonElement);
                }

                return retValue;
            }
        }

        #endregion Protected Properties

        #region Private Properties

        private TextureElement DropDownTexture
        {
            get
            {
                if (this.dropDownTexture == null)
                {
                    this.dropDownTexture = new TextureElement(this, () => this.ZOrder * 10)
                                           {
                                               CroppingDisabled = true
                                           };
                    this.ListElement.ScrollBarElement.CroppingDisabled = true;
                }

                return this.dropDownTexture;
            }
        }

        #endregion Private Properties

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

            if (disposing)
            {
                if (this.dropDownTexture != null)
                {
                    this.dropDownTexture.Dispose();
                    this.dropDownTexture = null;
                }

                if (this.buttonElement != null)
                {
                    this.buttonElement.MouseDown -= this.ButtonElementMouseDown;
                    this.buttonElement = null;
                }
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

            this.ResizeHelper.Scale(ref this.dropDownSize);
            this.DropDownTexture.Size = this.dropDownSize;

            var buttonSize = this.ButtonElement.Size;
            this.ResizeHelper.Scale(ref buttonSize);
            this.ButtonElement.Size = buttonSize;

            var buttonLoc = this.ButtonElement.Location;
            this.ResizeHelper.Scale(ref buttonLoc);
            this.ButtonElement.Location = buttonLoc;

            this.HideDropDownList();
        }

        /// <summary>
        /// Hides the drop down list.
        /// </summary>
        protected virtual void HideDropDownList()
        {
            if (this.DropDownVisible)
            {
                this.DropDownVisible = false;
                this.DropDownTexture.RemoveSprite();

                this.ListElement.Hide();
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.LostFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.HideDropDownList();
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            this.ButtonElement.InjectMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHover"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            base.OnMouseHover(e);
            this.ButtonElement.InjectMouseHover(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.ButtonElement.InjectMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            this.ButtonElement.InjectMouseUp(e);
        }

        /// <summary>
        /// Raises the <see cref="ListControl.SelectedIndexChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            this.HideDropDownList();

            int index = this.SelectedIndex;
            if (index >= 0)
            {
                this.Text = this.Items[index].Text;
            }

            if (this.GUI != null)
            {
                var guiMgr = this.GUI.GUIManager;
                if (guiMgr.GrabbedControl == this)
                {
                    guiMgr.GrabbedControl = null;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnSizeChanged(ChangedValueEventArgs<Size> e)
        {
            base.OnSizeChanged(e);
            this.DropDownSize = new Size(e.NewValue.Width, this.DropDownSize.Height);

            this.ButtonElement.Location = new Point(this.Width - this.Height, 0);
            this.ButtonElement.Size = new Size(this.Height, this.Height);
            this.ButtonElement.RemoveSprite();
        }

        /// <summary>
        /// Raises the <see cref="SkinnedControl.SkinChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnSkinChanged(EventArgs e)
        {
            Skin skin = this.Skin;
            if (skin != null)
            {
                this.DropDownTexture.Texture = skin.SubSkins[skin.Name + ".List"];
            }

            base.OnSkinChanged(e);
        }

        /// <summary>
        /// Shows the drop down list.
        /// </summary>
        protected virtual void ShowDropDownList()
        {
            if (!this.DropDownVisible)
            {
                this.DropDownVisible = true;

                this.DropDownTexture.Offset = new Point(0, this.Height);
                this.DropDownTexture.Size = this.dropDownSize;

                this.ListElement.Show();
            }
        }

        /// <summary>
        /// Switches the drop down list visibility.
        /// </summary>
        protected void SwitchDropDownListVisibility()
        {
            if (this.DropDownVisible && !this.ListElement.ScrollBarElement.IsThumbHit)
            {
                this.HideDropDownList();
            }
            else
            {
                this.ShowDropDownList();
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            if (!this.DropDownVisible)
            {
                this.ListElement.ItemsDirty = false;
            }
            else
            {
                this.DropDownTexture.Update(this.DeltaLocation, this.DeltaSize);
            }

            this.ButtonElement.Update(this.DeltaLocation, this.DeltaSize);

            base.UpdateCore();
        }

        #endregion Protected Methods

        #region Private Methods

        private void ButtonElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.SwitchDropDownListVisibility();
        }

        #endregion Private Methods

        #endregion Methods
    }
}