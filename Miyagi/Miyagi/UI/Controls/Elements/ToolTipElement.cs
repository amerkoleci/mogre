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
    using System.Security.Permissions;

    using Miyagi.Common;
    using Miyagi.Common.Animation;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;
    using Miyagi.Common.Serialization;

    /// <summary>
    /// An element for displaying a ToolTip item.
    /// </summary>
    public sealed class ToolTipElement : Element<IToolTipElementOwner, ToolTipStyle>, INamable, ITextElementOwner
    {
        #region Fields

        private System.Drawing.Bitmap bitmap;

        private LinearFunctionValueController<float> fadeController;
        private DateTime mouseEnterTime;
        private float opacity;
        private DateTime showStartTime;
        private Size size;
        private TextElement textElement;
        private TextureElement textureElement1;
        private TextureElement textureElement2;
        private TextureElement textureElement3;
        private TextureElement textureElement4;
        private TextureElement textureElement5;
        private TextureElement textureElement6;
        private TextureElement textureElement7;
        private TextureElement textureElement8;
        private TextureElement textureElement9;
        private TextureElement innerTextureElement;
        private bool visible;
        Point oldLocation = Point.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ToolTipElement class.
        /// </summary>
        /// <param name="owner">The owner of the ToolTipElement.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public ToolTipElement(IToolTipElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
            this.TextureName = "Miyagi_ToolTipTexture_" + this.Name + Guid.NewGuid();
        }

        #endregion Constructors

        #region Properties

        #region Public Static Properties

        /// <summary>
        /// Gets or sets the default texture.
        /// </summary>
        public static Texture DefaultTexture1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default texture.
        /// </summary>
        public static Texture DefaultTexture2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default texture.
        /// </summary>
        public static Texture DefaultTexture3
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default texture.
        /// </summary>
        public static Texture DefaultTexture4
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default texture.
        /// </summary>
        public static Texture DefaultTexture5
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default texture.
        /// </summary>
        public static Texture DefaultTexture6
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default texture.
        /// </summary>
        public static Texture DefaultTexture7
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default texture.
        /// </summary>
        public static Texture DefaultTexture8
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default texture.
        /// </summary>
        public static Texture DefaultTexture9
        {
            get;
            set;
        }



        #endregion Public Static Properties

        #region Public Properties

        /// <summary>
        /// Gets the displayed text.
        /// </summary>
        public string DisplayedText
        {
            get
            {
                return this.Owner.ToolTipText;
            }
        }

        private System.Drawing.Bitmap Bitmap
        {
            get
            {
                return this.bitmap;
            }

            set
            {
                if (this.bitmap != value)
                {
                    this.SetBitmap(value);
                }
            }
        }

        /// <summary>
        /// Gets the display rectangle.
        /// </summary>
        public override Rectangle DisplayRectangle
        {
            get
            {
                return this.TextBounds;
            }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public Point Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the opacity.
        /// </summary>
        /// <value>The opacity, ranging between 0 and 1.</value>
        public override float Opacity
        {
            get
            {
                return this.opacity;
            }
        }

        /// <summary>
        /// Gets the size of the ToolTipElement.
        /// </summary>
        public override Size Size
        {
            get
            {
                return this.size;
            }

            //set
            //{
            //    this.size = value;
            //}
        }

        /// <summary>
        /// Gets the SpriteRenderer.
        /// </summary>
        public override ISpriteRenderer SpriteRenderer
        {
            get
            {
                return this.Owner.MiyagiSystem.RenderManager.MainRenderer;
            }
        }

        private void ForEachActiveElement(Action<IElement> action)
        {
            var elements = this.ActiveElements;
            if (elements != null)
            {
                foreach (var ele in elements)
                {
                    action(ele);
                }
            }
        }

        private IEnumerable<IElement> ActiveElements
        {
            get 
            {
                if (this.textElement != null
                    && !IsBitmapTooltip)
                {
                    yield return this.textElement;
                }

                if (this.textureElement1 != null)
                {
                    yield return this.textureElement1;
                }

                if (this.textureElement2 != null)
                {
                    yield return this.textureElement2;
                }

                if (this.textureElement3 != null)
                {
                    yield return this.textureElement3;
                }

                if (this.textureElement4 != null)
                {
                    yield return this.textureElement4;
                }

                if (this.textureElement5 != null)
                {
                    yield return this.textureElement5;
                }

                if (this.textureElement6 != null)
                {
                    yield return this.textureElement6;
                }

                if (this.textureElement7 != null)
                {
                    yield return this.textureElement7;
                }

                if (this.textureElement8 != null)
                {
                    yield return this.textureElement8;
                }

                if (this.textureElement9 != null)
                {
                    yield return this.textureElement9;
                }

                if (this.innerTextureElement != null
                    && IsBitmapTooltip)
                {
                    yield return this.innerTextureElement;
                }
            }
        }

        /// <summary>
        /// Gets a collection of subelement.
        /// </summary>
        public override IEnumerable<IElement> SubElements
        {
            get
            {
                if (this.textElement != null)
                {
                    yield return this.textElement;
                }

                if (this.textureElement1 != null)
                {
                    yield return this.textureElement1;
                }

                if (this.textureElement2 != null)
                {
                    yield return this.textureElement2;
                }

                if (this.textureElement3 != null)
                {
                    yield return this.textureElement3;
                }

                if (this.textureElement4 != null)
                {
                    yield return this.textureElement4;
                }

                if (this.textureElement5 != null)
                {
                    yield return this.textureElement5;
                }

                if (this.textureElement6 != null)
                {
                    yield return this.textureElement6;
                }

                if (this.textureElement7 != null)
                {
                    yield return this.textureElement7;
                }

                if (this.textureElement8 != null)
                {
                    yield return this.textureElement8;
                }

                if (this.textureElement9 != null)
                {
                    yield return this.textureElement9;
                }

                if (this.innerTextureElement != null)
                {
                    yield return this.innerTextureElement;
                }

            }
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text
        {
            get
            {
                return this.Owner.ToolTipText;
            }
        }

        private bool IsBitmapTooltip
        {
            get 
            {
                if (Style == null)
                    return false;
                else return Style.IsBitmapTooltip;
            }
        }

        /// <summary>
        /// Gets the text rectangle.
        /// </summary>
        public Rectangle TextBounds
        {
            get
            {
                if (this.IsBitmapTooltip)
                {
                    return new Rectangle(
                        this.InnerTextureElement.Offset + new Point(this.Style.Padding.Left, this.Style.Padding.Top),
                        this.size);
                }
                else return new Rectangle(
                        this.TextElement.Style.Offset + new Point(this.Style.Padding.Left, this.Style.Padding.Top),
                        this.size);
            }
        }

        /// <summary>
        /// Gets the TextElement of the Label.
        /// </summary>
        /// <value>A TextElement representing the text of the Label.</value>
        public TextElement TextElement
        {
            get
            {
                return this.textElement ?? (this.textElement = new TextElement(this, () => this.GetZOrder() + 1)
                                                               {
                                                                   CroppingDisabled = true
                                                               });
            }
        }
        
        /// <summary>
        /// Gets the InnerTextureElement.
        /// </summary>
        public TextureElement InnerTextureElement
        {
            get
            {
                return this.innerTextureElement ?? (this.innerTextureElement = new TextureElement(this, () => this.GetZOrder() + 2)
                                                                     {
                                                                         CroppingDisabled = true,
                                                                         Offset = new Point(6, 4),
                                                                         Size = new Size(135, 25)
                                                                     });
            }
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement1
        {
            get
            {
                return this.textureElement1 ?? (this.textureElement1 = new TextureElement(this, this.GetZOrder)
                                                                     {
                                                                         CroppingDisabled = true,
                                                                         Offset = new Point(0,0),
                                                                         Size = new Size(6,4)
                                                                     });
            }
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement2
        {
            get
            {
                return this.textureElement2 ?? (this.textureElement2 = new TextureElement(this, this.GetZOrder)
                {
                    CroppingDisabled = true,
                    Offset = new Point(6, 0),
                    Size = new Size(135, 4)
                });
            }
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement3
        {
            get
            {
                return this.textureElement3 ?? (this.textureElement3 = new TextureElement(this, this.GetZOrder)
                {
                    CroppingDisabled = true,
                    Offset = new Point(141, 0),
                    Size = new Size(6, 4)
                });
            }
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement4
        {
            get
            {
                return this.textureElement4 ?? (this.textureElement4 = new TextureElement(this, this.GetZOrder)
                {
                    CroppingDisabled = true,
                    Offset = new Point(0, 4),
                    Size = new Size(6, 25)
                });
            }
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement5
        {
            get
            {
                return this.textureElement5 ?? (this.textureElement5 = new TextureElement(this, this.GetZOrder)
                {
                    CroppingDisabled = true,
                    Offset = new Point(6, 4),
                    Size = new Size(135, 25)
                });
            }
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement6
        {
            get
            {
                return this.textureElement6 ?? (this.textureElement6 = new TextureElement(this, this.GetZOrder)
                {
                    CroppingDisabled = true,
                    Offset = new Point(141, 4),
                    Size = new Size(6, 25)
                });
            }
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement7
        {
            get
            {
                return this.textureElement7 ?? (this.textureElement7 = new TextureElement(this, this.GetZOrder)
                {
                    CroppingDisabled = true,
                    Offset = new Point(0, 29),
                    Size = new Size(6, 4)
                });
            }
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement8
        {
            get
            {
                return this.textureElement8 ?? (this.textureElement8 = new TextureElement(this, this.GetZOrder)
                {
                    CroppingDisabled = true,
                    Offset = new Point(6, 29),
                    Size = new Size(135, 4)
                });
            }
        }

        /// <summary>
        /// Gets the TextureElement.
        /// </summary>
        public TextureElement TextureElement9
        {
            get
            {
                return this.textureElement9 ?? (this.textureElement9 = new TextureElement(this, this.GetZOrder)
                {
                    CroppingDisabled = true,
                    Offset = new Point(141, 29),
                    Size = new Size(6, 4)
                });
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tooltip is visible.
        /// </summary>
        public override bool Visible
        {
            get
            {
                return this.visible;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns whether the Sprites property is null.
        /// </summary>
        /// <returns><c>true</c> if the Sprites property is null; otherwise, <c>false</c>.</returns>
        public override bool AreAllSpritesNull()
        {
            return base.AreAllSpritesNull() && this.TextElement.AreAllSpritesNull() 
                && this.TextureElement1.AreAllSpritesNull()
                && this.TextureElement2.AreAllSpritesNull()
                && this.TextureElement3.AreAllSpritesNull()
                && this.TextureElement4.AreAllSpritesNull()
                && this.TextureElement5.AreAllSpritesNull()
                && this.TextureElement6.AreAllSpritesNull()
                && this.TextureElement7.AreAllSpritesNull()
                && this.TextureElement8.AreAllSpritesNull()
                && this.TextureElement9.AreAllSpritesNull()
                && this.InnerTextureElement.AreAllSpritesNull();
        }

        /// <summary>
        /// Gets the derived location.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        public Point GetLocationInViewport()
        {
            return this.Location;
        }

        public void UpdateText()
        {
            if (IsBitmapTooltip)
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }
                this.NeedsUpdate = true;
            }
            else
            {
                TextElement.UpdateType |= UpdateTypes.Text;
            }           
        }

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        public void Hide()
        {
            if (this.Owner.MiyagiSystem == null)
            {
                return;
            }

            if (this.Visible)
            {
                if (this.fadeController != null)
                {
                    this.fadeController.Stop();
                    this.fadeController = null;
                }

                this.SetVisible(false);

                if (this.Owner.MiyagiSystem.GUIManager.CurrentToolTip == this)
                {
                    this.Owner.MiyagiSystem.GUIManager.HideToolTip();
                }
                else if (!AreAllSpritesNull())
                {
                    this.RemoveSprite();
                }


                this.mouseEnterTime = DateTime.MinValue;
                this.showStartTime = DateTime.MinValue;
                if (this.bitmap != null)
                {
                    this.bitmap.Dispose();
                    this.bitmap = null;
                }

                RemoveTexture();
            }
        }

        /// <summary>
        /// Shows the tooltip.
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public void Show()
        {
            if (this.Owner.MiyagiSystem == null)
            {
                return;
            }

            if (!this.Visible)
            {
                if (IsBitmapTooltip
                    && Bitmap == null)
                {
                    Bitmap = Style.BitmapCreatorFunction(this.Owner.ToolTipText, Style.MaximumWidth);
                }
                
                MiyagiSystem system = this.Owner.MiyagiSystem;

                system.GUIManager.SetToolTip(this);
                this.SetVisible(true);

                // set the new size
                //var newSize = this.size = this.Style.TextStyle.Font.MeasureString(this.Owner.ToolTipText, Size.Empty, this.Owner.ToolTipText.Contains(Environment.NewLine));
                var newSize = Size.Empty;
                if (IsBitmapTooltip)
                {
                    newSize = new Common.Data.Size(Bitmap.Size.Width, Bitmap.Size.Height);
                }
                else
                {
                    newSize = this.Style.TextStyle.Font.MeasureString(this.Owner.ToolTipText, Size.Empty, true) + new Size(0, 2);
                }

                this.size = newSize;

                SetLocation();
                SetElementsSizes();

                // start the fadecontroller
                this.opacity = 1;

                if (this.fadeController != null)
                {
                    this.fadeController.Stop();
                }

                if (this.Style.FadeInDuration > TimeSpan.Zero)
                {
                    this.fadeController = new LinearFunctionValueController<float>(0, 1, this.Style.FadeInDuration);
                    this.fadeController.Start(system, true, this.SetOpacity);
                }

                this.showStartTime = this.MiyagiSystem.LastUpdate;
            }
        }

        private void SetElementsSizes()
        {
            MiyagiSystem system = this.Owner.MiyagiSystem;

            var newSize = Size;

            this.TextureElement2.Size = new Common.Data.Size(newSize.Width, this.TextureElement2.Size.Height);
            this.TextureElement4.Size = new Common.Data.Size(this.TextureElement4.Size.Width, newSize.Height);
            this.TextureElement5.Size = newSize;
            if (IsBitmapTooltip)
                InnerTextureElement.Size = this.TextureElement5.Size;

            this.TextureElement6.Size = new Common.Data.Size(this.TextureElement6.Size.Width, newSize.Height);
            this.TextureElement8.Size = new Common.Data.Size(newSize.Width, this.TextureElement8.Size.Height);

            this.TextureElement3.Offset = new Point(this.TextureElement1.Size.Width + this.TextureElement2.Size.Width, 0);
            this.TextureElement6.Offset = new Point(this.TextureElement4.Size.Width + this.TextureElement5.Size.Width, this.TextureElement3.Size.Height);
            this.TextureElement7.Offset = new Point(0, this.TextureElement1.Size.Height + this.TextureElement4.Size.Height);
            this.TextureElement8.Offset = new Point(this.TextureElement7.Size.Width, this.TextureElement2.Size.Height + this.TextureElement5.Size.Height);
            this.TextureElement9.Offset = new Point(this.TextureElement7.Size.Width + this.TextureElement8.Size.Width, this.TextureElement3.Size.Height + this.TextureElement6.Size.Height);

            // set the skin
            this.TextureElement1.Texture = DefaultTexture1 ?? (DefaultTexture1 = Skin.CreateForToolTip(system));
            this.TextureElement2.Texture = DefaultTexture2 ?? (DefaultTexture2 = Skin.CreateForToolTip(system));
            this.TextureElement3.Texture = DefaultTexture3 ?? (DefaultTexture3 = Skin.CreateForToolTip(system));
            this.TextureElement4.Texture = DefaultTexture4 ?? (DefaultTexture4 = Skin.CreateForToolTip(system));
            this.TextureElement5.Texture = DefaultTexture5 ?? (DefaultTexture5 = Skin.CreateForToolTip(system));
            this.TextureElement6.Texture = DefaultTexture6 ?? (DefaultTexture6 = Skin.CreateForToolTip(system));
            this.TextureElement7.Texture = DefaultTexture7 ?? (DefaultTexture7 = Skin.CreateForToolTip(system));
            this.TextureElement8.Texture = DefaultTexture8 ?? (DefaultTexture8 = Skin.CreateForToolTip(system));
            this.TextureElement9.Texture = DefaultTexture9 ?? (DefaultTexture9 = Skin.CreateForToolTip(system));
        }

        public void SetLocation()
        {
            MiyagiSystem system = this.Owner.MiyagiSystem;

            var newSize = Size;
            if (this.Style.Padding != Thickness.Empty)
            {
                newSize += new Size(this.Style.Padding.Horizontal, this.Style.Padding.Vertical);
            }

            // set the new location
            var newLocation = system.InputManager.MouseLocation;

            newLocation += Style.MouseOffset + new Point(10, 0);

            if (newLocation.X + newSize.Width + 20 > this.ViewportSize.Width)
            {
                newLocation = new Point(this.ViewportSize.Width - newSize.Width - 20, newLocation.Y);
            }            

            if (newLocation.Y + newSize.Height + system.GUIManager.Cursor.Size.Height > this.ViewportSize.Height - newSize.Height)
            {
                newLocation = new Point(newLocation.X, this.ViewportSize.Height - newSize.Height - 20); //kubatp it was - 10 initially
            }
            else
            {
                newLocation = new Point(newLocation.X, newLocation.Y + system.GUIManager.Cursor.Size.Height + 0);
            }

            this.Location = newLocation;

            int x = newLocation.X - oldLocation.X;
            int y = newLocation.Y - oldLocation.Y;

            oldLocation = newLocation;
            
            ForEachActiveElement(ele => ele.UpdateType |= UpdateTypes.OwnerLocation);
            ForEachActiveElement(ele => ele.Update(new Point(x, y), Point.Empty));
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Releases the unmanaged resources used by the element.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Hide();

                if (this.fadeController != null)
                {
                    this.fadeController.Stop();
                    this.fadeController = null;
                }

                RemoveTexture();

                if (this.bitmap != null)
                {
                    this.bitmap.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Inject an entering mouse.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> to inject.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            this.mouseEnterTime = this.MiyagiSystem.LastUpdate;
        }

        /// <summary>
        /// Inject a leaving mouse.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> to inject.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.Hide();
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
                case "TextStyle":
                    this.SetSubElementStyles();
                    break;
            }
        }

        /// <summary>
        /// Sets the style of subelements.
        /// </summary>
        protected override void SetSubElementStyles()
        {
            if (this.TextElement.Style != null)
            {
                this.TextElement.Style.Font = null;
                this.TextElement.Style = null;
            }

            if (this.Style != null)
            {
                this.TextElement.Style = this.Style.TextStyle;
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand)]
        private void ConvertBitmap()
        {
            if (this.Bitmap != null)
            {
                Backend backend = this.MiyagiSystem.Backend;
                backend.CreateTexture(this.TextureName, new Size(this.Bitmap.Size.Width, this.Bitmap.Size.Height));
                backend.WriteToTexture(this.Bitmap.ToByteArray(), this.TextureName);
            }
        }


        /// <summary>
        /// Updates the element.
        /// </summary>
        protected override void UpdateCore()
        {
            DateTime currentTime = this.MiyagiSystem.LastUpdate;

            if (this.Visible
                && this.showStartTime.Ticks > 0
                && this.Style.DisplayDuration > TimeSpan.Zero
                && currentTime - this.showStartTime > this.Style.DisplayDuration)
            {
                this.Hide();
                return;
            }

            if (!this.Visible)
            {
                if (!string.IsNullOrEmpty(this.Owner.ToolTipText)
                    && this.mouseEnterTime.Ticks > 0
                    && currentTime - this.mouseEnterTime > this.Style.HoverDuration)
                {
                    this.Show();
                }
            }
            else
            {
                if (this.NeedsUpdate
                    && IsBitmapTooltip)
                {
                    Size oldSize = Size;
                    if (bitmap == null)
                    {
                        Bitmap = Style.BitmapCreatorFunction(this.Owner.ToolTipText, Style.MaximumWidth);
                    }

                    this.NeedsUpdate = false;
                    this.ConvertBitmap();                    

                    this.InnerTextureElement.Size = new Size(this.Bitmap.Width, this.Bitmap.Height);

                    var newSize = Size.Empty;
                    if (IsBitmapTooltip)
                    {
                        newSize = new Common.Data.Size(Bitmap.Size.Width, Bitmap.Size.Height);
                    }
                    else
                    {
                        newSize = this.Style.TextStyle.Font.MeasureString(this.Owner.ToolTipText, Size.Empty, true) + new Size(0,2);
                    }

                    this.size = newSize;

                    this.InnerTextureElement.Texture = this.Bitmap != null
                                                      ? new Texture(this.TextureName)
                                                      : null;

                    this.bitmap.Dispose();
                    this.bitmap = null;

                    SetElementsSizes();
                }

                if (this.UpdateType.IsFlagSet(UpdateTypes.Opacity))
                {
                    this.TextureElement1.ApplyOpacity();
                    this.TextureElement2.ApplyOpacity();
                    this.TextureElement3.ApplyOpacity();
                    this.TextureElement4.ApplyOpacity();
                    this.TextureElement5.ApplyOpacity();
                    this.TextureElement6.ApplyOpacity();
                    this.TextureElement7.ApplyOpacity();
                    this.TextureElement8.ApplyOpacity();
                    this.TextureElement9.ApplyOpacity();
                    if (this.InnerTextureElement != null)
                        this.InnerTextureElement.ApplyOpacity();
                }


                ForEachActiveElement(ele => ele.Update());
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void SetOpacity(float value)
        {
            this.opacity = value;
            this.UpdateType |= UpdateTypes.Opacity;
        }

        /// <summary>
        /// Sets the bitmap.
        /// </summary>
        protected void SetBitmap(System.Drawing.Bitmap bmp)
        {
            if (this.bitmap != null)
            {
                this.bitmap.Dispose();
            }

            this.bitmap = bmp;
            RemoveTexture();
            this.NeedsUpdate = true;            
        }


        private void SetVisible(bool value)
        {
            this.visible = value;
        }

        private bool NeedsUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the texture.
        /// </summary>
        protected string TextureName
        {
            get;
            private set;
        }

        /// <summary>
        /// Removes the texture.
        /// </summary>
        protected void RemoveTexture()
        {
            if (this.innerTextureElement != null && this.innerTextureElement.Style != null)
            {
                this.InnerTextureElement.Texture = null;
            }

            if (this.MiyagiSystem != null)
            {
                this.MiyagiSystem.Backend.RemoveTexture(this.TextureName);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}