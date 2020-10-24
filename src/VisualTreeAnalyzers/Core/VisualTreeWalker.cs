using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Core
{
    /// <summary>
    /// The VisualTreeWalker can be used to scan entire subtrees of the VisualTree.
    /// Every analyzer added to the VisualTreeWalker will be called on all elements visited.
    /// The tree walker might not visit all nodes if no analyzer requests to visit the children of a specific element.
    /// 
    /// The VisualTreeWalker will always perform a breadth-first-search on the visual tree.
    /// </summary>
    public sealed class VisualTreeWalker
    {
        /// <summary>
        /// The <see cref="FrameworkElement"/> indicating the root of the visual tree to analyze.
        /// </summary>
        public FrameworkElement Root { get; set; }

        /// <summary>
        /// The collection of analyzers used by this VisualTreeWalker.
        /// </summary>
        public IList<IElementAnalyzer> Analyzers { get; } = new List<IElementAnalyzer>();

        /// <summary>
        /// Creates a new VisualTreeWalker object with the specified element as the root of the VisualTree to analyzer.
        /// </summary>
        /// <param name="root">The root of the visual tree that will be processed.</param>
        public VisualTreeWalker(FrameworkElement root)
        {
            Root = root;
        }

        private Queue<FrameworkElement> elementsToVisit = new Queue<FrameworkElement>();

        /// <summary>
        /// Creates a new VisualTreeWalker object with the specified element as the root of the VisualTree to analyzer.
        /// This also adds the specified analyzer to list of analyzers.
        /// </summary>
        /// <param name="root">The root of the visual tree that will be processed.</param>
        /// <param name="analyzer">The analyzer that will be added to the collection of analyzers.</param>
        public VisualTreeWalker(FrameworkElement root, IElementAnalyzer analyzer) : this(root)
        {
            Analyzers.Add(analyzer);
        }

        /// <summary>
        /// Scans the visual tree with the <see cref="Root"/> as the starting point of the tree scan.
        /// </summary>
        public void ScanVisualTree()
        {
            ScanElement(Root);
        }

        private void ScanElement(FrameworkElement rootElement)
        {
            elementsToVisit.Clear();

            foreach(var analyzer in Analyzers)
            {
                analyzer.ResetState();
            }

            if (rootElement == null)
            {
                return;
            }

            if (ProcessElementAndShouldVisitChildren(rootElement))
            {
                var childCount = VisualTreeHelper.GetChildrenCount(rootElement);
                if (childCount > 0)
                {
                    for (var i = 0; i < childCount; i++)
                    {
                        var child = VisualTreeHelper.GetChild(rootElement, i);
                        if (child is FrameworkElement childElement)
                        {
                            elementsToVisit.Enqueue(childElement);
                        }
                    }
                }
            }

            while(elementsToVisit.Count > 0)
            {
                var currentElement = elementsToVisit.Dequeue();
                if(ProcessElementAndShouldVisitChildren(currentElement))
                {
                    var childCount = VisualTreeHelper.GetChildrenCount(currentElement);
                    if (childCount > 0)
                    {
                        for (var i = 0; i < childCount; i++)
                        {
                            var child = VisualTreeHelper.GetChild(currentElement, i);
                            if (child is FrameworkElement childElement)
                            {
                                elementsToVisit.Enqueue(childElement);
                            }
                        }
                    }
                }
            }
        }

        private bool ProcessElementAndShouldVisitChildren(FrameworkElement element)
        {
            var childrenRequested = false;
            foreach (var analyzer in Analyzers)
            {
                analyzer.Analyze(element);
                if (analyzer.ShouldVisitChildren(element))
                {
                    childrenRequested = true;
                }
            }
            return childrenRequested;
        }
    }
}
