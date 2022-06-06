using Windows.UI.Xaml;

namespace VisualTreeAnalyzers.Core
{
    /// <summary>
    /// The default interface used by the <see cref="VisualTreeWalker"/> to analyze elements.
    /// To improve performance, element analyzers can opt out of visiting the children of an element.
    /// </summary>
    public interface IElementAnalyzer
    {
        /// <summary>
        /// Determines if the current analyzer requests to visit the children of this element.
        /// </summary>
        /// <param name="element">The element whose children should be visited.</param>
        /// <returns>Returns true when the children should be visited, false if not.</returns>
        bool ShouldVisitChildren(FrameworkElement element);

        /// <summary>
        /// Analyzes and process the passed element.
        /// </summary>
        /// <param name="element">The element to process.</param>
        void Analyze(FrameworkElement element);

        /// <summary>
        /// This method will be called form the <see cref="VisualTreeWalker"/> when a new scan will start and previous state can not be used anymore.
        /// </summary>
        void ResetState();
    }
}
