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

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.UI.Controls;

    internal class InputInjector
    {
        #region Fields

        private readonly Control control;

        #endregion Fields

        #region Constructors

        public InputInjector(Control control)
        {
            this.control = control;
        }

        #endregion Constructors

        #region Delegates

        public delegate void DragDelegate(DragEventArgs e);

        public delegate void EventArgsDelegate(EventArgs e);

        public delegate void KeyDelegate(KeyboardEventArgs e);

        public delegate void MouseButtonDelegate(MouseButtonEventArgs e);

        public delegate void MouseDelegate(MouseEventArgs e);

        public delegate void MouseDragDelegate(ChangedValueEventArgs<Point> e);

        public delegate void MouseGestureDelegate(MouseGestureEventArgs e);

        public delegate bool SubmitDelegate<T>(ValueEventArgs<T> e);

        #endregion Delegates

        #region Properties

        #region Public Properties

        public EventArgsDelegate OnClick
        {
            get;
            set;
        }

        public DragDelegate OnDragDrop
        {
            get;
            set;
        }

        public DragDelegate OnDragEnter
        {
            get;
            set;
        }

        public DragDelegate OnDragLeave
        {
            get;
            set;
        }

        public DragDelegate OnDragOver
        {
            get;
            set;
        }

        public KeyDelegate OnKeyDown
        {
            get;
            set;
        }

        public KeyDelegate OnKeyHeld
        {
            get;
            set;
        }

        public KeyDelegate OnKeyUp
        {
            get;
            set;
        }

        public MouseButtonDelegate OnMouseClick
        {
            get;
            set;
        }

        public MouseButtonDelegate OnMouseDoubleClick
        {
            get;
            set;
        }

        public MouseButtonDelegate OnMouseDown
        {
            get;
            set;
        }

        public MouseDragDelegate OnMouseDrag
        {
            get;
            set;
        }

        public MouseDelegate OnMouseEnter
        {
            get;
            set;
        }

        public MouseGestureDelegate OnMouseGesture
        {
            get;
            set;
        }

        public MouseButtonDelegate OnMouseHeld
        {
            get;
            set;
        }

        public MouseDelegate OnMouseHover
        {
            get;
            set;
        }

        public MouseDelegate OnMouseLeave
        {
            get;
            set;
        }

        public MouseButtonDelegate OnMouseUp
        {
            get;
            set;
        }

        public SubmitDelegate<int> OnMouseWheelMoved
        {
            get;
            set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        public void InjectDragDrop()
        {
            var e = this.control.MiyagiSystem.GUIManager.CurrentDragEventArgs;
            if (e != null && this.control.CanReactToInput && this.control.AllowDrop)
            {
                this.OnDragDrop(e);
            }
        }

        public void InjectDragEnter()
        {
            var e = this.control.MiyagiSystem.GUIManager.CurrentDragEventArgs;
            if (e != null && this.control.CanReactToInput && this.control.AllowDrop)
            {
                this.OnDragEnter(e);
            }
        }

        public void InjectDragLeave()
        {
            var e = this.control.MiyagiSystem.GUIManager.CurrentDragEventArgs;
            if (e != null && this.control.CanReactToInput && this.control.AllowDrop)
            {
                this.OnDragLeave(e);
            }
        }

        public void InjectDragOver()
        {
            var e = this.control.MiyagiSystem.GUIManager.CurrentDragEventArgs;
            if (e != null && this.control.CanReactToInput && this.control.AllowDrop)
            {
                this.OnDragOver(e);
            }
        }

        public void InjectKeyDown(KeyEvent arg)
        {
            if (this.control.CanReactToInput)
            {
                this.OnKeyDown(new KeyboardEventArgs(arg));
                if (arg.Key == ConsoleKey.Enter)
                {
                    this.OnClick(new EventArgs());
                }
            }
        }

        public void InjectKeyHeld(KeyEvent arg)
        {
            if (this.control.CanReactToInput)
            {
                this.OnKeyDown(new KeyboardEventArgs(arg));
            }
        }

        public void InjectKeyUp(KeyEvent arg)
        {
            if (this.control.CanReactToInput)
            {
                this.OnKeyUp(new KeyboardEventArgs(arg));
            }
        }

        public void InjectMouseClick(MouseButton mb)
        {
            if (this.control.CanReactToInput)
            {
                this.OnMouseClick(new MouseButtonEventArgs(mb, this.GetMouseLocation()));
                this.OnClick(new EventArgs());
            }
        }

        public void InjectMouseDoubleClick(MouseButton mb)
        {
            if (this.control.CanReactToInput)
            {
                this.OnMouseDoubleClick(new MouseButtonEventArgs(mb, this.GetMouseLocation()));
            }
        }

        public void InjectMouseDown(MouseButton mb)
        {
            if (this.control.CanReactToInput)
            {
                this.OnMouseDown(new MouseButtonEventArgs(mb, this.GetMouseLocation()));
            }
        }

        public void InjectMouseDrag(Point oldLocation, Point newLocation)
        {
            if (this.control.CanReactToInput)
            {
                oldLocation = this.OffsetLocation(oldLocation);
                newLocation = this.OffsetLocation(newLocation);
                this.OnMouseDrag(new ChangedValueEventArgs<Point>(oldLocation, newLocation));
            }
        }

        public void InjectMouseEnter()
        {
            if (this.control.CanReactToInput)
            {
                this.OnMouseEnter(new MouseEventArgs(this.GetMouseLocation()));
            }
        }

        public void InjectMouseGesture(MouseGestureEventArgs e)
        {
            if (this.control.CanReactToInput)
            {
                this.OnMouseGesture(e);
            }
        }

        public void InjectMouseHeld(MouseButton mb)
        {
            if (this.control.CanReactToInput)
            {
                this.OnMouseHeld(new MouseButtonEventArgs(mb, this.GetMouseLocation()));
            }
        }

        public void InjectMouseHover()
        {
            if (this.control.CanReactToInput)
            {
                this.OnMouseHover(new MouseEventArgs(this.GetMouseLocation()));
            }
        }

        public void InjectMouseLeave()
        {
            if (this.control.CanReactToInput)
            {
                this.OnMouseLeave(new MouseEventArgs(this.GetMouseLocation()));
            }
        }

        public void InjectMouseUp(MouseButton mb)
        {
            if (this.control.CanReactToInput)
            {
                this.OnMouseUp(new MouseButtonEventArgs(mb, this.GetMouseLocation()));
            }
        }

        public bool InjectMouseWheelMoved(int z)
        {
            if (this.control.CanReactToInput)
            {
                return this.OnMouseWheelMoved(new ValueEventArgs<int>(z));
            }
            return false;
        }

        #endregion Public Methods

        #region Private Methods

        private Point GetMouseLocation()
        {
            return this.OffsetLocation(this.control.MiyagiSystem.InputManager.MouseLocation);
        }

        private Point OffsetLocation(Point loc)
        {
            int x = loc.X;
            int y = loc.Y;
            this.control.GUI.SpriteRenderer.TransformCoordinate(ref x, ref y);
            return new Point(x, y);
        }

        #endregion Private Methods

        #endregion Methods
    }
}