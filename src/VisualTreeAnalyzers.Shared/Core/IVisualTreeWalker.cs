using Windows.UI.Xaml;

namespace VisualTreeAnalyzers.Core
{
    /// <summary>
    /// The default interface for visual tree walker objects.
    /// </summary>
    public interface IVisualTreeWalker
    {
        /// <summary>
        /// The <see cref="FrameworkElement"/> indicating the root of the visual tree to analyze.
        /// </summary>
        FrameworkElement Root { get; }

        /// <summary>
        /// Scans the visual tree starting with the <see cref="Root"/> element specified for this object.
        /// </summary>
        void ScanVisualTree();
    }
}