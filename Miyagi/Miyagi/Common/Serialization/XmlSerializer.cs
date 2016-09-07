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
namespace Miyagi.Common.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    using Miyagi.Internals;

    /// <summary>
    /// Serializes and deserializes into and from XML documents.
    /// </summary>
    public sealed class XmlSerializer : Serializer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializer"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public XmlSerializer(SerializationManager manager)
            : base(manager)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the list of supported file extensions.
        /// </summary>
        public override IList<string> FileExtensions
        {
            get
            {
                return new List<string>
                       {
                           ".mgx",
                           ".xml"
                       };
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Exports to a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="serializationData">The SerializationData.</param>
        public override void ExportToStream(Stream stream, IDictionary<IManager, SerializationData> serializationData)
        {
            Writer.Write(stream, this.SerializationManager.Encoding, serializationData);
        }

        /// <summary>
        /// Imports from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public override void ImportFromStream(Stream stream)
        {
            Reader.Read(stream, this.MiyagiSystem);
        }

        /// <summary>
        /// Imports from an XDocument.
        /// </summary>
        /// <param name="element">The element.</param>
        public void ImportFromXDocument(XDocument element)
        {
            new Reader(element, this.MiyagiSystem);
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class Reader
        {
            #region Fields

            private readonly XDocument currentDoc;
            private readonly Dictionary<string, object> redirectDict;
            private readonly MiyagiSystem system;

            #endregion Fields

            #region Constructors

            public Reader(XDocument element, MiyagiSystem system)
            {
                this.system = system;
                this.currentDoc = element;
                this.redirectDict = new Dictionary<string, object>();
                this.ReadManagers();
            }

            private Reader(XmlReader xr, MiyagiSystem system)
                : this(XDocument.Load(xr), system)
            {
            }

            #endregion Constructors

            #region Methods

            #region Public Static Methods

            public static void Read(Stream stream, MiyagiSystem system)
            {
                using (var xtr = new XmlTextReader(stream))
                {
                    new Reader(xtr, system);
                }
            }

            #endregion Public Static Methods

            #region Private Methods

            private object GetObjectFromXmlDoc(XElement typeElement, object parentObject)
            {
                XAttribute typeAtt = typeElement.Attribute("Type");
                Type type = typeAtt != null
                                ? Type.GetType(typeAtt.Value)
                                : parentObject.GetType().GetProperty(typeElement.Name.ToString()).PropertyType;

                return this.GetObjectFromXmlDoc(typeElement, type);
            }

            private object GetObjectFromXmlDoc(XElement typeElement, Type type)
            {
                object retValue;

                if (type.HasAttribute<SerializableTypeAttribute>())
                {
                    retValue = this.ReadType(typeElement, type);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
                {
                    retValue = Activator.CreateInstance(type);
                    this.ReadEnumerable(typeElement, retValue);
                }
                else
                {
                    retValue = ConvertFromInvariantString(type, typeElement.Value);
                }

                return retValue;
            }

            private void ReadEnumerable(XContainer element, object enumerable)
            {
                Type type = enumerable.GetType();

                if (enumerable is IDictionary)
                {
                    var items = element.Elements().ToList();
                    int count = items.Count();
                    var dict = (IDictionary)enumerable;
                    for (int i = 0; i < count; i++)
                    {
                        var item = items[i];
                        var types = type.GetGenericArguments();
                        dict.Add(
                            this.GetObjectFromXmlDoc(item.Element("Key"), types[0]),
                            this.GetObjectFromXmlDoc(item.Element("Value"), types[1]));
                    }
                }
                else
                {
                    var items = element.Elements().ToList();
                    int count = items.Count();
                    var list = (IList)enumerable;

                    for (int i = 0; i < count; i++)
                    {
                        var item = items[i];

                        Type itemType = item.Attribute("Type") != null
                                            ? Type.GetType(item.Attribute("Type").Value)
                                            : type.IsGenericType
                                                  ? type.GetGenericArguments()[0]
                                                  : type.GetElementType();

                        list.Add(this.GetObjectFromXmlDoc(item, itemType));
                    }
                }
            }

            private void ReadManagers()
            {
                foreach (var managerElement in this.currentDoc.Descendants("Manager"))
                {
                    string managerType = managerElement.Attribute("Type").Value;
                    var data = new SerializationData();
                    var manager = this.system.GetManager(managerType);

                    foreach (var element in managerElement.Elements())
                    {
                        data.Add(element.Name.ToString(), this.GetObjectFromXmlDoc(element, manager));
                    }

                    manager.LoadSerializationData(data);
                }
            }

            private object ReadProperty(XElement element, PropertyInfo prop, object parentObject)
            {
                var att = prop != null ? prop.GetAttribute<SerializerOptionsAttribute>() ?? new SerializerOptionsAttribute() : new SerializerOptionsAttribute();

                return !string.IsNullOrEmpty(att.Redirect)
                           ? this.ReadRedirect(element.Value, parentObject)
                           : this.GetObjectFromXmlDoc(element, prop.PropertyType);
            }

            private object ReadRedirect(string name, object obj)
            {
                object redirectObj;
                this.redirectDict.TryGetValue(name, out redirectObj);

                if (redirectObj == null)
                {
                    var split = name.Split('/');
                    redirectObj = this.GetObjectFromXmlDoc(this.currentDoc.Element("Miyagi").Element("Redirects").Element(split[0]).Element(split[1]), obj);
                    this.redirectDict[name] = redirectObj;
                }

                return redirectObj;
            }

            private object ReadType(XContainer parentElement, Type type)
            {
                object retValue;

                if (typeof(IXmlWritable).IsAssignableFrom(type))
                {
                    var mi = type.GetMethod("CreateFromXml", BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, new[] { typeof(XElement), typeof(MiyagiSystem) }, null);
                    retValue = mi.Invoke(null, new object[] { parentElement.FirstNode.NextNode ?? parentElement.FirstNode, this.system });
                }
                else
                {
                    retValue = Activator.CreateInstance(type);
                    foreach (var element in parentElement.Elements())
                    {
                        var prop = type.GetProperty(element.Name.LocalName);
                        if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                        {
                            object enumerable = prop.GetValue(retValue, null);
                            this.ReadEnumerable(element, enumerable);
                        }
                        else
                        {
                            var value = this.ReadProperty(element, prop, retValue);
                            prop.SetValue(retValue, value, null);
                        }
                    }
                }

                return retValue;
            }

            #endregion Private Methods

            #endregion Methods
        }

        private sealed class Writer
        {
            #region Fields

            private readonly XElement currentDoc;
            private readonly XElement redirects;

            #endregion Fields

            #region Constructors

            private Writer(XmlTextWriter xtw, IEnumerable<KeyValuePair<IManager, SerializationData>> managerInfo)
            {
                xtw.Formatting = Formatting.Indented;
                xtw.WriteStartDocument();
                xtw.WriteComment("AUTOMATICALLY GENERATED - DO NOT EDIT THIS FILE");

                this.currentDoc = new XElement(
                    "Miyagi",
                    new XAttribute("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
                this.redirects = new XElement("Redirects");

                foreach (var managerData in managerInfo)
                {
                    if (managerData.Value.HasData)
                    {
                        var managerElement = new XElement("Manager", new XAttribute("Type", managerData.Key.Type));
                        SerializationData data = managerData.Value;
                        if (data.HasData)
                        {
                            foreach (var entry in data.Where(entry => entry.Value != null))
                            {
                                this.AddObjectToXmlDoc(managerElement, entry.Value, entry.Key);
                            }
                        }

                        this.currentDoc.Add(managerElement);
                    }
                }

                this.currentDoc.Add(this.redirects);
                this.currentDoc.WriteTo(xtw);
            }

            #endregion Constructors

            #region Methods

            #region Public Static Methods

            public static void Write(Stream stream, Encoding encoding, IEnumerable<KeyValuePair<IManager, SerializationData>> managerInfo)
            {
                var xtw = new XmlTextWriter(stream, encoding);
                new Writer(xtw, managerInfo);
                xtw.Flush();
            }

            #endregion Public Static Methods

            #region Private Static Methods

            private static void WriteTypeAttribute(XElement parentElement, Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }

                if (parentElement.Attribute("Type") == null)
                {
                    parentElement.Add(
                        type.Assembly != Assembly.GetExecutingAssembly()
                            ? new XAttribute("Type", type.AssemblyQualifiedName)
                            : new XAttribute("Type", type.FullName));
                }
            }

            #endregion Private Static Methods

            #region Private Methods

            private XElement AddObjectToXmlDoc(XContainer parentElement, object obj, string name)
            {
                var childElement = new XElement(name);
                Type type = obj.GetType();

                if (type.HasAttribute<SerializableTypeAttribute>())
                {
                    this.WriteType(childElement, type, obj);
                }
                else if (obj is IEnumerable && type != typeof(string))
                {
                    this.WriteEnumerable(childElement, (IEnumerable)obj, name);
                }
                else
                {
                    childElement.Add(ConvertToInvariantString(obj));
                }

                parentElement.Add(childElement);
                return childElement;
            }

            private void WriteEnumerable(XContainer parentElement, IEnumerable enumerable, string name)
            {
                string itemName = name.EndsWith("s") ? name.Remove(name.Length - 1) : "Item";

                if (enumerable is IDictionary)
                {
                    var dict = (IDictionary)enumerable;
                    foreach (DictionaryEntry entry in dict)
                    {
                        var entryElement = new XElement(itemName);
                        this.AddObjectToXmlDoc(entryElement, entry.Key, "Key");
                        this.AddObjectToXmlDoc(entryElement, entry.Value, "Value");
                        parentElement.Add(entryElement);
                    }
                }
                else
                {
                    foreach (object entry in enumerable)
                    {
                        this.AddObjectToXmlDoc(parentElement, entry, itemName);
                    }
                }
            }

            private void WriteProperty(XContainer parentElement, object obj, string name, PropertyDescriptor prop)
            {
                SerializerOptionsAttribute att = prop != null ? prop.GetAttribute<SerializerOptionsAttribute>() ?? new SerializerOptionsAttribute() : new SerializerOptionsAttribute();

                if (!string.IsNullOrEmpty(att.Redirect))
                {
                    string newName = name;

                    var namable = obj as INamable;
                    if (namable != null)
                    {
                        newName = namable.Name;
                    }

                    parentElement.Add(new XElement(name, att.Redirect + "/" + newName));
                    this.WriteRedirect(att.Redirect, obj, newName);
                }
                else
                {
                    this.AddObjectToXmlDoc(parentElement, obj, name);
                }
            }

            private void WriteRedirect(string redirect, object obj, string name)
            {
                XElement redirectElement;
                if (this.redirects.Element(redirect) != null)
                {
                    redirectElement = this.redirects.Element(redirect);
                }
                else
                {
                    redirectElement = new XElement(redirect);
                    this.redirects.Add(redirectElement);
                }

                if (redirectElement.Element(name) == null)
                {
                    var objElement = this.AddObjectToXmlDoc(redirectElement, obj, name);
                    WriteTypeAttribute(objElement, obj.GetType());
                }
            }

            private void WriteType(XElement parentElement, Type type, object obj)
            {
                if (obj is IXmlWritable)
                {
                    parentElement.Add(((IXmlWritable)obj).ToXElement());
                }
                else
                {
                    foreach (PropertyDescriptor desc in GetProperties(type))
                    {
                        object propertyValue = desc.GetValue(obj);
                        if (propertyValue != null)
                        {
                            this.WriteProperty(parentElement, propertyValue, desc.Name, desc);
                        }
                    }

                    WriteTypeAttribute(parentElement, type);
                }
            }

            #endregion Private Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}