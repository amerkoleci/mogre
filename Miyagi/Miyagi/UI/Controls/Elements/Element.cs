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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// The base class for elements.
    /// </summary>
    /// <typeparam name = "TOwner">The type of the owner.</typeparam>
    /// <typeparam name = "TStyle">The type of the style.</typeparam>
    public abstract class Element<TOwner, TStyle> : IElement
        where TOwner : class, IElementOwner
        where TStyle : Style, new()
    {
        #region Fields

        private readonly Func<int> zorderGetter;

        private bool croppingDisabled;
        private TextureFrame currentFrame;
        private RectangleF currentUV;
        private Size size;
        private bool stopAnimation;
        private TStyle style;
        private Texture texture;
        private TimeSpan textureAnimationTime;
        private UpdateTypes updateType;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Element{TOwner, TStyle}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        protected Element(TOwner owner, Func<int> zorderGetter)
        {
            this.Owner = owner;
            this.Style = new TStyle();
            this.zorderGetter = zorderGetter;
        }

        /// <summary>
        /// Finalizes an instance of the Element class.
        /// </summary>
        ~Element()
        {
            this.Dispose(false);
        }

        #endregion Constructors

        #region Properties

        #region Explicit Interface Properties

        IElementOwner IElement.Owner
        {
            get
            {
                return this.Owner;
            }
        }

        #endregion Explicit Interface Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether sprites of the element can be cropped.
        /// </summary>
        public bool CroppingDisabled
        {
            get
            {
                return this.croppingDisabled;
            }

            set
            {
                this.croppingDisabled = value;
                if (this.Sprite != null)
                {
                    this.Sprite.RemoveCrop();
                }

                this.ForEachSubElement(ele => ele.CroppingDisabled = value);
            }
        }

        /// <summary>
        /// Gets the display rectangle.
        /// </summary>
        /// <value>The display rectangle.</value>
        public virtual Rectangle DisplayRectangle
        {
            get
            {
                return this.Owner.DisplayRectangle;
            }
        }

        /// <summary>
        /// Gets the MiyagiSystem.
        /// </summary>
        public MiyagiSystem MiyagiSystem
        {
            get
            {
                return this.Owner == null ? null : this.Owner.MiyagiSystem;
            }
        }

        /// <summary>
        /// Gets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public virtual float Opacity
        {
            get
            {
                return this.Owner.Opacity;
            }
        }

        /// <summary>
        /// Gets or sets the owner of the element.
        /// </summary>
        /// <value>A IElementOwner representing the owner.</value>
        public TOwner Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the owning control.
        /// </summary>
        /// <value>A Control representing the owner of the element.</value>
        /// <exception cref="InvalidOperationException"><c>InvalidOperationException</c>.</exception>
        public Control OwningControl
        {
            get
            {
                // get the owning control
                IElementOwner elementOwner = this.Owner;
                while (elementOwner != null)
                {
                    if (elementOwner is Control)
                    {
                        return (Control)elementOwner;
                    }

                    if (elementOwner is ListItem)
                    {
                        return ((ListItem)elementOwner).Owner;
                    }

                    if (elementOwner is IElement)
                    {
                        elementOwner = ((IElement)elementOwner).Owner;
                    }
                    else
                    {
                        return null;
                        //throw new InvalidOperationException();
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element can be resized.
        /// </summary>
        public bool ResizeDisabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public virtual Size Size
        {
            get
            {
                if (this.Sprite != null)
                {
                    var q = this.Sprite.GetQuad(0);
                    int height = (int)Math.Ceiling((q.Height / 2) * this.ViewportSize.Height);
                    int width = (int)Math.Ceiling((q.Width / 2) * this.ViewportSize.Width);
                    return new Size(width, height);
                }

                return this.size;
            }

            set
            {
                this.size = value;
                this.UpdateType |= UpdateTypes.Size;
                this.UpdateSpriteCrop();
            }
        }

        /// <summary>
        /// Gets or sets the sprite of the element.
        /// </summary>
        public UISprite Sprite
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the SpriteRenderer.
        /// </summary>
        public virtual ISpriteRenderer SpriteRenderer
        {
            get
            {
                return this.Owner.SpriteRenderer;
            }
        }

        /// <summary>
        /// Gets or sets the style of the element.
        /// </summary>
        public TStyle Style
        {
            get
            {
                return this.style;
            }

            set
            {
                if (this.style != value)
                {
                    if (this.style != null)
                    {
                        this.style.PropertyChanged -= this.OnStylePropertyChanged;
                        this.style.PropertyChanging -= this.OnStylePropertyChanging;
                    }

                    this.style = value;

                    if (this.style != null)
                    {
                        this.style.PropertyChanged += this.OnStylePropertyChanged;
                        this.style.PropertyChanging += this.OnStylePropertyChanging;
                    }

                    this.SetSubElementStyles();

                    this.RemoveSprite();
                }
            }
        }

        /// <summary>
        /// Gets a collection of subelement.
        /// </summary>
        public virtual IEnumerable<IElement> SubElements
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        public Texture Texture
        {
            get
            {
                return this.texture ?? this.GetDefaultTexture();
            }

            set
            {
                if (this.texture != value)
                {
                    this.texture = value;
                    this.updateType |= UpdateTypes.Texture;
                }
            }
        }

        /// <summary>
        /// Gets the texture filtering.
        /// </summary>
        public TextureFiltering TextureFiltering
        {
            get
            {
                return this.Owner.TextureFiltering;
            }
        }

        /// <summary>
        /// Gets or sets the UpdateType.
        /// </summary>
        public virtual UpdateTypes UpdateType
        {
            get
            {
                return this.updateType;
            }

            set
            {
                this.updateType = value;

                if (value == UpdateTypes.None)
                {
                    this.ForEachSubElement(ele => ele.UpdateType = UpdateTypes.None);
                }
            }
        }

        /// <summary>
        /// Gets the screen resolution.
        /// </summary>
        public Size ViewportSize
        {
            get
            {
                return this.SpriteRenderer.Viewport.Size;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Element&lt;TOwner, TStyle&gt;"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public virtual bool Visible
        {
            get
            {
                return this.Owner.Visible;
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the current frame.
        /// </summary>
        protected TextureFrame CurrentFrame
        {
            get
            {
                return this.currentFrame;
            }

            private set
            {
                if (this.currentFrame != value)
                {
                    this.currentFrame = value;
                    this.UpdateType |= UpdateTypes.Texture;
                    this.CurrentUV = value.UV;
                }
            }
        }

        /// <summary>
        /// Gets the current uv-coordinates.
        /// </summary>
        protected RectangleF CurrentUV
        {
            get
            {
                return this.currentUV;
            }

            private set
            {
                if (this.currentUV != value)
                {
                    this.currentUV = value;
                    if (this.Sprite != null)
                    {
                        this.Sprite.SetUV(value);
                    }
                }
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Applies the opacity.
        /// </summary>
        public virtual void ApplyOpacity()
        {
            if (this.Sprite != null)
            {
                this.Sprite.Opacity = this.Owner.Opacity;
            }

            this.ForEachSubElement(ele => ele.ApplyOpacity());
        }

        /// <summary>
        /// Applies the TextureFiltering.
        /// </summary>
        public virtual void ApplyTextureFiltering()
        {
            if (this.Sprite != null)
            {
                this.Sprite.TexFilter = this.Owner.TextureFiltering;
            }

            this.ForEachSubElement(ele => ele.ApplyTextureFiltering());
        }

        /// <summary>
        /// Applies the visibility.
        /// </summary>
        public virtual void ApplyVisibility()
        {
            if (this.Sprite != null)
            {
                this.Sprite.Visible = this.Visible;
            }

            this.ForEachSubElement(ele => ele.ApplyVisibility());
        }

        /// <summary>
        /// Applies the ZOrder.
        /// </summary>
        public virtual void ApplyZOrder()
        {
            if (this.Sprite != null)
            {
                this.Sprite.ZOrder = this.GetZOrder();
            }

            this.ForEachSubElement(ele => ele.ApplyZOrder());
        }

        /// <summary>
        /// Returns whether the Sprites property is null.
        /// </summary>
        /// <returns><c>true</c> if the Sprites property is null; otherwise, <c>false</c>.</returns>
        public virtual bool AreAllSpritesNull()
        {
            return this.Sprite == null;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the element.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the Z-order.
        /// </summary>
        /// <returns>An <see cref="Int32"/> representing the Z-order.</returns>
        public int GetZOrder()
        {
            return this.zorderGetter();
        }

        /// <summary>
        /// Performs a hit test.
        /// </summary>
        /// <param name="p">The coordinate.</param>
        /// <returns>
        /// <c>true</c> if the coordinates hit the element; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HitTest(Point p)
        {
            bool pixelPerfect = false;
            if (this.MiyagiSystem != null && this.MiyagiSystem.InputManager != null)
            {
                pixelPerfect = this.MiyagiSystem.GUIManager.PixelPerfectHitDetection;
            }

            if (this.Sprite != null && this.Sprite.HitTest(p, pixelPerfect))
            {
                return true;
            }

            IEnumerable<IElement> elements = this.SubElements;
            return elements != null && elements.Any(t => t.HitTest(p));
        }

        /// <summary>
        /// Injects a pressed key.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> to inject.</param>
        public void InjectKeyDown(KeyboardEventArgs e)
        {
            this.OnKeyDown(e);
        }

        /// <summary>
        /// Injects a held key.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.KeyboardEventArgs"/> instance containing the event data.</param>
        public void InjectKeyHeld(KeyboardEventArgs e)
        {
            this.OnKeyHeld(e);
        }

        /// <summary>
        /// Inject a released key.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> to inject.</param>
        public void InjectKeyUp(KeyboardEventArgs e)
        {
            this.OnKeyUp(e);
        }

        /// <summary>
        /// Inject a pressed mouse button.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> to inject.</param>
        public void InjectMouseDown(MouseButtonEventArgs e)
        {
            this.OnMouseDown(e);
        }

        /// <summary>
        /// Inject a dragged mouse.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> to inject.</param>
        public void InjectMouseDrag(ChangedValueEventArgs<Point> e)
        {
            this.OnMouseDrag(e);
        }

        /// <summary>
        /// Inject an entering mouse.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> to inject.</param>
        public void InjectMouseEnter(MouseEventArgs e)
        {
            this.OnMouseEnter(e);
        }

        /// <summary>
        /// Injects a held mouse button.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        public void InjectMouseHeld(MouseButtonEventArgs e)
        {
            this.OnMouseHeld(e);
        }

        /// <summary>
        /// Inject a hovering mouse.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> to inject.</param>
        public void InjectMouseHover(MouseEventArgs e)
        {
            this.OnMouseHover(e);
        }

        /// <summary>
        /// Inject a leaving mouse.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> to inject.</param>
        public void InjectMouseLeave(MouseEventArgs e)
        {
            this.OnMouseLeave(e);
        }

        /// <summary>
        /// Inject a released mouse button.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> to inject.</param>
        public void InjectMouseUp(MouseButtonEventArgs e)
        {
            this.OnMouseUp(e);
        }

        /// <summary>
        /// Inject a moved mouse wheel.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> to inject.</param>
        public void InjectMouseWheelMoved(ValueEventArgs<int> e)
        {
            this.OnMouseWheelMoved(e);
        }

        /// <summary>
        /// Moves the sprites of the element.
        /// </summary>
        /// <param name="offset">The distance to move.</param>
        public virtual void Move(Point offset)
        {
            if (this.Sprite != null)
            {
                this.Sprite.Move(offset);
            }

            this.ForEachSubElement(ele => ele.Move(offset));
        }

        //protected bool SetBufferDirty
        //{
        //    get 
        //    {
        //        if (Owner != null
        //            && (Owner is ToolTipElement || Owner is IToolTipElementOwner))
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //}

        /// <summary>
        /// Removes the sprite.
        /// </summary>
        public virtual void RemoveSprite()
        {
            this.ForEachSubElement(ele => ele.RemoveSprite());

            if (this.Sprite == null)
            {
                return;
            }

            if (this.Owner != null)
            {
                this.Sprite.RemoveFromRenderer();
                this.Sprite = null;
            }
        }

        /// <summary>
        /// Resizes the sprites of the element.
        /// </summary>
        /// <param name="diff">The distance to resize.</param>
        public virtual void Resize(Point diff)
        {
            // nothing to do
        }

        /// <summary>
        /// Restarts the texture animation.
        /// </summary>
        public void RestartTextureAnimation()
        {
            this.textureAnimationTime = TimeSpan.Zero;
            this.stopAnimation = false;
        }

        /// <summary>
        /// Stops the texture animation.
        /// </summary>
        public void StopTextureAnimation()
        {
            this.stopAnimation = true;
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        public void Update()
        {
            this.Update(Point.Empty, Point.Empty);
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        /// <param name="deltaLocation">The location delta.</param>
        /// <param name="deltaSize">The size delta.</param>
        public void Update(Point deltaLocation, Point deltaSize)
        {
            if (this.updateType != UpdateTypes.None && !this.AreAllSpritesNull())
            {
                // change zorder
                if (this.updateType.IsFlagSet(UpdateTypes.ZOrder))
                {
                    this.ApplyZOrder();
                }

                // change visibility
                if (this.updateType.IsFlagSet(UpdateTypes.Visibility))
                {
                    this.ApplyVisibility();
                }

                // change opacity
                if (this.updateType.IsFlagSet(UpdateTypes.Opacity))
                {
                    this.ApplyOpacity();
                }

                // change texture filtering
                if (this.updateType.IsFlagSet(UpdateTypes.TextureFiltering))
                {
                    this.ApplyTextureFiltering();
                }

                // move sprites to new position
                if (this.updateType.IsFlagSet(UpdateTypes.OwnerLocation))
                {
                    this.Move(deltaLocation);
                    this.UpdateSpriteCrop();
                }

                // resize sprites
                if (this.updateType.IsFlagSet(UpdateTypes.OwnerSize))
                {
                    if (!this.ResizeDisabled)
                    {
                        this.Resize(deltaSize);
                    }

                    this.UpdateType |= UpdateTypes.Size;
                    this.UpdateSpriteCrop();
                }

                // update sprite crop
                if (this.updateType.IsFlagSet(UpdateTypes.SpriteCrop))
                {
                    this.UpdateSpriteCrop();
                }
            }

            var texture = this.Texture;
            if (texture != null)
            {
                // get the next frame
                var nextFrame = texture.GetFrameFromTime(this.textureAnimationTime);
                RectangleF uv = this.currentFrame != null ? this.currentUV : nextFrame.UV;
                this.CurrentFrame = nextFrame;

                if (!this.stopAnimation)
                {
                    if (texture.Frames.Count > 1)
                    {
                        this.textureAnimationTime += this.MiyagiSystem.TimeSinceLastUpdate;
                    }

                    if (texture.ScrollVector != PointF.Empty)
                    {
                        this.CurrentUV = texture.GetScrollVectorOffset(uv, this.MiyagiSystem.TimeSinceLastUpdate.TotalMilliseconds);
                    }
                }
            }

            this.UpdateCore();

            this.UpdateType = UpdateTypes.None;
        }

        /// <summary>
        /// Crops the sprites.
        /// </summary>
        public virtual void UpdateSpriteCrop()
        {
            if (!this.CroppingDisabled)
            {
                if (this.Sprite != null)
                {
                    this.Sprite.UpdateCrop();
                }

                this.ForEachSubElement(ele => ele.UpdateSpriteCrop());
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Releases the unmanaged resources used by the element.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.style != null)
                {
                    this.Style = null;
                }

                this.ForEachSubElement(ele => ele.Dispose());
            }

            this.RemoveSprite();
        }

        /// <summary>
        /// Executes an action for each sub element.
        /// </summary>
        /// <param name="action">The Action.</param>
        protected void ForEachSubElement(Action<IElement> action)
        {
            var elements = this.SubElements;
            if (elements != null)
            {
                foreach (var ele in elements)
                {
                    action(ele);
                }
            }
        }

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected virtual Rectangle GetBounds()
        {
            return Rectangle.Empty;
        }

        /// <summary>
        /// Gets the default texture.
        /// </summary>
        /// <returns>The default texture.</returns>
        protected virtual Texture GetDefaultTexture()
        {
            return null;
        }

        /// <summary>
        /// Handles KeyDown injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.KeyboardEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyDown(KeyboardEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles KeyHeld injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.KeyboardEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyHeld(KeyboardEventArgs e)
        {
        }

        /// <summary>
        /// Handles KeyUp injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.KeyboardEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyUp(KeyboardEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles MouseDown injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseDown(MouseButtonEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles MouseDrag injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles MouseEnter injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseEnter(MouseEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles MouseHeld injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseHeld(MouseButtonEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles MouseHover injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseHover(MouseEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles MouseLeave injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseLeave(MouseEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles MouseUp injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseUp(MouseButtonEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles MouseWheel injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.ValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual void OnMouseWheelMoved(ValueEventArgs<int> e)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected virtual void OnStylePropertyChanged(string name)
        {
            // nothing to do
        }

        /// <summary>
        /// Handles changing style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected virtual void OnStylePropertyChanging(string name)
        {
            // nothing to do
        }

        /// <summary>
        /// Prepares the sprite of the element.
        /// </summary>
        /// <param name="quads">The quads.</param>
        protected void PrepareSprite(params Quad[] quads)
        {
            this.Sprite = new UISprite(this, quads)
                          {
                              Visible = this.Visible,
                              Opacity = this.Opacity,
                              ZOrder = this.GetZOrder(),
                              TexFilter = this.TextureFiltering,
                          };

            if (this.Texture != null && this.Texture.GpuPrograms.Count > 0)
            {
                this.Sprite.GpuPrograms = this.Texture.GpuPrograms;
            }
        }

        /// <summary>
        /// Sets the texture of the sprite to the current frame of the element.
        /// </summary>
        protected void SetSpriteTexture()
        {
            if (this.Texture == null)
            {
                // remove sprites if texture is null
                if (this.Sprite != null)
                {
                    this.Sprite.RemoveFromRenderer();
                    this.Sprite = null;
                }
            }
            else
            {
                // change texture
                if (this.Sprite != null)
                {
                    this.Sprite.SetTexture(this.CurrentFrame.FileName);
                    this.Sprite.SetUV(this.CurrentFrame.UV);
                    this.Sprite.GpuPrograms = this.Texture.GpuPrograms;
                    this.UpdateSpriteCrop();
                }
            }
        }

        /// <summary>
        /// Sets the style of the sub elements.
        /// </summary>
        protected virtual void SetSubElementStyles()
        {
            // nothing to do
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        protected virtual void UpdateCore()
        {
            // nothing to do
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnStylePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnStylePropertyChanged(e.PropertyName);
        }

        private void OnStylePropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            this.OnStylePropertyChanging(e.PropertyName);
        }

        #endregion Private Methods

        #endregion Methods
    }
}