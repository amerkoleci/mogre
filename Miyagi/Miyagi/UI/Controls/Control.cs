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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    using Miyagi.Common;
    using Miyagi.Common.Animation;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Serialization;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Layout;

    /// <summary>
    /// Represents the base class for controls.
    /// </summary>
    [SerializableType]
    public abstract class Control : IDisposable, INamable, IElementOwner, IControlCollectionOwner, INotifyPropertyChanged
    {
        #region Fields

        private const float OpacityThreshold = 0.01f;

        private static readonly NameGenerator NameGenerator = new NameGenerator();

        private bool alwaysOnBottom;
        private bool alwaysOnTop;
        private AnchorStyles anchor;
        private bool autoSize;
        private AutoSizeMode autoSizeMode;
        private Size clientSize;
        private DockStyle dock;
        private bool enabled;
        private LinearFunctionValueController<float> fadeController;
        private bool focused;
        private int height;
        private bool hittestVisible;
        private bool layoutPending;
        private int layoutSuspended;
        private int left;
        private Thickness margin;
        private Size maxSize;
        private Size minSize;
        private float opacity;
        private Thickness padding;
        private Control parent;
        private int tabIndex;
        private bool tabStop;
        private TextureFiltering textureFiltering = TextureFiltering.Anisotropic;
        private int top;
        protected bool visible;
        private int width;
        private bool isBroughtToFrontOnceFocused = true;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Control class.
        /// </summary>
        /// <param name="name">The name of the control.</param>
        protected Control(string name)
        {
            NameGenerator.NextWhenNullOrEmpty(this, name);

            this.TextureFiltering = TextureFiltering.Point;
            this.Controls = new ControlCollection(this);

            this.opacity = 1f;
            this.visible = true;
            this.enabled = true;
            this.tabStop = true;
            this.hittestVisible = true;

            // Default sizes
            this.GetDefaultValues();
            this.autoSizeMode = AutoSizeMode.GrowOnly;

            // Layout
            this.LayoutEngine = new DefaultLayout();
            this.RecalculateDistances = true;
            this.anchor = AnchorStyles.Top | AnchorStyles.Left;

            this.SetBounds(0, 0, 0, 0);

            this.InputInjector = CreateInputInjector(this);
        }

        /// <summary>
        /// Finalizes an instance of the Control class.
        /// </summary>
        ~Control()
        {
            this.Dispose(false);
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the <see cref="Anchor"/> property changes.
        /// </summary>
        public event EventHandler AnchorChanged;

        /// <summary>
        /// Occurs when the <see cref="AutoSize"/> property changes.
        /// </summary>
        public event EventHandler AutoSizeChanged;

        /// <summary>
        /// Occurs when a mouse button is clicked over the control or the Enter key is pressed.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// Occurs when the <see cref="ClientSize"/> property changes.
        /// </summary>
        public event EventHandler ClientSizeChanged;

        /// <summary>
        /// Occurs when a control has been added to the <see cref="Controls"/> property.
        /// </summary>
        public event EventHandler<ValueEventArgs<Control>> ControlAdded;

        /// <summary>
        /// Occurs when a control has been removed from the <see cref="Controls"/> property.
        /// </summary>
        public event EventHandler<ValueEventArgs<Control>> ControlRemoved;

        /// <summary>
        /// Occurs when the <see cref="Dock"/> property changes.
        /// </summary>
        public event EventHandler DockChanged;

        /// <summary>
        /// Occurs when [drag drop].
        /// </summary>
        public event EventHandler<DragEventArgs> DragDrop;

        /// <summary>
        /// Occurs when [drag enter].
        /// </summary>
        public event EventHandler<DragEventArgs> DragEnter;

        /// <summary>
        /// Occurs when [drag leave].
        /// </summary>
        public event EventHandler<DragEventArgs> DragLeave;

        /// <summary>
        /// Occurs when [drag over].
        /// </summary>
        public event EventHandler<DragEventArgs> DragOver;

        /// <summary>
        /// Occurs when the <see cref="Enabled"/> property changes.
        /// </summary>
        public event EventHandler EnabledChanged;

        /// <summary>
        /// Occurs when this control gets the focus.
        /// </summary>
        public event EventHandler GotFocus;

        /// <summary>
        /// Occurs when a key is pressed and the control has the focus.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyDown;

        /// <summary>
        /// Occurs when a key held is held and the control has the focus.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyHeld;

        /// <summary>
        /// Occurs when a key is released and the control has the focus.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyUp;

        /// <summary>
        /// Occurs when a control should reposition its child controls.
        /// </summary>
        public event EventHandler<LayoutEventArgs> Layout;

        /// <summary>
        /// Occurs when the <see cref="Location"/> property changes.
        /// </summary>
        public event EventHandler<ChangedValueEventArgs<Point>> LocationChanged;

        /// <summary>
        /// Occurs when this control loses the focus.
        /// </summary>
        public event EventHandler LostFocus;

        /// <summary>
        /// Occurs when the <see cref="Margin"/> property changes.
        /// </summary>
        public event EventHandler MarginChanged;

        /// <summary>
        /// Occurs when the <see cref="MaxSize"/> property changes.
        /// </summary>
        public event EventHandler MaxSizeChanged;

        /// <summary>
        /// Occurs when the <see cref="MinSize"/> property changes.
        /// </summary>
        public event EventHandler MinSizeChanged;

        /// <summary>
        /// Occurs when a mouse button is clicked over the control.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseClick;

        /// <summary>
        /// Occurs when a mouse button is clicked twice over the control.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDoubleClick;

        /// <summary>
        /// Occurs when a mouse button is pressed over the control.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDown;

        /// <summary>
        /// Occurs when the mouse has been dragged over the control.
        /// </summary>
        public event EventHandler<ChangedValueEventArgs<Point>> MouseDrag;

        /// <summary>
        /// Occurs when the mouse cursor enters the control.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseEnter;

        /// <summary>
        /// Occurs when a mouse button is held and the control has the focus.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseHeld;

        /// <summary>
        /// Occurs when the mouse cursor is moved over the control.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseHover;

        /// <summary>
        /// Occurs when the mouse cursor leaves the control.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseLeave;

        /// <summary>
        /// Occurs when a mouse button is released over the control.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseUp;

        /// <summary>
        /// Occurs when the mouse wheel is moved and the control has the focus.
        /// </summary>
        /// <remarks>Submits the relative wheel movement.</remarks>
        public event EventHandler<ValueEventArgs<int>> MouseWheelMoved;

        /// <summary>
        /// Occurs when the <see cref="Opacity"/> property changes.
        /// </summary>
        public event EventHandler OpacityChanged;

        /// <summary>
        /// Occurs when the <see cref="Padding"/> property changes.
        /// </summary>
        public event EventHandler<ChangedValueEventArgs<Thickness>> PaddingChanged;

        /// <summary>
        /// Occurs when the <see cref="Parent"/> property changes.
        /// </summary>
        public event EventHandler ParentChanged;

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the <see cref="Size"/> property changes.
        /// </summary>
        public event EventHandler SizeChanged;

        /// <summary>
        /// Occurs when a hit test has been performed successfully.
        /// </summary>
        public event EventHandler<CancelEventArgs> SuccessfulHitTest;

        /// <summary>
        /// Occurs when the <see cref="TabIndex"/> property changes.
        /// </summary>
        public event EventHandler TabIndexChanged;

        /// <summary>
        /// Occurs when the <see cref="TabStop"/> property changes.
        /// </summary>
        public event EventHandler TabStopChanged;

        /// <summary>
        /// Occurs when the <see cref="Visible"/> property changes.
        /// </summary>
        public event EventHandler VisibleChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets an enumerator for a simple iteration over all descendent controls.
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
        /// Gets an enumerator for a simple iteration over all descendent controls in reverse direction.
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
        /// Gets or sets a value indicating whether the control accepts data that has been dragged onto it.
        /// </summary>
        public bool AllowDrop
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control should be displayed as the bottommost control.
        /// </summary>
        public bool AlwaysOnBottom
        {
            get
            {
                return this.alwaysOnBottom;
            }

            set
            {
                if (this.alwaysOnBottom != value)
                {
                    if (value)
                    {
                        this.alwaysOnTop = false;
                    }

                    this.alwaysOnBottom = value;
                    if (this.GUI != null)
                    {
                        this.GUI.EnsureZOrder();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control should be displayed as the topmost control.
        /// </summary>
        public bool AlwaysOnTop
        {
            get
            {
                return this.alwaysOnTop;
            }

            set
            {
                if (this.alwaysOnTop != value)
                {
                    if (value)
                    {
                        this.alwaysOnBottom = false;
                    }

                    this.alwaysOnTop = value;
                    if (this.GUI != null)
                    {
                        this.GUI.EnsureZOrder();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the edges of the parent to which the control is bound.
        /// </summary>
        /// <value>A bitwise combination of <see cref="AnchorStyles"/>. The default is <b>Top</b> and <b>Left</b>.</value>
        public virtual AnchorStyles Anchor
        {
            get
            {
                return this.anchor;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.anchor != value)
                {
                    LayoutType = LayoutType.Anchor;

                    this.anchor = value;
                    if (value != AnchorStyles.None)
                    {
                        this.LayoutType = LayoutType.Anchor;
                        this.dock = DockStyle.None;
                    }

                    this.OnAnchorChanged(EventArgs.Empty);
                    this.OnPropertyChanged("Anchor");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control will resize according to the setting of <see cref="AutoSizeMode"/>.
        /// </summary>
        public virtual bool AutoSize
        {
            get
            {
                return this.autoSize;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.autoSize != value)
                {
                    this.autoSize = value;

                    if (value)
                    {
                        if (this.parent != null)
                        {
                            this.parent.PerformLayout(this, "AutoSize");
                        }
                    }

                    this.OnAutoSizeChanged(EventArgs.Empty);
                    this.OnPropertyChanged("AutoSize");
                }
            }
        }

        /// <summary>
        /// Gets or sets the automatic sizing behavior of the control.
        /// </summary>
        /// <value>One of the <see cref="AutoSizeMode"/> values. The default value is <b>GrowOnly</b>.</value>
        public AutoSizeMode AutoSizeMode
        {
            get
            {
                return this.autoSizeMode;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.autoSizeMode != value)
                {
                    this.autoSizeMode = value;
                    this.PerformLayout(this, "AutoSizeMode");
                }
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the bottom edge in pixels.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public virtual int Bottom
        {
            get
            {
                return this.top + this.height;
            }

            set
            {
                this.SetBounds(this.left, value - this.height, this.width, this.height, BoundsSpecified.Y);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is centered on the cursor when grabbed.
        /// </summary>
        /// <value>Default is <c>false</c>.</value>
        public bool CenterOnGrab
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the collection of child controls.
        /// </summary>
        [SerializerOptions(LoadLast = true)]
        public ControlCollection Controls
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the cursor mode that will be displayed if the cursor is over the control.
        /// </summary>
        public string Cursor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the default key commands are enabled.
        /// </summary>
        /// <value><c>true</c> if default key commands are enabled, otherwise <c>false</c>. Default is <c>true</c>.</value>
        /// <remarks>This can be used to prevent Slider and ListBox from reacting to their default keys.</remarks>
        public bool DefaultKeysEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the rectangle that represents the display area of the control.
        /// </summary>
        public Rectangle DisplayRectangle
        {
            get
            {
                return new Rectangle(this.ClientLocation, this.clientSize);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is disposing.
        /// </summary>
        /// <value><c>true</c> if the control is disposing; otherwise, <c>false</c>.</value>
        public bool Disposing
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent control and determines how a control is resized with its parent.
        /// </summary>
        /// <value>One of the <see cref="DockStyle"/> values. The default is <b>None</b>.</value>
        public virtual DockStyle Dock
        {
            get
            {
                return this.dock;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.dock != value)
                {
                    this.dock = value;

                    if (value != DockStyle.None)
                    {
                        this.LayoutType = LayoutType.Dock;
                        this.anchor = AnchorStyles.None;
                    }
                    else
                    {
                        this.LayoutType = LayoutType.Anchor;
                        this.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    }

                    if (this.parent != null)
                    {
                        this.parent.PerformLayout(this, "Dock");
                    }
                    else if (this.Controls.Count > 0)
                    {
                        this.PerformLayout();
                    }

                    this.OnDockChanged(EventArgs.Empty);
                    this.OnPropertyChanged("Dock");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control and its children should respond to events.
        /// </summary>
        /// <value>If set to <c>false</c>, the control and its children ignore all events.</value>
        public bool Enabled
        {
            get
            {
                // If enabled is set to false, the control won't be enabled no matter what. Otherwise, the control will be enabled if the parent is also enabled.
                return this.enabled && (this.parent == null || this.parent.Enabled);
            }

            set
            {
                if (this.enabled != value)
                {
                    this.ThrowIfDisposed();
                    this.enabled = value;
                    this.OnEnabledChanged(EventArgs.Empty);
                    this.OnPropertyChanged("Enabled");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has the focus.
        /// </summary>
        /// <value>If <c>true</c>, the control has the focus.</value>
        public bool Focused
        {
            get
            {
                return this.focused;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.focused != value)
                {
                    this.focused = value;

                    if (this.GUI != null)
                    {
                        var guiManager = this.GUI.GUIManager;
                        if (value)
                        {
                            if (this.Enabled)
                            {
                                guiManager.FocusedControl = this;
                                this.OnGotFocus(EventArgs.Empty);
                            }
                        }
                        else
                        {
                            if (guiManager.FocusedControl == this)
                            {
                                guiManager.FocusedControl = null;
                            }

                            this.OnLostFocus(EventArgs.Empty);
                        }
                    }

                    this.OnPropertyChanged("Focused");
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="GUI"/> of the control.
        /// </summary>
        public GUI GUI
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the height of the control in pixels.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public virtual int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.SetBounds(this.left, this.top, this.width, value, BoundsSpecified.Height);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is visible to a hit test.
        /// </summary>
        public bool HitTestVisible
        {
            get
            {
                return this.Visible && this.opacity >= OpacityThreshold && this.hittestVisible;
            }

            set
            {
                this.hittestVisible = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is disposed.
        /// </summary>
        /// <value><c>true</c> if the control is disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is over the control.
        /// </summary>
        /// <value><c>true</c> if the mouse is over the control; otherwise, <c>false</c>.</value>
        public bool IsMouseOver
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this control is a top level control.
        /// </summary>
        public bool IsTopLevelControl
        {
            get
            {
                return this.parent == null;
            }
        }

        public bool IsBroughtToFrontOnceFocused
        {
            get
            {
                return isBroughtToFrontOnceFocused;
            }
            set
            {
                isBroughtToFrontOnceFocused = value;
            }
        }

        /// <summary>
        /// Gets or sets the engine that is responsible for the layout of the control.
        /// </summary>
        public LayoutEngine LayoutEngine
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the left edge in pixels.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public virtual int Left
        {
            get
            {
                return this.left;
            }

            set
            {
                this.SetBounds(value, this.top, this.width, this.height, BoundsSpecified.X);
            }
        }

        /// <summary>
        /// Gets or sets the location of the control.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public virtual Point Location
        {
            get
            {
                return new Point(this.left, this.top);
            }

            set
            {
                this.SetBounds(value.X, value.Y, this.width, this.height, BoundsSpecified.Location);
            }
        }

        /// <summary>
        /// Gets or sets the space between controls.
        /// </summary>
        public Thickness Margin
        {
            get
            {
                return this.margin;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.margin != value)
                {
                    this.margin = value;
                    this.OnMarginChanged(EventArgs.Empty);

                    if (this.parent != null)
                    {
                        this.parent.PerformLayout(this, "Margin");
                    }

                    this.OnPropertyChanged("Margin");
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximal size of the control.
        /// </summary>
        public Size MaxSize
        {
            get
            {
                return this.maxSize;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.maxSize != value)
                {
                    this.maxSize = value;
                    if (this.Height > value.Height)
                    {
                        this.Height = value.Height;
                    }

                    if (this.Width > value.Width)
                    {
                        this.Width = value.Width;
                    }

                    this.OnMaxSizeChanged(new EventArgs());
                    this.OnPropertyChanged("MaxSize");
                }
            }
        }

        /// <summary>
        /// Gets or sets the minial size of the control.
        /// </summary>
        public Size MinSize
        {
            get
            {
                return this.minSize;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.minSize != value)
                {
                    this.minSize = value;
                    if (this.Height < value.Height)
                    {
                        this.Height = value.Height;
                    }

                    if (this.Width < value.Width)
                    {
                        this.Width = value.Width;
                    }

                    this.OnMinSizeChanged(new EventArgs());
                    this.OnPropertyChanged("MinSize");
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="MiyagiSystem"/>.
        /// </summary>
        public MiyagiSystem MiyagiSystem
        {
            get
            {
                return this.GUI == null || this.GUI.GUIManager == null
                           ? null
                           : this.GUI.GUIManager.MiyagiSystem;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is movable.
        /// </summary>
        /// <value>If set to <c>true</c>, the control can be grabbed and moved by the cursor. Default is <c>false</c>.</value>
        public bool Movable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the control.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ID of control - is not used internally
        /// </summary>
        public string ButtonId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the opacity of the control and its children.
        /// </summary>
        /// <value>The opacity of the control. Ranging between 0 and 1.</value>
        public float Opacity
        {
            get
            {
                return this.opacity;
            }

            set
            {
                this.ThrowIfDisposed();
                value = value.Clamp(0f, 1f);
                if (this.opacity != value)
                {
                    this.opacity = value;
                    this.OnOpacityChanged(EventArgs.Empty);

                    // adjust all children to inherit this opacity
                    this.Controls.ForEach(c => c.Opacity = value);

                    this.OnPropertyChanged("Opacity");
                }
            }
        }

        /// <summary>
        /// Gets or sets padding within the control.
        /// </summary>
        public Thickness Padding
        {
            get
            {
                return this.padding;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.padding != value)
                {
                    Thickness old = this.padding;
                    this.padding = value;
                    this.UpdateClientSize();

                    this.OnPaddingChanged(new ChangedValueEventArgs<Thickness>(old, this.padding));

                    if (this.AutoSize && this.parent != null)
                    {
                        this.parent.PerformLayout(this, "Padding");
                    }
                    else
                    {
                        this.PerformLayout(this, "Padding");
                    }

                    this.OnPropertyChanged("Padding");
                }
            }
        }

        /// <summary>
        /// Gets or sets the parent of the control.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public Control Parent
        {
            get
            {
                return this.parent;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.parent != value)
                {
                    this.SetParent(value);
                    this.OnPropertyChanged("Parent");
                }
            }
        }

        /// <summary>
        /// Gets the size of a rectangular area into which the control can fit.
        /// </summary>
        public Size PreferredSize
        {
            get
            {
                return this.GetPreferredSize(Size.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a Rectangle representing the bounds of the control.
        /// </summary>
        public virtual Rectangle Rectangle
        {
            get
            {
                return new Rectangle(this.Location, this.Size);
            }

            set
            {
                this.SetBounds(value.X, value.Y, value.Width, value.Height);
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the right edge in pixels.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public virtual int Right
        {
            get
            {
                return this.left + this.width;
            }

            set
            {
                this.SetBounds(value - this.width, this.top, this.width, this.height, BoundsSpecified.X);
            }
        }

        /// <summary>
        /// Gets or sets the width and height of the control.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public virtual Size Size
        {
            get
            {
                return new Size(this.width, this.height);
            }

            set
            {
                this.SetBounds(this.left, this.top, value.Width, value.Height, BoundsSpecified.Size);
            }
        }

        /// <summary>
        /// Gets the <see cref="ISpriteRenderer"/> of the control.
        /// </summary>
        public ISpriteRenderer SpriteRenderer
        {
            get
            {
                return this.GUI != null ? this.GUI.SpriteRenderer : null;
            }
        }

        /// <summary>
        /// Gets or sets the tab order of the control.
        /// </summary>
        public int TabIndex
        {
            get
            {
                return this.tabIndex;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.tabIndex != value)
                {
                    this.tabIndex = value;

                    this.OnTabIndexChanged(EventArgs.Empty);
                    this.OnPropertyChanged("TabIndex");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.
        /// </summary>
        /// <value><c>true</c> if the user can give the focus to the control using the TAB key; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        public bool TabStop
        {
            get
            {
                return this.tabStop;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.tabStop != value)
                {
                    this.tabStop = value;

                    this.OnTabStopChanged(EventArgs.Empty);
                    this.OnPropertyChanged("TabStop");
                }
            }
        }

        /// <summary>
        /// Gets or sets the texture filtering of the sprite of the control.
        /// </summary>
        /// <value>A <see cref="TextureFiltering"/> representing the texture filtering.</value>
        public TextureFiltering TextureFiltering
        {
            get
            {
                return this.textureFiltering;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.textureFiltering != value)
                {
                    this.textureFiltering = value;
                    this.AddUpdateType(UpdateTypes.TextureFiltering);
                }
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the top edge in pixels.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public virtual int Top
        {
            get
            {
                return this.top;
            }

            set
            {
                this.SetBounds(this.left, value, this.width, this.height, BoundsSpecified.Y);
            }
        }

        /// <summary>
        /// Gets the control that is an ancestor of this control and has no parent, or the control itself if it has got no parent.
        /// </summary>
        public Control TopLevelControl
        {
            get
            {
                return this.parent != null
                           ? this.parent.TopLevelControl
                           : this;
            }
        }

        /// <summary>
        /// Gets or sets an object which contains data about the control.
        /// </summary>
        /// <remarks>This is ignored by the serializer.</remarks>
        [SerializerOptions(Ignore = true)]
        public object UserData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control and its children are visible.
        /// </summary>
        /// <value>If the control is visible <c>true</c>; otherwise, <c>false</c>.</value>
        public virtual bool Visible
        {
            get
            {
                // If the visibility is set to false, the control won't be visible. Otherwise, the control will be visible if the parent is also visible.
                return this.visible
                       && (this.parent != null
                               ? this.parent.Visible
                               : this.GUI != null
                                     ? this.GUI.Visible
                                     : true);
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.visible != value)
                {
                    this.visible = value;
                    this.OnVisibleChanged(EventArgs.Empty);
                    this.OnPropertyChanged("Visible");
                }
            }
        }

        /// <summary>
        /// Gets the value of the visibility boolean itself
        /// </summary>
        /// <value>The real value of visibility.</value>
        public bool RealVisibility
        {
            get { return visible; }
        }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        /// <value>The width of the control in pixels.</value>
        [SerializerOptions(Ignore = true)]
        public virtual int Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.SetBounds(this.left, this.top, value, this.height, BoundsSpecified.Width);
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal bool CanReactToInput
        {
            get
            {
                return this.Enabled && this.HitTestVisible && !this.IsDisposed && this.MiyagiSystem != null; //pkubat added MiyagiSystem
            }
        }

        internal int DistBottom
        {
            get;
            set;
        }

        internal int DistRight
        {
            get;
            set;
        }

        internal InputInjector InputInjector
        {
            get;
            private set;
        }

        internal bool RecalculateDistances
        {
            get;
            set;
        }

        internal ResizeHelper ResizeHelper
        {
            get;
            private set;
        }

        #endregion Internal Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets the derived rectangle.
        /// </summary>
        protected internal virtual Rectangle AbsoluteRectangle
        {
            get
            {
                return new Rectangle(this.GetAbsoluteLocation(), this.Size);
            }
        }

        /// <summary>
        /// Gets or sets the height and width of the client area of the control.
        /// </summary>
        /// <value>A Size representing the size of the client area.</value>
        protected internal Size ClientSize
        {
            get
            {
                return this.clientSize;
            }

            set
            {
                if (this.clientSize != value)
                {
                    this.clientSize = value;
                    this.OnClientSizeChanged(EventArgs.Empty);
                    this.OnPropertyChanged("ClientSize");
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether arrow key movement is blocked.
        /// </summary>
        /// <value>
        /// <c>true</c> if arrow key movement is blocked; otherwise, <c>false</c>.
        /// </value>
        protected internal virtual bool IsArrowKeyMovementBlocked
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control exceeds its parent.
        /// </summary>
        protected internal virtual bool IsExceedingParent
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether magnetically docking is enabled.
        /// </summary>
        protected internal virtual bool IsMagneticallyDockingEnabled
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the type of layout used for this class.
        /// </summary>
        /// <value>A LayoutType representing the type of layout.</value>
        /// <remarks>Don't set this property directly. Do it through the Anchor and Dock properties.</remarks>
        protected internal LayoutType LayoutType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the z-order.
        /// </summary>
        public int ZOrder
        {
            get;
            private set;
        }

        #endregion Protected Internal Properties

        #region Protected Properties

        /// <summary>
        /// Gets a <see cref="Point"/> representing the offset that is applied to added controls.
        /// </summary>
        protected virtual Point ChildOffset
        {
            get
            {
                return this.parent != null ? this.parent.ChildOffset : Point.Empty;
            }
        }

        /// <summary>
        /// Gets the client location.
        /// </summary>
        protected virtual Point ClientLocation
        {
            get
            {
                return new Point(this.padding.Left, this.padding.Top);
            }
        }

        /// <summary>
        /// Gets the space, in pixels, that is specified by default between controls.
        /// </summary>
        /// <value>A Padding representing the space, in pixels, that is specified by default between controls.</value>
        protected virtual Thickness DefaultMargin
        {
            get
            {
                return Thickness.Empty;
            }
        }

        /// <summary>
        /// Gets the default maximum size of a control.
        /// </summary>
        /// <value>The default maximum size of a control.</value>
        protected virtual Size DefaultMaxSize
        {
            get
            {
                return Size.Empty;
            }
        }

        /// <summary>
        /// Gets the default spacing of the contents of a control.
        /// </summary>
        /// <value>The default spacing of the contents of a control.</value>
        protected virtual Size DefaultMinSize
        {
            get
            {
                return Size.Empty;
            }
        }

        /// <summary>
        /// Gets the default spacing of the contents of a control.
        /// </summary>
        /// <value>The default spacing of the contents of a control.</value>
        protected virtual Thickness DefaultPadding
        {
            get
            {
                return Thickness.Empty;
            }
        }

        /// <summary>
        /// Gets the difference between the old and new position.
        /// </summary>
        protected Point DeltaLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the difference between the old and new size.
        /// </summary>
        protected Point DeltaSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        protected virtual IList<IElement> Elements
        {
            get
            {
                return new Collection<IElement>();
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Explicit Interface Methods

        /// <summary>
        /// Ensures the Z-order of all controls.
        /// </summary>
        void IControlCollectionOwner.EnsureZOrder()
        {
            if (this.GUI != null)
            {
                this.GUI.EnsureZOrder();
            }
        }

        /// <summary>
        /// Notifies the control that a new child control has been added.
        /// </summary>
        /// <param name="control">The new control.</param>
        /// <remarks>This method supports the internal infrastructure and should not be called by user code.</remarks>
        void IControlCollectionOwner.NotifyControlAdded(Control control)
        {
            control.Parent = this;
            control.RecalculateDistances = true;
            control.Location += this.ChildOffset;

            var g = this.GUI;
            if (g != null)
            {
                g.AddChildControl(control);                            
                g.EnsureZOrder();
            }

            this.OnControlAdded(new ValueEventArgs<Control>(control));
            this.PerformLayout(control, "Added");
        }

        /// <summary>
        /// Notifies the control that a child control has been removed.
        /// </summary>
        /// <param name="control">The removed control.</param>
        /// <remarks>This method supports the internal infrastructure and should not be called by user code.</remarks>
        void IControlCollectionOwner.NotifyControlRemoved(Control control)
        {
            IControlCollectionOwner g = this.GUI;
            if (g != null)
            {
                g.NotifyControlRemoved(control);
            }

            control.Parent = null;

            this.OnControlRemoved(new ValueEventArgs<Control>(control));
            this.PerformLayout(control, "Removed");
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Moves the control to the front.
        /// </summary>
        public void BringToFront()
        {
            var controlCollection = this.GetParentCollection();
            if (controlCollection != null)
            {
                controlCollection.BringToFront(this);
            }
        }

        /// <summary>
        /// Disposes the control.
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.Disposing = true;
            this.Dispose(true);
            this.Disposing = false;
            this.IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initializes a drag-and-drop operation.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="effects">The effects.</param>
        public void DoDragDrop(object data, DragDropEffect effects)
        {
            this.GUI.GUIManager.SetDragDrop(data, effects, this);
        }

        /// <summary>
        /// Fades the control.
        /// </summary>
        /// <param name="startOpacity">The start opacity.</param>
        /// <param name="endOpacity">The end opacity.</param>
        /// <param name="fadeDuration">The duration of the fade operation.</param>
        public void Fade(float startOpacity, float endOpacity, TimeSpan fadeDuration)
        {
            this.ThrowIfDisposed();

            if (this.MiyagiSystem != null)
            {
                if (this.fadeController != null)
                {
                    this.fadeController.Stop();
                }

                this.fadeController = new LinearFunctionValueController<float>(startOpacity, endOpacity, fadeDuration);
                this.fadeController.Start(this.MiyagiSystem, true, val => this.Opacity = val);
                this.fadeController.Finished += (sender, e) => this.Opacity = endOpacity;
            }
            else
            {
                this.Opacity = endOpacity;
            }
        }

        /// <summary>
        /// Fades the control.
        /// </summary>
        /// <param name="startOpacity">The start opacity.</param>
        /// <param name="endOpacity">The end opacity.</param>
        /// <param name="fadeDuration">The duration of the fade operation in milliseconds.</param>
        public void Fade(float startOpacity, float endOpacity, int fadeDuration)
        {
            this.Fade(startOpacity, endOpacity, TimeSpan.FromMilliseconds(fadeDuration));
        }

        /// <summary>
        /// Forces a redraw of the control and its children with the next update.
        /// </summary>
        /// <param name="updateChildren">Indicated whether the children should be redrawn, too.</param>
        /// <param name="updateViewportBounds">Indicated whether the viewport bounds should be updated.</param>
        public void ForceRedraw(bool updateChildren, bool updateViewportBounds)
        {
            this.ThrowIfDisposed();

            foreach (var element in this.Elements)
            {
                element.RemoveSprite();
            }

            if (updateChildren)
            {
                this.Controls.ForEach(c => c.ForceRedraw(true, updateViewportBounds));
            }

            if (updateViewportBounds)
            {
                this.GUI.SpriteRenderer.Viewport.UpdateBounds();
            }
        }

        /// <summary>
        /// Gets the absolute location of the control.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the absolute location.</returns>
        public Point GetAbsoluteLocation()
        {
            return this.GetAbsoluteLocation(true);
        }

        /// <summary>
        /// Gets the location of the control relative to its viewport origin.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        public Point GetLocationInViewport()
        {
            return this.GetAbsoluteLocation(false);
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>A <see cref="Size"/> representing the width and height of a rectangle.</returns>
        public Size GetPreferredSize(Size proposedSize)
        {
            this.ThrowIfDisposed();
            var preferredSize = this.GetPreferredSizeCore(proposedSize);

            if (this.AutoSizeMode == AutoSizeMode.GrowOnly)
            {
                int prefWidth = Math.Max(preferredSize.Width, Size.Width);
                int prefHeight = Math.Max(preferredSize.Height, Size.Height);
                preferredSize = new Size(prefWidth, prefHeight);
            }

            return this.ApplySizeConstraints(preferredSize);
        }

        /// <summary>
        /// Gets a value indicating whether the provided coordinates are inside the control.
        /// </summary>
        /// <param name="p">The coordinate.</param>
        /// <returns><c>true</c> if the coordinates are inside the control; otherwise, <c>false</c>.</returns>
        public bool HitTest(Point p)
        {
            this.ThrowIfDisposed();

            // check if we can ignore the control
            if (!this.HitTestVisible)
            {
                return false;
            }

            bool retValue = this.HitTestCore(p);

            // look for children that exceed this control (e.g. DropDownLists)
            if (!retValue)
            {
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    var child = this.Controls[i];
                    if (child.IsExceedingParent)
                    {
                        retValue = child.HitTest(p);
                        if (retValue)
                            break;
                    }
                }
            }

            if (retValue)
            {
                if (this.SuccessfulHitTest != null)
                {
                    var e = new CancelEventArgs();
                    this.SuccessfulHitTest(this, e);
                    return !e.Cancel;
                }
            }

            return retValue;
        }

        /// <summary>
        /// Gets a value indicating whether the specified control is a child of this control or its children.
        /// </summary>
        /// <param name="childControl">The control to find.</param>
        /// <returns><c>true</c> if the control is a child; otherwise, <c>false</c>.</returns>
        public bool IsAncestor(Control childControl)
        {
            this.ThrowIfDisposed();
            return this.Controls.Contains(childControl) || this.Controls.Any(t => t.IsAncestor(childControl));
        }

        /// <summary>
        /// Recomputes the layout of the control.
        /// </summary>
        public void PerformLayout()
        {
            this.PerformLayout(null, null);
        }

        /// <summary>
        /// Restarts the texture animation.
        /// </summary>
        public void RestartTextureAnimation()
        {
            var elementList = this.Elements;
            foreach (var element in elementList.Where(element => !element.AreAllSpritesNull()))
            {
                element.RestartTextureAnimation();
            }
        }

        /// <summary>
        /// Resumes usual layout logic.
        /// </summary>
        public void ResumeLayout()
        {
            this.ResumeLayout(true);
        }

        /// <summary>
        /// Resumes usual layout logic, optionally forcing an immediate layout of pending layout requests.
        /// </summary>
        /// <param name="performLayout"><c>true</c> to execute pending layout requests; otherwise, <c>false</c>.</param>
        public void ResumeLayout(bool performLayout)
        {
            this.ThrowIfDisposed();

            if (this.layoutSuspended > 0)
            {
                this.layoutSuspended--;
            }

            if (this.layoutSuspended == 0)
            {
                if (!performLayout)
                {
                    foreach (var c in this.Controls)
                    {
                        c.UpdateDistances();
                    }
                }

                if (performLayout && this.layoutPending)
                {
                    this.PerformLayout(this, "ResumeLayout");
                }
            }
        }

        /// <summary>
        /// Moves the control to the back.
        /// </summary>
        public void SendToBack()
        {
            var controlCollection = this.GetParentCollection();
            if (controlCollection != null)
            {
                controlCollection.SendToBack(this);
            }
        }

        /// <summary>
        /// Sets the bounds of the control to the specified location and size.
        /// </summary>
        /// <param name="x">The new <see cref="Left"/> property value of the control.</param>
        /// <param name="y">The new <see cref="Top"/> property value of the control.</param>
        /// <param name="newWidth">The new <see cref="Width"/> property value of the control.</param>
        /// <param name="newHeight">The new <see cref="Height"/> property value of the control.</param>
        public void SetBounds(int x, int y, int newWidth, int newHeight)
        {
            this.SetBounds(x, y, newWidth, newHeight, BoundsSpecified.All);
        }

        /// <summary>
        /// Sets the bounds of the control to the specified location and size.
        /// </summary>
        /// <param name="x">The new <see cref="Left"/> property value of the control.</param>
        /// <param name="y">The new <see cref="Top"/> property value of the control.</param>
        /// <param name="newWidth">The new <see cref="Width"/> property value of the control.</param>
        /// <param name="newHeight">The new <see cref="Height"/> property value of the control.</param>
        /// <param name="specified">A bitwise combination of <see cref="BoundsSpecified"/> values.</param>
        public void SetBounds(int x, int y, int newWidth, int newHeight, BoundsSpecified specified)
        {
            this.ThrowIfDisposed();

            // Complete values that were not provided, only if Bounds != None
            if (specified != BoundsSpecified.None)
            {
                if (!specified.IsFlagSet(BoundsSpecified.X))
                {
                    x = this.left;
                }

                if (!specified.IsFlagSet(BoundsSpecified.Y))
                {
                    y = this.top;
                }

                if (!specified.IsFlagSet(BoundsSpecified.Width))
                {
                    newWidth = this.width;
                }

                if (!specified.IsFlagSet(BoundsSpecified.Height))
                {
                    newHeight = this.height;
                }
            }

            // Only call the function if something changed
            if (this.left != x || this.top != y || this.width != newWidth || this.height != newHeight)
            {
                this.SetBoundsCore(x, y, newWidth, newHeight, specified);
            }
        }

        /// <summary>
        /// Stops the texture animation.
        /// </summary>
        public void StopTextureAnimation()
        {
            var elementList = this.Elements;
            foreach (var element in elementList.Where(element => !element.AreAllSpritesNull()))
            {
                element.StopTextureAnimation();
            }
        }

        /// <summary>
        /// Temporarily suspends the layout logic for the control.
        /// </summary>
        public void SuspendLayout()
        {
            this.ThrowIfDisposed();
            this.layoutSuspended++;
        }

        /// <summary>
        /// Returns the name of the control.
        /// </summary>
        /// <returns>The name of the control.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        public void Update()
        {
            Update(false);
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        public void Update(bool forcedUpdate)
        {
            if (!forcedUpdate
                && (!visible || (GUI != null && !GUI.Visible))
                && (Elements.Count == 0 || ((Elements[0].UpdateType & UpdateTypes.Visibility) == UpdateTypes.None)))
                return;//HACK pkubat changed

            this.UpdateCore();

            this.DeltaSize = Point.Empty;
            this.DeltaLocation = Point.Empty;

            int count = this.Controls.Count;
            for (int i = 0; i < count; i++)
            {
                this.Controls[i].Update(forcedUpdate);
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void NotifyGUIVisibleChanged()
        {
            this.OnVisibleChanged(EventArgs.Empty);
        }

        internal void Resize(double widthFactor, double heightFactor)
        {
            if (this.ResizeHelper == null)
            {
                this.ResizeHelper = new ResizeHelper(this.DoResize);
            }

            this.Controls.ForEach(c => c.Resize(widthFactor, heightFactor));

            this.ResizeHelper.Resize(widthFactor, heightFactor);

            this.UpdateClientSize();
            this.RecalculateDistances = true;
            this.PerformLayout();
            this.ForceRedraw(false, false);
        }

        internal void SetZOrder(int zorder)
        {
            this.ThrowIfDisposed();

            if (this.ZOrder != zorder)
            {
                this.ZOrder = zorder;
                this.AddUpdateType(UpdateTypes.ZOrder);

                this.OnZOrderChanged();
            }
        }

        #endregion Internal Methods

        #region Protected Internal Methods

        /// <summary>
        /// Gets the collection which contains the control.
        /// </summary>
        /// <returns>A ControlCollection which contains the control.</returns>
        protected internal ControlCollection GetParentCollection()
        {
            return this.parent != null
                       ? this.parent.Controls
                       : this.GUI != null
                             ? this.GUI.Controls
                             : null;
        }

        /// <summary>
        /// Recomputes the layout of the control.
        /// </summary>
        /// <param name="affectedControl">The affected control.</param>
        /// <param name="affectedProperty">The affected property.</param>
        protected internal void PerformLayout(Control affectedControl, string affectedProperty)
        {
            this.ThrowIfDisposed();
            var layoutEventArgs = new LayoutEventArgs(affectedControl, affectedProperty);

            if (!this.visible)
            {
                return;
            }

            for (int i = 0; i < this.Controls.Count; i++)
            {
                var c = this.Controls[i];
                if (c.RecalculateDistances)
                {
                    c.UpdateDistances();
                }
            }

            if (this.layoutSuspended > 0)
            {
                this.layoutPending = true;
                return;
            }

            this.layoutPending = false;

            // Prevent us from getting messed up
            this.layoutSuspended++;

            // Layout the contents of the control
            try
            {
                this.LayoutEngine.Layout(this, layoutEventArgs);
                this.OnLayout(layoutEventArgs);
            }
            finally
            {
                // Need to make sure we decremend layoutSuspended
                this.layoutSuspended--;
            }
        }

        #endregion Protected Internal Methods

        #region Protected Methods

        /// <summary>
        /// Adds an UpdateType to the UpdateType property of all elements.
        /// </summary>
        /// <param name="updType">The UpdateType to add.</param>
        protected void AddUpdateType(UpdateTypes updType)
        {
            var list = this.Elements;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                list[i].UpdateType |= updType;
            }
        }

        /// <summary>
        /// Takes a proposed Size and returns a new one that satisfies all the constraints of the controller.
        /// </summary>
        /// <param name="proposedSize">The size proposed.</param>
        /// <returns>A size that is valid for the controller.</returns>
        protected virtual Size ApplySizeConstraints(Size proposedSize)
        {
            var validSize = proposedSize;

            // Check that the size is above the minimum specified size
            if (!this.MinSize.IsEmpty)
            {
                int validWidth = Math.Max(validSize.Width, this.MinSize.Width);
                int validHeight = Math.Max(validSize.Height, this.MinSize.Height);
                validSize = new Size(validWidth, validHeight);
            }

            // Check that the size is above the minimum specified size
            if (!this.MaxSize.IsEmpty)
            {
                int validWidth = Math.Min(validSize.Width, this.MaxSize.Width);
                int validHeight = Math.Min(validSize.Height, this.MaxSize.Height);
                validSize = new Size(validWidth, validHeight);
            }

            return validSize;
        }

        /// <summary>
        /// Calculates and sets the delta location.
        /// </summary>
        /// <param name="oldLocation">The old position.</param>
        /// <param name="newLocation">The new position.</param>
        protected virtual void CalculateDeltaLocation(Point oldLocation, Point newLocation)
        {
            if (this.MiyagiSystem != null)
            {
                int x = this.DeltaLocation.X + newLocation.X - oldLocation.X;
                int y = this.DeltaLocation.Y + newLocation.Y - oldLocation.Y;
                this.DeltaLocation = new Point(x, y);
                this.AddUpdateType(UpdateTypes.OwnerLocation | UpdateTypes.SpriteCrop);
            }
        }

        /// <summary>
        /// Calculates and sets the delta size.
        /// </summary>
        /// <param name="oldSize">The old size.</param>
        /// <param name="newSize">The new size.</param>
        protected virtual void CalculateDeltaSize(Size oldSize, Size newSize)
        {
            if (this.MiyagiSystem != null)
            {
                int x = this.DeltaSize.X + newSize.Width - oldSize.Width;
                int y = this.DeltaSize.Y + newSize.Height - oldSize.Height;
                this.DeltaSize = new Point(x, y);
                this.AddUpdateType(UpdateTypes.OwnerSize);
            }
        }

        /// <summary>
        /// Calculates a client area based on the size of a control.
        /// </summary>
        /// <param name="size">The size of the control.</param>
        /// <returns>A size representing said client area.</returns>
        protected virtual Size ClientSizeFromSize(Size size)
        {
            int clientWidth = size.Width - this.Padding.Horizontal;
            int clientHeight = size.Height - this.Padding.Vertical;

            return new Size(clientWidth, clientHeight);
        }

        /// <summary>
        /// Disposes the control.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.ReleaseEvents();

                    if (this.fadeController != null)
                    {
                        this.fadeController.Stop();
                        this.fadeController = null;
                    }

                    if (this.GUI != null)
                    {
                        var guiManager = this.GUI.GUIManager;
                        if (guiManager.GrabbedControl == this)
                        {
                            guiManager.GrabbedControl = null;
                        }

                        if (guiManager.FocusedControl == this)
                        {
                            guiManager.FocusedControl = null;
                        }
                    }

                    // delete children first.
                    if (this.Controls.Count > 0)
                    {
                        while (this.Controls.Count > 0)
                        {
                            this.Controls[0].Dispose();
                        }

                        this.Controls.Clear();
                    }

                    this.GetParentCollection().Remove(this);

                    foreach (var element in this.Elements)
                    {
                        element.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Resizes the control.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected virtual void DoResize(double widthFactor, double heightFactor)
        {
            // change bounds
            ResizeHelper.Scale(ref this.left, widthFactor);
            ResizeHelper.Scale(ref this.top, heightFactor);
            ResizeHelper.Scale(ref this.width, widthFactor);
            ResizeHelper.Scale(ref this.height, heightFactor);

            this.ResizeHelper.Scale(ref this.padding);
            this.ResizeHelper.Scale(ref this.margin);

            this.ResizeHelper.Scale(ref this.minSize);
            this.ResizeHelper.Scale(ref this.maxSize);
        }

        /// <summary>
        /// Gets the absolute location.
        /// </summary>
        /// <param name="viewportOffset">Indicates whether the location should be offset be the viewport offset.</param>
        /// <returns>A <see cref="Point"/> representing the absolute location.</returns>
        protected virtual Point GetAbsoluteLocation(bool viewportOffset)
        {
            var retValue = this.Location;
            if (!this.IsTopLevelControl)
            {
                retValue += this.parent.GetAbsoluteLocation(viewportOffset) + this.parent.DisplayRectangle.Location;
            }
            else if (viewportOffset && this.SpriteRenderer != null)
            {
                retValue += this.SpriteRenderer.Viewport.Offset;
            }

            return retValue;
        }

        /// <summary>
        /// Actual implementation of the method that retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>A Size representing the width and height of a rectangle.</returns>
        protected virtual Size GetPreferredSizeCore(Size proposedSize)
        {
            return this.Size;
        }

        /// <summary>
        /// Gets a value indicating whether the provided coordinates are inside the control.
        /// </summary>
        /// <param name="p">The coordinate.</param>
        /// <returns><c>true</c> if the coordinates are inside the control; otherwise, <c>false</c>.</returns>
        protected virtual bool HitTestCore(Point p)
        {
            var elements = this.Elements;
            int count = elements.Count;
            for (int i = 0; i < count; i++)
            {
                if (elements[i].HitTest(p))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Offsets the child controls.
        /// </summary>
        /// <param name="p">A <see cref="Point"/> representing the offset.</param>
        protected void OffsetChildren(Point p)
        {
            this.Controls.ForEach(c =>
                                  {
                                      c.CalculateDeltaLocation(Point.Empty, p);
                                      c.OffsetChildren(p);
                                  });
        }

        /// <summary>
        /// Raises the <see cref="Control.AnchorChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnAnchorChanged(EventArgs e)
        {
            this.UpdateDistances();

            if (this.parent != null)
            {
                this.parent.PerformLayout(this, "Anchor");
            }

            if (this.AnchorChanged != null)
            {
                this.AnchorChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.AutoSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnAutoSizeChanged(EventArgs e)
        {
            if (this.AutoSizeChanged != null)
            {
                this.AutoSizeChanged(this, e);
            }
        }

        /// <summary>
        /// Handles child location changes.
        /// </summary>
        /// <param name="child">The child control which location has been changed.</param>
        protected virtual void OnChildLocationChanged(Control child)
        {
        }

        /// <summary>
        /// Handles child size changes.
        /// </summary>
        /// <param name="child">The child control which size has been changed.</param>
        protected virtual void OnChildSizeChanged(Control child)
        {
        }

        /// <summary>
        /// Handles child visibility changes.
        /// </summary>
        /// <param name="child">The child control which visibility has been changed.</param>
        protected virtual void OnChildVisibleChanged(Control child)
        {
        }

        /// <summary>
        /// Raises the <see cref="Control.Click"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnClick(EventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.ClientSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnClientSizeChanged(EventArgs e)
        {
            foreach (var control in this.AllControls)
            {
                control.AddUpdateType(UpdateTypes.SpriteCrop);
            }

            if (this.ClientSizeChanged != null)
            {
                this.ClientSizeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.ControlAdded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual void OnControlAdded(ValueEventArgs<Control> e)
        {
            if (this.ControlAdded != null)
            {
                this.ControlAdded(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.ControlRemoved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual void OnControlRemoved(ValueEventArgs<Control> e)
        {
            if (this.ControlRemoved != null)
            {
                this.ControlRemoved(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.DockChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnDockChanged(EventArgs e)
        {
            if (this.DockChanged != null)
            {
                this.DockChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.DragDrop"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDragDrop(DragEventArgs e)
        {
            if (this.DragDrop != null)
            {
                this.DragDrop(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.DragEnter"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDragEnter(DragEventArgs e)
        {
            if (this.DragEnter != null)
            {
                this.DragEnter(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.DragLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDragLeave(DragEventArgs e)
        {
            if (this.DragLeave != null)
            {
                this.DragLeave(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.DragOver"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDragOver(DragEventArgs e)
        {
            if (this.DragOver != null)
            {
                this.DragOver(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.EnabledChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnEnabledChanged(EventArgs e)
        {
            this.CheckCanHoldFocus();
            this.Controls.ForEach(child => child.OnEnabledChanged(e));

            if (this.EnabledChanged != null)
            {
                this.EnabledChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.GotFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnGotFocus(EventArgs e)
        {
            if (IsBroughtToFrontOnceFocused
                && this.TopLevelControl.IsBroughtToFrontOnceFocused)
                this.TopLevelControl.BringToFront();

            if (this.GotFocus != null)
            {
                this.GotFocus(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyDown(KeyboardEventArgs e)
        {
            if (this.KeyDown != null)
            {
                this.KeyDown(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyHeld"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.KeyboardEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyHeld(KeyboardEventArgs e)
        {
            if (this.KeyHeld != null)
            {
                this.KeyHeld(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyUp(KeyboardEventArgs e)
        {
            if (this.KeyUp != null)
            {
                this.KeyUp(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Layout"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        protected virtual void OnLayout(LayoutEventArgs e)
        {
            if (this.Layout != null)
            {
                this.Layout(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.LocationChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual void OnLocationChanged(ChangedValueEventArgs<Point> e)
        {
            this.CalculateDeltaLocation(e.OldValue, e.NewValue);

            this.Controls.ForEach(c => c.OnParentLocationChanged(e));

            if (this.parent != null)
            {
                this.parent.OnChildLocationChanged(this);
            }

            if (this.LocationChanged != null)
            {
                this.LocationChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.LostFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnLostFocus(EventArgs e)
        {
            if (this.LostFocus != null)
            {
                this.LostFocus(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MarginChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMarginChanged(EventArgs e)
        {
            if (this.MarginChanged != null)
            {
                this.MarginChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MaxSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMaxSizeChanged(EventArgs e)
        {
            if (this.MaxSizeChanged != null)
            {
                this.MaxSizeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MinSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMinSizeChanged(EventArgs e)
        {
            if (this.MinSizeChanged != null)
            {
                this.MinSizeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseClick(MouseButtonEventArgs e)
        {
            if (!e.CancelEvent && this.MouseClick != null)
            {
                this.MouseClick(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDoubleClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (!e.CancelEvent && this.MouseDoubleClick != null)
            {
                this.MouseDoubleClick(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseDown(MouseButtonEventArgs e)
        {
            if (!e.CancelEvent && this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDrag"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            if (this.MouseDrag != null)
            {
                this.MouseDrag(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseEnter"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseEnter(MouseEventArgs e)
        {
            this.IsMouseOver = true;
            if (this.MouseEnter != null)
            {
                this.MouseEnter(this, e);
            }
        }

        /// <summary>
        /// Handles mouse gestures.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnMouseGesture(MouseGestureEventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHeld"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseHeld(MouseButtonEventArgs e)
        {
            if (this.MouseHeld != null)
            {
                this.MouseHeld(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHover"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseHover(MouseEventArgs e)
        {
            if (this.MouseHover != null)
            {
                this.MouseHover(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseLeave(MouseEventArgs e)
        {
            this.IsMouseOver = false;
            if (this.MouseLeave != null)
            {
                this.MouseLeave(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseUp(MouseButtonEventArgs e)
        {
            if (!e.CancelEvent && this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseWheelMoved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual bool OnMouseWheelMoved(ValueEventArgs<int> e)
        {
            if (this.MouseWheelMoved != null)
            {
                this.MouseWheelMoved(this, e);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns if there are any event handlers attached to Mousewheel event
        /// </summary>
        public virtual bool HasMouseWheelHandlers
        {
            get { return (MouseWheelMoved != null); }
        }


        /// <summary>
        /// Raises the <see cref="Control.OpacityChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnOpacityChanged(EventArgs e)
        {
            this.CheckCanHoldFocus();
            this.AddUpdateType(UpdateTypes.Opacity);

            if (this.OpacityChanged != null)
            {
                this.OpacityChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.PaddingChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual void OnPaddingChanged(ChangedValueEventArgs<Thickness> e)
        {
            this.OffsetChildren(new Point(e.NewValue.Left - e.OldValue.Left, e.NewValue.Top - e.OldValue.Top));

            if (this.PaddingChanged != null)
            {
                this.PaddingChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.ParentChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnParentChanged(EventArgs e)
        {
            this.AddUpdateType(UpdateTypes.SpriteCrop);

            if (this.ParentChanged != null)
            {
                this.ParentChanged(this, e);
            }
        }

        /// <summary>
        /// Handles parent location changes.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnParentLocationChanged(ChangedValueEventArgs<Point> e)
        {
            this.OnLocationChanged(e);
        }

        /// <summary>
        /// Handles parent size changes.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnParentSizeChanged(ChangedValueEventArgs<Size> e)
        {
        }

        /// <summary>
        /// Raises the <see cref="Control.PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected virtual void OnSizeChanged(ChangedValueEventArgs<Size> e)
        {
            this.Controls.ForEach(c => c.OnParentSizeChanged(e));

            if (this.parent != null)
            {
                this.parent.OnChildSizeChanged(this);
            }

            if (this.SizeChanged != null)
            {
                this.SizeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.TabIndexChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnTabIndexChanged(EventArgs e)
        {
            if (this.TabIndexChanged != null)
            {
                this.TabIndexChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.TabStopChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnTabStopChanged(EventArgs e)
        {
            if (this.TabStopChanged != null)
            {
                this.TabStopChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.VisibleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
            this.CheckCanHoldFocus();

            this.AddUpdateType(UpdateTypes.Visibility);

            this.Controls.ForEach(child => child.OnVisibleChanged(e));

            if (this.Visible)
            {
                if (this.Parent != null)
                {
                    this.Parent.PerformLayout(this, "Visible");
                    this.Parent.OnChildVisibleChanged(this);
                }
                else
                {
                    this.PerformLayout(this, "Visible");
                }
            }
            else
            {
                this.IsMouseOver = false;
            }

            if (this.VisibleChanged != null)
            {
                this.VisibleChanged(this, e);
            }
        }

        /// <summary>
        /// Handles z-order changes.
        /// </summary>
        protected virtual void OnZOrderChanged()
        {
        }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        /// <param name="x">The new <see cref="Left"/> property value of the control.</param>
        /// <param name="y">The new <see cref="Top"/> property value of the control.</param>
        /// <param name="newWidth">The new <see cref="Width"/> property value of the control.</param>
        /// <param name="newHeight">The new <see cref="Height"/> property value of the control.</param>
        /// <param name="specified">A bitwise combination of <see cref="BoundsSpecified"/> values.</param>
        protected virtual void SetBoundsCore(int x, int y, int newWidth, int newHeight, BoundsSpecified specified)
        {
            var rectangle = this.ApplyBoundsConstraints(x, y, newWidth, newHeight);
            newWidth = rectangle.Width;
            newHeight = rectangle.Height;
            x = rectangle.X;
            y = rectangle.Y;

            this.UpdateBounds(x, y, newWidth, newHeight);

            if (specified != BoundsSpecified.None)
            {
                this.UpdateDistances();
            }

            if (this.parent != null)
            {
                this.parent.PerformLayout(this, "Bounds");
            }
        }

        /// <summary>
        /// Calculates the size of the control based on the size of the client area.
        /// </summary>
        /// <param name="size">The size of the client area.</param>
        /// <returns>A size for the whole control.</returns>
        protected virtual Size SizeFromClientSize(Size size)
        {
            int retWidth = size.Width + this.Padding.Horizontal;
            int retHeight = size.Height + this.Padding.Vertical;

            return new Size(retWidth, retHeight);
        }

        /// <summary>
        /// Throws an ObjectDisposedException if the control is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><c>ObjectDisposedException</c>.</exception>
        protected void ThrowIfDisposed()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.Name);
            }
        }

        /// <summary>
        /// Updates the ClientSize.
        /// </summary>
        protected void UpdateClientSize()
        {
            if (this.Disposing)
            {
                return;
            }

            this.ClientSize = this.ClientSizeFromSize(this.Size);
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected virtual void UpdateCore()
        {
            if (this.IsMouseOver && this.CanReactToInput && !string.IsNullOrEmpty(this.Cursor))
            {
                this.GUI.GUIManager.Cursor.ActiveMode = this.Cursor;
            }
        }

        #endregion Protected Methods

        #region Private Static Methods

        private static InputInjector CreateInputInjector(Control control)
        {
            return new InputInjector(control)
                   {
                       OnClick = control.OnClick,
                       OnDragDrop = control.OnDragDrop,
                       OnDragEnter = control.OnDragEnter,
                       OnDragLeave = control.OnDragLeave,
                       OnDragOver = control.OnDragOver,
                       OnKeyDown = control.OnKeyDown,
                       OnKeyHeld = control.OnKeyHeld,
                       OnKeyUp = control.OnKeyUp,
                       OnMouseClick = control.OnMouseClick,
                       OnMouseDoubleClick = control.OnMouseDoubleClick,
                       OnMouseDown = control.OnMouseDown,
                       OnMouseDrag = control.OnMouseDrag,
                       OnMouseEnter = control.OnMouseEnter,
                       OnMouseGesture = control.OnMouseGesture,
                       OnMouseHeld = control.OnMouseHeld,
                       OnMouseHover = control.OnMouseHover,
                       OnMouseLeave = control.OnMouseLeave,
                       OnMouseUp = control.OnMouseUp,
                       OnMouseWheelMoved = control.OnMouseWheelMoved
                   };
        }

        #endregion Private Static Methods

        #region Private Methods

        /// <summary>
        /// Takes a rectangle with the suggested position and size for the control, and returns a new one that takes into consideration all the applicable constraints for the control.
        /// </summary>
        /// <param name="suggestedX">The X value of the upper Left corner relative to the container control.</param>
        /// <param name="suggestedY">The Y value of the upper Left corner relative to the container control.</param>
        /// <param name="proposedWidth">The width in pixels.</param>
        /// <param name="proposedHeight">The height in pixels.</param>
        /// <returns>The new rectangle.</returns>
        private Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
        {
            Size size = this.ApplySizeConstraints(new Size(proposedWidth, proposedHeight));
            return new Rectangle(new Point(suggestedX, suggestedY), size);
        }

        private void CheckCanHoldFocus()
        {
            if (!this.CanReactToInput && this.Focused)
            {
                this.Focused = false;
            }
        }

        private void GetDefaultValues()
        {
            this.MaxSize = this.DefaultMaxSize;
            this.MinSize = this.DefaultMinSize;
            this.padding = this.DefaultPadding;
            this.margin = this.DefaultMargin;
        }

        private void ReleaseEvents()
        {
            foreach (var eventInfo in this.GetType().GetEvents(BindingFlags.Instance | BindingFlags.Public))
            {
                var type = eventInfo.DeclaringType;
                var field = type.GetField(eventInfo.Name, BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    var del = field.GetValue(this) as Delegate;
                    if (del != null)
                    {
                        foreach (var d in del.GetInvocationList())
                        {
                            eventInfo.RemoveEventHandler(this, d);
                        }
                    }
                }
            }
        }

        private void SetParent(Control newParent)
        {
            // remove control from the old collection
            var parentCollection = this.GetParentCollection();
            if (parentCollection != null)
            {
                parentCollection.Remove(this);
            }

            bool preVisible = this.Visible;
            bool preEnabled = this.Enabled;

            this.parent = newParent;

            if (newParent != null)
            {
                // Check if control has been added to children before
                if (!newParent.Controls.Contains(this))
                {
                    newParent.Controls.Add(this);
                }

                if (preVisible != this.Visible)
                {
                    this.OnVisibleChanged(EventArgs.Empty);
                }

                if (preEnabled != this.Enabled)
                {
                    this.OnEnabledChanged(EventArgs.Empty);
                }
            }

            this.ForceRedraw(true, false);
            this.OnParentChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Updates the position and size for the control.
        /// </summary>
        /// <param name="x">The X value of the upper Left corner relative to the container control.</param>
        /// <param name="y">The Y value of the upper Left corner relative to the container control.</param>
        /// <param name="newWidth">The width in pixels.</param>
        /// <param name="newHeight">The height in pixels.</param>
        private void UpdateBounds(int x, int y, int newWidth, int newHeight)
        {
            bool moved = false;
            bool resized = false;
            var oldLocation = Point.Empty;
            var oldSize = Size.Empty;

            if ((this.left != x) || (this.top != y))
            {
                moved = true;
                oldLocation = this.Location;
            }

            if ((this.width != newWidth) || (this.height != newHeight))
            {
                resized = true;
                oldSize = this.Size;
            }

            this.left = x;
            this.top = y;
            this.width = newWidth;
            this.height = newHeight;

            this.UpdateClientSize();

            if (moved)
            {
                this.OnLocationChanged(new ChangedValueEventArgs<Point>(oldLocation, this.Location));
                this.OnPropertyChanged("Location");
            }

            if (resized)
            {
                this.CalculateDeltaSize(oldSize, this.Size);
                this.PerformLayout(this, "Size");
                this.OnSizeChanged(new ChangedValueEventArgs<Size>(oldSize, this.Size));
                this.OnPropertyChanged("Size");
            }
        }

        /// <summary>
        /// Update the distance to the right and bottom of the client area of the parent container.
        /// </summary>
        private void UpdateDistances()
        {
            if (this.parent != null)
            {
                this.DistRight = this.parent.DisplayRectangle.Width - this.Left - this.Width;
                this.DistBottom = this.parent.DisplayRectangle.Height - this.Top - this.Height;

                this.RecalculateDistances = false;
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}