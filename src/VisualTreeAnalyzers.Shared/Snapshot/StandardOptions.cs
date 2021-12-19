using System.Collections.Generic;

namespace VisualTreeAnalyzers.Snapshot
{
    /// <summary>
    /// This class holds default options that aim to make using the <see cref="Snapshot"/> namespace easier.
    /// </summary>
    public static class StandardOptions
    {
        /// <summary>
        /// The list of common properties to include in visual tree snapshots.
        /// </summary>
        public static IList<string> StandardPropertyNames { get; } = new string[]
        {
            "Name",
            "Visibility",
            "Margin",
            "Padding",
            "Background",
            "BorderBrush",
            "BorderThickness",
            "Foreground"
        };
    }
}
