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
    using System.Collections.Generic;
    using System.Diagnostics;

    using Miyagi.Common;

    /// <summary>
    /// A custom collection of controls.
    /// </summary>
    public sealed class ControlCollection : MiyagiCollection<Control>
    {
        #region Fields

        private readonly List<Control> zorderList;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ControlCollection class.
        /// </summary>
        /// <param name="owner">The parent of the collection.</param>
        public ControlCollection(IControlCollectionOwner owner)
        {
            this.Owner = owner;
            this.zorderList = new List<Control>();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the owner of the ControlCollection.
        /// </summary>
        /// <value>An object representing the owner.</value>
        public IControlCollectionOwner Owner
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds a control to the collection.
        /// </summary>
        /// <param name="item">The new control.</param>
        public override void Add(Control item)
        {
            if (item == null)
            {
                return;
            }

            base.Add(item);
            this.zorderList.Add(item);
            this.SetupChild(item);
        }

        /// <summary>
        /// Brings the specified control to the front.
        /// </summary>
        /// <param name="control">The control that should be brought to the front.</param>
        public void BringToFront(Control control)
        {
            int newIndex = this.Count - 1;
            if (this.zorderList.IndexOf(control) != newIndex)
            {
                this.zorderList.Remove(control);
                this.zorderList.Add(control);
                this.Owner.EnsureZOrder();
            }
        }

        /// <summary>
        /// Inserts a control into the list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which control should be inserted.</param>
        /// <param name="item">The control to insert.</param>
        public override void Insert(int index, Control item)
        {
            base.Insert(index, item);
            this.zorderList.Insert(index, item);
            this.SetupChild(item);
        }

        /// <summary>
        /// Removes a control from the collection.
        /// </summary>
        /// <param name="item">The control to remove.</param>
        /// <returns><c>true</c> if the control has been removed sucessfully; otherwise, <c>false</c>.</returns>
        public override bool Remove(Control item)
        {
            bool success = base.Remove(item);

            if (success)
            {
                this.zorderList.Remove(item);
                this.Owner.NotifyControlRemoved(item);
            }

            return success;
        }

        /// <summary>
        /// Sends the specified control behind another specified control
        /// </summary>
        public void SendBehind(Control control, Control behindControl)
        {
            int initialZorderOfControl = this.zorderList.IndexOf(control);
            int initialZorderOfBehindControl = this.zorderList.IndexOf(behindControl);
            if (initialZorderOfBehindControl != initialZorderOfControl + 1)
            {
                this.zorderList.Remove(control);
                initialZorderOfBehindControl = this.zorderList.IndexOf(behindControl);
                this.zorderList.Insert(initialZorderOfBehindControl, control);
                this.Owner.EnsureZOrder();
            }
        }

        /// <summary>
        /// Sends the specified control to the back.
        /// </summary>
        /// <param name="control">The control that should be send to the back.</param>
        public void SendToBack(Control control)
        {
            if (this.zorderList.IndexOf(control) != 0)
            {
                this.zorderList.Remove(control);
                this.zorderList.Insert(0, control);
                this.Owner.EnsureZOrder();
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void EnsureZOrder(ref int startZOrder)
        {
            Debug.Assert(this.zorderList.Count == this.Count, "item count mismatch");

            for (int i = 0; i < this.Count; i++)
            {
                var c = this.Items[i];
                if (c.AlwaysOnTop)
                {
                    this.zorderList.Remove(c);
                    this.zorderList.Add(c);
                }
                else if (c.AlwaysOnBottom)
                {
                    this.zorderList.Remove(c);
                    this.zorderList.Insert(0, c);
                }
            }

            for (int i = 0; i < this.zorderList.Count; i++)
            {
                var c = this.zorderList[i];
                c.SetZOrder(startZOrder++);
                c.Controls.EnsureZOrder(ref startZOrder);
            }
        }

        internal Control GetControlByZOrder(int index)
        {
            return this.zorderList[index];
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Sets up the newly added child.
        /// </summary>
        /// <param name="item">The control added to the collection.</param>
        private void SetupChild(Control item)
        {
            this.Owner.NotifyControlAdded(item);
        }

        #endregion Private Methods

        #endregion Methods
    }
}