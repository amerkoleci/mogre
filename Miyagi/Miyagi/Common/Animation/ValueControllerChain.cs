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
namespace Miyagi.Common.Animation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A chain of ValueControllers.
    /// </summary>
    /// <typeparam name = "T">The type of the value.</typeparam>
    public sealed class ValueControllerChain<T>
        where T : struct
    {
        #region Fields

        private readonly bool autoUpdate;
        private readonly Dictionary<int, List<ChainLink<T>>> chainLinkLists;
        private readonly SetterDelegate<T> commonSetterDelegate;

        private List<ChainLink<T>> currentLayer;
        private int finishedChainLinks;
        private bool isRunning;
        private int maxLayer;
        private MiyagiSystem miyagiSystem;
        private int nextLayer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ValueControllerChain class.
        /// </summary>
        public ValueControllerChain()
            : this(null, false, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ValueControllerChain class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        /// <param name="autoUpdate">Indicates whether the ValueControllerChain should update itself.</param>
        public ValueControllerChain(MiyagiSystem system, bool autoUpdate)
            : this(system, autoUpdate, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ValueControllerChain class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        /// <param name="autoUpdate">Indicates whether the ValueControllerChain should update itself.</param>
        /// <param name="commonSetterDelegate">The common setter delegate.</param>
        public ValueControllerChain(MiyagiSystem system, bool autoUpdate, SetterDelegate<T> commonSetterDelegate)
        {
            this.miyagiSystem = system;
            this.autoUpdate = autoUpdate;
            this.commonSetterDelegate = commonSetterDelegate;
            this.chainLinkLists = new Dictionary<int, List<ChainLink<T>>>();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the ValueControllerChain has finished.
        /// </summary>
        public event EventHandler Finished;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the ValueControllerChain is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }

            private set
            {
                this.isRunning = value;

                if (this.autoUpdate)
                {
                    if (!value)
                    {
                        this.miyagiSystem.Updating -= this.MiyagiSystemUpdating;
                    }
                    else
                    {
                        this.miyagiSystem.Updating += this.MiyagiSystemUpdating;
                    }
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds a ValueController to the ValueControllerChain.
        /// </summary>
        /// <param name="layer">The layer of the ValueController</param>
        /// <param name="valueController">The ValueController.</param>
        /// <remarks>ValueController on the same layer are started simultaneously.</remarks>
        public void AddValueController(int layer, ValueController<T> valueController)
        {
            this.AddValueController(layer, valueController, null);
        }

        /// <summary>
        /// Adds a ValueController to the ValueControllerChain.
        /// </summary>
        /// <param name="layer">The layer of the ValueController</param>
        /// <param name="valueController">The ValueController.</param>
        /// <param name="setterDelegate">The setter delegate.</param>
        /// <remarks>ValueController on the same layer are started simultaneously.</remarks>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        public void AddValueController(int layer, ValueController<T> valueController, SetterDelegate<T> setterDelegate)
        {
            if (this.commonSetterDelegate == null && setterDelegate == null)
            {
                throw new ArgumentNullException("setterDelegate");
            }

            var vcce = new ChainLink<T>
                       {
                           ValueController = valueController,
                           SetterDelegate = setterDelegate
                       };

            if (layer > this.maxLayer)
            {
                this.maxLayer = layer;
            }

            if (!this.chainLinkLists.ContainsKey(layer))
            {
                this.chainLinkLists[layer] = new List<ChainLink<T>>();
            }

            this.chainLinkLists[layer].Add(vcce);
        }

        /// <summary>
        /// Starts the ValueControllerChain.
        /// </summary>
        public void Start()
        {
            if (this.isRunning)
            {
                this.Stop();
            }

            this.IsRunning = true;
            this.nextLayer = 0;
            this.StartNextLayer();
        }

        /// <summary>
        /// Stops the ValueControllerChain.
        /// </summary>
        public void Stop()
        {
            if (this.isRunning)
            {
                this.IsRunning = false;
                this.miyagiSystem = null;

                if (this.currentLayer != null)
                {
                    this.currentLayer.ForEach(
                        link =>
                        {
                            link.ValueController.Finished -= this.CurrentValueControllerFinished;
                            link.ValueController.Stop();
                            link.SetterDelegate = null;
                        });
                }

                if (this.Finished != null)
                {
                    this.Finished(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Updates the ValueControllerChain.
        /// </summary>
        public void Update()
        {
            if (this.isRunning)
            {
                if (this.currentLayer != null)
                {
                    this.currentLayer.ForEach(ele => ele.ValueController.Update());
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void CurrentValueControllerFinished(object sender, EventArgs e)
        {
            if (++this.finishedChainLinks == this.currentLayer.Count)
            {
                if (++this.nextLayer > this.maxLayer)
                {
                    this.Stop();
                }
                else
                {
                    this.StartNextLayer();
                }
            }
        }

        private void MiyagiSystemUpdating(object sender, EventArgs e)
        {
            this.Update();
        }

        private void StartNextLayer()
        {
            if (this.currentLayer != null)
            {
                this.currentLayer.ForEach(ele => ele.ValueController.Finished -= this.CurrentValueControllerFinished);
            }

            while (!this.chainLinkLists.ContainsKey(this.nextLayer))
            {
                this.nextLayer++;
                if (this.nextLayer >= this.maxLayer)
                {
                    return;
                }
            }

            this.currentLayer = this.chainLinkLists[this.nextLayer];
            this.finishedChainLinks = 0;

            this.currentLayer.ForEach(
                link =>
                {
                    link.ValueController.Finished += this.CurrentValueControllerFinished;
                    SetterDelegate<T> setterDel = link.SetterDelegate ?? this.commonSetterDelegate;
                    link.ValueController.Start(this.miyagiSystem, this.autoUpdate, setterDel);
                });
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Types

        private sealed class ChainLink<TChainLink>
            where TChainLink : struct
        {
            #region Properties

            #region Public Properties

            public SetterDelegate<TChainLink> SetterDelegate
            {
                get;
                set;
            }

            public ValueController<TChainLink> ValueController
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