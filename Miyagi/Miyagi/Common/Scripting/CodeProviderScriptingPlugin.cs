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
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Security.Permissions;

    /// <summary>
    /// An abstract base class for ScriptingPlugins which are based on a CodeDomProvider.
    /// </summary>
    public abstract class CodeProviderScriptingPlugin : ScriptingPlugin
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CodeProviderScriptingPlugin class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        protected CodeProviderScriptingPlugin(MiyagiSystem system)
            : base(system)
        {
            this.Assemblies = new Dictionary<int, Assembly>();
        }

        #endregion Constructors

        #region Properties

        #region Protected Properties

        /// <summary>
        /// Gets a Dictionary of created assemblies.
        /// </summary>
        protected Dictionary<int, Assembly> Assemblies
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Is called when the plugin is unloaded.
        /// </summary>
        public override void NotifyUnloaded()
        {
            base.NotifyUnloaded();
            this.Assemblies.Clear();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Binds the events.
        /// </summary>
        /// <param name="scheme">The ScriptingScheme.</param>
        protected override void BindEvents(ScriptingScheme scheme)
        {
            // create or retrieve assembly
            Assembly asm;
            this.Assemblies.TryGetValue(scheme.Source.GetHashCode(), out asm);

            if (asm == null)
            {
                asm = this.CreateAssembly(scheme);

                if (asm != null)
                {
                    this.Assemblies[scheme.Source.GetHashCode()] = asm;
                }
            }

            // bind events
            if (asm != null)
            {
                object instance = asm.CreateInstance(asm.GetTypes()[0].FullName);
                if (instance != null)
                {
                    foreach (ScriptMap emd in scheme.ScriptMaps)
                    {
                        // Iterate through events
                        foreach (KeyValuePair<string, string> kvp in emd.EventMethods)
                        {
                            // get event of control
                            EventInfo ei = emd.Target.GetType().GetEvent(kvp.Key);

                            if (ei != null)
                            {
                                ei.AddEventHandler(emd.Target, Delegate.CreateDelegate(ei.EventHandlerType, instance, kvp.Value));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an assembly.
        /// </summary>
        /// <param name="scheme">The ScriptingScheme.</param>
        /// <returns>The newly created assembly.</returns>
        protected abstract Assembly CreateAssembly(ScriptingScheme scheme);

        /// <summary>
        /// Creates an assembly.
        /// </summary>
        /// <param name="scheme">The ScriptingScheme.</param>
        /// <param name="provider">The CodeDomProvider.</param>
        /// <param name="cp">The CompilerParameters.</param>
        /// <returns>The newly created assembly.</returns>
        [SecurityPermission(SecurityAction.LinkDemand)]
        protected virtual Assembly CreateAssembly(ScriptingScheme scheme, CodeDomProvider provider, CompilerParameters cp)
        {
            cp.GenerateInMemory = true;

            foreach (string s in scheme.ReferencedAssemblies)
            {
                cp.ReferencedAssemblies.Add(s);
            }

            var cr = provider.CompileAssemblyFromSource(cp, scheme.Source);
            if (!cr.Errors.HasErrors)
            {
                return cr.CompiledAssembly;
            }

            foreach (var error in cr.Errors)
            {
                Console.Error.WriteLine(error);
            }

            if (scheme.ThrowOnCompileError)
            {
                throw new ArgumentException("Errors while compiling script.", "scheme");
            }

            return null;
        }

        #endregion Protected Methods

        #endregion Methods
    }
}