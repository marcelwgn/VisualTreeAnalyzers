using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;

namespace VisualTreeAnalyzers.Tests.TestInfra.TestImplementations
{
    class AnalyzeInvokeCountAnalyzer : IElementAnalyzer
    {
        public int AnalyzeCount { get; private set; }
        public void Analyze(FrameworkElement element)
        {
            AnalyzeCount++;
        }

        public void ResetState()
        {
        }

        public bool ShouldVisitChildren(FrameworkElement element)
        {
            return true;
        }
    }
}
