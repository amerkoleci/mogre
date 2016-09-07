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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    using Miyagi.Common.Serialization;
    using Miyagi.Internals;

    /// <summary>
    /// The LocaleManager.
    /// </summary>
    public class LocaleManager : IManager
    {
        #region Fields

        private readonly XmlDocument internalNeutralLocale;
        private readonly Dictionary<CultureInfo, string> localeFolderDict;

        private CultureInfo culture;
        private string currentLocaleFolder;
        private XmlDocument internalLocale;
        private string neutralLocaleFolder;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LocaleManager class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        protected internal LocaleManager(MiyagiSystem system)
        {
            this.MiyagiSystem = system;

            // fallback to English
            this.internalNeutralLocale = new XmlDocument();
            this.internalNeutralLocale.Load(GetInternalStream(CultureInfo.GetCultureInfo("en")));

            this.localeFolderDict = new Dictionary<CultureInfo, string>();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the CurrentCulture property changes.
        /// </summary>
        public event EventHandler CurrentCultureChanged;

        /// <summary>
        /// Occurs when the LocaleManager is disposing.
        /// </summary>
        public event EventHandler Disposing;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the current culture.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public CultureInfo CurrentCulture
        {
            get
            {
                return this.culture;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (this.culture == null || this.culture.Name != value.Name)
                {
                    this.culture = value;
                    this.OnCurrentCultureChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the manager has been disposed.
        /// </summary>
        /// <value></value>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of the manager.
        /// </summary>
        public string Type
        {
            get
            {
                return "Locale";
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the MiyagiSystem.
        /// </summary>
        protected MiyagiSystem MiyagiSystem
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Explicit Interface Methods

        void IManager.LoadSerializationData(SerializationData data)
        {
        }

        void IManager.NotifyManagerRegistered(IManager manager)
        {
        }

        void IManager.SaveSerializationData(SerializationData data)
        {
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Adds a new locale folder.
        /// </summary>
        /// <param name="cultureInfo">The CultureInfo.</param>
        /// <param name="folder">The folder containg the locale files.</param>
        public virtual void AddLocaleFolder(CultureInfo cultureInfo, string folder)
        {
            this.AddLocaleFolder(cultureInfo, folder, false);
        }

        /// <summary>
        /// Adds a new locale folder.
        /// </summary>
        /// <param name="cultureInfo">The CultureInfo.</param>
        /// <param name="folder">The folder containg the locale files.</param>
        /// <param name="isNeutral">Specifies whether the locale is also the neutral one.</param>
        public virtual void AddLocaleFolder(CultureInfo cultureInfo, string folder, bool isNeutral)
        {
            if (isNeutral)
            {
                this.neutralLocaleFolder = folder;
            }

            this.localeFolderDict[cultureInfo] = folder;

            this.SetCurrentCultureFolder();
        }

        /// <summary>
        /// Gets a string from a resource key.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="localeResourceKey">The locale resource key.</param>
        /// <exception cref="InvalidOperationException">Folder for the neutral culture is not set.</exception>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        /// <exception cref="ArgumentException">Malformed resource key.</exception>
        public virtual void ApplyResourceKey(object targetObject, string localeResourceKey)
        {
            if (string.IsNullOrEmpty(this.neutralLocaleFolder))
            {
                throw new InvalidOperationException("Folder for the neutral culture is not set.");
            }

            if (string.IsNullOrEmpty(localeResourceKey))
            {
                throw new ArgumentNullException("localeResourceKey");
            }

            string[] path = localeResourceKey.Split('@');

            if (path.Length != 2)
            {
                throw new ArgumentException("Malformed resource key.", "localeResourceKey");
            }

            string ext = Path.GetExtension(path[1]).ToLowerInvariant();

            switch (ext)
            {
                case ".xml":
                    XmlReader.ReadXmlString(targetObject, path, this.currentLocaleFolder, this.neutralLocaleFolder);
                    break;
            }
        }

        /// <summary>
        /// Disposes the LocaleManager.
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.Disposing != null)
            {
                this.Disposing(this, EventArgs.Empty);
            }

            this.Dispose(true);
            GC.SuppressFinalize(this);
            this.IsDisposed = true;
            this.MiyagiSystem.UnregisterManager(this);
            this.MiyagiSystem = null;
        }

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        public virtual void Initialize()
        {
            this.CurrentCulture = CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Updates the manager.
        /// </summary>
        public virtual void Update()
        {
        }

        #endregion Public Methods

        #region Internal Methods

        internal string GetStringInternal(string name)
        {
            string retValue = string.Empty;

            XmlDocument doc = this.internalLocale ?? this.internalNeutralLocale;

            if (doc != null)
            {
                XmlNode node = doc.SelectSingleNode("root");
                node = node.FirstChild;

                while (node != null)
                {
                    if (node.Attributes["name"].Value == name)
                    {
                        retValue = node.SelectSingleNode("value").InnerText;
                        break;
                    }

                    node = node.NextSibling;
                }
            }

            return retValue;
        }

        #endregion Internal Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the LocaleManager.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.Disposing = null;
            this.CurrentCultureChanged = null;
        }

        /// <summary>
        /// Raises the CurrentCultureChanged event.
        /// </summary>
        protected virtual void OnCurrentCultureChanged()
        {
            this.internalLocale = null;
            this.LoadInternalXml();

            this.SetCurrentCultureFolder();

            if (this.CurrentCultureChanged != null)
            {
                this.CurrentCultureChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets the current culture folder.
        /// </summary>
        protected virtual void SetCurrentCultureFolder()
        {
            if (!this.localeFolderDict.TryGetValue(this.culture, out this.currentLocaleFolder))
            {
                if (!this.localeFolderDict.TryGetValue(this.culture.Parent, out this.currentLocaleFolder))
                {
                    this.currentLocaleFolder = this.neutralLocaleFolder;
                }
            }
        }

        #endregion Protected Methods

        #region Private Static Methods

        private static Stream GetInternalStream(CultureInfo cultureInfo)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resNames = asm.GetManifestResourceNames();
            string fileName = string.Format("Miyagi.Internals.Resources.Locale.{0}.xml", cultureInfo.Name);
            return resNames.Any(t => t == fileName) ? asm.GetManifestResourceStream(fileName) : null;
        }

        #endregion Private Static Methods

        #region Private Methods

        private void LoadInternalXml()
        {
            Stream s = GetInternalStream(this.culture) ?? GetInternalStream(this.culture.Parent);

            // try the parent culture
            if (s != null)
            {
                this.internalLocale = new XmlDocument();
                this.internalLocale.Load(s);
                s.Dispose();
            }
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Types

        private static class XmlReader
        {
            #region Methods

            #region Public Static Methods

            public static void ReadXmlString(object targetObject, IList<string> path, string currentLocaleFolder, string neutralLocaleFolder)
            {
                var currentDoc = new XmlDocument();
                currentDoc.Load(Path.Combine(currentLocaleFolder, path[1]));

                if (!SetStrings(targetObject, currentDoc, path[0]))
                {
                    var neutralDoc = new XmlDocument();
                    neutralDoc.Load(Path.Combine(neutralLocaleFolder, path[1]));
                    SetStrings(targetObject, neutralDoc, path[0]);
                }
            }

            #endregion Public Static Methods

            #region Private Static Methods

            private static bool SetStrings(object targetObject, XmlNode cultureDoc, string name)
            {
                var node = cultureDoc.SelectSingleNode("Root").SelectSingleNode("Key");

                while (node != null)
                {
                    if (node.Attributes["Name"].Value == name)
                    {
                        var propertyNode = node.SelectSingleNode("Property");
                        while (propertyNode != null)
                        {
                            string propertyName = propertyNode.Attributes["Name"].Value;
                            string propertyValue = propertyNode.Attributes["Value"].Value;
                            var pi = targetObject.GetType().GetProperty(propertyName);
                            var localizableAttribute = pi.GetAttribute<LocalizableAttribute>();

                            if (localizableAttribute != null && localizableAttribute.IsLocalizable)
                            {
                                pi.SetValue(targetObject, propertyValue, null);
                            }
                            else
                            {
                                throw new NotSupportedException("Property '" + propertyName + "' is not marked as localizable.");
                            }

                            propertyNode = propertyNode.NextSibling;
                        }

                        return true;
                    }

                    node = node.NextSibling;
                }

                return false;
            }

            #endregion Private Static Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}