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
    using Miyagi.Common.Events;
    using Miyagi.Common.Serialization;
    using Miyagi.UI.Controls;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.Internals;

    /// <summary>
    /// Manages the GUIs.
    /// </summary>
    public class GUIManager : IManager
    {
        #region Fields

        private readonly ExtendedStack<IModalDialog> modalDialogs;

        private Cursor cursor;
        private Control focusedControl;
        private MiyagiCollection<GUI> guis;
        private InputManager inputManager;
        private MouseButton lastMouseButton;
        private Control lastMouseClickControl;
        private DateTime lastMouseClickTime;
        private Control lastMouseDownControl;
        private LocaleManager localeManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GUIManager class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        protected internal GUIManager(MiyagiSystem system)
        {
            this.MiyagiSystem = system;
            this.GUIs = new MiyagiCollection<GUI>();
            this.modalDialogs = new ExtendedStack<IModalDialog>();
            this.CheckCursorVisibility = true;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the manager is disposing.
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// Occurs after the GUIManager has been updated.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Occues before the GUIManager has been updated.
        /// </summary>
        public event EventHandler Updating;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets an enumerator for a simple iteration over all GUI controls.
        /// </summary>
        /// <value>An enumerator for all gui controls.</value>
        public IEnumerable<Control> AllControls
        {
            get
            {
                return this.GUIs.SelectMany(t => t.AllControls);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor visibility should be checked before processing mouse events.
        /// </summary>
        public bool CheckCursorVisibility
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cursor.
        /// </summary>
        public Cursor Cursor
        {
            get
            {
                return this.cursor;
            }

            set
            {
                if (this.cursor != value)
                {
                    if (this.cursor != null)
                    {
                        this.cursor.Dispose();
                    }

                    this.cursor = value;

                    if (this.cursor != null)
                    {
                        this.cursor.SetGUIManager(this);
                        this.cursor.SetActiveMode(CursorMode.Main);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the focused control.
        /// </summary>
        /// <remarks>Focused controls have to have their <see cref="Control.Enabled"/> property set to true.</remarks>
        public Control FocusedControl
        {
            get
            {
                return this.focusedControl;
            }

            set
            {
                if (this.focusedControl != value)
                {
                    var oldFocused = this.focusedControl;
                    if (oldFocused != null && !oldFocused.IsDisposed && !oldFocused.Disposing)
                    {
                        oldFocused.Focused = false;
                    }

                    if (value != null && value.Enabled)
                    {
                        this.focusedControl = value;
                        value.Focused = true;
                    }
                    else
                    {
                        this.focusedControl = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the control which is currently grabbed by the mouse.
        /// </summary>
        public Control GrabbedControl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the collection of GUIs.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public MiyagiCollection<GUI> GUIs
        {
            get
            {
                return this.guis;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (this.guis != value)
                {
                    if (this.guis != null)
                    {
                        this.guis.ItemAdded -= this.OnGUIAdded;
                        this.guis.ItemInserted -= this.OnGUIAdded;
                        this.guis.ItemRemoved -= this.OnGUIRemoved;
                    }

                    value.ItemAdded += this.OnGUIAdded;
                    value.ItemInserted += this.OnGUIAdded;
                    value.ItemRemoved += this.OnGUIRemoved;
                    this.guis = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the manager has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether a drag operation is currently active.
        /// </summary>
        /// <value>
        /// <c>true</c> if a drag operation is currently active; otherwise, <c>false</c>.
        /// </value>
        public bool IsDragActive
        {
            get
            {
                return this.CurrentDragEventArgs != null;
            }
        }

        /// <summary>
        /// Gets the MiyagiSystem.
        /// </summary>
        public MiyagiSystem MiyagiSystem
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether hit detection should be pixel-perfect.
        /// </summary>
        public bool PixelPerfectHitDetection
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
                return "GUI";
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal DragEventArgs CurrentDragEventArgs
        {
            get;
            set;
        }

        internal ToolTipElement CurrentToolTip
        {
            get;
            private set;
        }

        internal bool IsCursorVisible
        {
            get
            {
                return !this.CheckCursorVisibility || (this.Cursor != null && this.Cursor.Visible);
            }
        }

        #endregion Internal Properties

        #region Protected Properties

        /// <summary>
        /// Gets the input manager.
        /// </summary>
        protected InputManager InputManager
        {
            get
            {
                return this.inputManager;
            }

            private set
            {
                if (this.inputManager != value)
                {
                    if (this.inputManager != null)
                    {
                        this.inputManager.KeyDown -= this.InputManagerKeyDown;
                        this.inputManager.KeyUp -= this.InputManagerKeyUp;
                        this.inputManager.KeyHeld -= this.InputManagerKeyHeld;
                        this.inputManager.MouseHeld -= this.InputManagerMouseHeld;
                        this.inputManager.MouseDown -= this.InputManagerMouseDown;
                        this.inputManager.MouseUp -= this.InputManagerMouseUp;
                        this.inputManager.MouseWheelMoved -= this.InputManagerMouseWheelMoved;
                        this.inputManager.MouseLocationChanged -= this.InputManagerMouseLocationChanged;
                        this.inputManager.MouseGestureRecognized -= this.InputManagerMouseGestureRecognized;
                        this.inputManager.Disposing -= this.InputManagerDisposing;
                    }

                    if (value != null)
                    {
                        value.KeyDown += this.InputManagerKeyDown;
                        value.KeyUp += this.InputManagerKeyUp;
                        value.KeyHeld += this.InputManagerKeyHeld;
                        value.MouseHeld += this.InputManagerMouseHeld;
                        value.MouseDown += this.InputManagerMouseDown;
                        value.MouseUp += this.InputManagerMouseUp;
                        value.MouseWheelMoved += this.InputManagerMouseWheelMoved;
                        value.MouseLocationChanged += this.InputManagerMouseLocationChanged;
                        value.MouseGestureRecognized += this.InputManagerMouseGestureRecognized;
                        value.Disposing += this.InputManagerDisposing;
                    }

                    this.inputManager = value;
                }
            }
        }

        /// <summary>
        /// Gets the locale manager.
        /// </summary>
        protected LocaleManager LocaleManager
        {
            get
            {
                return this.localeManager;
            }

            private set
            {
                if (this.localeManager != value)
                {
                    if (this.localeManager != null)
                    {
                        this.localeManager.CurrentCultureChanged -= this.LocaleManagerCurrentCultureChanged;
                        this.localeManager.Disposing -= this.LocaleManagerDisposing;
                    }

                    if (value != null)
                    {
                        value.CurrentCultureChanged += this.LocaleManagerCurrentCultureChanged;
                        value.Disposing += this.LocaleManagerDisposing;
                    }

                    this.localeManager = value;
                }
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Explicit Interface Methods

        void IManager.LoadSerializationData(SerializationData data)
        {
            this.Cursor = (Cursor)data["Cursor"];
            this.GUIs = (MiyagiCollection<GUI>)data["GUIs"];
            foreach (var gui in this.guis)
            {
                this.AddGUI(gui);
            }
        }

        void IManager.NotifyManagerRegistered(IManager manager)
        {
            if (manager.Type == "Input")
            {
                this.InputManager = (InputManager)manager;
            }

            if (manager.Type == "Locale")
            {
                this.LocaleManager = (LocaleManager)manager;
            }
        }

        void IManager.SaveSerializationData(SerializationData data)
        {
            data.Add("Cursor", this.cursor);
            data.Add("GUIs", this.GUIs);
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Disposes the GUIManager.
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
        /// Disposes all GUIs.
        /// </summary>
        public virtual void DisposeAllGUIs()
        {
            while (this.GUIs.Count > 0)
            {
                this.GUIs[0].Dispose();
            }

            this.modalDialogs.Clear();
        }

        /// <summary>
        /// Force the redraw of the cursor and all GUIs.
        /// </summary>
        public void ForceRedraw()
        {
            this.MiyagiSystem.RenderManager.MainViewport.UpdateBounds();

            if (this.Cursor != null)
            {
                this.Cursor.ForceRedraw();
            }

            this.GUIs.ForEach(g => g.ForceRedraw());
        }

        /// <summary>
        /// Gets a control of the specified type by name.
        /// </summary>
        /// <param name="name">The name of the control.</param>
        /// <returns>The first control of that name if it exists; otherwise, null.</returns>
        /// <typeparam name="T">The type of the control.</typeparam>
        public virtual T GetControl<T>(string name)
            where T : Control
        {
            return this.GUIs.Select(gui => gui.GetControl<T>(name)).Where(control => control != null).FirstOrDefault();
        }

        /// <summary>
        /// Gets a control by name.
        /// </summary>
        /// <param name="name">The name of the control.</param>
        /// <returns>The first control of that name if it exists; otherwise, null.</returns>
        public virtual Control GetControl(string name)
        {
            return this.GUIs.Select(gui => gui.GetControl(name)).FirstOrDefault(control => control != null);
        }

        /// <summary>
        /// Gets a control by path.
        /// </summary>
        /// <param name="path">The path of the control.</param>
        /// <returns>The first control with the specified path if it exists; otherwise, null.</returns>
        /// <exception cref="ArgumentException">Insufficent amount of parameters. Expected at least 2.</exception>
        public virtual Control GetControlByPath(params string[] path)
        {
            if (path.Length < 2)
            {
                throw new ArgumentException("Insufficent amount of parameters. Expected at least 2.", "path");
            }

            GUI gui = this.GUIs[path[0]];
            if (gui != null)
            {
                ControlCollection controlCollection = gui.Controls;
                Control control = null;
                for (int i = 1; i < path.Length; i++)
                {
                    control = controlCollection[path[i]];
                    if (control == null)
                    {
                        return null;
                    }

                    controlCollection = control.Controls;
                }

                return control;
            }

            return null;
        }

        /// <summary>
        /// Gets a GUI by name.
        /// </summary>
        /// <param name="name">The name of the GUI.</param>
        /// <returns>The first GUI of that name if it exists; otherwise, null.</returns>
        public virtual GUI GetGUI(string name)
        {
            return this.GUIs[name];
        }

        /// <summary>
        /// Returns the topmost control under the mouse cursor.
        /// </summary>
        /// <returns>If there is a control under the cursor the topmost; otherwise, null.</returns>
        public Control GetTopControl()
        {
            return this.GetTopControlAt(this.MiyagiSystem.InputManager.MouseLocation);
        }

        /// <summary>
        /// Returns the topmost control at the specified position.
        /// </summary>
        /// <param name="pos">The position where you want to look for a control.</param>
        /// <returns>If there is a control at the position the topmost; otherwise, null.</returns>
        public Control GetTopControlAt(Point pos)
        {
            return this.GetTopControlAt(pos.X, pos.Y);
        }

        /// <summary>
        /// Returns the topmost control at the specified position.
        /// </summary>
        /// <param name="x">The x-coordinate of the position where you want to look for a control.</param>
        /// <param name="y">The y-coordinate of the position where you want to look for a control.</param>
        /// <returns>If there is a control at the position the topmost; otherwise, null.</returns>
        public Control GetTopControlAt(int x, int y)
        {
            Control top;
            this.GetTopGUIAt(x, y, out top);
            return top;
        }

        /// <summary>
        /// Returns the topmost GUI under the mouse cursor.
        /// </summary>
        /// <returns>If there is a GUI under the cursor the topmost; otherwise, null.</returns>
        public GUI GetTopGUI()
        {
            return this.GetTopGUIAt(this.MiyagiSystem.InputManager.MouseLocation);
        }

        /// <summary>
        /// Returns the topmost GUI at the specified position.
        /// </summary>
        /// <param name="pos">The position where you want to look for a control.</param>
        /// <returns>If there is a GUI at the position the topmost; otherwise, null.</returns>
        public GUI GetTopGUIAt(Point pos)
        {
            return this.GetTopGUIAt(pos.X, pos.Y);
        }

        /// <summary>
        /// Returns the topmost GUI at the specified position.
        /// </summary>
        /// <param name="x">The x-coordinate of the position where you want to look for a GUI.</param>
        /// <param name="y">The y-coordinate of the position where you want to look for a GUI.</param>
        /// <returns>If there is a GUI at the position the topmost; otherwise, null.</returns>
        public GUI GetTopGUIAt(int x, int y)
        {
            Control top;
            return this.GetTopGUIAt(x, y, out top);
        }

        /// <summary>
        /// Hides the mouse cursor.
        /// </summary>
        public virtual void HideCursor()
        {
            if (this.Cursor != null)
            {
                this.Cursor.Visible = false;
            }
        }

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        public virtual void Initialize()
        {
            if (this.MiyagiSystem.HasManager("Input"))
            {
                this.InputManager = this.MiyagiSystem.InputManager;
            }

            if (this.MiyagiSystem.HasManager("Locale"))
            {
                this.LocaleManager = this.MiyagiSystem.LocaleManager;
            }
        }

        /// <summary>
        /// Resizes the cursor and all GUIs.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        public void Resize(double widthFactor, double heightFactor)
        {
            if (this.Cursor != null)
            {
                this.Cursor.Resize(widthFactor, heightFactor);
            }

            this.GUIs.ForEach(g => g.Resize(widthFactor, heightFactor));
        }

        /// <summary>
        /// Shows the mouse cursor.
        /// </summary>
        public virtual void ShowCursor()
        {
            if (this.Cursor != null)
            {
                this.Cursor.Visible = true;
            }
        }

        /// <summary>
        /// Updates all GUIs.
        /// </summary>
        public virtual void Update()
        {
            if (this.Updating != null)
            {
                this.Updating(this, EventArgs.Empty);
            }

            // apply DragDropEffect
            var dragDragEventArgs = this.CurrentDragEventArgs;
            if (dragDragEventArgs != null)
            {
                var topControl = this.GetTopControl();

                if (topControl != null && !topControl.AllowDrop && topControl != dragDragEventArgs.DragSource)
                {
                    this.Cursor.SetActiveMode(CursorMode.BlockDrop);
                }
                else
                {
                    var effect = dragDragEventArgs.Effect;
                    if (effect != null)
                    {
                        if (!string.IsNullOrEmpty(effect.Cursor))
                        {
                            this.Cursor.ActiveMode = effect.Cursor;
                        }
                    }
                }
            }

            // update controls
            if (!IsModalDialogVisible())
            {
                int count = this.GUIs.Count;
                for (int i = 0; i < count; i++)
                {
                    this.GUIs[i].Update();
                }
            }
            else
            {
                //this.modalDialogs.Peek().Update();//pkubat changed

                foreach (IModalDialog md in this.modalDialogs)
                {
                    md.Update();
                }

            }

            // hide tooltip if owner is invisible
            if (this.CurrentToolTip != null)
            {
                if (this.CurrentToolTip.Style == null
                    || !this.CurrentToolTip.Style.IsStandAlone)
                {
                    if (this.CurrentToolTip.Owner == null
                        || !this.CurrentToolTip.Owner.Visible)
                    {
                        this.CurrentToolTip.Hide();
                    }
                }
                else
                {
                    this.CurrentToolTip.Update();
                }
            }

            // update cursor
            if (this.Cursor != null)
            {
                this.Cursor.Update();
                this.Cursor.SetActiveMode(CursorMode.Main);
            }

            if (this.Updated != null)
            {
                this.Updated(this, EventArgs.Empty);
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void HideToolTip()
        {
            if (this.CurrentToolTip != null)
            {
                this.CurrentToolTip.RemoveSprite();
                this.CurrentToolTip = null;
            }
        }

        internal void PopModalDialog(IModalDialog modalDialog)
        {
            if (this.modalDialogs.Count > 0)
            {
                //if (this.modalDialogs.Peek() == modalDialog)pkubat changed
                //{
                //    this.modalDialogs.Pop();
                //}

                if (this.modalDialogs.Contains(modalDialog))
                {
                    this.modalDialogs.Remove(modalDialog);
                }
            }
        }

        internal void PushModalDialog(IModalDialog modalDialog)
        {
            if (this.CurrentToolTip != null)
            {
                this.CurrentToolTip.Hide();
            }

            this.modalDialogs.Push(modalDialog);
        }

        internal void ReleaseFocusedAndGrabbedControl()
        {
            this.FocusedControl = null;
            this.GrabbedControl = null;
        }

        internal void SetDragDrop(object data, DragDropEffect effects, Control dragSource)
        {
            this.CurrentDragEventArgs = new DragEventArgs(data, effects, dragSource);
        }

        public void SetToolTip(ToolTipElement toolTip)
        {
            if (toolTip != this.CurrentToolTip)
            {
                this.HideToolTip();
            }

            this.CurrentToolTip = toolTip;
        }

        #endregion Internal Methods

        #region Protected Internal Methods

        /// <summary>
        /// Returns the topmost GUI at the specified position.
        /// </summary>
        /// <param name="x">The x-coordinate of the position where you want to look for a GUI.</param>
        /// <param name="y">The y-coordinate of the position where you want to look for a GUI.</param>
        /// <param name="topControl">The topmost control.</param>
        /// <returns>If there is a GUI at the position the topmost; otherwise, null.</returns>
        protected internal virtual GUI GetTopGUIAt(int x, int y, out Control topControl)
        {
            topControl = null;

            // return the first modal dialog
            if (IsModalDialogVisible())
            {
                //IModalDialog md = this.modalDialogs.Peek();pkubat changed
                //if (CheckBounds(ref x, ref y, md.SpriteRenderer.Viewport.Bounds))
                //{
                //    topControl = md.GetTopControlAt(x, y);
                //    return md as GUI;
                //}

                foreach (IModalDialog md in this.modalDialogs)
                {
                    if (!md.CanReactToInput)
                        continue;

                    if (CheckBounds(ref x, ref y, md.SpriteRenderer.Viewport.Bounds))
                    {
                        topControl = md.GetTopControlAt(x, y);
                        if (topControl != null)
                            return md as GUI;
                    }
                }

                return null;
            }

            // iterate through guis
            GUI topGUI = null;
            int topGUIZOrder = 0;

            for (int i = this.GUIs.Count - 1; i >= 0; i--)
            {
                GUI gui = this.GUIs[i];
                int newX = x, newY = y;
                if (CheckBounds(ref newX, ref newY, gui.SpriteRenderer.Viewport.Bounds))
                {
                    if (gui.ZOrder >= topGUIZOrder)
                    {
                        Control control = gui.GetTopControlAt(newX, newY);

                        if (control != null)
                        {
                            topGUI = gui;
                            topControl = control;
                            topGUIZOrder = gui.ZOrder;
                        }
                    }
                }
            }

            return topGUI;
        }

        #endregion Protected Internal Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the GUIManager.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.InputManager = null;
            this.LocaleManager = null;

            this.ReleaseFocusedAndGrabbedControl();
            this.lastMouseClickControl = null;
            this.lastMouseDownControl = null;

            if (this.CurrentToolTip != null)
            {
                this.CurrentToolTip.Dispose();
                this.CurrentToolTip = null;
            }

            this.DisposeAllGUIs();

            if (this.Cursor != null)
            {
                this.Cursor.Dispose();
                this.Cursor = null;
            }

            this.Disposing = null;
        }

        /// <summary>
        /// Localizes all controls.
        /// </summary>
        /// <param name="controls">The controls.</param>
        protected virtual void LocalizeControls(IEnumerable<Control> controls)
        {
            foreach (var control in controls)
            {
                var localizable = control as ILocalizable;
                if (localizable != null && !string.IsNullOrEmpty(localizable.LocaleResourceKey))
                {
                    this.LocaleManager.ApplyResourceKey(localizable, localizable.LocaleResourceKey);
                }
            }
        }

        #endregion Protected Methods

        #region Private Static Methods

        private static bool CheckBounds(ref int x, ref int y, Rectangle bounds)
        {
            if (bounds.Contains(x, y))
            {
                Point offset = bounds.Location;
                x -= offset.X;
                y -= offset.Y;
                return true;
            }

            return false;
        }

        #endregion Private Static Methods

        #region Private Methods

        private void AddGUI(GUI g)
        {
            g.GUIManager = this;
            if (g.SpriteRenderer == null)
            {
                g.CreateSpriteRenderer(this.MiyagiSystem);
            }

            this.LocalizeControls(g.Controls);
        }

        private void CenterGrabbedControl(Point center)
        {
            int x = center.X, y = center.Y;
            Control grabbedCon = this.GrabbedControl;
            if (grabbedCon.CanReactToInput)
            {
                var p = grabbedCon.GetLocationInViewport() - grabbedCon.Location;
                int width = this.GrabbedControl.Width / 2;
                int height = this.GrabbedControl.Height / 2;
                this.GrabbedControl.Location = new Point(x - width - p.X, y - height - p.Y);
            }
        }

        private void InputManagerDisposing(object sender, EventArgs e)
        {
            this.InputManager = null;
        }

        private void InputManagerKeyDown(object sender, KeyboardEventArgs e)
        {
            Control tempFocusedControl = this.FocusedControl;
            if (tempFocusedControl != null)
            {
                var keyEvent = e.KeyEvent;
                this.TabMovement(keyEvent);
                tempFocusedControl.InputInjector.InjectKeyDown(keyEvent);
            }            
        }

        private void InputManagerKeyHeld(object sender, KeyboardEventArgs e)
        {
            if (this.FocusedControl != null)
            {
                var keyEvent = e.KeyEvent;
                this.TabMovement(keyEvent);
                this.FocusedControl.InputInjector.InjectKeyHeld(keyEvent);
            }
        }

        private void InputManagerKeyUp(object sender, KeyboardEventArgs e)
        {
            // send key released to focused control
            if (this.FocusedControl != null)
            {
                Control control = this.FocusedControl;
                KeyEvent keyEvent = e.KeyEvent;
                control.InputInjector.InjectKeyUp(keyEvent);
            }
        }

        private void InputManagerMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsCursorVisible)
            {
                this.lastMouseButton = e.MouseButton;
                var topControl = this.GetTopControl();

                if (topControl != null && topControl.Enabled)
                {
                    this.FocusedControl = topControl;

                    if (e.MouseButton == this.inputManager.MouseSelectButton && topControl.Movable)
                    {
                        this.GrabbedControl = topControl; // grab this control
                    }

                    // top control gets mouse down
                    this.lastMouseDownControl = topControl;
                    topControl.InputInjector.InjectMouseDown(e.MouseButton);

                    if (this.GrabbedControl != null && this.GrabbedControl.CenterOnGrab)
                    {
                        this.CenterGrabbedControl(this.inputManager.MouseLocation);
                    }
                }
                else
                {
                    this.FocusedControl = null;
                    this.lastMouseDownControl = null;
                    this.lastMouseClickControl = null;
                }
            }
        }

        private void InputManagerMouseGestureRecognized(object sender, MouseGestureEventArgs e)
        {
            if (this.focusedControl != null)
            {
                this.focusedControl.InputInjector.InjectMouseGesture(e);
            }
        }

        private void InputManagerMouseHeld(object sender, MouseButtonEventArgs e)
        {
            if (this.FocusedControl != null)
            {
                this.FocusedControl.InputInjector.InjectMouseHeld(e.MouseButton);
            }
        }

        private bool IsModalDialogVisible()
        {
            if (this.modalDialogs.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (IModalDialog md in this.modalDialogs)
                {
                    if (md.CanReactToInput)
                        return true;
                }
            }
            return false;
        }

        private void InputManagerMouseLocationChanged(object sender, ChangedValueEventArgs<Point> e)
        {
            if (this.IsCursorVisible)
            {
                if (this.Cursor != null)
                {
                    this.Cursor.Location = e.NewValue - this.Cursor.ActiveHotspot;
                }

                // drag the grabbed control
                var grabbedControl = this.GrabbedControl;
                if (grabbedControl != null && grabbedControl.CanReactToInput)
                {
                    var spriteRenderer = grabbedControl.GUI.SpriteRenderer;
                    var translatedOld = spriteRenderer.TransformCoordinate(e.OldValue);
                    var translatedNew = spriteRenderer.TransformCoordinate(e.NewValue);

                    var parent = grabbedControl.Parent;

                    if (parent == null || parent.AbsoluteRectangle.Contains(translatedNew) || grabbedControl.IsExceedingParent)
                    {
                        if (grabbedControl.CenterOnGrab)
                        {
                            // this centers the grabbed control to the mouse cursor
                            this.CenterGrabbedControl(translatedNew);
                        }
                        else
                        {
                            // follow mouse cursor
                            grabbedControl.Location = grabbedControl.Location - translatedOld + translatedNew;
                        }
                    }
                }

                Control topControl;
                var topGUI = this.GetTopGUIAt(this.inputManager.MouseLocation.X, this.inputManager.MouseLocation.Y, out topControl);

                // inject mouse drag
                if (this.inputManager.IsMouseButtonDown(this.inputManager.MouseSelectButton) && this.FocusedControl != null)
                {
                    var translatedNew = this.focusedControl.GUI.SpriteRenderer.TransformCoordinate(e.NewValue);
                    var parent = this.focusedControl.Parent;

                    if (parent == null || parent.AbsoluteRectangle.Contains(translatedNew) || this.focusedControl.IsExceedingParent)
                    {
                        this.focusedControl.InputInjector.InjectMouseDrag(e.OldValue, e.NewValue);

                        if (topControl != null)
                        {
                            topControl.InputInjector.InjectDragOver();
                        }
                    }
                }

                // trigger mouse moves
                if (!IsModalDialogVisible())
                {
                    if (topGUI != null)
                    {
                        topGUI.InjectMouseMoved(topControl);
                    }

                    int count = this.GUIs.Count;
                    for (int i = 0; i < count; i++)
                    {
                        this.GUIs[i].InjectMouseLeave(topControl);
                    }
                }
                else
                {
                    //IModalDialog md = this.modalDialogs.Peek();//pkubat changed
                    //md.InjectMouseMoved();
                    //md.InjectMouseLeave();

                    foreach (IModalDialog md in this.modalDialogs)
                    {
                        if (md.CanReactToInput)
                        {
                            md.InjectMouseMoved();
                            md.InjectMouseLeave();
                        }
                    }
                }
            }
        }

        private void InputManagerMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsCursorVisible)
            {
                var control = this.focusedControl;
                var lastUpdate = this.MiyagiSystem.LastUpdate;

                if (control != null)
                {
                    // detect click
                    if (control == this.lastMouseDownControl && e.MouseButton == this.lastMouseButton)
                    {
                        control.InputInjector.InjectMouseClick(e.MouseButton);

                        if (control == this.lastMouseClickControl
                            && lastUpdate - this.lastMouseClickTime < this.inputManager.MouseDoubleClickSpeed)
                        {
                            control.InputInjector.InjectMouseDoubleClick(e.MouseButton);
                            this.lastMouseClickControl = null;
                        }
                        else
                        {
                            this.lastMouseClickControl = control;
                        }

                        this.lastMouseClickTime = lastUpdate;
                    }

                    control.InputInjector.InjectMouseUp(e.MouseButton);
                }
                else
                {
                    this.lastMouseClickControl = null;
                }

                if (this.CurrentDragEventArgs != null)
                {
                    var topControl = this.GetTopControl();
                    if (topControl != null)
                    {
                        topControl.InputInjector.InjectDragDrop();
                    }

                    this.CurrentDragEventArgs = null;
                }

                if (this.inputManager != null && e.MouseButton == this.inputManager.MouseSelectButton)
                {
                    // stop carrying the grabbed control
                    this.GrabbedControl = null;
                }
            }
        }

        private void InputManagerMouseWheelMoved(object sender, ValueEventArgs<int> e)
        {
            if (this.IsCursorVisible)
            {
                if (e.Data != 0)
                {
                    bool tryScrolling = true;
                    Control tempControl = this.GetTopControl();
                    if (tempControl != null
                        && tempControl.HitTestVisible
                        && tempControl.HasMouseWheelHandlers)
                    {
                        tempControl.InputInjector.InjectMouseWheelMoved(e.Data);
                        tryScrolling = false;
                    }
                    else if (this.FocusedControl != null)
                    {
                        tryScrolling = !this.FocusedControl.InputInjector.InjectMouseWheelMoved(e.Data);
                    }

                    Control initialTempControl = tempControl;

                    if (tryScrolling)
                    {
                        while (tempControl != null)
                        {
                            if (tempControl.CanReactToInput
                                && tempControl is ScrollableControl)
                            {
                                ScrollableControl scrollableControl = tempControl as ScrollableControl;
                                if (scrollableControl.VScrollBarStyle != null
                                    && scrollableControl.VScrollBarStyle.Extent > 0)
                                {
                                    Point initialScroll = scrollableControl.Scroll;
                                    scrollableControl.Scroll += new Point(0, -e.Data);
                                    if (initialScroll != scrollableControl.Scroll)
                                    {
                                        tryScrolling = false;
                                        break;
                                    }

                                }
                            }
                            tempControl = tempControl.Parent;
                        }
                    }

                    if (tryScrolling)
                    {
                        tempControl = initialTempControl;
                        while (tempControl != null)
                        {
                            if (tempControl.CanReactToInput
                                && tempControl is ScrollableControl)
                            {
                                ScrollableControl scrollableControl = tempControl as ScrollableControl;
                                if (scrollableControl.HScrollBarStyle != null
                                    && scrollableControl.HScrollBarStyle.Extent > 0)
                                {
                                    Point initialScroll = scrollableControl.Scroll;
                                    scrollableControl.Scroll += new Point(-e.Data, 0);
                                    if (initialScroll != scrollableControl.Scroll)
                                    {
                                        tryScrolling = false;
                                        break;
                                    }

                                }
                            }
                            tempControl = tempControl.Parent;
                        }
                    }
                }
            }
        }

        private void LocaleManagerCurrentCultureChanged(object sender, EventArgs e)
        {
            this.LocalizeControls(this.AllControls);
        }

        private void LocaleManagerDisposing(object sender, EventArgs e)
        {
            this.LocaleManager = null;
        }

        private void OnGUIAdded(object sender, CollectionEventArgs<GUI> e)
        {
            this.AddGUI(e.Item);
        }

        private void OnGUIRemoved(object sender, CollectionEventArgs<GUI> e)
        {
            this.RemoveGUI(e.Item);
        }

        private void RemoveGUI(GUI g)
        {
            if (g.GUIManager == this)
            {
                g.DestroySpriteRenderer();
                g.GUIManager = null;
            }
        }

        private void TabMovement(KeyEvent arg)
        {
            if ((this.FocusedControl.IsArrowKeyMovementBlocked && arg.Key != ConsoleKey.Tab)
                || (arg.Key == ConsoleKey.Tab && arg.Modifiers == ConsoleModifiers.Alt))
            {
                return;
            }

            bool forward;
            if (arg.Key == ConsoleKey.LeftArrow || arg.Key == ConsoleKey.UpArrow || (arg.Modifiers == ConsoleModifiers.Shift && arg.Key == ConsoleKey.Tab))
            {
                forward = false;
            }
            else if (arg.Key == ConsoleKey.RightArrow || arg.Key == ConsoleKey.DownArrow || arg.Key == ConsoleKey.Tab)
            {
                forward = true;
            }
            else
            {
                return;
            }

            var focused = this.FocusedControl;
            var controls = focused.GetParentCollection();

            var sorted = new LinkedList<Control>(controls.Where(c => c.CanReactToInput && c.TabStop).OrderBy(c => c.TabIndex));
            var node = sorted.Find(focused) ?? sorted.First;
            LinkedListNode<Control> newNode;
            if (forward)
            {
                newNode = node != sorted.Last ? node.Next : sorted.First;
            }
            else
            {
                newNode = node != sorted.First ? node.Previous : sorted.Last;
            }

            this.FocusedControl = newNode != null ? newNode.Value : null;
        }

        #endregion Private Methods

        #endregion Methods
    }
}