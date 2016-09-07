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
namespace Miyagi.Common.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Miyagi.Common.Data;
    using Miyagi.Common.Serialization;
    using Miyagi.Internals;

    /// <summary>
    /// A Texture represents different TextureFrames.
    /// </summary>
    [SerializableType]
    public sealed class Texture : IDeepCopiable<Texture>, IXmlWritable
    {
        #region Fields

        private readonly List<TextureFrame> frames;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Texture class.
        /// </summary>
        public Texture()
        {
            this.frames = new List<TextureFrame>();
            this.GpuPrograms = new List<GpuProgram>();
        }

        /// <summary>
        /// Initializes a new instance of the Texture class.
        /// </summary>
        /// <param name="fileName">The filename of the texture.</param>
        public Texture(string fileName)
            : this(fileName, RectangleF.FromLTRB(0, 0, 1, 1))
        {
        }

        /// <summary>
        /// Initializes a new instance of the Texture class.
        /// </summary>
        /// <param name="fileName">The filename of the texture.</param>
        /// <param name="uvCoordinates">A <see cref="RectangleF"/> representing the uv coordinates.</param>
        public Texture(string fileName, RectangleF uvCoordinates)
            : this(new TextureFrame(fileName, uvCoordinates, 0))
        {
        }

        /// <summary>
        /// Initializes a new instance of the Texture class.
        /// </summary>
        /// <param name="frames">The frames of the texture.</param>
        public Texture(params TextureFrame[] frames)
            : this(FrameAnimationMode.ForwardOnce, frames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Texture class.
        /// </summary>
        /// <param name="animationMode">A <see cref="FrameAnimationMode"/> representing the way the frames should be animated.</param>
        /// <param name="frames">The frames of the texture.</param>
        public Texture(FrameAnimationMode animationMode, params TextureFrame[] frames)
            : this(animationMode, (IEnumerable<TextureFrame>)frames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Texture class.
        /// </summary>
        /// <param name="animationMode">A <see cref="FrameAnimationMode"/> representing the way the frames should be animated.</param>
        /// <param name="frames">The frames of the texture.</param>
        public Texture(FrameAnimationMode animationMode, IEnumerable<TextureFrame> frames)
            : this(animationMode, frames, PointF.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Texture class.
        /// </summary>
        /// <param name="animationMode">A <see cref="FrameAnimationMode"/> representing the way the frames should be animated.</param>
        /// <param name="frames">The frames of the texture.</param>
        /// <param name="scrollVector">A <see cref="PointF"/> representing the scroll vector.</param>
        public Texture(FrameAnimationMode animationMode, IEnumerable<TextureFrame> frames, PointF scrollVector)
            : this(animationMode, frames, scrollVector, Enumerable.Empty<GpuProgram>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the Texture class.
        /// </summary>
        /// <param name="animationMode">A <see cref="FrameAnimationMode"/> representing the way the frames should be animated.</param>
        /// <param name="frames">The frames of the texture.</param>
        /// <param name="scrollVector">A <see cref="PointF"/> representing the scroll vector.</param>
        /// <param name="gpuPrograms">A collection of GPU programs.</param>
        public Texture(FrameAnimationMode animationMode, IEnumerable<TextureFrame> frames, PointF scrollVector, IEnumerable<GpuProgram> gpuPrograms)
        {
            this.FrameAnimationMode = animationMode;
            this.frames = new List<TextureFrame>(frames);
            this.ScrollVector = scrollVector;
            this.GpuPrograms = new List<GpuProgram>(gpuPrograms);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the FrameAnimationMode.
        /// </summary>
        public FrameAnimationMode FrameAnimationMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the frames.
        /// </summary>
        public IList<TextureFrame> Frames
        {
            get
            {
                return this.frames;
            }
        }

        /// <summary>
        /// Gets the list of GPU programs.
        /// </summary>
        public IList<GpuProgram> GpuPrograms
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the scroll vector.
        /// </summary>
        public PointF ScrollVector
        {
            get;
            set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Creates a texture from Xml.
        /// </summary>
        /// <param name="xElement">An XElement that contains the XML representation of the Texture.</param>
        /// <param name="system">The MiyagiSystem.</param>
        /// <returns>The newly created Texture.</returns>
        public static Texture CreateFromXml(XElement xElement, MiyagiSystem system)
        {
            return new Texture(
                ReadFrameAnimationMode(xElement.Element("FrameAnimationMode")),
                ReadFrames(xElement.Descendants("Frame")),
                ReadScrollVector(xElement.Element("ScrollVector")),
                ReadGpuPrograms(system, xElement.Descendants("GpuProgram")));
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Creates a deep copy of the texture.
        /// </summary>
        /// <returns>A deep copy of the texture.</returns>
        public Texture CreateDeepCopy()
        {
            var newFrames = new List<TextureFrame>(this.frames.Count);
            newFrames.AddRange(this.frames.Select(frame => frame.CreateDeepCopy()));

            var retValue = new Texture(this.FrameAnimationMode, newFrames, this.ScrollVector);

            foreach (GpuProgram gpu in this.GpuPrograms)
            {
                retValue.GpuPrograms.Add(gpu.CreateDeepCopy());
            }

            return retValue;
        }

        /// <summary>
        /// Gets a frame according to the specified animation time.
        /// </summary>
        /// <param name="time">The animation time.</param>
        /// <returns>The frame that corresponds to the specified animation time.</returns>
        public TextureFrame GetFrameFromTime(TimeSpan time)
        {
            if (this.frames.Count == 1)
            {
                return this.frames[0];
            }

            TextureFrame retValue = null;

            switch (this.FrameAnimationMode)
            {
                case FrameAnimationMode.ForwardOnce:
                    retValue = this.GetFrameForwardOnce(time) ?? this.frames[this.frames.Count - 1];
                    break;

                case FrameAnimationMode.BackwardOnce:
                    retValue = this.GetFrameBackwardOnce(time) ?? this.frames[0];
                    break;

                case FrameAnimationMode.ForwardLoop:
                    retValue = this.GetFrameForwardLoop(time);
                    break;

                case FrameAnimationMode.BackwardLoop:
                    retValue = this.GetFrameBackwardLoop(time);
                    break;

                case FrameAnimationMode.ForwardBackwardOnce:
                    retValue = this.GetFrameForwardBackwardOnce(time) ?? this.frames[0];
                    break;

                case FrameAnimationMode.ForwardBackwardLoop:
                    retValue = this.GetFrameForwardBackwardLoop(time);
                    break;
            }

            return retValue;
        }

        /// <summary>
        /// Gets the scroll vector offset.
        /// </summary>
        /// <param name="currentUV">The current UV.</param>
        /// <param name="timeSinceLastUpdateMs">The time since last update in ms.</param>
        /// <returns>The scroll vector offset.</returns>
        public RectangleF GetScrollVectorOffset(RectangleF currentUV, double timeSinceLastUpdateMs)
        {
            var f = (float)Math.Min(timeSinceLastUpdateMs / 1000, 1);

            float xOffset = f * this.ScrollVector.X;
            float yOffset = f * this.ScrollVector.Y;

            float left = currentUV.Left + xOffset;
            left = left > 1 ? left - 1 : left;
            float top = currentUV.Top + yOffset;
            top = top > 1 ? top - 1 : top;
            float right = left + currentUV.Width;
            float bottom = top + currentUV.Height;

            return RectangleF.FromLTRB(left, top, right, bottom);
        }

        /// <summary>
        /// Converts the texture to an XElement.
        /// </summary>
        /// <returns>An <see cref="XElement"/> representing the texture.</returns>
        public XElement ToXElement()
        {
            return this.ToXElement("Texture");
        }

        /// <summary>
        /// Converts the texture to an XElement.
        /// </summary>
        /// <param name="elementName">The name of the root element.</param>
        /// <returns>An <see cref="XElement"/> representing the texture.</returns>
        public XElement ToXElement(string elementName)
        {
            return new XElement(
                elementName,
                new XElement(
                    "Frames",
                    this.Frames.Select(frame => new XElement(
                                                    "Frame",
                                                    new XElement("FileName", frame.FileName),
                                                    new XElement("Duration", frame.Duration.TotalMilliseconds),
                                                    frame.UV.ToXElement("UV")))),
                new XElement(
                    "GpuPrograms",
                    this.GpuPrograms.Select(gpuProg => new XElement(
                                                           "GpuProgram",
                                                           new XAttribute("Name", gpuProg.Name),
                                                           new XAttribute("Type", gpuProg.Type),
                                                           new XElement("Language", gpuProg.Language),
                                                           new XElement(
                                                               "Parameters",
                                                               gpuProg.Parameters.Select(kvp => new XElement(
                                                                                                    "Parameter",
                                                                                                    new XElement("Key", kvp.Key),
                                                                                                    new XElement("Value", kvp.Value)))),
                                                           new XElement("SourceFile", gpuProg.SourceFile)))),
                new XElement("FrameAnimationMode", this.FrameAnimationMode),
                this.ScrollVector.ToXElement("ScrollVector"));
        }

        #endregion Public Methods

        #region Private Static Methods

        private static bool CheckFrameDelay(TimeSpan startDelay, TextureFrame frame, TimeSpan time)
        {
            return startDelay <= time && startDelay + frame.Duration >= time;
        }

        private static FrameAnimationMode ReadFrameAnimationMode(XElement element)
        {
            return element != null ? element.Value.ParseEnum<FrameAnimationMode>() : FrameAnimationMode.ForwardOnce;
        }

        private static IEnumerable<TextureFrame> ReadFrames(IEnumerable<XElement> elements)
        {
            return
                from frameNode in elements
                select new TextureFrame(
                    frameNode.Element("FileName").Value,
                    RectangleF.FromXElement(frameNode.Element("UV")),
                    (int)frameNode.Element("Duration"));
        }

        private static IEnumerable<GpuProgram> ReadGpuPrograms(MiyagiSystem system, IEnumerable<XElement> elements)
        {
            return
                elements.Select(gpuNode => GpuProgram.Create(
                    system,
                    gpuNode.Attribute("Name").Value,
                    gpuNode.Element("Language").Value,
                    gpuNode.Element("SourceFile").Value,
                    gpuNode.Attribute("Type").Value.ParseEnum<GpuProgramType>(),
                    gpuNode.Descendants("Parameter").Select(paramNode => new
                                                                         {
                                                                             paramNode,
                                                                             parameter = new
                                                                                         {
                                                                                             Key = paramNode.Element("Key").Value,
                                                                                             paramNode.Element("Value").Value
                                                                                         }
                                                                         }).Select(t => t.parameter).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)));
        }

        private static PointF ReadScrollVector(XElement element)
        {
            return element != null ? PointF.FromXElement(element) : PointF.Empty;
        }

        #endregion Private Static Methods

        #region Private Methods

        private TextureFrame GetFrameBackwardLoop(TimeSpan time)
        {
            return this.GetFrameBackwardOnce(this.NormalizeLoopTime(time));
        }

        private TextureFrame GetFrameBackwardOnce(TimeSpan time)
        {
            TimeSpan startDelay = TimeSpan.Zero;
            for (int i = this.frames.Count - 1; i >= 0; i--)
            {
                TextureFrame frame = this.frames[i];
                if (CheckFrameDelay(startDelay, frame, time))
                {
                    return frame;
                }

                startDelay += frame.Duration;
            }

            return null;
        }

        private TextureFrame GetFrameForwardBackwardLoop(TimeSpan time)
        {
            return this.GetFrameForwardBackwardOnce(this.NormalizeLoopTime(time));
        }

        private TextureFrame GetFrameForwardBackwardOnce(TimeSpan time)
        {
            TimeSpan startDelay = TimeSpan.Zero;
            for (int i = 0; i < this.frames.Count; i++)
            {
                TextureFrame frame = this.frames[i];
                if (CheckFrameDelay(startDelay, frame, time))
                {
                    return frame;
                }

                startDelay += frame.Duration;
            }

            for (int i = this.frames.Count - 1; i >= 0; i--)
            {
                TextureFrame frame = this.frames[i];
                if (CheckFrameDelay(startDelay, frame, time))
                {
                    return frame;
                }

                startDelay += frame.Duration;
            }

            return null;
        }

        private TextureFrame GetFrameForwardLoop(TimeSpan time)
        {
            return this.GetFrameForwardOnce(this.NormalizeLoopTime(time));
        }

        private TextureFrame GetFrameForwardOnce(TimeSpan time)
        {
            TimeSpan startDelay = TimeSpan.Zero;
            foreach (TextureFrame frame in this.frames)
            {
                if (CheckFrameDelay(startDelay, frame, time))
                {
                    return frame;
                }

                startDelay += frame.Duration;
            }

            return null;
        }

        private TimeSpan NormalizeLoopTime(TimeSpan time)
        {
            long total = this.frames.Sum(frame => (long)frame.Duration.TotalMilliseconds);

            if (this.FrameAnimationMode == FrameAnimationMode.ForwardBackwardLoop)
            {
                total *= 2;
            }

            return TimeSpan.FromMilliseconds((long)time.TotalMilliseconds - (((long)time.TotalMilliseconds / total) * total));
        }

        #endregion Private Methods

        #endregion Methods
    }
}