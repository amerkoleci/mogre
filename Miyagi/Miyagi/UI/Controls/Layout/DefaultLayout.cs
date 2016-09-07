/*
//
// DefaultLayout.cs
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2006 Jonathan Pobst
//
// Authors:
//    Jonathan Pobst (monkey@jpobst.com)
//    Stefan Noack (noackstefan@googlemail.com)
//
 */
namespace Miyagi.UI.Controls.Layout
{
    using Miyagi.Common.Data;

    /// <summary>
    /// The Default layout engine.
    /// </summary>
    public class DefaultLayout : LayoutEngine
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Performs the layout for the given container. The default layout only deals with anchors and docking.
        /// </summary>
        /// <param name="parent">The control whose layout has to be done.</param>
        /// <param name="layoutEventArgs">The LayoutEventArgs.</param>
        /// <returns>Always false.</returns>
        public override bool Layout(Control parent, LayoutEventArgs layoutEventArgs)
        {
            LayoutDockedChildren(parent);
            LayoutAnchoredChildren(parent);
            LayoutAutoSizedChildren(parent);
            LayoutTopLevelControl(parent);

            return false;
        }

        #endregion Public Methods

        #region Private Static Methods

        /// <summary>
        /// Calculates the horizontal anchoring for a given control relative to a given space. This includes the left position and the width of the control.
        /// </summary>
        /// <param name="space">The rectangle where the control lays.</param>
        /// <param name="control">The control that needs anchoring.</param>
        /// <param name="anchor">The type of anchor for the control.</param>
        /// <param name="left">Initially stores the left position of the control, it is modified according to the anchor.</param>
        /// <param name="width">Initially stores the width of the control, is is modified according to the anchor.</param>
        /// <remarks>It can be that the value saved for the distance to the right inside the control is changed too.</remarks>
        private static void HorizontalAnchor(Rectangle space, Control control, AnchorStyles anchor, ref int left, ref int width)
        {
            if ((anchor & AnchorStyles.HorizontalCenter) == AnchorStyles.HorizontalCenter)
            {
                left = (space.Width - width) / 2;
            }
            else if ((anchor & AnchorStyles.Right) == AnchorStyles.Right)
            {
                if ((anchor & AnchorStyles.Left) == AnchorStyles.Left)
                {
                    width = space.Width - control.DistRight - left;
                }
                else
                {
                    left = space.Width - control.DistRight - width;
                }
            }
            else if ((anchor & AnchorStyles.Left) != AnchorStyles.Left)
            {
                left += (space.Width - (left + width + control.DistRight)) / 2;
                control.DistRight = space.Width - (left + width);
            }
        }

        /// <summary>
        /// Does the layout for anchored children, which are the ones that preserve the distance to a set of specified edges.
        /// </summary>
        /// <param name="parent">The parent control.</param>
        private static void LayoutAnchoredChildren(Control parent)
        {
            var space = new Rectangle(Point.Empty, parent.DisplayRectangle.Size);

            foreach (Control child in parent.Controls)
            {
                if (!child.Visible || child.LayoutType == LayoutType.Dock)
                {
                    continue;
                }

                AnchorStyles anchor = child.Anchor;

                int left = child.Location.X;
                int top = child.Location.Y;

                int width = child.Width;
                int height = child.Height;

                HorizontalAnchor(space, child, anchor, ref left, ref width);
                VerticalAnchor(space, child, anchor, ref top, ref height);

                // Sanity
                if (width < 0)
                {
                    width = 0;
                }

                if (height < 0)
                {
                    height = 0;
                }

                child.SetBounds(left, top, width, height, BoundsSpecified.None);
            }
        }

        /// <summary>
        /// Adjusts the size of children with the AutoSize property, except if they are docked.
        /// </summary>
        /// <param name="parent">The parent control whose children are to be adjusted.</param>
        private static void LayoutAutoSizedChildren(IControlCollectionOwner parent)
        {
            foreach (Control child in parent.Controls)
            {
                if (!child.Visible || child.LayoutType == LayoutType.Dock || !child.AutoSize)
                {
                    continue;
                }

                AnchorStyles anchor = child.Anchor;
                Size preferredsize = child.GetPreferredSize(child.Size);

                if (((anchor & AnchorStyles.Left) != 0) || ((anchor & AnchorStyles.Right) == 0))
                {
                    child.DistRight += child.Width - preferredsize.Width;
                }

                if (((anchor & AnchorStyles.Top) != 0) || ((anchor & AnchorStyles.Bottom) == 0))
                {
                    child.DistBottom += child.Height - preferredsize.Height;
                }

                child.SetBounds(child.Left, child.Top, preferredsize.Width, preferredsize.Height, BoundsSpecified.None);
            }
        }

