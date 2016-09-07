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

    #region Enumerations

    /// <summary>
    /// Specifies the alignment of content.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// No alignment.
        /// </summary>
        None,

        /// <summary>
        /// Left alignment.
        /// </summary>
        MiddleLeft,

        /// <summary>
        /// Right alignment.
        /// </summary>
        MiddleRight,

        /// <summary>
        /// Center alignment.
        /// </summary>
        MiddleCenter,

        /// <summary>
        /// Top left alignment.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Top center alignment.
        /// </summary>
        TopCenter,

        /// <summary>
        /// Top right alignment.
        /// </summary>
        TopRight,

        /// <summary>
        /// Bottom left alignment.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Bottom center alignment.
        /// </summary>
        BottomCenter,

        /// <summary>
        /// Bottom right alignment.
        /// </summary>
        BottomRight
    }

    /// <summary>
    /// Specifies the texture animation mode.
    /// </summary>
    public enum FrameAnimationMode
    {
        /// <summary>
        /// Frames are animated backwards once.
        /// </summary>
        BackwardOnce,

        /// <summary>
        /// Frames are looped backwards.
        /// </summary>
        BackwardLoop,

        /// <summary>
        /// Frames are animated forwards and backwards once.
        /// </summary>
        ForwardBackwardOnce,

        /// <summary>
        /// Frames are looped forwards and backwards.
        /// </summary>
        ForwardBackwardLoop,

        /// <summary>
        /// Frames are animated forwards once.
        /// </summary>
        ForwardOnce,

        /// <summary>
        /// Frames are looped forwards.
        /// </summary>
        ForwardLoop,
    }

    /// <summary>
    /// Gpu program types.
    /// </summary>
    public enum GpuProgramType
    {
        /// <summary>
        /// Fragment gpu program.
        /// </summary>
        Fragment,

        /// <summary>
        /// Vertex gpu program.
        /// </summary>
        Vertex
    }

    /// <summary>
    /// Logger filter level.
    /// </summary>
    public enum LoggerLevel
    {
        /// <summary>
        /// No message is logged.
        /// </summary>
        None,

        /// <summary>
        /// Every message is logged.
        /// </summary>
        Debug,

        /// <summary>
        /// Messages with Information severity and higher are logged.
        /// </summary>
        Information,

        /// <summary>
        /// Messages with Warning severity and higher are logged.
        /// </summary>
        Warning,

        /// <summary>
        /// Only messages with Error severity are logged.
        /// </summary>
        Error
    }

    /// <summary>
    /// Specifies the severity of a log message.
    /// </summary>
    public enum LogSeverity
    {
        /// <summary>
        /// Debug severity.
        /// </summary>
        Debug,

        /// <summary>
        /// Information severity.
        /// </summary>
        Information,

        /// <summary>
        /// Warning severity.
        /// </summary>
        Warning,

        /// <summary>
        /// Error severity.
        /// </summary>
        Error
    }

    /// <summary>
    /// Specifies a mouse button.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// The left mouse button.
        /// </summary>
        Left,

        /// <summary>
        /// The middle mouse button.
        /// </summary>
        Middle,

        /// <summary>
        /// The right mouse button.
        /// </summary>
        Right
    }

    /// <summary>
    /// Mouse gestures.
    /// </summary>
    [Flags]
    public enum MouseGestures
    {
        /// <summary>
        /// No mouse gesture.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Up mouse gesture.
        /// </summary>
        Up = 0x00000001,

        /// <summary>
        /// Down mouse gesture.
        /// </summary>
        Down = 0x00000002,

        /// <summary>
        /// Right mouse gesture.
        /// </summary>
        Right = 0x00000004,

        /// <summary>
        /// Left mouse gesture.
        /// </summary>
        Left = 0x00000008,

        /// <summary>
        /// UpLeft mouse gesture.
        /// </summary>
        UpLeft = Up | Left,

        /// <summary>
        /// UpRight mouse gesture.
        /// </summary>
        UpRight = Up | Right,

        /// <summary>
        /// DownLeft mouse gesture.
        /// </summary>
        DownLeft = Down | Left,

        /// <summary>
        /// DownRight mouse gesture.
        /// </summary>
        DownRight = Down | Right
    }

    /// <summary>
    /// Specifies the orientation of a control.
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// Horizontal orientation.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Vertical orientation.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Specifies the type of a primitive.
    /// </summary>
    public enum PrimitiveType
    {
        /// <summary>
        /// A triangle primitive.
        /// </summary>
        Triangle,

        /// <summary>
        /// A quad primitive.
        /// </summary>
        Quad,

        /// <summary>
        /// A custom primitive.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Specifies the progression.
    /// </summary>
    public enum Progression
    {
        /// <summary>
        /// Constant progression.
        /// </summary>
        Constant,

        /// <summary>
        /// Increasing progression.
        /// </summary>
        Increasing,

        /// <summary>
        /// Decreasing progression.
        /// </summary>
        Decreasing
    }

    /// <summary>
    /// Specifies the type of action used to raise the Scroll event.
    /// </summary>
    public enum ScrollEventType
    {
        /// <summary>
        /// The scroll box was moved a small distance. The user clicked the left(horizontal) or top(vertical) scroll arrow, or pressed the UP ARROW key.
        /// </summary>
        SmallDecrement,

        /// <summary>
        /// The scroll box was moved a small distance. The user clicked the right(horizontal) or bottom(vertical) scroll arrow, or pressed the DOWN ARROW key.
        /// </summary>
        SmallIncrement,

        /// <summary>
        /// The scroll box moved a large distance. The user clicked the scroll bar to the left(horizontal) or above(vertical) the scroll box, or pressed the PAGE UP key.
        /// </summary>
        LargeDecrement,

        /// <summary>
        /// The scroll box moved a large distance. The user clicked the scroll bar to the right(horizontal) or below(vertical) the scroll box, or pressed the PAGE DOWN key.
        /// </summary>
        LargeIncrement,

        /// <summary>
        /// The scroll box was moved.
        /// </summary>
        ThumbLocation,

        /// <summary>
        /// The scroll box is currently being moved.
        /// </summary>
        ThumbTrack,

        /// <summary>
        /// The scroll box was moved to the Minimum position.
        /// </summary>
        First,

        /// <summary>
        /// The scroll box was moved to the Maximum position.
        /// </summary>
        Last,

        /// <summary>
        /// The scroll box has stopped moving.
        /// </summary>
        EndScroll
    }

    /// <summary>
    /// Skin changing events.
    /// </summary>
    public enum SkinChangingEvent
    {
        /// <summary>
        /// Occurs when a mouse button is pressed over a control.
        /// </summary>
        MouseDown,

        /// <summary>
        /// Occurs when a mouse button is released over a control.
        /// </summary>
        MouseUp,

        /// <summary>
        /// Occurs when the mouse cursor enters a control.
        /// </summary>
        MouseEnter,

        /// <summary>
        /// Occurs when the mouse cursor leaves a control.
        /// </summary>
        MouseLeave
    }

    /// <summary>
    /// Specifies the texture filtering.
    /// </summary>
    public enum TextureFiltering
    {
        /// <summary>
        /// No texture filtering.
        /// </summary>
        None,

        /// <summary>
        /// Anisotropic texture filtering.
        /// </summary>
        Anisotropic,

        /// <summary>
        /// Point texture filtering.
        /// </summary>
        Point,

        /// <summary>
        /// Linear texture filtering.
        /// </summary>
        Linear
    }

    #endregion Enumerations
}