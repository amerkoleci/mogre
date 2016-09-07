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
namespace Miyagi.Common.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// A ScriptingScheme specifies how a scripting plugin binds events.
    /// </summary>
    public sealed class ScriptingScheme : INamable
    {
        #region Fields

        private string source;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScriptingScheme class.
        /// </summary>
        /// <param name="name">The name of the ScriptingScheme.</param>
        /// <param name="language">The used programming language.</param>
        public ScriptingScheme(string name, string language)
        {
            this.Name = name;
            this.ReferencedAssemblies = new List<string>();
            this.ScriptMaps = new Collection<ScriptMap>();
            this.Language = language;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the ScriptingScheme has been bound.
        /// </summary>
        public bool IsBound
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the name of the scripting language.
        /// </summary>
        public string Language
        {
            get;
            private set;
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
        /// Gets a list of the referenced assemblies.
        /// </summary>
        public IList<string> ReferencedAssemblies
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a collection of ScriptMap.
        /// </summary>
        public Collection<ScriptMap> ScriptMaps
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the source code.
        /// </summary>
        public string Source
        {
            get
            {
                return this.source;
            }

            set
            {
                if (this.IsBound)
                {
                    throw new InvalidOperationException();
                }

                this.source = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an exception is thrown when an error occurs during compilation.
        /// </summary>
        public bool ThrowOnCompileError
        {
            get;
            set;
        }

        #endregion Public Properties

        #endregion Properties
    }
}