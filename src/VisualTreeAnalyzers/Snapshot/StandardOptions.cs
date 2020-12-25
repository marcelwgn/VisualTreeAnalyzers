using System.Collections.Generic;

namespace VisualTreeAnalyzers.Snapshot
{
    public static class StandardOptions
    {
        public static IList<string> StandardPropertyNames { get; } = new string[]
        {
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
