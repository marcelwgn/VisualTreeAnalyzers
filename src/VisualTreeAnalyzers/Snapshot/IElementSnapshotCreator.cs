using System.Collections.Generic;
using Windows.UI.Xaml;

namespace VisualTreeAnalyzers.Snapshot
{
    public interface IElementSnapshotCreator
    {
        IList<string> PropertyNames { get; }
        DependencyObject Element { get; set; }
        IElementSnapshot CreateSnapshot();
        IElementSnapshot CreateSnapshot(DependencyObject element);
    }
}