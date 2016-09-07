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

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Resources;
    using Miyagi.Common.Serialization;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// The abstract base class for textured controls.
    /// </summary>
    public abstract class SkinnedControl : Label, IBorderElementOwner, ISkinElementOwner
    {
        #region Fields

        private BorderElement borderElement;
        private Skin skin;
        private SkinElement skinElement;        

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SkinnedControl class.
        /// </summary>
        /// <param name="name">The name of the SkinnedControl.</param>
        protected SkinnedControl(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the skin of this control changes.
        /// </summary>
        public event EventHandler SkinChanged;

        #endregion Events

        #region Properties

        #region Explicit Interface Properties

        UISprite IBorderElementOwner.Sprite
        {
            get
            {
                return this.SkinElement.Sprite;
            }
        }

        BorderElement ISkinElementOwner.BorderElement
        {
            get
            {
                return this.BorderElement;
            }
        }

        #endregion Explicit Interface Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        public BorderStyle BorderStyle
        {
            get
            {
                return this.BorderElement.Style;
            }

            set
            {
                this.ThrowIfDisposed();
                this.BorderElement.Style = value;
            }
        }

        protected internal override bool IsExceedingParent
        {
            get
            {
                if (skinElement == null)
                    return false;
                else return skinElement.CroppingDisabled;
            }
        }

        /// <summary>
        /// Gets the combined name of the skin.
        /// </summary>
        public virtual string CombinedSkinName
        {
            get
            {
                Skin skin = this.Skin;
                if (skin != null)
                {
                    string retValue = skin.Name;

                    // focused
                    string focused = string.Concat(retValue, Skin.FocusedSkin);
                    retValue = this.Focused && skin.IsSubSkinDefined(focused)
                                   ? focused : retValue;

                    // disabled
                    string disabled = string.Concat(skin.Name, Skin.DisabledSkin);
                    retValue = !this.Enabled && skin.IsSubSkinDefined(disabled)
                                   ? disabled : retValue;

                    return retValue;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return this.BorderElement.HasBorder();
            }
        }

        /// <summary>
        /// Gets or sets the skin.
        /// </summary>
        [SerializerOptions(Redirect = "Skins")]
        public Skin Skin
        {
            get
            {
                return this.skin;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.skin != value)
                {
                    if (this.skin != null)
                    {
                        this.skin.SubSkinChanged -= this.OnSubSkinChanged;
                    }

                    this.skin = value;

                    if (this.skin != null)
                    {
                        this.skin.SubSkinChanged += this.OnSubSkinChanged;
                    }

                    this.OnSkinChanged(new EventArgs());
                }
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the border.
        /// </summary>
        /// <value>A BorderElement representing the border.</value>
        protected BorderElement BorderElement
        {
            get
            {
                return this.borderElement ?? (this.borderElement = new BorderElement(this, () => (this.ZOrder * 10) + 1)
                                                                   {
                                                                       ThicknessChangedCallback = (oldSize, newSize) => this.OnBorderThicknessChanged(new ChangedValueEventArgs<Thickness>(oldSize, newSize))
                                                                   });
            }
        }

        /// <summary>
        /// Gets the client location.
        /// </summary>
        protected override Point ClientLocation
        {
            get
            {
                var retValue = base.ClientLocation;
                if (this.HasBorder)
                {
                    var t = this.BorderElement.Style.Thickness;
                    retValue = new Point(retValue.X + t.Left, retValue.Y + t.Top);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        protected override IList<IElement> Elements
        {
            get
            {
                var retValue = base.Elements;
                if (this.skinElement != null)
                {
                    retValue.Add(this.skinElement);
                }

                if (this.borderElement != null)
                {
                    retValue.Add(this.borderElement);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <value>A SkinElement representing the texture.</value>
        protected SkinElement SkinElement
        {
            get
            {
                return this.skinElement ?? (this.skinElement = new SkinElement(this, () => this.ZOrder * 10));
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Sets the background texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        public void SetBackgroundTexture(Texture texture)
        {
            this.SkinElement.Texture = texture;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Changes the current texture.
        /// </summary>
        /// <param name="subSkin">The name of the subskin.</param>
        /// <param name="reason">The reason for the change.</param>
        protected virtual void ChangeTexture(string subSkin, string reason)
        {
            if (this.Skin != null)
            {
                this.SkinElement.Texture = this.Skin.SubSkins[subSkin];
            }
        }

        /// <summary>
        /// Calculates a client area based on the size of a control.
        /// </summary>
        /// <param name="size">The size of the control.</param>
        /// <returns>A size representing said client area.</returns>
        protected override Size ClientSizeFromSize(Size size)
        {
            var retValue = base.ClientSizeFromSize(size);
            return !this.HasBorder
                       ? retValue
                       : retValue - this.BorderElement.Style.Thickness;
        }

        /// <summary>
        /// Disposes the control.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);            

            if (disposing)
            {
                this.skinElement = null;
                this.borderElement = null;                
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

            this.SkinElement.Size = this.Size;

            if (this.HasBorder)
            {
                this.BorderStyle.Resize(widthFactor, heightFactor);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the provided coordinates are inside the control.
        /// </summary>
        /// <param name="p">The coordinate.</param>
        /// <returns><c>true</c> if the coordinates are inside the control; otherwise, <c>false</c>.</returns>
        protected override bool HitTestCore(Point p)
        {
            return this.Skin == null ? this.AbsoluteRectangle.Contains(p) : base.HitTestCore(p);
        }

        /// <summary>
        /// Handles border thickness changes.
        /// </summary>
        /// <param name="e">A ChangedValueEventArgs that contains the event data.</param>
        protected virtual void OnBorderThicknessChanged(ChangedValueEventArgs<Thickness> e)
        {
            this.UpdateClientSize();
            this.OffsetChildren(new Point(e.NewValue.Left - e.OldValue.Left, e.NewValue.Top - e.OldValue.Top));
        }

        /// <summary>
        /// Raises the <see cref="Control.EnabledChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            if (this.ToolTipElement.Visible)
            {
                this.ToolTipElement.Hide();
            }

            this.ChangeTexture(this.CombinedSkinName, "EnabledChanged");
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.GotFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            this.ChangeTexture(this.CombinedSkinName, "GotFocus");
            base.OnGotFocus(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.LostFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            this.ChangeTexture(this.CombinedSkinName, "LostFocus");
            base.OnLostFocus(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnSizeChanged(ChangedValueEventArgs<Size> e)
        {
            base.OnSizeChanged(e);

            if (this.SkinElement.Sprite == null)
            {
                this.SkinElement.Size = e.NewValue;
            }
        }

        /// <summary>
        /// Raises the <see cref="SkinnedControl.SkinChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSkinChanged(EventArgs e)
        {
            // HasBorder might have changed, so update client size
            this.UpdateClientSize();
            this.ForceRedraw(false, false);
            this.SkinElement.Texture = this.skin.IsSubSkinDefined(this.CombinedSkinName) ? this.skin.SubSkins[this.CombinedSkinName] : null;

            if (this.SkinChanged != null)
            {
                this.SkinChanged(this, e);
            }
        }

        /// <summary>
        /// Calculates the size of the control based on the size of the client area.
        /// </summary>
        /// <param name="size">The size of the client area.</param>
        /// <returns>A size for the whole control.</returns>
        protected override Size SizeFromClientSize(Size size)
        {
            Size retValue = base.SizeFromClientSize(size);

            return !this.HasBorder
                       ? retValue
                       : new Size(
                             retValue.Width + this.BorderElement.Style.Thickness.Horizontal,
                             retValue.Height + this.BorderElement.Style.Thickness.Vertical);
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();

            this.SkinElement.Update(this.DeltaLocation, this.DeltaSize);

            if (this.HasBorder)
            {
                this.BorderElement.Update(this.DeltaLocation, this.DeltaSize);
            }

            if (!string.IsNullOrEmpty(this.ToolTipText) && this.IsMouseOver && this.Visible)
            {
                this.ToolTipElement.Update(this.DeltaLocation, this.DeltaSize);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnSubSkinChanged(object sender, EventArgs e)
        {
            this.OnSkinChanged(e);
        }

        #endregion Private Methods

        #endregion Methods
    }
}