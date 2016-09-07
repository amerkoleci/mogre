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
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Serialization;
    using Miyagi.Internals;
    using Miyagi.UI.Controls;

    /// <summary>
    /// A GUI represents a composite group of controls.
    /// </summary>
    [SerializableType]
    public class GUI : IDisposable, INamable, IControlCollectionOwner
    {
        #region Fields

        private static readonly NameGenerator NameGenerator = new NameGenerator();

        private bool visible;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GUI"/> class.
        /// </summary>
        public GUI()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GUI"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public GUI(string name)
        {
            NameGenerator.NextWhenNullOrEmpty(this, name);
            this.Controls = new ControlCollection(this);
            this.visible = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="GUI"/> class.
        /// </summary>
        ~GUI()
        {
            this.Dispose(false);
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs after the GUI has been updated.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Occurs before the GUI has been updated.
        /// </summary>
        public event EventHandler Updating;

        /// <summary>
        /// Occurs when the <see cref="Visible"/> property changes.
        /// </summary>
        public event EventHandler VisibleChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets an enumerator for a simple iteration over all controls of the GUI.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public IEnumerable<Control> AllControls
        {
            get
            {
                int count = this.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    yield return this.Controls[i];
                    foreach (Control c in this.Controls[i].AllControls)
                    {
                        yield return c;
                    }
                }
            }
        }

        /// <summary>
        /// Gets an enumerator for a simple iteration over all controls of the GUI in reverse direction.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public IEnumerable<Control> AllControlsReverse
        {
            get
            {
                int count = this.Controls.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    yield return this.Controls[i];
                    foreach (Control c in this.Controls[i].AllControlsReverse)
                    {
                        yield return c;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the collection of controls.
        /// </summary>
        public ControlCollection Controls
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="GUIManager"/>.
        /// </summary>
        public GUIManager GUIManager
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the <see cref="MiyagiSystem"/>.
        /// </summary>
        public MiyagiSystem MiyagiSystem
        {
            get
            {
                return this.GUIManager == null ? null : this.GUIManager.MiyagiSystem;
            }
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
        /// Gets or sets the <see cref="ISpriteRenderer"/>.
        /// </summary>
        /// <remarks>The sprite renderer is created after the GUI has been added to the <see cref="GUIManager"/>.</remarks>
        public ISpriteRenderer SpriteRenderer
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="GUI"/> is visible.
        /// </summary>
        /// <value><c>true</c> if the <see cref="GUI"/> is visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get
            {
                return this.visible;
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

        /// <summary>
        /// Gets or sets the z-order.
        /// </summary>
        public int ZOrder
        {
            get
            {
                return this.SpriteRenderer != null ? this.SpriteRenderer.ZOrder : -1;
            }

            set
            {
                if (value < 1)
                {
                    value = 1;
                }

                if (this.SpriteRenderer != null)
                {
                    this.SpriteRenderer.ZOrder = value;
                }
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal ResizeHelper ResizeHelper
        {
            get;
            private set;
        }

        #endregion Internal Properties

        #endregion Properties

        #region Methods

        #region Explicit Interface Methods

        /// <summary>
        /// Notifies the GUI that a new control has been added.
        /// </summary>
        /// <param name="control">The new control.</param>
        /// <remarks>This method supports the internal infrastructure and should not be called by user code.</remarks>
        void IControlCollectionOwner.NotifyControlAdded(Control control)
        {
            this.OnTopControlAdded(new ValueEventArgs<Control>(control));
        }

        /// <summary>
        /// Notifies the GUI that a control has been removed.
        /// </summary>
        /// <param name="control">The removed control.</param>
        /// <remarks>This method supports the internal infrastructure and should not be called by user code.</remarks>
        void IControlCollectionOwner.NotifyControlRemoved(Control control)
        {
            this.OnControlRemoved(new ValueEventArgs<Control>(control));
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Releases the unmanaged resources used by the GUI and disposes all of its controls.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all Controls.
        /// </summary>
        public void DisposeAllControls()
        {
            // cycle through the top buttons only, that way the rest will be deleted in a non-conflictive order.
            while (this.Controls.Count > 0)
            {
                this.Controls[0].Dispose();
            }

            this.Controls.Clear();
            this.Controls = null;
        }

        /// <summary>
        /// Ensures the Z-order of all controls.
        /// </summary>
        public virtual void EnsureZOrder()
        {
            int i = 0;
            this.Controls.EnsureZOrder(ref i);
        }

        /// <summary>
        /// Fades the GUI.
        /// </summary>
        /// <param name="startOpacity">The start opacity.</param>
        /// <param name="endOpacity">The end opacity.</param>
        /// <param name="fadeDuration">The duration of the fade operation.</param>
        public void Fade(float startOpacity, float endOpacity, TimeSpan fadeDuration)
        {
            this.Controls.ForEach(c => c.Fade(startOpacity, endOpacity, fadeDuration));
        }

        /// <summary>
        /// Fades the GUI.
        /// </summary>
        /// <param name="startOpacity">The start opacity.</param>
        /// <param name="endOpacity">The end opacity.</param>
        /// <param name="fadeDuration">The duration of the fade operation in milliseconds.</param>
        public void Fade(float startOpacity, float endOpacity, int fadeDuration)
        {
            this.Fade(startOpacity, endOpacity, TimeSpan.FromMilliseconds(fadeDuration));
        }

        /// <summary>
        /// Forces a redraw of all controls with the next update.
        /// </summary>
        public virtual void ForceRedraw()
        {
            this.SpriteRenderer.Viewport.UpdateBounds();
            this.Controls.ForEach(c => c.ForceRedraw(true, false));
        }

        /// <summary>
        /// Gets a control of the specified type by name.
        /// </summary>
        /// <param name="name">The name of the control.</param>
        /// <returns>The first control of that name if it exists, otherwise null.</returns>
        /// <typeparam name="T">The type of the control.</typeparam>
        public T GetControl<T>(string name)
            where T : Control
        {
            return this.AllControls.Where(control => control.Name == name).FirstOrDefault() as T;
        }

        /// <summary>
        /// Gets a control by name.
        /// </summary>
        /// <param name="name">The name of the control.</param>
        /// <returns>If it exists the first control with that name, otherwise null.</returns>
        public Control GetControl(string name)
        {
            return this.AllControls.FirstOrDefault(control => control.Name == name);
        }

        /// <summary>
        /// Returns the topmost control of the GUI under the mouse cursor.
        /// </summary>
        /// <returns>If there is a control under the cursor the topmost, otherwise null.</returns>
        public Control GetTopControl()
        {
            return this.GetTopControlAt(this.GUIManager.MiyagiSystem.InputManager.MouseLocation);
        }

        /// <summary>
        /// Returns the topmost control at the specified coordinate.
        /// </summary>
        /// <param name="p">The coordinate.</param>
        /// <returns>If there is a control at the position the topmost, otherwise null.</returns>
        public virtual Control GetTopControlAt(Point p)
        {
            return GetTopControlAt(this.Controls, p);
        }

        /// <summary>
        /// Returns the topmost control at the specified coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>If there is a control at the position the topmost, otherwise null.</returns>
        public Control GetTopControlAt(int x, int y)
        {
            return this.GetTopControlAt(new Point(x, y));
        }

        /// <summary>
        /// Triggers mouse leave events.
        /// </summary>
        public void InjectMouseLeave()
        {
            this.InjectMouseLeave(this.GetTopControl());
        }

        /// <summary>
        /// Triggers mouse move events.
        /// </summary>
        public void InjectMouseMoved()
        {
            this.InjectMouseMoved(this.GetTopControl());
        }

        /// <summary>
        /// Resizes the GUI.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        public void Resize(double widthFactor, double heightFactor)
        {
            if (this.ResizeHelper == null)
            {
                this.ResizeHelper = new ResizeHelper(this.DoResize);
            }

            this.ResizeHelper.Resize(widthFactor, heightFactor);
        }

        /// <summary>
        /// Returns the name of the GUI.
        /// </summary>
        /// <returns>The name of the GUI.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Updates the GUI.
        /// </summary>
        public void Update()
        {
            //if (!visible)
            //    return;//HACK pkubat changed

            if (this.Updating != null)
            {
                this.Updating(this, EventArgs.Empty);
            }

            this.UpdateCore();

            if (this.Updated != null)
            {
                this.Updated(this, EventArgs.Empty);
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddChildControl(Control c)
        {
            c.GUI = this;
            c.Controls.ForEach(this.AddChildControl);
        }

        #endregion Internal Methods

        #region Protected Internal Methods

        /// <summary>
        /// Creates the sprite renderer.
        /// </summary>
        /// <param name="system">The system.</param>
        protected internal virtual void CreateSpriteRenderer(MiyagiSystem system)
        {
            if (this.SpriteRenderer != null)
            {
                this.DestroySpriteRenderer();
            }

            this.SpriteRenderer = system.RenderManager.Create2DRenderer();
            this.SpriteRenderer.ZOrder = this.ZOrder;
        }

        /// <summary>
        /// Destroys the sprite renderer.
        /// </summary>
        protected internal virtual void DestroySpriteRenderer()
        {
            if (this.MiyagiSystem.HasManager("Render") && this.SpriteRenderer != null)
            {
                this.MiyagiSystem.RenderManager.DestroyRenderer(this.SpriteRenderer);
                this.SpriteRenderer = null;
            }
        }

        /// <summary>
        /// Triggers mouse leave events.
        /// </summary>
        /// <param name="topControl">The top control.</param>
        protected internal virtual void InjectMouseLeave(Control topControl)
        {
            foreach (Control c in this.AllControls)
            {
                if (c != topControl)
                {
                    // if the mouse was previously over, trigger the mouse off.
                    if (c.IsMouseOver && c.Enabled)
                    {
                        c.InputInjector.InjectMouseLeave();
                        c.InputInjector.InjectDragLeave();
                    }
                }
            }
        }

        /// <summary>
        /// Triggers mouse move events.
        /// </summary>
        /// <param name="topControl">The top control.</param>
        protected internal virtual void InjectMouseMoved(Control topControl)
        {
            if (topControl != null && topControl.Enabled)
            {
                if (!topControl.IsMouseOver)
                {
                    topControl.InputInjector.InjectMouseEnter();
                    topControl.InputInjector.InjectDragEnter();
                }
                else
                {
                    topControl.InputInjector.InjectMouseHover();
                }
            }
        }

        #endregion Protected Internal Methods

        #region Protected Static Methods

        /// <summary>
        /// Returns the topmost control at the specified coordinate.
        /// </summary>
        /// <param name="cc">The control collection.</param>
        /// <param name="p">The coordinate.</param>
        /// <returns>
        /// If there is a control at the position the topmost, otherwise null.
        /// </returns>
        protected static Control GetTopControlAt(ControlCollection cc, Point p)
        {
            for (int i = cc.Count - 1; i >= 0; i--)
            {
                Control c = cc.GetControlByZOrder(i);

                if (c.HitTest(p))
                {
                    return GetTopControlAt(c.Controls, p) ?? c;
                }
            }

            return null;
        }

        #endregion Protected Static Methods

        #region Protected Methods

        /// <summary>
        /// Releases the unmanaged resources used by the GUI and disposed all its controls.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeAllControls();

                MiyagiCollection<GUI> guiList = this.GUIManager.GUIs;
                if (guiList.Contains(this))
                {
                    guiList.Remove(this);
                }
            }
        }

        /// <summary>
        /// Resizes the GUI.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected virtual void DoResize(double widthFactor, double heightFactor)
        {
            this.Controls.ForEach(c => c.Resize(widthFactor, heightFactor));
        }

        /// <summary>
        /// Handles removal of controls.
        /// </summary>
        /// <param name="e">The ValueEventArgs.</param>
        protected virtual void OnControlRemoved(ValueEventArgs<Control> e)
        {
            if (!e.Data.IsDisposed)
            {
                e.Data.ForceRedraw(false, false);
                e.Data.GUI = null;
                e.Data.Controls.ForEach(c => this.OnControlRemoved(new ValueEventArgs<Control>(c)));
            }
        }

        /// <summary>
        /// Handles addition of controls.
        /// </summary>
        /// <param name="e">The ValueEventArgs.</param>
        protected virtual void OnTopControlAdded(ValueEventArgs<Control> e)
        {
            e.Data.Parent = null;
            e.Data.GUI = this;
            e.Data.Controls.ForEach(this.AddChildControl);
            this.EnsureZOrder();
        }

        /// <summary>
        /// Raises the VisibleChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
            this.Controls.ForEach(c => c.NotifyGUIVisibleChanged());

            if (this.GUIManager != null)
            {
                this.GUIManager.ReleaseFocusedAndGrabbedControl();
            }

            if (this.VisibleChanged != null)
            {
                this.VisibleChanged(this, e);
            }
        }

        /// <summary>
        /// Updates the GUI
        /// </summary>
        protected virtual void UpdateCore()
        {
            int count = this.Controls.Count;
            for (int i = 0; i < count; i++)
            {
                this.Controls[i].Update(false);
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}