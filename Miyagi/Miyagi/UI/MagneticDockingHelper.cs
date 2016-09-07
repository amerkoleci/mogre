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
    using Miyagi.Internals;
    using Miyagi.UI.Controls;

    /// <summary>
    /// Helper class for magnetically docking.
    /// </summary>
    /// <typeparam name="T">The type of the magnetically dockable control.</typeparam>
    public sealed class MagneticDockingHelper<T>
        where T : Control, IMagneticDockable
    {
        #region Fields

        private readonly T owner;

        private int dockHeight;
        private int dockWidth;
        private Control horizontalControl;
        private Directions horizontalDirection;
        private Directions verticalDirection;
        private Control verticalPanel;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MagneticDockingHelper{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public MagneticDockingHelper(T owner)
        {
            this.owner = owner;
        }

        #endregion Constructors

        #region Enumerations

        private enum Directions
        {
            None = 0,
            Left = 1,
            Right = 2,
            Up = 4,
            Down = 8
        }

        #endregion Enumerations

        #region Methods

        #region Public Methods

        /// <summary>
        /// Recalculates the new coordinates.
        /// </summary>
        /// <param name="oldX">The old x-coordinate.</param>
        /// <param name="oldY">The old y-coordinate.</param>
        /// <param name="newX">The new x-coordinate.</param>
        /// <param name="newY">The new y-coordinate.</param>
        public void Do(int oldX, int oldY, ref int newX, ref int newY)
        {
            var threshold = this.owner.MagneticDockThreshold;

            int panelLeft = oldX;
            int panelRight = panelLeft + this.owner.Size.Width;
            int panelTop = oldY;
            int panelBottom = panelTop + this.owner.Size.Height;

            bool foundVertical = false;
            bool foundHorizontal = false;

            // get direction and check if screen border is hit
            var movement = Directions.None;

            // horizontal movement and screen border check
            if (newX != oldX)
            {
                movement |= newX < oldX ? Directions.Left : Directions.Right;

                if (!movement.IsFlagSet(this.horizontalDirection))
                {
                    this.horizontalControl = null;
                }

                if (this.owner.MagneticDockToScreenEdges)
                {
                    if (movement.IsFlagSet(Directions.Left))
                    {
                        foundHorizontal = DockScreenEdges(ref newX, oldX, panelLeft, panelRight, threshold.Left, this.owner.GUI.SpriteRenderer.Viewport.Size.Width);
                    }
                    else if (movement.IsFlagSet(Directions.Right))
                    {
                        foundHorizontal = DockScreenEdges(ref newX, oldX, panelLeft, panelRight, threshold.Right, this.owner.GUI.SpriteRenderer.Viewport.Size.Width);
                    }
                }
            }

            // vertical movement and screen border check
            if (newY != oldY)
            {
                movement |= newY < oldY ? Directions.Up : Directions.Down;

                if (!movement.IsFlagSet(this.verticalDirection))
                {
                    this.verticalPanel = null;
                }

                if (this.owner.MagneticDockToScreenEdges)
                {
                    if (movement.IsFlagSet(Directions.Up))
                    {
                        foundVertical = DockScreenEdges(ref newY, oldY, panelTop, panelBottom, threshold.Top, this.owner.GUI.SpriteRenderer.Viewport.Size.Height);
                    }
                    else if (movement.IsFlagSet(Directions.Down))
                    {
                        foundVertical = DockScreenEdges(ref newY, oldY, panelTop, panelBottom, threshold.Bottom, this.owner.GUI.SpriteRenderer.Viewport.Size.Height);
                    }
                }
            }

            // iterate through all controls
            foreach (var checkPanel in this.owner.GUI.GUIManager.AllControls)
            {
                if (checkPanel.IsMagneticallyDockingEnabled
                    && !this.owner.IsAncestor(checkPanel)
                    && !checkPanel.IsAncestor(this.owner)
                    && this.owner != checkPanel
                    && checkPanel.CanReactToInput)
                {
                    var otherPos = checkPanel.GetLocationInViewport();

                    int otherLeft = otherPos.X;
                    int otherRight = otherLeft + checkPanel.Size.Width;
                    int otherTop = otherPos.Y;
                    int otherBottom = otherTop + checkPanel.Size.Height;

                    // check if hitting horizontal
                    if (!foundHorizontal && panelTop < otherBottom && panelBottom > otherTop)
                    {
                        if (movement.IsFlagSet(Directions.Left))
                        {
                            foundHorizontal = this.CheckLowerSide(Directions.Left, ref newX, oldX, checkPanel, threshold.Left, panelLeft, otherRight);
                        }
                        else if (movement.IsFlagSet(Directions.Right))
                        {
                            foundHorizontal = this.CheckUpperSide(Directions.Right, ref newX, oldX, checkPanel, threshold.Right, panelRight, otherLeft);
                        }
                    }

                    // check if hitting vertical
                    if (!foundVertical && panelRight > otherLeft && panelLeft < otherRight)
                    {
                        if (movement.IsFlagSet(Directions.Up))
                        {
                            foundVertical = this.CheckLowerSide(Directions.Up, ref newY, oldY, checkPanel, threshold.Top, panelTop, otherBottom);
                        }
                        else if (movement.IsFlagSet(Directions.Down))
                        {
                            foundVertical = this.CheckUpperSide(Directions.Down, ref newY, oldY, checkPanel, threshold.Bottom, panelBottom, otherTop);
                        }
                    }

                    if (foundVertical && foundHorizontal)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.dockWidth = 0;
            this.horizontalControl = null;
            this.dockHeight = 0;
            this.verticalPanel = null;
        }

        #endregion Public Methods

        #region Private Static Methods

        private static bool DockScreenEdges(ref int newPos, int oldPos, int lowerSide, int upperSide, int magneticSize, int screenBorder)
        {
            bool retValue = false;

            if (newPos < oldPos && lowerSide - magneticSize < 0)
            {
                newPos = oldPos - lowerSide;
                retValue = true;
            }
            else if (newPos > oldPos && upperSide + magneticSize > screenBorder)
            {
                newPos = oldPos + (screenBorder - upperSide);
                retValue = true;
            }

            return retValue;
        }

        #endregion Private Static Methods

        #region Private Methods

        private bool CheckLowerSide(Directions newDir, ref int newPos, int oldPos, Control checkPanel, int checkMagSize, int ownSide, int otherSide)
        {
            var magPanel = newDir == Directions.Left ? this.horizontalControl : this.verticalPanel;
            int magSize = newDir == Directions.Left ? this.dockWidth : this.dockHeight;
            var magDir = newDir == Directions.Left ? this.horizontalDirection : this.verticalDirection;

            bool retValue = false;
            if (magPanel == null && ownSide - checkMagSize < otherSide && ownSide > otherSide)
            {
                newPos = oldPos - (ownSide - otherSide);
                retValue = true;

                magSize = 0;
                magPanel = checkPanel;
                magDir = newDir;
            }
            else if (checkPanel == magPanel)
            {
                if (magSize > checkMagSize)
                {
                    newPos -= checkMagSize;
                    retValue = true;

                    magPanel = null;
                }
                else
                {
                    magSize += oldPos - newPos;
                    newPos = oldPos - (ownSide - otherSide);
                }
            }

            switch (newDir)
            {
                case Directions.Left:
                    this.horizontalControl = magPanel;
                    this.dockWidth = magSize;
                    this.horizontalDirection = magDir;
                    break;
                case Directions.Up:
                    this.verticalPanel = magPanel;
                    this.dockHeight = magSize;
                    this.verticalDirection = magDir;
                    break;
            }

            return retValue;
        }

        private bool CheckUpperSide(Directions newDir, ref int newPos, int oldPos, Control checkPanel, int checkMagSize, int ownSide, int otherSide)
        {
            var magPanel = newDir == Directions.Right ? this.horizontalControl : this.verticalPanel;
            int magSize = newDir == Directions.Right ? this.dockWidth : this.dockHeight;
            var magDir = newDir == Directions.Right ? this.horizontalDirection : this.verticalDirection;

            bool retValue = false;
            if (magPanel == null && ownSide + checkMagSize > otherSide && ownSide < otherSide)
            {
                newPos = oldPos + (otherSide - ownSide);
                retValue = true;

                magSize = 0;
                magPanel = checkPanel;
                magDir = newDir;
            }
            else if (checkPanel == magPanel)
            {
                if (magSize > checkMagSize)
                {
                    newPos += checkMagSize;
                    retValue = true;

                    magPanel = null;
                }
                else
                {
                    magSize += newPos - oldPos;
                    newPos = oldPos + (otherSide - ownSide);
                }
            }

            switch (newDir)
            {
                case Directions.Right:
                    this.horizontalControl = magPanel;
                    this.dockWidth = magSize;
                    this.horizontalDirection = magDir;
                    break;
                case Directions.Down:
                    this.verticalPanel = magPanel;
                    this.dockHeight = magSize;
                    this.verticalDirection = magDir;
                    break;
            }

            return retValue;
        }

        #endregion Private Methods

        #endregion Methods
    }
}