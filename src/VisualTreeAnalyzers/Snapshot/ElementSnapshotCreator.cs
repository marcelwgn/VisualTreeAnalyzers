using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Snapshot
{
    public sealed class ElementSnapshotCreator
    {
        public FrameworkElement Root { get; set; }
        public IList<DependencyProperty> PropertiesToSnapshot { get; }

        private readonly IList<string> propertyNames;

        public ElementSnapshotCreator(FrameworkElement root, IList<DependencyProperty> properties, IList<string> propertyNames)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (properties == null) throw new ArgumentNullException(nameof(root));
            if (propertyNames == null) throw new ArgumentNullException(nameof(root));
            if (properties.Count != propertyNames.Count)
            {
                throw new ArgumentException("Property count and property name cound did not match.");
            }

            Root = root;
            PropertiesToSnapshot = properties;
            this.propertyNames = propertyNames;
        }

        public ElementSnapshot CreateTreeSnapshot()
        {
            return GetSnapshot(Root);
        }

        private ElementSnapshot GetSnapshot(DependencyObject element)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(element);
            var propertyValues = new object[propertyNames.Count];
            for (int i = 0; i < propertyNames.Count; i++)
            {
                try
                {
                    // Sometimes, get value might crash, e.g. when trying to access properties not present on 
                    // an object such as TextBlock.Text on a Grid
                    propertyValues[i] = element.GetValue(PropertiesToSnapshot[i]);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (System.Exception)
#pragma warning restore CA1031 // Do not catch general exception types
                {

                }
            }
            var childrenSnapshots = new ElementSnapshot[childCount];
            for (int i = 0; i < childCount; i++)
            {
                childrenSnapshots[i] = GetSnapshot(VisualTreeHelper.GetChild(element, i));
            }

            return new ElementSnapshot(childrenSnapshots, element.GetType().Name,
                element is FrameworkElement fwElement ? fwElement.Name : "Unknown",
                propertyNames, propertyValues);
        }

    }
}
