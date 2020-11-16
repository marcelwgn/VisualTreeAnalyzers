using System.Collections.Generic;
using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;

namespace VisualTreeAnalyzers.Tests.TestImplementations
{
    class CountingElementAnalyzer : IElementAnalyzer
    {
        private List<FrameworkElement> visitedElements;
        private readonly bool visitChildren;
        private readonly bool shouldThrowWhenVisited;

        public CountingElementAnalyzer(List<FrameworkElement> elementCollection, bool shouldVisitChildren = true, bool shouldThrowWhenVisited = true)
        {
            visitedElements = elementCollection;
            visitChildren = shouldVisitChildren;
            this.shouldThrowWhenVisited = shouldThrowWhenVisited;
        }

        public void Analyze(FrameworkElement element)
        {
            if(!visitedElements.Contains(element))
            {
                visitedElements.Add(element);
            }
            else if(shouldThrowWhenVisited)
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
