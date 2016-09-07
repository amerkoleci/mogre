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
    using System.Globalization;
    using System.Reflection;

    #region Delegates

    /// <summary>
    /// The setter delegate for ValueControllers.
    /// </summary>
    /// <param name="obj">The new value.</param>
    /// <typeparam name = "T">The type of the value.</typeparam>
    public delegate void SetterDelegate<T>(T obj);

    #endregion Delegates

    /// <summary>
    /// The base ValueController.
    /// </summary>
    /// <typeparam name = "T">The type of the value.</typeparam>
    public abstract class ValueController<T>
        where T : struct
    {
        #region Fields

        private bool isRunning;
        private SetterDelegate<T> setterDelegate;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ValueController class.
        /// </summary>
        /// <param name="duration">A TimeSpan representing the duration.</param>
        protected ValueController(TimeSpan duration)
        {
            this.Duration = duration;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the ValueController has finished.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Occurs when the ValueController has started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when the ValueController has been updated.
        /// </summary>
        public event EventHandler Updated;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the ValueController is running.
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

                if (this.AutoUpdate)
                {
                    if (!value)
                    {
                        this.MiyagiSystem.Updating -= this.MiyagiSystemUpdating;
                    }
                    else
                    {
                        this.MiyagiSystem.Updating += this.MiyagiSystemUpdating;
                    }
                }
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets a value indicating whether the ValueController should update itself.
        /// </summary>
        protected bool AutoUpdate
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        protected TimeSpan Duration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ratio of elapsed time and duration.
        /// </summary>
        protected double ElapsedPercentage
        {
            get
            {
                return this.ElapsedTime.TotalMilliseconds / this.Duration.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Gets the elapsed time.
        /// </summary>
        protected TimeSpan ElapsedTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the MiyagiSystem.
        /// </summary>
        protected MiyagiSystem MiyagiSystem
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        protected DateTime StartTime
        {
            get;
            set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Starts the ValueController.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        /// <param name="autoUpdate">Indicates whether the ValueController should update itself.</param>
        /// <param name="setter">The setter delegate for the value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="setter" /> is <c>null</c>.</exception>
        public void Start(MiyagiSystem system, bool autoUpdate, SetterDelegate<T> setter)
        {
            this.MiyagiSystem = system;
            if (this.isRunning)
            {
                this.Stop();
            }

            if (setter == null)
            {
                throw new ArgumentNullException("setter");
            }

            this.setterDelegate = setter;
            this.AutoUpdate = autoUpdate;

            this.OnStart(EventArgs.Empty);
        }

        /// <summary>
        /// Starts the ValueController.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        /// <param name="autoUpdate">Indicates whether the ValueController should update itself.</param>
        /// <param name="propertyName">The name of the property which value should be changed.</param>
        /// <param name="obj">The object which property should be changed.</param>
        /// <exception cref="ArgumentException">propertyName</exception>
        public void Start(MiyagiSystem system, bool autoUpdate, string propertyName, object obj)
        {
            PropertyInfo prop = obj.GetType().GetProperty(propertyName);

            if (prop.PropertyType != typeof(T))
            {
                throw new ArgumentException("Property type mismatch. Expected: " + typeof(T).Name + ". Was: " + prop.PropertyType.Name, "propertyName");
            }

            this.Start(system, autoUpdate, val => prop.SetValue(obj, val, null));
        }

        /// <summary>
        /// Stops the ValueController.
        /// </summary>
        public void Stop()
        {
            if (this.isRunning)
            {
                this.OnFinished(EventArgs.Empty);
                this.MiyagiSystem = null;
                this.setterDelegate = null;
            }
        }

        /// <summary>
        /// Updated the ValueController.
        /// </summary>
        public void Update()
        {
            if (this.isRunning)
            {
                this.ElapsedTime = this.MiyagiSystem.LastUpdate - this.StartTime;

                if (this.ElapsedTime > this.Duration)
                {
                    this.ElapsedTime = this.Duration;
                    this.OnUpdate(EventArgs.Empty);
                    this.OnFinished(EventArgs.Empty);
                }
                else
                {
                    this.OnUpdate(EventArgs.Empty);
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Gets the current value.
        /// </summary>
        /// <returns>An object representing the current value.</returns>
        protected abstract object GetValue();

        /// <summary>
        /// Raises the Finished event.
        /// </summary>
        /// <param name="e">A EventArgs that contains the event data.</param>
        protected virtual void OnFinished(EventArgs e)
        {
            this.IsRunning = false;

            if (this.Finished != null)
            {
                this.Finished(this, e);
            }
        }

        /// <summary>
        /// Raises the Started event.
        /// </summary>
        /// <param name="e">A EventArgs that contains the event data.</param>
        protected virtual void OnStart(EventArgs e)
        {
            this.IsRunning = true;
            this.StartTime = this.MiyagiSystem.LastUpdate;

            if (this.Started != null)
            {
                this.Started(this, e);
            }
        }

        /// <summary>
        /// Raises the Updated event.
        /// </summary>
        /// <param name="e">A EventArgs that contains the event data.</param>
        protected virtual void OnUpdate(EventArgs e)
        {
            this.UpdateValue();

            if (this.Updated != null)
            {
                this.Updated(this, e);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void MiyagiSystemUpdating(object sender, EventArgs e)
        {
            this.Update();
        }

        private void UpdateValue()
        {
            this.setterDelegate((T)Convert.ChangeType(this.GetValue(), typeof(T), CultureInfo.InvariantCulture));
        }

        #endregion Private Methods

        #endregion Methods
    }
}