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

    using Miyagi.UI.Controls.Styles;

    #region Enumerations

    /// <summary>
    /// Specifies the AnchorStyle.
    /// </summary>
    [Flags]
    public enum AnchorStyles
    {
        /// <summary>
        /// The object is not anchored.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Left anchor.
        /// </summary>
        Left = 0x00000001,

        /// <summary>
        /// Top anchor.
        /// </summary>
        Top = 0x00000002,

        /// <summary>
        /// Right anchor.
        /// </summary>
        Right = 0x00000004,

        /// <summary>
        /// Bottom anchor.
        /// </summary>
        Bottom = 0x00000008,

        /// <summary>
        /// Top and Bottom alignment.
        /// </summary>
        Vertical = Top | Bottom,

        /// <summary>
        /// Left and Right alignment.
        /// </summary>
        Horizontal = Left | Right,

        /// <summary>
        /// Vertical and Horizontal alignment.
        /// </summary>
        All = Vertical | Horizontal,

        /// <summary>
        /// Horizontally centered alignment.
        /// </summary>
        HorizontalCenter = 0x00000010,

        /// <summary>
        /// Vertically centered alignment.
        /// </summary>
        VerticalCenter = 0x00000020,

        /// <summary>
        /// Center alignment.
        /// </summary>
        Center = HorizontalCenter | VerticalCenter
    }

    /// <summary>
    /// Specifies how a control will behave when its AutoSize property is enabled.
    /// </summary>
    public enum AutoSizeMode
    {
        /// <summary>
        /// The control grows or shrinks to fit its contents. The control cannot be resized manually.
        /// </summary>
        GrowAndShrink,

        /// <summary>
        /// The control grows as much as necessary to fit its contents but does not shrink smaller than the value of its Size property. The form can be resized, but cannot be made so small that any of its contained controls are hidden.
        /// </summary>
        GrowOnly
    }

    /// <summary>
    /// Specifies the bounds of the control to use when defining a control's size and position.
    /// </summary>
    [Flags]
    public enum BoundsSpecified
    {
        /// <summary>
        /// No bounds are specified.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The left edge of the control is defined.
        /// </summary>
        X = 0x00000001,

        /// <summary>
        /// The top edge of the control is defined.
        /// </summary>
        Y = 0x00000002,

        /// <summary>
        /// Both X and Y coordinates of the control are defined.
        /// </summary>
        Location = X | Y,

        /// <summary>
        /// The width of the control is defined.
        /// </summary>
        Width = 0x00000004,

        /// <summary>
        /// The height of the control is defined.
        /// </summary>
        Height = 0x00000008,

        /// <summary>
        /// Both Width and Height property values of the control are defined.
        /// </summary>
        Size = Width | Height,

        /// <summary>
        /// Both Point and Size property values are defined.
        /// </summary>
        All = Location | Size
    }

    /// <summary>
    /// Specifies the state of a CheckBox.
    /// </summary>
    public enum CheckState
    {
        /// <summary>
        /// The CheckBox is unchecked.
        /// </summary>
        Unchecked,

        /// <summary>
        /// The CheckBox is checked.
        /// </summary>
        Checked,

        /// <summary>
        /// The CheckBox is in an indeterminate state.
        /// </summary>
        Indeterminate
    }

    /// <summary>
    /// Specifies the mode of the cursor.
    /// </summary>
    public enum CursorMode
    {
        /// <summary>
        /// The name of the BlockDrop cursor.
        /// </summary>
        BlockDrop,

        /// <summary>
        /// The name of the main cursor.
        /// </summary>
        Main,

        /// <summary>
        /// The name of the ResizeBottom cursor.
        /// </summary>
        ResizeBottom,

        /// <summary>
        /// The name of the ResizeBottomLeft cursor.
        /// </summary>
        ResizeBottomLeft,

        /// <summary>
        /// The name of the ResizeBottomRight cursor.
        /// </summary>
        ResizeBottomRight,

        /// <summary>
        /// The name of the ResizeLeft cursor.
        /// </summary>
        ResizeLeft,

        /// <summary>
        /// The name of the ResizeRight cursor.
        /// </summary>
        ResizeRight,

        /// <summary>
        /// The name of the ResizeTop cursor.
        /// </summary>
        ResizeTop,

        /// <summary>
        /// The name of the ResizeTopLeft cursor.
        /// </summary>
        ResizeTopLeft,

        /// <summary>
        /// The name of the ResizeTopRight cursor.
        /// </summary>
        ResizeTopRight,

        /// <summary>
        /// The name of the TextInput cursor.
        /// </summary>
        TextInput
    }

    /// <summary>
    /// Specifies the which buttons a dialog box uses.
    /// </summary>
    [Flags]
    public enum DialogBoxButtons
    {
        /// <summary>
        /// No Buttons.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The Ok Button.
        /// </summary>
        Ok = 0x00000001,

        /// <summary>
        /// The Cancel Button.
        /// </summary>
        Cancel = 0x00000002,

        /// <summary>
        /// Ok and Cancel Buttons.
        /// </summary>
        OkCancel = Ok | Cancel,

        /// <summary>
        /// The Yes Button.
        /// </summary>
        Yes = 0x00000004,

        /// <summary>
        /// The No Button.
        /// </summary>
        No = 0x00000008,

        /// <summary>
        /// Yes and No Buttons.
        /// </summary>
        YesNo = Yes | No,

        /// <summary>
        /// Yes, No and Cancel Buttons.
        /// </summary>
        YesNoCancel = YesNo | Cancel
    }

    /// <summary>
    /// Specifies the result of a modal dialog.
    /// </summary>
    public enum DialogResult
    {
        /// <summary>
        /// No result.
        /// </summary>
        None,

        /// <summary>
        /// The result is Okay.
        /// </summary>
        Ok,

        /// <summary>
        /// The result is Cancel.
        /// </summary>
        Cancel,

        /// <summary>
        /// The result is Yes.
        /// </summary>
        Yes,

        /// <summary>
        /// The result is No.
        /// </summary>
        No
    }

    /// <summary>
    /// Specifies the position and manner in which a control is docked.
    /// </summary>
    public enum DockStyle
    {
        /// <summary>
        /// The control is not docked.
        /// </summary>
        None,

        /// <summary>
        /// The control's top edge is docked to the top of its containing control.
        /// </summary>
        Top,

        /// <summary>
        /// The control's bottom edge is docked to the bottom of its containing control.
        /// </summary>
        Bottom,

        /// <summary>
        /// The control's left edge is docked to the left edge of its containing control.
        /// </summary>
        Left,

        /// <summary>
        /// The control's right edge is docked to the right edge of its containing control.
        /// </summary>
        Right,

        /// <summary>
        /// All the control's edges are docked to the all edges of its containing control and sized appropriately.
        /// </summary>
        Fill
    }

    /// <summary>
    /// Defines constants that specify the direction in which consecutive user interface (UI) elements are placed in a linear layout container.
    /// </summary>
    public enum FlowDirection
    {
        /// <summary>
        /// Elements flow from the left edge of the design surface to the right.
        /// </summary>
        LeftToRight,

        /// <summary>
        /// Elements flow from the top of the design surface to the bottom.
        /// </summary>
        TopDown,

        /// <summary>
        /// Elements flow from the right edge of the design surface to the left.
        /// </summary>
        RightToLeft,

        /// <summary>
        /// Elements flow from the bottom of the design surface to the top.
        /// </summary>
        BottomUp
    }

    /// <summary>
    /// Types of layouts for a control.
    /// </summary>
    public enum LayoutType
    {
        /// <summary>
        /// Preserves the distance between the edge of a control and the adjacent edge of a control's container.
        /// </summary>
        Anchor,

        /// <summary>
        ///  Allows a control to stick itself to an edge of a control's container, or to grow using free space.
        /// </summary>
        Dock
    }

    /// <summary>
    /// Specifies the mode of a ProgressBarElement.
    /// </summary>
    public enum ProgressBarMode
    {
        /// <summary>
        /// Indicates progress through blocks.
        /// </summary>
        Blocks,

        /// <summary>
        /// Indicates progress through a continuous bar.
        /// </summary>
        Continuous,

        /// <summary>
        /// Indicates progress through continuously scrolling.
        /// </summary>
        Marquee
    }

    /// <summary>
    /// Specifies the orientation of a control.
    /// </summary>
    [Flags]
    public enum ResizeModes
    {
        /// <summary>
        /// Not resizable.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Horizontally resizable.
        /// </summary>
        Horizontal = 0x00000001,

        /// <summary>
        /// Vertically resizable.
        /// </summary>
        Vertical = 0x00000002,

        /// <summary>
        /// Diagonally resizable.
        /// </summary>
        Diagonal = 0x00000004,

        /// <summary>
        /// Fully resizable.
        /// </summary>
        All = Horizontal | Vertical | Diagonal
    }

    /// <summary>
    /// Specifies how rows or columns of user interface elements should be sized relative to their container.
    /// </summary>
    public enum SizeType
    {
        /// <summary>
        /// The row or column should be sized to an exact number of pixels.
        /// </summary>
        Absolute,

        /// <summary>
        /// The row or column should be automatically sized to share space with its peers.
        /// </summary>
        AutoSize,

        /// <summary>
        /// The row or column should be sized as a percentage of the parent container.
        /// </summary>
        Percent
    }

    /// <summary>
    /// Specifies how a TabBar draws the tabs.
    /// </summary>
    public enum TabMode
    {
        /// <summary>
        /// The tabs are spread equally along the TabBar.
        /// </summary>
        Fill,

        /// <summary>
        /// The tabs are resized to fit their contents.
        /// </summary>
        AutoSize,

        /// <summary>
        /// The value specified in <see cref="TabStyle.FixedSize"/> is used.
        /// </summary>
        FixedSize
    }

    /// <summary>
    /// Specifies which updates have to be done.
    /// </summary>
    [Flags]
    public enum UpdateTypes
    {
        /// <summary>
        /// No update is required.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The Colour requires an update.
        /// </summary>
        Colour = 0x00000001,

        /// <summary>
        /// The Opacity requires an update.
        /// </summary>
        Opacity = 0x00000004,

        /// <summary>
        /// Sprite crop has to be updated.
        /// </summary>
        SpriteCrop = 0x00000008,

        /// <summary>
        /// The Location requires an update.
        /// </summary>
        Location = 0x00000010,

        /// <summary>
        /// The SelectedText requires an update.
        /// </summary>
        SelectedText = 0x00000020,

        /// <summary>
        /// The Size requires an update.
        /// </summary>
        Size = 0x00000040,

        /// <summary>
        /// Text has to be recreated.
        /// </summary>
        Text = 0x00000080,

        /// <summary>
        /// The Texture requires an update.
        /// </summary>
        Texture = 0x00000100,

        /// <summary>
        /// The TextureFiltering requires an update.
        /// </summary>
        TextureFiltering = 0x00000200,

        /// <summary>
        /// Visibility requires an update.
        /// </summary>
        Visibility = 0x00001000,

        /// <summary>
        /// The ZOrder requires an update.
        /// </summary>
        ZOrder = 0x00002000,

        /// <summary>
        /// The OwnerSize requires an update.
        /// </summary>
        OwnerSize = 0x00004000,

        /// <summary>
        /// The OwnerLocation requires an update.
        /// </summary>
        OwnerLocation = 0x00008000
    }

    #endregion Enumerations
}