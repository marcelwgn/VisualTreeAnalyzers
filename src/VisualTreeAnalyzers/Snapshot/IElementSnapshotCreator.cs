using System.Collections.Generic;
using Windows.UI.Xaml;

namespace VisualTreeAnalyzers.Snapshot
{
    /// <summary>
    /// Interface defining the default information for an element snapshot creator.
    /// 
    /// Element snapshot creators define a list of properties to save in every snapshot and an optional element that can be used as the visual tree element for snapshot creation.
    /// </summary>
    public interface IElementSnapshotCreator
    {
        /// <summary>
        /// The list of properties that will be saved in the snapshot.
        /// Note that the properties will be accessed using reflection and as such need to match the actual properties on the objects.
        /// </summary>
        IList<string> PropertyNames { get; }

        /// <summary>
        /// The element whose snapshot will be created and returned when calling <see cref="CreateSnapshot()"/>.
        /// </summary>
        DependencyObject Element { get; set; }

        /// <summary>
        /// Creates a snapshot of this elements <see cref="Element"/> object.
        /// </summary>
        /// <returns>The snapshot of <see cref="Element"/>.</returns>
        IElementSnapshot CreateSnapshot();

        /// <summary>
        /// Creates the snapshot of the element passed to this function.
        /// </summary>
        /// <param name="element">The element whose snapshot will be created.</param>
        /// <returns>The snapshot of the object passed to this function.</returns>
        IElementSnapshot CreateSnapshot(DependencyObject element);
    }
}