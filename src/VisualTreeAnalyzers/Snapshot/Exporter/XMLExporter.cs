using System;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;

namespace VisualTreeAnalyzers.Snapshot.Exporter
{
    /// <summary>
    /// Converter that allows creating a XML representation of <see cref="IElementSnapshot"/> objects.
    /// Allows usage of <see cref="IObjectToStringConverter"/> objects to improve output.
    /// Also allows excluding null values from the XML output.
    /// </summary>
    public sealed class XMLExporter
    {
        private IObjectToStringConverter ObjectToStringConverter { get; }

        /// <summary>
        /// Creates a new <see cref="XMLExporter"/> object which will export all properties and use the <see cref="StandardObjectToStringConverter"/> for string representations.
        /// </summary>
        public XMLExporter() : this(new StandardObjectToStringConverter()) { }

        /// <summary>
        /// Creates a new <see cref="XMLExporter"/> object with the given parameters.
        /// </summary>
        /// <param name="converter">The converter to use for object string representation calculation. If none is specified, ToString will be returned.</param>
        public XMLExporter(IObjectToStringConverter converter)
        {
            ObjectToStringConverter = converter;
        }

        /// <summary>
        /// Creates an <see cref="XmlDocument"/> represengint the provided snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot to convert to XML.</param>
        /// <param name="indluceNullAndEmptyValues">Indicates whether null values should be included in the export or not. Set to true to include null values.</param>
        /// <param name="includeNameSpaces">Determines whether the full namespace of an element will be included or not. Set to false to only include the type name but not the namespace.</param>
        /// <returns>The <see cref="XmlDocument"/> representation of the object.</returns>
        public XmlDocument CreateXMLDocument(IElementSnapshot snapshot, bool indluceNullAndEmptyValues, bool includeNameSpaces)
        {
            if (snapshot is null) throw new ArgumentNullException(nameof(snapshot));
            var xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(CreateXMLElement(xmlDocument, snapshot, indluceNullAndEmptyValues, includeNameSpaces));
            return xmlDocument;
        }

        /// <summary>
        /// Creates an <see cref="XmlDocument"/> represengint the provided snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot to convert to a formatted XML string.</param>
        /// <param name="indluceNullAndEmptyValues">Indicates whether null values should be included in the export or not. Set to true to include null values.</param>
        /// <param name="includeNameSpaces">Determines whether the full namespace of an element will be included or not. Set to false to only include the type name but not the namespace.</param>
        /// <returns>The formatted XML string representation.</returns>
        public string CreateFormattedXMLString(IElementSnapshot snapshot, bool indluceNullAndEmptyValues, bool includeNameSpaces)
        {
            var rawXML = CreateXMLDocument(snapshot, indluceNullAndEmptyValues, includeNameSpaces).GetXml();
            return XDocument.Parse(rawXML).ToString();
        }

        private XmlElement CreateXMLElement(XmlDocument document, IElementSnapshot snapshot, bool indluceNullAndEmptyValues, bool includeNameSpaces)
        {
            XmlElement xmlElement;
            if (includeNameSpaces)
            {
                xmlElement = document.CreateElement(snapshot.FullTypeName);
            }
            else
            {
                xmlElement = document.CreateElement(NamespaceHelper.GetTypeName(snapshot.FullTypeName));
            }

            // The XML export should be somewhat usable as XAML.
            // Because of that, the export will strip the Windows.UI.Xaml.Controls namespace.
            for (int i = 0; i < snapshot.PropertyNames.Count; i++)
            {
                if(indluceNullAndEmptyValues || !(snapshot.PropertyValues[i] is null))
                {
                    var attributeNode = document.CreateAttribute(snapshot.PropertyNames[i]);
                    if(ObjectToStringConverter is null)
                    {
                        attributeNode.Value = snapshot.PropertyValues[i]?.ToString() ?? "[null]";
                    }
                    else
                    {
                        attributeNode.Value = ObjectToStringConverter.GetStringRepresentation(snapshot.PropertyValues[i]) ?? "[null]";
                    }
                    if(indluceNullAndEmptyValues || !string.IsNullOrEmpty(attributeNode.Value))
                    {
                        xmlElement.Attributes.SetNamedItem(attributeNode);
                    }
                }
            }

            for (int i = 0; i < snapshot.Children.Count; i++)
            {
                xmlElement.AppendChild(CreateXMLElement(document, snapshot.Children[i], indluceNullAndEmptyValues, includeNameSpaces));
            }

            return xmlElement;
        }

    }
}
