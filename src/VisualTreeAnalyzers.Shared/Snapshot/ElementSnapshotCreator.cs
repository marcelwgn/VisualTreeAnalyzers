using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Snapshot
{
    /// <summary>
    /// This class allows creating <see cref="IElementSnapshot"/> objects for visual tree elements.
    /// </summary>
    public sealed class ElementSnapshotCreator : IElementSnapshotCreator
    {
        /// <inheritdoc/>
        public DependencyObject Element { get; set; }

        /// <inheritdoc/>
        public IList<string> PropertyNames { get; }

        /// <summary>
        /// Creates a new <see cref="ElementSnapshotCreator"/> instance with the given list of property names.
        /// <see cref="Element"/> will be null for this instance unless changed later.
        /// </summary>
        /// <param name="propertyNames">List of the properties this object will save in the snapshots.</param>
        public ElementSnapshotCreator(IList<string> propertyNames) : this(propertyNames, null) { }

        /// <summary>
        /// Creates a new <see cref="ElementSnapshotCreator"/> instance with the given list of property names.
        /// </summary>
        /// <param name="propertyNames">List of the properties this object will save in the snapshots.</param>
        /// <param name="element">The element that will be used for <see cref="CreateSnapshot()"/></param>
        public ElementSnapshotCreator(IList<string> propertyNames, DependencyObject element)
        {
            Element = element;
            PropertyNames = propertyNames ?? throw new ArgumentNullException(nameof(propertyNames));
        }

        /// <inheritdoc/>
        public IElementSnapshot CreateSnapshot()
        {
            return CreateSnapshot(Element);
        }

        /// <inheritdoc/>
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

            return new ElementSnapshot(childrenSnapshots, element.GetType().FullName,
                element is FrameworkElement fwElement ? fwElement.Name : "Unknown",
                PropertyNames, propertyValues);
        }

    }
}
