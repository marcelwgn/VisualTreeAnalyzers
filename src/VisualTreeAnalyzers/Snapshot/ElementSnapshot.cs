using System;
using System.Collections.Generic;

namespace VisualTreeAnalyzers.Snapshot
{
    public sealed class ElementSnapshot : IElementSnapshot
    {
        public IList<IElementSnapshot> Children { get; private set; }
        public string TypeName { get; }
        public string ElementName { get; }
        public IList<string> PropertyNames { get; }
        public IList<object> PropertyValues { get; }

        public ElementSnapshot(IList<IElementSnapshot> children, string typeName, string elementName, IList<string> propertyNames, IList<object> propertyValues)
        {
            Children = children;
            TypeName = typeName;
            ElementName = elementName;
            PropertyNames = propertyNames ?? Array.Empty<string>();
            PropertyValues = propertyValues ?? Array.Empty<object>();
        }
    }
}
