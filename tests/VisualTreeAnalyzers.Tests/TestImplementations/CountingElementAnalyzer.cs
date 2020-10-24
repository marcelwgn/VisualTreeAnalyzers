using System.Collections.Generic;
using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;

namespace VisualTreeAnalyzers.Tests
{
    class CountingElementAnalyzer : IElementAnalyzer
    {
        private List<FrameworkElement> visitedElements;
        private bool visitChildren;

        public CountingElementAnalyzer(List<FrameworkElement> elementCollection, bool shouldVisitChildren = true)
        {
            visitedElements = elementCollection;
            visitChildren = shouldVisitChildren;
        }

        public void Analyze(FrameworkElement element)
        {
            if(!visitedElements.Contains(element))
            {
                visitedElements.Add(element);
            }
            else
            {
                throw new System.InvalidOperationException("Error: Element already visited.");
            }
        }

        public void ResetState() { }

        public bool ShouldVisitChildren(FrameworkElement element)
        {
            return visitChildren;
        }
    }
}
