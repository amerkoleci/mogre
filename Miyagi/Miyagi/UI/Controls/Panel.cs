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

    using Miyagi.Common;
    using Miyagi.Common.Animation;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Internals;

    /// <summary>
    /// A Panel control.
    /// </summary>
    public class Panel : ScrollableControl, IMagneticDockable
    {
        #region Fields

        private readonly MagneticDockingHelper<Panel> magneticDockingHelper;

        private CursorMode cursorMode;
        private bool hitBottomBorder;
        private bool hitLeftBorder;
        private bool hitRightBorder;
        private bool hitTopBorder;
        private bool hoverBottomBorder;
        private bool hoverLeftBorder;
        private bool hoverRightBorder;
        private bool hoverTopBorder;
        private Thickness magneticDockThreshold;
        private bool magneticDockToScreenEdges;
        private Thickness resizeThreshold;
        private bool throwable;
        private Point throwVector;
        private WaypointController throwWayPoint;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Panel class.
        /// </summary>
        public Panel()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Panel class.
        /// </summary>
        /// <param name="name">The name of the Panel.</param>
        public Panel(string name)
            : base(name)
        {
            this.ResizeMode = ResizeModes.None;
            this.MaxSize = new Size(2048, 2048);
            this.ResizeThreshold = new Thickness(5);
            this.cursorMode = CursorMode.Main;
            this.magneticDockingHelper = new MagneticDockingHelper<Panel>(this);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the distance in which this Panel docks magnetically on other Panels or the screen edges.
        /// </summary>
        /// <remarks>If <see cref="Control.CenterOnGrab"/> is true, MagneticDockThreshold is ignored.</remarks>
        public Thickness MagneticDockThreshold
        {
            get
            {
                return this.magneticDockThreshold;
            }

            set
            {
                this.ThrowIfDisposed();
                this.magneticDockThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Panel should dock to the screen edges.
        /// </summary>
        public bool MagneticDockToScreenEdges
        {
            get
            {
                return this.magneticDockToScreenEdges;
            }

            set
            {
                this.ThrowIfDisposed();
                this.magneticDockToScreenEdges = value;
            }
        }

        /// <summary>
        /// Gets or sets the ResizeMode of the Panel.
        /// </summary>
        /// <value>A bitwise combination of <see cref="ResizeModes"/> representing how the panel can be resized.</value>
        public ResizeModes ResizeMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the area where resizing is triggered.
        /// </summary>
        public Thickness ResizeThreshold
        {
            get
            {
                return this.resizeThreshold;
            }

            set
            {
                this.ThrowIfDisposed();
                this.resizeThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Panel is throwable.
        /// </summary>
        public bool Throwable
        {
            get
            {
                return this.Movable && this.throwable;
            }

            set
            {
                this.throwable = value;
            }
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets a value indicating whether magnetically docking is enabled.
        /// </summary>
        /// <value></value>
        protected internal override bool IsMagneticallyDockingEnabled
        {
            get
            {
                return true;
            }
        }

        #endregion Protected Internal Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Resizes the control.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            base.DoResize(widthFactor, heightFactor);
            var magDockSize = this.MagneticDockThreshold;
            this.ResizeHelper.Scale(ref magDockSize);
            this.MagneticDockThreshold = magDockSize;
            this.ResizeHelper.Scale(ref this.resizeThreshold);
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which the parent and its childs can be fitted.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>A Size representing the width and height of a rectangle.</returns>
        /// <remarks>As the DefaultLayout doesn't implement margins, there are ignored for this calculation too.</remarks>
        protected override Size GetPreferredSizeCore(Size proposedSize)
        {
            int width = 0;
            int height = 0;

            foreach (Control child in Controls)
            {
                Size childSize = child.Size;
                if (child.AutoSize)
                {
                    childSize = child.PreferredSize;
                }

                int right = child.Left + childSize.Width;
                int bottom = child.Top + childSize.Height;

                // Width
                if (right > width)
                {
                    width = right;
                }

                if (bottom > height)
                {
                    height = bottom;
                }
            }

            return this.SizeFromClientSize(new Size(width, height));
        }

        /// <summary>
        /// Handles losing the focus.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.hoverBottomBorder = this.hoverLeftBorder = this.hoverRightBorder = this.hoverTopBorder = false;
            this.hitBottomBorder = this.hitLeftBorder = this.hitRightBorder = this.hitTopBorder = false;
            this.cursorMode = CursorMode.Main;
        }

        /// <summary>
        /// Handles mouse press events.
        /// </summary>
        /// <param name="e">A MouseButtonEventArgs that contains the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.throwWayPoint != null)
            {
                this.throwWayPoint.Stop();
                this.throwVector = Point.Empty;
            }

            this.hitTopBorder = this.hoverTopBorder;
            this.hitBottomBorder = this.hoverBottomBorder;
            this.hitLeftBorder = this.hoverLeftBorder;
            this.hitRightBorder = this.hoverRightBorder;

            if (this.hitLeftBorder || this.hitRightBorder || this.hitTopBorder || this.hitBottomBorder)
            {
                // ungrab if we hit the border
                this.GUI.GUIManager.GrabbedControl = null;
            }
        }

        /// <summary>
        /// Raises the MouseDrag event.
        /// </summary>
        /// <param name="e">A ChangedValueEventArgs that contains the event data.</param>
        protected override void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            base.OnMouseDrag(e);

            Point newLocation = e.NewValue;
            Point oldLocation = e.OldValue;

            this.throwVector = new Point(newLocation.X - oldLocation.X, newLocation.Y - oldLocation.Y);

            // resize panel on drag
            this.Resize(newLocation);
        }

        /// <summary>
        /// Handles mouse hover events.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            base.OnMouseHover(e);

            GUIManager guiMgr = this.GUI.GUIManager;
            InputManager iptMgr = guiMgr.MiyagiSystem.InputManager;
            this.hoverBottomBorder = this.hoverLeftBorder = this.hoverRightBorder = this.hoverTopBorder = false;

            if (this.ResizeMode == ResizeModes.None
                || iptMgr.IsMouseButtonDown(iptMgr.MouseSelectButton)
                || this != guiMgr.GetTopControl())
            {
                return;
            }

            // check if we are over a border
            Point pos = this.GetLocationInViewport();
            Point mouseLoc = e.MouseLocation;

            if (this.ResizeMode.IsFlagSet(ResizeModes.Horizontal))
            {
                if (mouseLoc.X >= pos.X && mouseLoc.X <= pos.X + this.ResizeThreshold.Left)
                {
                    this.hoverLeftBorder = true;
                }
                else if (mouseLoc.X >= pos.X + this.Width - this.ResizeThreshold.Right && mouseLoc.X <= pos.X + this.Width)
                {
                    this.hoverRightBorder = true;
                }
            }

            if (this.ResizeMode.IsFlagSet(ResizeModes.Vertical))
            {
                if (mouseLoc.Y >= pos.Y && mouseLoc.Y <= pos.Y + this.ResizeThreshold.Top)
                {
                    this.hoverTopBorder = true;
                }
                else if (mouseLoc.Y >= pos.Y + this.Height - this.ResizeThreshold.Bottom && mouseLoc.Y <= pos.Y + this.Height)
                {
                    this.hoverBottomBorder = true;
                }
            }

            this.SetCursorMode();
        }

        /// <summary>
        /// Handles mouse enter events.
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            InputManager iptMgr = this.MiyagiSystem.InputManager;
            if (!(this.Focused && iptMgr.IsMouseButtonDown(iptMgr.MouseSelectButton)))
            {
                base.OnMouseLeave(e);
                this.cursorMode = CursorMode.Main;
                this.hitBottomBorder = this.hitLeftBorder = this.hitRightBorder = this.hitTopBorder = false;
            }
        }

        /// <summary>
        /// Handles mouse release events.
        /// </summary>
        /// <param name="e">A MouseButtonEventArgs that contains the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            // throw the panel
            if (Math.Abs(this.throwVector.X) > 5 || Math.Abs(this.throwVector.Y) > 5)
            {
                this.Throw(this.Location + (this.throwVector * 25));
            }

            // set mouse cursor back to main
            this.cursorMode = CursorMode.Main;

            // reset magnetic dock
            this.magneticDockingHelper.Reset();
        }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        /// <param name="x">The new <see cref="Control.Left"/> property value of the control.</param>
        /// <param name="y">The new <see cref="Control.Top"/> property value of the control.</param>
        /// <param name="newWidth">The new <see cref="Control.Width"/> property value of the control.</param>
        /// <param name="newHeight">The new <see cref="Control.Height"/> property value of the control.</param>
        /// <param name="specified">A bitwise combination of <see cref="BoundsSpecified"/> values.</param>
        protected override void SetBoundsCore(int x, int y, int newWidth, int newHeight, BoundsSpecified specified)
        {
            if (this.MiyagiSystem != null && this.MagneticDockThreshold != Thickness.Empty && !this.CenterOnGrab)
            {
                switch (specified)
                {
                    case BoundsSpecified.X:
                    case BoundsSpecified.Y:
                    case BoundsSpecified.Location:
                        var pos = this.GetLocationInViewport();
                        if (x != pos.X || y != pos.Y)
                        {
                            this.magneticDockingHelper.Do(pos.X, pos.Y, ref x, ref y);
                        }

                        break;
                }
            }

            base.SetBoundsCore(x, y, newWidth, newHeight, specified);
        }

        /// <summary>
        /// Throws the panel to the specified destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        protected void Throw(Point destination)
        {
            if (this.Throwable)
            {
                Size screenSize = this.GUI.SpriteRenderer.Viewport.Size;
                if (this.Left > 0 && this.Right < screenSize.Width && this.Top > 0 && this.Bottom < screenSize.Height)
                {
                    this.throwWayPoint = new WaypointController(this.Location)
                    {
                        Progression = Progression.Decreasing
                    };

                    // prevent throwing out of screen
                    if (destination.X < 0
                        || destination.X + this.Width > screenSize.Width
                        || destination.Y < 0
                        || destination.Y + this.Height > screenSize.Height)
                    {
                        var lbc = new LiangBarskyClipping(new Rectangle(0, 0, screenSize.Width - this.Width, screenSize.Height - this.Height));
                        destination = lbc.ClipLineEndPoint(this.Location, destination);
                    }

                    this.throwWayPoint.AddWaypoint(destination, null);
                    this.throwWayPoint.Start(this.MiyagiSystem, true, c => this.Location = c);
                }
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();

            if (this.cursorMode != CursorMode.Main)
            {
                this.GUI.GUIManager.Cursor.SetActiveMode(this.cursorMode);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void Resize(Point mouseLocation)
        {
            Point pos = this.GetLocationInViewport();
            if (this.hitLeftBorder)
            {
                int xdiff = pos.X - mouseLocation.X;
                int newWidth = this.Width + xdiff;
                if (newWidth >= this.MinSize.Width && newWidth <= this.MaxSize.Width)
                {
                    this.SetBounds(this.Left - xdiff, this.Top, newWidth, this.Height);
                }
            }
            else if (this.hitRightBorder)
            {
                int newWidth = this.Width + (mouseLocation.X - (pos.X + this.Width));
                if (newWidth >= this.MinSize.Width && newWidth <= this.MaxSize.Width)
                {
                    this.Width = newWidth;
                }
            }

            if (this.hitTopBorder)
            {
                int ydiff = pos.Y - mouseLocation.Y;
                int newHeight = this.Height + ydiff;
                if (newHeight >= this.MinSize.Height && newHeight <= this.MaxSize.Height)
                {
                    this.SetBounds(this.Left, this.Top - ydiff, this.Width, newHeight);
                }
            }
            else if (this.hitBottomBorder)
            {
                int newHeight = this.Height + (mouseLocation.Y - (pos.Y + this.Height));
                if (newHeight >= this.MinSize.Height && newHeight <= this.MaxSize.Height)
                {
                    this.Height = newHeight;
                }
            }
        }

        private void SetCursorMode()
        {
            this.cursorMode = CursorMode.Main;

            if (this.ResizeMode.IsFlagSet(ResizeModes.Horizontal))
            {
                if (this.hoverLeftBorder)
                {
                    this.cursorMode = CursorMode.ResizeLeft;
                }
                else if (this.hoverRightBorder)
                {
                    this.cursorMode = CursorMode.ResizeRight;
                }
            }

            if (this.ResizeMode.IsFlagSet(ResizeModes.Vertical))
            {
                if (this.hoverTopBorder)
                {
                    this.cursorMode = CursorMode.ResizeTop;
                }
                else if (this.hoverBottomBorder)
                {
                    this.cursorMode = CursorMode.ResizeBottom;
                }
            }

            if (this.ResizeMode.IsFlagSet(ResizeModes.Diagonal))
            {
                if (this.hoverLeftBorder)
                {
                    if (this.hoverTopBorder)
                    {
                        this.cursorMode = CursorMode.ResizeTopLeft;
                    }
                    else if (this.hoverBottomBorder)
                    {
                        this.cursorMode = CursorMode.ResizeBottomLeft;
                    }
                }
                else if (this.hoverRightBorder)
                {
                    if (this.hoverTopBorder)
                    {
                        this.cursorMode = CursorMode.ResizeTopRight;
                    }
                    else if (this.hoverBottomBorder)
                    {
                        this.cursorMode = CursorMode.ResizeBottomRight;
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}