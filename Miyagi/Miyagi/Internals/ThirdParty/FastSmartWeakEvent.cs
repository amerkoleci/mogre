/*
// Copyright (c) 2008 Daniel Grunwald
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
*/
namespace Miyagi.Internals.ThirdParty
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// A class for managing a weak event.
    /// </summary>
    /// <typeparam name="T">The delegate type.</typeparam>
    internal sealed class FastSmartWeakEvent<T>
        where T : class
    {
        #region Fields

        private readonly List<EventEntry> eventEntries = new List<EventEntry>();

        #endregion Fields

        #region Constructors

        static FastSmartWeakEvent()
        {
            if (!typeof(T).IsSubclassOf(typeof(Delegate)))
            {
                throw new ArgumentException("T must be a delegate type");
            }

            MethodInfo invoke = typeof(T).GetMethod("Invoke");
            if (invoke == null || invoke.GetParameters().Length != 2)
            {
                throw new ArgumentException("T must be a delegate type taking 2 parameters");
            }

            ParameterInfo senderParameter = invoke.GetParameters()[0];
            if (senderParameter.ParameterType != typeof(object))
            {
                throw new ArgumentException("The first delegate parameter must be of type 'object'");
            }

            ParameterInfo argsParameter = invoke.GetParameters()[1];
            if (!typeof(EventArgs).IsAssignableFrom(argsParameter.ParameterType))
            {
                throw new ArgumentException("The second delegate parameter must be derived from type 'EventArgs'");
            }

            if (invoke.ReturnType != typeof(void))
            {
                throw new ArgumentException("The delegate return type must be void.");
            }
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        public void Add(T eh)
        {
            if (eh != null)
            {
                Delegate d = (Delegate)(object)eh;
                if (this.eventEntries.Count == this.eventEntries.Capacity)
                {
                    this.RemoveDeadEntries();
                }

                MethodInfo targetMethod = d.Method;
                object targetInstance = d.Target;
                WeakReference target = targetInstance != null ? new WeakReference(targetInstance) : null;
                this.eventEntries.Add(new EventEntry(FastSmartWeakEventForwarderProvider.GetForwarder(targetMethod), targetMethod, target));
            }
        }

        public void Raise(object sender, EventArgs e)
        {
            bool needsCleanup = false;
            foreach (EventEntry ee in this.eventEntries.ToArray())
            {
                needsCleanup |= ee.Forwarder(ee.TargetReference, sender, e);
            }

            if (needsCleanup)
            {
                this.RemoveDeadEntries();
            }
        }

        public void Remove(T eh)
        {
            if (eh != null)
            {
                var d = (Delegate)(object)eh;
                object targetInstance = d.Target;
                var targetMethod = d.Method;
                for (int i = this.eventEntries.Count - 1; i >= 0; i--)
                {
                    var entry = this.eventEntries[i];
                    if (entry.TargetReference != null)
                    {
                        object target = entry.TargetReference.Target;
                        if (target == null)
                        {
                            this.eventEntries.RemoveAt(i);
                        }
                        else if (target == targetInstance && entry.TargetMethod == targetMethod)
                        {
                            this.eventEntries.RemoveAt(i);
                            break;
                        }
                    }
                    else
                    {
                        if (targetInstance == null && entry.TargetMethod == targetMethod)
                        {
                            this.eventEntries.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RemoveDeadEntries()
        {
            this.eventEntries.RemoveAll(ee => ee.TargetReference != null && !ee.TargetReference.IsAlive);
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Types

        private struct EventEntry
        {
            #region Fields

            public readonly FastSmartWeakEventForwarderProvider.ForwarderDelegate Forwarder;
            public readonly MethodInfo TargetMethod;
            public readonly WeakReference TargetReference;

            #endregion Fields

            #region Constructors

            public EventEntry(FastSmartWeakEventForwarderProvider.ForwarderDelegate forwarder, MethodInfo targetMethod, WeakReference targetReference)
            {
                this.Forwarder = forwarder;
                this.TargetMethod = targetMethod;
                this.TargetReference = targetReference;
            }

            #endregion Constructors
        }

        #endregion Nested Types
    }
}