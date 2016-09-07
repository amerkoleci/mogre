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
    using System.Collections.Generic;
    using System.Linq;

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Serialization;
    using Miyagi.Internals;

    /// <summary>
    /// An InputManager.
    /// </summary>
    public class InputManager : IManager
    {
        #region Fields

        private readonly Dictionary<MouseButton, bool> pressedMouseButtons = new Dictionary<MouseButton, bool>();

        private Rectangle? cursorClipArea;
        private bool isKeyDown;
        private bool keyHeld;
        private KeyEvent lastKeyDownEvent;
        private DateTime lastKeyHeldTime;
        private MouseGestures lastMouseGesture;
        private DateTime lastMouseHeldTime;
        private MouseButton lastPressedMouseButton;
        private Point mouseGestureEndLocation;
        private List<MouseGestures> mouseGestures;
        private List<Point> mouseGesturesLocations;
        private bool mouseHeld;
        private Point mouseLocation;
        private Point oldMouseGesturePositon;
        private bool moveMouseWhenCursorIsHidden = true;//pkubat new moveMouseWhenCursorIsHidden
        private bool ignoreMouseMovemenent = false;//pkubat new ignoreMouseMovemenent

        #endregion Fields

        #region Constructors

        static InputManager()
        {
            FlowTime = TimeSpan.FromMilliseconds(50);
            HeldWaitTime = TimeSpan.FromMilliseconds(300);
        }

        /// <summary>
        /// Initializes a new instance of the InputManager class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        protected internal InputManager(MiyagiSystem system)
        {
            this.MiyagiSystem = system;
            this.MouseSelectButton = MouseButton.Left;
            this.MouseGesturesButton = MouseButton.Right;
            this.MouseDoubleClickSpeed = TimeSpan.FromMilliseconds(500);
            this.MouseGestureSegmentsAngle = 120;
            this.MouseGestureMinDistance = 8;
            this.CaptureOnUpdate = true;

            this.pressedMouseButtons = new Dictionary<MouseButton, bool>();
            foreach (MouseButton mb in Enum.GetValues(typeof(MouseButton)))
            {
                this.pressedMouseButtons[mb] = false;
            }
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the capture of input devices is requested.
        /// </summary>
        public event EventHandler CaptureRequested;

        /// <summary>
        /// Occurs when the InputManager is disposing.
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// Occurs when a key is released.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyDown;

        /// <summary>
        /// Occurs when a key is held.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyHeld;

        /// <summary>
        /// Occurs when a key is released.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyUp;

        /// <summary>
        /// Occurs when a mouse button is pressed.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDown;

        /// <summary>
        /// Occurs when a mouse gesture has been recognized.
        /// </summary>
        public event EventHandler<MouseGestureEventArgs> MouseGestureRecognized;

        /// <summary>
        /// Occurs when a mouse button is held.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseHeld;

        /// <summary>
        /// Occurs when the MouseLocation property changes.
        /// </summary>
        public event EventHandler<ChangedValueEventArgs<Point>> MouseLocationChanged;

        /// <summary>
        /// Occurs when a mouse button is released.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseUp;

        /// <summary>
        /// Occurs when the mouse wheel is moved.
        /// </summary>
        public event EventHandler<ValueEventArgs<int>> MouseWheelMoved;

        #endregion Events

        #region Properties

        #region Public Static Properties

        /// <summary>
        /// Gets or sets the waiting time for automatically repeating held keys or mouse buttons.
        /// </summary>
        public static TimeSpan FlowTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time to wait until a key or mouse button is considered hold.
        /// </summary>
        public static TimeSpan HeldWaitTime
        {
            get;
            set;
        }

        #endregion Public Static Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the input devices are captured on update.
        /// </summary>
        /// <remarks>Default is true.</remarks>
        public bool CaptureOnUpdate
        {
            get;
            set;
        }

        public bool MoveMouseWhenCursorIsHidden
        {
            get { return moveMouseWhenCursorIsHidden; }
            set { moveMouseWhenCursorIsHidden = value; }
        }

        public bool IgnoreMouseMovemenent
        {
            get { return ignoreMouseMovemenent;}
            set { ignoreMouseMovemenent = value; }
        }

        /// <summary>
        /// Gets or sets the area within which the mouse cursor will be confined.
        /// </summary>
        /// <remarks>If set to null, the mouse cursor does not get clipped.</remarks>
        public Rectangle? CursorClipArea
        {
            get
            {
                return this.cursorClipArea;
            }

            set
            {
                if (value != null)
                {
                    this.cursorClipArea = value.Value;
                    Rectangle rect = value.Value;
                    if (!rect.Contains(this.mouseLocation))
                    {
                        this.MouseLocation = rect.Location;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the manager has been disposed.
        /// </summary>
        /// <value></value>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the threshold for recognising mouse double-clicks.
        /// </summary>
        /// <remarks>Default is 500 ms.</remarks>
        public TimeSpan MouseDoubleClickSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum distance the mouse position has to change before a mouse gesture is recognized.
        /// </summary>
        public int MouseGestureMinDistance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default MouseButton that needs to be pressed for mouse gestures.
        /// </summary>
        /// <remarks>Default is <see cref="MouseButton.Right"/>.</remarks>
        public MouseButton MouseGesturesButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the mouse gesture segments in degrees.
        /// </summary>
        /// <remarks>Default is 120.</remarks>
        public int MouseGestureSegmentsAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location of the mouse.
        /// </summary>
        public Point MouseLocation
        {
            get
            {
                return this.mouseLocation;
            }

            set
            {
                if (value != this.mouseLocation
                    && !ignoreMouseMovemenent)
                {
                    if (moveMouseWhenCursorIsHidden
                        || (this.MiyagiSystem == null || this.MiyagiSystem.GUIManager == null || this.MiyagiSystem.GUIManager.Cursor == null || this.MiyagiSystem.GUIManager.Cursor.Visible)) //pkubat moveMouseWhenCursorIsHidden and whole if is new
                    {
                        Point old = this.mouseLocation;                        
                        this.mouseLocation = value;

                        this.OnMouseMoved(old, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the default MouseButton for selecting a control.
        /// </summary>
        /// <remarks>Default is <see cref="MouseButton.Left"/>.</remarks>
        public MouseButton MouseSelectButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the type of the manager.
        /// </summary>
        public string Type
        {
            get
            {
                return "Input";
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the MiyagiSystem.
        /// </summary>
        protected MiyagiSystem MiyagiSystem
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Explicit Interface Methods

        void IManager.LoadSerializationData(SerializationData data)
        {
            this.CaptureOnUpdate = (bool)data["CaptureOnUpdate"];

            if (data["CursorClipArea"] != null)
            {
                this.cursorClipArea = (Rectangle)data["CursorClipArea"];
            }

            this.MouseDoubleClickSpeed = (TimeSpan)data["MouseDoubleClickSpeed"];
            this.MouseGestureMinDistance = (int)data["MouseGestureMinDistance"];
            this.MouseGesturesButton = (MouseButton)data["MouseGesturesButton"];
            this.MouseGestureSegmentsAngle = (int)data["MouseGestureSegmentsAngle"];
            this.MouseSelectButton = (MouseButton)data["MouseSelectButton"];
        }

        void IManager.NotifyManagerRegistered(IManager manager)
        {
        }

        void IManager.SaveSerializationData(SerializationData data)
        {
            data.Add("CaptureOnUpdate", this.CaptureOnUpdate);

            if (this.cursorClipArea != null)
            {
                data.Add("CursorClipArea", this.cursorClipArea.Value);
            }

            data.Add("MouseDoubleClickSpeed", this.MouseDoubleClickSpeed);
            data.Add("MouseGestureMinDistance", this.MouseGestureMinDistance);
            data.Add("MouseGesturesButton", this.MouseGesturesButton);
            data.Add("MouseGestureSegmentsAngle", this.MouseGestureSegmentsAngle);
            data.Add("MouseSelectButton", this.MouseSelectButton);
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Releases the unmanaged resources used by the InputManager.
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.Disposing != null)
            {
                this.Disposing(this, EventArgs.Empty);
            }

            this.Dispose(true);
            GC.SuppressFinalize(this);
            this.IsDisposed = true;
            this.MiyagiSystem.UnregisterManager(this);
            this.MiyagiSystem = null;
        }

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Injects a pressed key.
        /// </summary>
        /// <param name="keyEvent">The KeyEvent.</param>
        public void InjectKeyDown(KeyEvent keyEvent)
        {
            this.isKeyDown = true;
            this.lastKeyDownEvent = keyEvent;
            this.lastKeyHeldTime = this.MiyagiSystem.LastUpdate;

            if (this.KeyDown != null)
            {
                this.KeyDown(this, new KeyboardEventArgs(keyEvent));
            }
        }

        /// <summary>
        /// Injects a held key.
        /// </summary>
        public void InjectKeyHeld()
        {
            if (this.KeyHeld != null)
            {
                this.KeyHeld(this, new KeyboardEventArgs(this.lastKeyDownEvent));
            }
        }

        /// <summary>
        /// Injects a released key.
        /// </summary>
        /// <param name="keyEvent">The KeyEvent.</param>
        public void InjectKeyUp(KeyEvent keyEvent)
        {
            this.isKeyDown = false;
            this.keyHeld = false;

            if (this.KeyUp != null)
            {
                this.KeyUp(this, new KeyboardEventArgs(keyEvent));
            }
        }

        /// <summary>
        /// Injects a pressed mouse button.
        /// </summary>
        /// <param name="mb">The mouse button.</param>
        public void InjectMouseDown(MouseButton mb)
        {
            this.pressedMouseButtons[mb] = true;
            this.lastPressedMouseButton = mb;
            this.lastMouseHeldTime = this.MiyagiSystem.LastUpdate;

            // MouseGestures
            if (mb == this.MouseGesturesButton)
            {
                this.oldMouseGesturePositon = this.mouseLocation;
                this.lastMouseGesture = MouseGestures.None;
                this.mouseGestures = new List<MouseGestures>();
                this.mouseGesturesLocations = new List<Point>();
            }

            if (this.MouseDown != null)
            {
                this.MouseDown(this, new MouseButtonEventArgs(mb, this.mouseLocation));
            }
        }

        /// <summary>
        /// Injects a held mouse button.
        /// </summary>
        public void InjectMouseHeld()
        {
            if (this.MouseHeld != null)
            {
                this.MouseHeld(this, new MouseButtonEventArgs(this.lastPressedMouseButton, this.MouseLocation));
            }
        }

        /// <summary>
        /// Injects a released mouse button.
        /// </summary>
        /// <param name="mb">The mouse button.</param>
        public void InjectMouseUp(MouseButton mb)
        {
            this.pressedMouseButtons[mb] = false;

            if (this.MouseUp != null)
            {
                this.MouseUp(this, new MouseButtonEventArgs(mb, this.mouseLocation));
            }

            if (mb == this.MouseGesturesButton)
            {
                this.mouseGestureEndLocation = this.mouseLocation;
                this.FireMouseGestureEvent();
            }
        }

        /// <summary>
        /// Injects mouse wheel movement.
        /// </summary>
        /// <param name="delta">The movement of the wheel.</param>
        public virtual void InjectMouseWheelMoved(int delta)
        {
            if (this.MouseWheelMoved != null)
            {
                this.MouseWheelMoved(this, new ValueEventArgs<int>(delta));
            }
        }

        /// <summary>
        /// Determines whether any mouse button is down.
        /// </summary>
        /// <returns>
        /// <c>true</c> if any mouse button is down; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAnyMouseButtonDown()
        {
            return this.pressedMouseButtons.Any(kvp => kvp.Value);
        }

        /// <summary>
        /// Gets whether the specified MouseButton is down.
        /// </summary>
        /// <param name="mb">The MouseButton.</param>
        /// <returns><c>true</c> if the MouseButton is down; otherwise, <c>false</c>.</returns>
        public bool IsMouseButtonDown(MouseButton mb)
        {
            bool retValue;
            this.pressedMouseButtons.TryGetValue(mb, out retValue);
            return retValue;
        }

        /// <summary>
        /// Captures the input devices.
        /// </summary>
        public virtual void Update()
        {
            if (this.CaptureOnUpdate)
            {
                this.Capture();
            }

            var currentTime = this.MiyagiSystem.LastUpdate;

            // key held
            TestHold(currentTime, this.isKeyDown, ref this.keyHeld, ref this.lastKeyHeldTime, this.InjectKeyHeld);

            // mouse held
            TestHold(currentTime, this.IsAnyMouseButtonDown(), ref this.mouseHeld, ref this.lastMouseHeldTime, this.InjectMouseHeld);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Captures the input devices.
        /// </summary>
        protected virtual void Capture()
        {
            if (this.CaptureRequested != null)
            {
                this.CaptureRequested(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Disposes the InputManager.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.Disposing = null;
            this.CaptureRequested = null;
            this.KeyDown = null;
            this.KeyUp = null;
            this.MouseDown = null;
            this.MouseGestureRecognized = null;
            this.MouseLocationChanged = null;
            this.MouseUp = null;
            this.MouseWheelMoved = null;
        }

        /// <summary>
        /// Handles mouse moves.
        /// </summary>
        /// <param name="oldLocation">The old mouse location.</param>
        /// <param name="newLocation">The new mouse location.</param>
        protected virtual void OnMouseMoved(Point oldLocation, Point newLocation)
        {
            if (this.CursorClipArea != null && !this.CursorClipArea.Value.Contains(newLocation))
            {
                var clip = new LiangBarskyClipping(this.CursorClipArea.Value);
                this.mouseLocation = newLocation = clip.ClipLineEndPoint(this.CursorClipArea.Value.Center, newLocation);
            }

            // MouseGestures
            if (this.pressedMouseButtons[this.MouseGesturesButton])
            {
                Point delta = newLocation - this.oldMouseGesturePositon;
                if (Math.Abs(delta.X) >= this.MouseGestureMinDistance || Math.Abs(delta.Y) >= this.MouseGestureMinDistance)
                {
                    this.AddMouseGesture(this.GetMouseGestures(this.oldMouseGesturePositon.AngleBetween(newLocation)), this.oldMouseGesturePositon);
                    this.oldMouseGesturePositon = newLocation;
                }
            }

            if (this.MouseLocationChanged != null)
            {
                this.MouseLocationChanged(this, new ChangedValueEventArgs<Point>(oldLocation, newLocation));
            }
        }

        #endregion Protected Methods

        #region Private Static Methods

        private static void TestHold(DateTime currentTime, bool isDown, ref bool isHold, ref DateTime lastHeldTime, Action heldAction)
        {
            if (isDown)
            {
                if (isHold)
                {
                    var timePassed = currentTime - lastHeldTime;
                    if (timePassed != TimeSpan.Zero)
                    {
                        int numEvents = (int)(timePassed.TotalMilliseconds / FlowTime.TotalMilliseconds);

                        if (numEvents != 0)
                        {
                            lastHeldTime = currentTime;

                            for (int i = 0; i < numEvents; i++)
                            {
                                heldAction();
                            }
                        }
                    }
                }
                else if (HeldWaitTime < (currentTime - lastHeldTime))
                {
                    lastHeldTime = currentTime;
                    isHold = true;
                }
            }
        }

        #endregion Private Static Methods

        #region Private Methods

        private void AddMouseGesture(MouseGestures mg, Point pos)
        {
            if (mg == MouseGestures.None)
            {
                return;
            }

            if (mg != this.lastMouseGesture)
            {
                this.mouseGestures.Add(mg);
                this.mouseGesturesLocations.Add(pos);
                this.lastMouseGesture = mg;
            }
        }

        private void FireMouseGestureEvent()
        {
            if (this.mouseGestures != null && this.mouseGestures.Count > 0)
            {
                var e = new MouseGestureEventArgs(this.mouseGestures, this.mouseGesturesLocations, this.mouseGestureEndLocation);

                if (this.MouseGestureRecognized != null)
                {
                    this.MouseGestureRecognized(this, e);
                }
            }
        }

        private MouseGestures GetMouseGestures(double angle)
        {
            var retValue = MouseGestures.None;

            double segAngle = (float)this.MouseGestureSegmentsAngle / 2;
            if (angle >= 360 - segAngle || angle < segAngle)
            {
                retValue |= MouseGestures.Up;
            }
            else if (angle >= 180 - segAngle && angle < 180 + segAngle)
            {
                retValue |= MouseGestures.Down;
            }

            if (angle >= 90 - segAngle && angle < 90 + segAngle)
            {
                retValue |= MouseGestures.Right;
            }
            else if (angle >= 270 - segAngle && angle < 270 + segAngle)
            {
                retValue |= MouseGestures.Left;
            }

            return retValue;
        }

        #endregion Private Methods

        #endregion Methods
    }
}