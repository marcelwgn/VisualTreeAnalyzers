using System;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;

namespace VisualTreeAnalyzers.Snapshot.Exporter
{
    /// <summary>
    /// Converter that allows creating a XML representation of <see cref="IElementSnapshot"/> objects.
    /// Allows usage of <see cref="IObjectToStringConverter"/> objects to improve output.
    /// Also allows excluding null values from the XML output.
    /// </summary>
    public sealed class JsonExporter
    {
        private IObjectToStringConverter ObjectToStringConverter { get; }

        /// <summary>
        /// Creates a new <see cref="JSONExporter"/> object which will export all properties and use the <see cref="StandardObjectToStringConverter"/> for string representations.
        /// </summary>
        public JsonExporter() : this(new StandardObjectToStringConverter()) { }

        /// <summary>
        /// Creates a new <see cref="JSONExporter"/> object with the given parameters.
        /// </summary>
        /// <param name="converter">The converter to use for object string representation calculation. If none is specified, ToString will be returned.</param>
        public JsonExporter(IObjectToStringConverter converter)
        {
            ObjectToStringConverter = converter;
        }

        /// <summary>
        /// Creates an <see cref="XmlDocument"/> representing the provided snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot to convert to XML.</param>
        /// <param name="indluceNullAndEmptyValues">Indicates whether null values should be included in the export or not. Set to true to include null values.</param>
        /// <param name="includeNameSpaces">Determines whether the full namespace of an element will be included or not. Set to false to only include the type name but not the namespace.</param>
        /// <returns>The <see cref="XmlDocument"/> representation of the object.</returns>
        public JsonObject CreateJsonObject(IElementSnapshot snapshot, bool indluceNullAndEmptyValues, bool includeNameSpaces)
        {
            if (snapshot is null) throw new ArgumentNullException(nameof(snapshot));
            JsonObject jsonObject = new JsonObject();
            if (includeNameSpaces)
            {
                jsonObject.Add("type", JsonValue.CreateStringValue(snapshot.FullTypeName));
            }
            else
            {
                jsonObject.Add("type", JsonValue.CreateStringValue(NamespaceHelper.GetTypeName(snapshot.FullTypeName)));
            }

            // The XML export should be somewhat usable as XAML.
            // Because of that, the export will strip the Windows.UI.Xaml.Controls namespace.
            for (int i = 0; i < snapshot.PropertyNames.Count; i++)
            {
                if (indluceNullAndEmptyValues || !(snapshot.PropertyValues[i] is null))
                {
                    var name = snapshot.PropertyNames[i];
                    string stringValue;
                    if (ObjectToStringConverter is null)
                    {
                        stringValue = snapshot.PropertyValues[i]?.ToString() ?? "[null]";
                    }
                    else
                    {
                        stringValue = ObjectToStringConverter.GetStringRepresentation(snapshot.PropertyValues[i]) ?? "[null]";
                    }
                    if (indluceNullAndEmptyValues || !string.IsNullOrEmpty(stringValue))
                    {
                        jsonObject.Add(name, JsonValue.CreateStringValue(stringValue));
                    }
                }
            }

            var jsonArray = new JsonArray();
            for (int i = 0; i < snapshot.Children.Count; i++)
            {
                jsonArray.Add(CreateJsonObject(snapshot.Children[i], indluceNullAndEmptyValues, includeNameSpaces));
            }
            jsonObject.Add("children", jsonArray);
            return jsonObject;
        }

        /// <summary>
        /// Creates an <see cref="XmlDocument"/> representing the provided snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot to convert to a formatted XML string.</param>
        /// <param name="indluceNullAndEmptyValues">Indicates whether null values should be included in the export or not. Set to true to include null values.</param>
        /// <param name="includeNameSpaces">Determines whether the full namespace of an element will be included or not. Set to false to only include the type name but not the namespace.</param>
        /// <returns>The formatted XML string representation.</returns>
        public string CreateFormattedJSONString(IElementSnapshot snapshot, bool indluceNullAndEmptyValues, bool includeNameSpaces)
        {
            var rawJSON = CreateJsonObject(snapshot, indluceNullAndEmptyValues, includeNameSpaces).Stringify();
            return JsonHelper.FormatJson(rawJSON);
        }
    }
}
