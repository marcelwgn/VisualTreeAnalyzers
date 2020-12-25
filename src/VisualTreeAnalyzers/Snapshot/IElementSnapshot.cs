using System.Collections.Generic;

namespace VisualTreeAnalyzers.Snapshot
{
    /// <summary>
    /// Interface defining the default information for an element snapshot.
    /// 
    /// Every snapshot contains basic informations about the element such as its type name or element name but also contains a reference to its children elements.
    /// In addition to that, a snapshot contains a list of property names and values which are usually set by the <see cref="ElementSnapshotCreator"/>.
    /// </summary>
    public interface IElementSnapshot
    {
        /// <summary>
        /// The list of snapshots representing the visual tree children of this element,
        /// </summary>
        IList<IElementSnapshot> Children { get; }

        /// <summary>
        /// The name of the element this snapshot represents
        /// </summary>
        string ElementName { get; }

        /// <summary>
        /// The name of the elements type.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// List of the properties names saved in this snapshot.
        /// </summary>
        IList<string> PropertyNames { get; }

        /// <summary>
        /// List of the properties values saved in this snapshot.
        /// </summary>
        IList<object> PropertyValues { get; }
    }
}