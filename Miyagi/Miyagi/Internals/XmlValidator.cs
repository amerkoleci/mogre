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
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    internal class XmlValidator
    {
        #region Fields

        private static readonly Dictionary<string, XmlSchemaSet> XmlSchemaSetDict = new Dictionary<string, XmlSchemaSet>();

        private bool lastValidationOk;

        #endregion Fields

        #region Properties

        #region Public Properties

        public IList<string> ValidationMessages
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        public bool ValidateAgainstInternalSchema(XElement xElement, string schemaFile)
        {
            using (Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Miyagi.Internals.Resources.SchemaFiles." + schemaFile + ".xsd"))
            {
                var settings = new XmlReaderSettings();
                settings.ValidationEventHandler += this.ValidationCallback;

                XmlSchemaSet xss;
                if (XmlSchemaSetDict.ContainsKey(schemaFile))
                {
                    xss = XmlSchemaSetDict[schemaFile];
                }
                else
                {
                    xss = new XmlSchemaSet();
                    xss.Add(XmlSchema.Read(schemaStream, this.ValidationCallback));
                    xss.Compile();
                    XmlSchemaSetDict[schemaFile] = xss;
                }

                settings.Schemas = xss;
                settings.ValidationType = ValidationType.Schema;

                using (XmlReader xer = xElement.CreateReader())
                {
                    using (XmlReader xr = XmlReader.Create(xer, settings))
                    {
                        this.lastValidationOk = true;
                        this.ValidationMessages = new List<string>();

                        while (xr.Read())
                        {
                        }
                    }
                }

                settings.ValidationEventHandler -= this.ValidationCallback;
            }

            return this.lastValidationOk;
        }

        #endregion Public Methods

        #region Private Methods

        private void ValidationCallback(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
                this.lastValidationOk = false;
            }

            this.ValidationMessages.Add(e.Exception.ToString());
        }

        #endregion Private Methods

        #endregion Methods
    }
}