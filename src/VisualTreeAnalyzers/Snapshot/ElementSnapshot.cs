using System;
using System.Collections.Generic;

namespace VisualTreeAnalyzers.Snapshot
{
    /// <summary>
    /// Class representing a the snapshot of a visual tree.
    /// Every snapshot contains basic informations about the element such as its type name or element name but also contains a reference to its children elements.
    /// In addition to that, a snapshot contains a list of property names and values which are usually set by the <see cref="ElementSnapshotCreator"/>.
    /// </summary>
    public sealed class ElementSnapshot : IElementSnapshot
    {
        /// <inheritdoc/>
        public IList<IElementSnapshot> Children { get; private set; }

        /// <inheritdoc/>
        public string FullTypeName { get; }

        /// <inheritdoc/>
        public string ElementName { get; }

        /// <inheritdoc/>
        public IList<string> PropertyNames { get; }

        /// <inheritdoc/>
        public IList<object> PropertyValues { get; }

        /// <summary>
        /// Creates a new ElementSnapshot instance using the passed parameters.
        /// </summary>
        /// <param name="children">List of children elements.</param>
        /// <param name="typeName">Name of this elements type.</param>
        /// <param name="elementName">Name of the visual tree element.</param>
        /// <param name="propertyNames">List of names of the properties saved by this snapshot.</param>
        /// <param name="propertyValues">List of the property values.</param>
        public ElementSnapshot(IList<IElementSnapshot> children, string typeName, string elementName, IList<string> propertyNames, IList<object> propertyValues)
        {
            Children = children;
            FullTypeName = typeName;
            ElementName = elementName;
            PropertyNames = propertyNames ?? Array.Empty<string>();
            PropertyValues = propertyValues ?? Array.Empty<object>();
        }
    }
}
