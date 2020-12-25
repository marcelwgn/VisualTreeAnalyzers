using System.Collections.Generic;

namespace VisualTreeAnalyzers.Snapshot
{
    public interface IElementSnapshot
    {
        IList<IElementSnapshot> Children { get; }
        string ElementName { get; }
        IList<string> PropertyNames { get; }
        IList<object> PropertyValues { get; }
        string TypeName { get; }
    }
}