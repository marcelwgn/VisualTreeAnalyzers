using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTreeAnalyzers.Snapshot
{
    public sealed class ElementSnapshot
    {

        public IList<ElementSnapshot> Children { get; private set; }
        public string TypeName { get; }
        public string ElementName { get; }
        public IList<string> PropertyNames { get; }
        public IList<object> PropertyValues { get; }

        public ElementSnapshot(IList<ElementSnapshot> children, string typeName, string elementName, IList<string> propertyNames, IList<object> propertyValues)
        {
            Children = children;
            TypeName = typeName;
            ElementName = elementName;
            PropertyNames = propertyNames ?? Array.Empty<string>();
            PropertyValues = propertyValues ?? Array.Empty<object>();
        }
    }
}
