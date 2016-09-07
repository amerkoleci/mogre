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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Common.Serialization;
    using Miyagi.Internals;

    /// <summary>
    /// Represents a mouse cursor.
    /// </summary>
    [SerializableType]
    public class Cursor : IDisposable
    {
        #region Fields

        private Point activeHotspot;
        private string activeMode;
        private bool activeModeChanged;
        private TextureFrame currentFrame;
        private GUIManager guiManager;
        private Point location;
        private ResizeHelper resizeHelper;
        private Size size;
        private Skin skin;
        private Sprite sprite;
        private Point spritesDeltaLocation;
        private Point spritesDeltaSize;
        private Texture texture;
        private TimeSpan textureAnimationTime;
        private UpdateTypes updateType;
        private bool visible;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Cursor class.
        /// </summary>
        public Cursor()
        {
            this.Hotspots = new Dictionary<string, Point>();
        }

        /// <summary>
        /// Initializes a new instance of the Cursor class.
        /// </summary>
        /// <param name="skin">The name of the cursor's Skin.</param>
        /// <param name="size">The size of the cursor.</param>
        /// <param name="hotspotLocation">The position of the cursor's main hotspot.</param>
        /// <param name="visibility">Whether the cursor is initially visible.</param>
        public Cursor(Skin skin, Size size, Point hotspotLocation, bool visibility = true)
            : this()
        {
            this.Skin = skin;
            this.Size = size;
            this.Visible = visibility;

            if (!this.Hotspots.ContainsKey(CursorMode.Main.ToString()))
            {
                this.Hotspots.Add(CursorMode.Main.ToString(), hotspotLocation);
            }
        }

        /// <summary>
        /// Finalizes an instance of the Cursor class.
        /// </summary>
        ~Cursor()
        {
            this.Dispose(false);
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the <see cref="ActiveHotspot"/> property changes.
        /// </summary>
        public event EventHandler ActiveHotspotChanged;

        /// <summary>
        /// Occurs when the <see cref="ActiveMode"/> property changes.
        /// </summary>
        public event EventHandler ActiveModeChanged;

        /// <summary>
        /// Occurs when the <see cref="Location"/> property changes.
        /// </summary> 
        public event EventHandler LocationChanged;

        /// <summary>
        /// Occurs when the <see cref="Size"/> property changes.
        /// </summary>
        public event EventHandler SizeChanged;

        /// <summary>
        /// Occurs when the <see cref="Skin"/> property changes.
        /// </summary>
        public event EventHandler SkinChanged;

        /// <summary>
        /// Occurs when the <see cref="Visible"/> property changes.
        /// </summary>
        public event EventHandler VisibleChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the active hotspot.
        /// </summary>
        public Point ActiveHotspot
        {
            get
            {
                return this.activeHotspot;
            }

            private set
            {
                if (this.activeHotspot != value)
                {
                    this.activeHotspot = value;
                    this.OnActiveHotspotChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the active cursor.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public string ActiveMode
        {
            get
            {
                return this.activeMode;
            }

            set
            {
                if (this.activeMode != value)
                {
                    this.activeMode = value;
                    this.OnActiveModeChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a dictionary of hotspot coordinates.
        /// </summary>
        public IDictionary<string, Point> Hotspots
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the location of the cursor.
        /// </summary>
        public Point Location
        {
            get
            {
                return this.location;
            }

            internal set
            {
                if (this.location != value)
                {
                    if (this.sprite != null)
                    {
                        this.spritesDeltaLocation = Point.Add(
                            this.spritesDeltaLocation,
                            value.X - this.location.X,
                            value.Y - this.location.Y);
                        this.updateType |= UpdateTypes.Location;
                    }

                    this.location = value;
                    this.OnLocationChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the cursor.
        /// </summary>
        public Size Size
        {
            get
            {
                return this.size;
            }

            set
            {
                if (this.size != value)
                {
                    if (this.sprite != null)
                    {
                        var oldPos = new Point(this.size.Width, this.size.Height);
                        var newPos = new Point(value.Width, value.Height);

                        this.spritesDeltaSize = this.spritesDeltaSize + (newPos - oldPos);

                        this.updateType |= UpdateTypes.Size;
                    }

                    this.size = value;
                    this.OnSizeChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Skin.
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
                if (this.skin != value)
                {
                    if (this.skin != null)
                    {
                        this.skin.SubSkinChanged -= this.SkinTextureChanged;
                    }

                    this.skin = value;

                    if (value != null)
                    {
                        this.skin.SubSkinChanged += this.SkinTextureChanged;
                    }

                    this.OnSkinChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get
            {
                return this.visible && this.Skin != null;
            }

            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.OnVisibleChanged(EventArgs.Empty);
                }
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>The texture.</value>
        protected Texture Texture
        {
            get
            {
                return this.texture;
            }

            set
            {
                if (this.texture != value)
                {
                    this.textureAnimationTime = TimeSpan.Zero;
                    this.texture = value;
                    this.updateType |= UpdateTypes.Texture;
                }
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Releases the unmanaged resources used by the cursor.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Forces a redraw of the cursor.
        /// </summary>
        public void ForceRedraw()
        {
            this.RemoveSprite();
        }

        /// <summary>
        /// Resizes the cursor.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        public void Resize(double widthFactor, double heightFactor)
        {
            if (this.resizeHelper == null)
            {
                this.resizeHelper = new ResizeHelper(this.DoResize);
            }

            this.resizeHelper.Resize(widthFactor, heightFactor);
        }

        /// <summary>
        /// Sets the active mode.
        /// </summary>
        /// <param name="mode">A CursorMode representing the active mode.</param>
        public void SetActiveMode(CursorMode mode)
        {
            this.ActiveMode = mode.ToString();
        }

        /// <summary>
        /// Sets the hotspot of a cursor mode.
        /// </summary>
        /// <param name="mode">The cursor mode.</param>
        /// <param name="hotspot">The hotspot of the cursor mode.</param>
        public void SetHotspot(CursorMode mode, Point hotspot)
        {
            this.Hotspots[mode.ToString()] = hotspot;
        }

        /// <summary>
        /// Updates the Cursor.
        /// </summary>
        public virtual void Update()
        {
            var system = this.guiManager.MiyagiSystem;

            if (this.activeModeChanged)
            {
                this.ChangeActiveMode(this.activeMode);
                this.activeModeChanged = false;
            }

            if (this.Texture != null)
            {
                if (this.Texture.Frames.Count > 1)
                {
                    this.textureAnimationTime += system.TimeSinceLastUpdate;
                }

                this.currentFrame = this.Texture.GetFrameFromTime(this.textureAnimationTime);

                if (this.sprite == null)
                {
                    // create new sprites
                    var rec = new Rectangle(this.location, this.size).ToScreenCoordinates(system.RenderManager.MainViewport.Size);
                    this.sprite = new Sprite(system.RenderManager.MainRenderer, new Quad(rec, this.currentFrame.UV))
                                  {
                                      Visible = this.Visible,
                                      Opacity = this.Visible ? 1 : 0,
                                      ZOrder = int.MaxValue,
                                      TexFilter = TextureFiltering.Linear,
                                      GpuPrograms = this.Texture.GpuPrograms
                                  };

                    this.sprite.SetTexture(this.currentFrame.FileName);
                }
            }

            if (this.sprite != null)
            {
                if (this.Texture == null)
                {
                    this.sprite.RemoveFromRenderer();
                    this.sprite = null;
                }
                else if (this.updateType != UpdateTypes.None)
                {
                    if (this.updateType.IsFlagSet(UpdateTypes.Texture))
                    {
                        // change texture
                        this.sprite.SetTexture(this.currentFrame.FileName);
                        this.sprite.SetUV(this.currentFrame.UV.GetPoints());
                        this.sprite.GpuPrograms = this.Texture.GpuPrograms;
                    }

                    // move sprites to new position
                    if (this.updateType.IsFlagSet(UpdateTypes.Location))
                    {
                        this.sprite.Move(this.spritesDeltaLocation);
                        this.spritesDeltaLocation = Point.Empty;

                        //this section is a hack for problem with mouse - issue 118
                        PointF[] points = new Rectangle(this.location, this.size).ToScreenCoordinates(system.RenderManager.MainViewport.Size).GetPoints();
                        PointF[] currentPoints = sprite.GetPrimitive(0).VertexPositions;
                        const float minDistance = 0.0005f;

                        if (Math.Abs(points[0].X - currentPoints[0].X) > minDistance
                            || Math.Abs(points[0].Y - currentPoints[0].Y) > minDistance)
                        {
                            ForceRedraw();
                            //System.Diagnostics.Debug.WriteLine("CURSOR FORCED TO REDRAW");
                        }
                    }

                    // resize sprites
                    if (this.updateType.IsFlagSet(UpdateTypes.Size))
                    {
                        this.sprite.Resize(this.spritesDeltaSize);
                        this.spritesDeltaSize = Point.Empty;
                    }

                    // change visibility
                    if (this.updateType.IsFlagSet(UpdateTypes.Visibility))
                    {
                        this.sprite.Visible = this.visible;
                    }

                    this.updateType = UpdateTypes.None;
                }
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void SetGUIManager(GUIManager guiMgr)
        {
            this.guiManager = guiMgr;
            this.Location = guiMgr.MiyagiSystem.InputManager.MouseLocation - this.ActiveHotspot;
        }

        #endregion Internal Methods

        #region Protected Methods

        /// <summary>
        /// Changes the active mode.
        /// </summary>
        /// <param name="newMode">A string representing the new mode.</param>
        protected virtual void ChangeActiveMode(string newMode)
        {
            if (this.Skin == null)
            {
                return;
            }

            // hotspot is not set, try to fallback to alternatives
            if (!this.Hotspots.ContainsKey(newMode))
            {
                if (new List<string>(Enum.GetNames(typeof(CursorMode))).Contains(newMode))
                {
                    var cursorMode = newMode.ParseEnum<CursorMode>();
                    switch (cursorMode)
                    {
                        case CursorMode.ResizeTop:
                            newMode = CursorMode.ResizeBottom.ToString();
                            break;
                        case CursorMode.ResizeBottom:
                            newMode = CursorMode.ResizeTop.ToString();
                            break;
                        case CursorMode.ResizeLeft:
                            newMode = CursorMode.ResizeRight.ToString();
                            break;
                        case CursorMode.ResizeRight:
                            newMode = CursorMode.ResizeLeft.ToString();
                            break;
                        case CursorMode.ResizeTopRight:
                            newMode = CursorMode.ResizeBottomLeft.ToString();
                            break;
                        case CursorMode.ResizeBottomLeft:
                            newMode = CursorMode.ResizeTopRight.ToString();
                            break;
                        case CursorMode.ResizeTopLeft:
                            newMode = CursorMode.ResizeBottomRight.ToString();
                            break;
                        case CursorMode.ResizeBottomRight:
                            newMode = CursorMode.ResizeTopLeft.ToString();
                            break;
                    }
                }
            }

            string main = CursorMode.Main.ToString();
            if (!this.Hotspots.ContainsKey(newMode))
            {
                newMode = main;
            }

            // check if hotspot is set for cursor
            if (this.Hotspots.ContainsKey(newMode))
            {
                this.Texture = newMode == main ? this.Skin.SubSkins[this.Skin.Name] : this.GetCursorTexture(newMode);

                int offx = this.ActiveHotspot.X - this.Hotspots[newMode].X;
                int offy = this.ActiveHotspot.Y - this.Hotspots[newMode].Y;

                this.Location = new Point(this.Location.X + offx, this.Location.Y + offy);
                this.ActiveHotspot = this.Hotspots[newMode];
                this.activeMode = newMode;
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the cursor.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Skin != null)
                {
                    this.Skin.SubSkinChanged -= this.SkinTextureChanged;
                }
            }

            this.RemoveSprite();
        }

        /// <summary>
        /// Resizes the cursor.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected virtual void DoResize(double widthFactor, double heightFactor)
        {
            this.location = Point.Empty;
            this.guiManager.MiyagiSystem.InputManager.MouseLocation = Point.Empty;
            this.ActiveHotspot = Point.Empty;

            this.resizeHelper.Scale(ref this.size);

            this.Hotspots =
                this.Hotspots.Select(oldHotspot => new
                                                   {
                                                       oldHotspot,
                                                       newHotspot = new
                                                                    {
                                                                        oldHotspot.Key,
                                                                        Value = new Point((int)Math.Round(oldHotspot.Value.X * widthFactor), (int)Math.Round(oldHotspot.Value.Y * heightFactor))
                                                                    }
                                                   }).Select(t => t.newHotspot).ToDictionary(k => k.Key, k => k.Value);

            this.ActiveMode = CursorMode.Main.ToString();

            this.ForceRedraw();
        }

        /// <summary>
        /// Gets the cursor texture.
        /// </summary>
        /// <param name="skin">The name of the skin.</param>
        /// <returns>A <see cref="Texture"/> representing the cursor texture.</returns>
        protected virtual Texture GetCursorTexture(string skin)
        {
            TextureCollection subSkins = this.skin.SubSkins;
            string name = this.skin.Name + ".";
            var retValue = subSkins[name + skin];

            if (retValue != null)
            {
                return retValue;
            }

            switch (skin)
            {
                case "ResizeTop":
                    retValue = subSkins[name + "ResizeBottom"];
                    break;
                case "ResizeBottom":
                    retValue = subSkins[name + "ResizeTop"];
                    break;
                case "ResizeLeft":
                    retValue = subSkins[name + "ResizeRight"];
                    break;
                case "ResizeRight":
                    retValue = subSkins[name + "ResizeLeft"];
                    break;
                case "ResizeBottomLeft":
                    retValue = subSkins[name + "ResizeTopRight"];
                    break;
                case "ResizeTopRight":
                    retValue = subSkins[name + "ResizeBottomLeft"];
                    break;
                case "ResizeBottomRight":
                    retValue = subSkins[name + "ResizeTopLeft"];
                    break;
                case "ResizeTopLeft":
                    retValue = subSkins[name + "ResizeBottomRight"];
                    break;
            }

            return retValue ?? subSkins[this.Skin.Name];
        }

        /// <summary>
        /// Raises the <see cref="Cursor.ActiveHotspotChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnActiveHotspotChanged(EventArgs e)
        {
            if (this.ActiveHotspotChanged != null)
            {
                this.ActiveHotspotChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Cursor.ActiveModeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnActiveModeChanged(EventArgs e)
        {
            this.activeModeChanged = true;
            if (this.ActiveModeChanged != null)
            {
                this.ActiveModeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Cursor.LocationChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnLocationChanged(EventArgs e)
        {
            if (this.LocationChanged != null)
            {
                this.LocationChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Cursor.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSizeChanged(EventArgs e)
        {
            if (this.SizeChanged != null)
            {
                this.SizeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Cursor.SkinChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSkinChanged(EventArgs e)
        {
            this.Texture = this.skin.IsSubSkinDefined(this.skin.Name) ? this.skin.SubSkins[this.skin.Name] : null;

            if (this.SkinChanged != null)
            {
                this.SkinChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Cursor.VisibleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
            this.updateType |= UpdateTypes.Visibility;

            if (this.visible && this.guiManager != null)
            {
                this.Location = this.guiManager.MiyagiSystem.InputManager.MouseLocation - this.ActiveHotspot;//pkubat activehotspot was added
            }

            if (this.VisibleChanged != null)
            {
                this.VisibleChanged(this, e);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void RemoveSprite()
        {
            if (this.sprite != null)
            {
                this.sprite.RemoveFromRenderer();
                this.sprite = null;
            }
        }

        private void SkinTextureChanged(object sender, EventArgs e)
        {
            this.updateType |= UpdateTypes.Texture;
        }

        #endregion Private Methods

        #endregion Methods
    }
}