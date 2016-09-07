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
    using System.Collections.Generic;

    using Miyagi.Common.Rendering;

    /// <summary>
    /// Defines a program which runs on the Gpu.
    /// </summary>
    public class GpuProgram : IDeepCopiable<GpuProgram>
    {
        #region Constructors

        private GpuProgram()
        {
            this.NamedConstants = new Dictionary<string, object>();
            this.AdditonalTextures = new List<string>();
        }

        private GpuProgram(string name, string language, GpuProgramType type, IDictionary<string, string> parameters, string sourceFile)
            : this()
        {
            this.Name = name;
            this.Language = language;
            this.Type = type;
            this.Parameters = parameters;
            this.SourceFile = Backend.NormalizeFilePath(sourceFile);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the additonal textures for multi-texture shaders.
        /// </summary>
        public IList<string> AdditonalTextures
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        public string Language
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the named constant dictionary.
        /// </summary>
        public IDictionary<string, object> NamedConstants
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parameter dictionary.
        /// </summary>
        public IDictionary<string, string> Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path to the source file.
        /// </summary>
        public string SourceFile
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public GpuProgramType Type
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Create a GpuProgram.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        /// <param name="name">The name of the GpuProgam.</param>
        /// <param name="language">The shader language of GpuProgram.</param>
        /// <param name="sourceFile">The path to the source file.</param>
        /// <param name="type">The type of the GpuProgram.</param>
        /// <param name="parameters">The parameters of the GpuProgram.</param>
        /// <returns>The newly created GpuProgram.</returns>
        public static GpuProgram Create(MiyagiSystem system, string name, string language, string sourceFile, GpuProgramType type, IDictionary<string, string> parameters)
        {
            GpuProgram retValue = new GpuProgram(name, language, type, parameters, sourceFile);
            system.Backend.CreateGpuProgram(retValue);
            return retValue;
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Creates a deep copy of the GpuProgram.
        /// </summary>
        /// <returns>A deep copy of the GpuProgram.</returns>
        public GpuProgram CreateDeepCopy()
        {
            return new GpuProgram
                   {
                       Language = this.Language,
                       SourceFile = this.SourceFile,
                       Type = this.Type,
                       NamedConstants = new Dictionary<string, object>(this.NamedConstants),
                       Parameters = new Dictionary<string, string>(this.Parameters)
                   };
        }

        #endregion Public Methods

        #endregion Methods
    }
}