        /// <summary>
        /// Does the layout for docked children, which are children that can grow to fit a specified edge, or take all the remaining space.
        /// </summary>
        /// <param name="parent">The parent control.</param>
        private static void LayoutDockedChildren(Control parent)
        {
            var space = new Rectangle(Point.Empty, parent.DisplayRectangle.Size);

            // Deal with docking; go through in reverse
            foreach (Control child in parent.Controls.GetReverseEnumerator())
            {
                if (!child.Visible || child.LayoutType == LayoutType.Anchor)
                {
                    continue;
                }

                Size preferredSize = child.AutoSize ? child.GetPreferredSize(child.Size) : child.Size;

                int left = space.Left + child.Margin.Left;
                int top = space.Top + child.Margin.Top;
                int right = space.Right - child.Margin.Right;
                int bottom = space.Bottom - child.Margin.Bottom;

                int width = space.Width - child.Margin.Horizontal;
                int height = space.Height - child.Margin.Vertical;

                switch (child.Dock)
                {
                    case DockStyle.None:
                        break;

                    case DockStyle.Left:
                        child.SetBounds(left, top, preferredSize.Width, height, BoundsSpecified.None);
                        space = Rectangle.FromLTRB(left + preferredSize.Width, top, right, bottom);
                        break;

                    case DockStyle.Top:
                        child.SetBounds(left, top, width, preferredSize.Height, BoundsSpecified.None);
                        space = Rectangle.FromLTRB(left, top + preferredSize.Height, right, bottom);
                        break;

                    case DockStyle.Right:
                        child.SetBounds(right - preferredSize.Width, top, preferredSize.Width, height, BoundsSpecified.None);
                        space = Rectangle.FromLTRB(left, top, right - preferredSize.Width, bottom);
                        break;

                    case DockStyle.Bottom:
                        child.SetBounds(left, bottom - preferredSize.Height, width, preferredSize.Height, BoundsSpecified.None);
                        space = Rectangle.FromLTRB(left, top, right, bottom - preferredSize.Height);
                        break;

                    case DockStyle.Fill:
                        child.SetBounds(left, top, width, height, BoundsSpecified.None);
                        space = Rectangle.Empty;
                        break;
                }
            }
        }

        /// <summary>
        /// Adjusts the size of the parent control if it has the AutoSize property and it is a top level control.
        /// </summary>
        /// <param name="control">The top level control.</param>
        private static void LayoutTopLevelControl(Control control)
        {
            if (!control.IsTopLevelControl)
            {
                return;
            }

            if (!control.Visible || control.LayoutType == LayoutType.Dock || !control.AutoSize)
            {
                return;
            }

            Size preferredsize = control.GetPreferredSize(control.Size);

            control.SetBounds(control.Left, control.Top, preferredsize.Width, preferredsize.Height, BoundsSpecified.None);
        }

        /// <summary>
        /// Calculates the vertical anchoring for a given control relative to a given space. This includes the top position and the height of the control.
        /// </summary>
        /// <param name="space">The rectangle where the control lays.</param>
        /// <param name="control">The control that needs anchoring.</param>
        /// <param name="anchor">The type of anchor for the control.</param>
        /// <param name="top">Initially stores the top position of the control, it is modified according to the anchor.</param>
        /// <param name="height">Initially stores the height of the control, is is modified according to the anchor.</param>
        /// <remarks>It can be that the value saved for the distance to the bottom inside the control is changed too.</remarks>
        private static void VerticalAnchor(Rectangle space, Control control, AnchorStyles anchor, ref int top, ref int height)
        {
            if ((anchor & AnchorStyles.VerticalCenter) == AnchorStyles.VerticalCenter)
            {
                top = (space.Height - height) / 2;
            }
            else if ((anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
            {
                if ((anchor & AnchorStyles.Top) == AnchorStyles.Top)
                {
                    height = space.Height - control.DistBottom - top;
                }
                else
                {
                    top = space.Height - control.DistBottom - height;
                }
            }
            else if ((anchor & AnchorStyles.Top) != AnchorStyles.Top)
            {
                top += (space.Height - (top + height + control.DistBottom)) / 2;
                control.DistBottom = space.Height - (top + height);
            }
        }

        #endregion Private Static Methods

        #endregion Methods
    }
}