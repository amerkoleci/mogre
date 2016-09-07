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
namespace Miyagi.Internals
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;

    internal sealed class InvokeHelper : ISynchronizeInvoke
    {
        #region Fields

        private static readonly object AsyncPadLock = new object();
        private static readonly object SyncPadLock = new object();

        private readonly Queue<InvokeHelperItem> asyncQueue = new Queue<InvokeHelperItem>();
        private readonly Thread thread;

        private InvokeHelperItem syncItem;

        #endregion Fields

        #region Constructors

        public InvokeHelper(Thread thread)
        {
            this.thread = thread;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        public bool InvokeRequired
        {
            get
            {
                return !ReferenceEquals(this.thread, Thread.CurrentThread);
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            var retValue = new InvokeHelperItem(method, args);

            lock (AsyncPadLock)
            {
                this.asyncQueue.Enqueue(retValue);
            }

            return retValue;
        }

        public object EndInvoke(IAsyncResult result)
        {
            object retValue;

            lock (AsyncPadLock)
            {
                var ihi = (InvokeHelperItem)result;

                if (!ihi.IsCompleted && !this.asyncQueue.Contains(ihi))
                {
                    return null;
                }

                while (!ihi.IsCompleted)
                {
                    Monitor.Wait(AsyncPadLock);
                }

                retValue = ihi.MethodResult;
            }

            return retValue;
        }

        public object Invoke(Delegate method, object[] args)
        {
            object retValue;

            lock (SyncPadLock)
            {
                if (!this.InvokeRequired)
                {
                    return method.DynamicInvoke(args);
                }

                this.syncItem = new InvokeHelperItem(method, args);

                Monitor.Wait(SyncPadLock);
                retValue = this.syncItem.MethodResult;
                this.syncItem = null;
            }

            return retValue;
        }

        public void Update()
        {
            if (this.InvokeRequired)
            {
                throw new InvalidOperationException();
            }

            lock (SyncPadLock)
            {
                if (this.syncItem != null)
                {
                    this.syncItem.MethodResult = this.syncItem.Method.DynamicInvoke(this.syncItem.MethodArgs);
                    this.syncItem.CompletedSynchronously = true;
                    this.syncItem.IsCompleted = true;
                }

                Monitor.Pulse(SyncPadLock);
            }

            lock (AsyncPadLock)
            {
                while (this.asyncQueue.Count > 0)
                {
                    InvokeHelperItem ihi = this.asyncQueue.Dequeue();
                    ihi.MethodResult = ihi.Method.DynamicInvoke(ihi.MethodArgs);
                    ihi.CompletedSynchronously = false;
                    ihi.IsCompleted = true;
                }

                Monitor.Pulse(AsyncPadLock);
            }
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class InvokeHelperItem : IAsyncResult
        {
            #region Constructors

            public InvokeHelperItem(Delegate method, object[] methodArgs)
            {
                this.Method = method;
                this.MethodArgs = methodArgs;
            }

            #endregion Constructors

            #region Properties

            #region Public Properties

            public object AsyncState
            {
                get
                {
                    return null;
                }
            }

            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            public bool CompletedSynchronously
            {
                get;
                set;
            }

            public bool IsCompleted
            {
                get;
                set;
            }

            public Delegate Method
            {
                get;
                private set;
            }

            public object[] MethodArgs
            {
                get;
                private set;
            }

            public object MethodResult
            {
                get;
                set;
            }

            #endregion Public Properties

            #endregion Properties
        }

        #endregion Nested Types
    }
}