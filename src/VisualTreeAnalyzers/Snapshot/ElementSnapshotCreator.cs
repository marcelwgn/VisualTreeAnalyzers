using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Snapshot
{
    public sealed class ElementSnapshotCreator : IElementSnapshotCreator
    {
        public DependencyObject Element { get; set; }
        public IList<string> PropertyNames { get; }

        public ElementSnapshotCreator(IList<string> propertyNames) : this(propertyNames, null) { }

        public ElementSnapshotCreator(IList<string> propertyNames, DependencyObject element)
        {
            Element = element;
            PropertyNames = propertyNames ?? throw new ArgumentNullException(nameof(propertyNames));
        }

        public IElementSnapshot CreateSnapshot()
        {
            return CreateSnapshot(Element);
        }

        public IElementSnapshot CreateSnapshot(DependencyObject element)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));
            var childCount = VisualTreeHelper.GetChildrenCount(element);
            var propertyValues = new object[PropertyNames.Count];
            for (int i = 0; i < PropertyNames.Count; i++)
            {
                if (element?.GetType().GetProperty(PropertyNames[i]) is PropertyInfo propertyInfo)
                {
                    propertyValues[i] = propertyInfo.GetValue(element);
                }
            }
            var childrenSnapshots = new IElementSnapshot[childCount];
            for (int i = 0; i < childCount; i++)
            {
                childrenSnapshots[i] = CreateSnapshot(VisualTreeHelper.GetChild(element, i));
            }

            return new ElementSnapshot(childrenSnapshots, element.GetType().Name,
                element is FrameworkElement fwElement ? fwElement.Name : "Unknown",
                PropertyNames, propertyValues);
        }

    }
}
