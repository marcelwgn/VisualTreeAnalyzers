using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Core;
using VisualTreeAnalyzers.Tests.DemoVisualTrees;
using VisualTreeAnalyzers.Tests.TestImplementations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.Core
{
    [TestClass]
    public class VisualTreeWalkerTests
    {
        [UITestMethod]
        public void VerifyAllElementsVisitedSimplePage()
        {
            VerifyElementsVisitedCount(new SimplePage(), 3);
        }

        [UITestMethod]
        public void VerifyAllElementsVisitedFlatStructure()
        {
            VerifyElementsVisitedCount(new PageWithFlatStructure(), 302);
        }

        [UITestMethod]
        public void VerifyAllElementsVisitedDeepNesting()
        {
            VerifyElementsVisitedCount(new PageWithDeepNesting(), 203);
        }

        [UITestMethod]
        public void VerifyAllElementsVisitedFlatAndNested()
        {
            VerifyElementsVisitedCount(new PageWithFlatAndNestedLayout(), 6602);
        }

        [UITestMethod]
        public void VerifyVisitingOrderIsBreadthFirstSearch()
        {
            App.Content = new BreadthFirsSearchTestTree();
            var visitedItems = new List<FrameworkElement>();
            var analyzer = new CountingElementAnalyzer(visitedItems);
            var walker = new VisualTreeWalker(App.Content);
            walker.Analyzers.Add(analyzer);

            walker.ScanVisualTree();

            Assert.AreEqual(28, visitedItems.Count, "Should have visited every element exactly once.");
            for (int i = 0; i < visitedItems.Count; i++)
            {
                Assert.AreEqual(i.ToString(), visitedItems[i].Tag, "Should have visited items in specific orders");
            }
        }

        [UITestMethod]
        public void VerifySkippingBehaviorIsCorrect()
        {
            App.Content = new StackPanel()
            {
                Children =
                {
                    new Button(),
                    new Button(),
                    new Button(),
                    new Button(),
                    new Button()
                }
            };

            var visitedItems = new List<FrameworkElement>();
            var analyzer = new CountingElementAnalyzer(visitedItems, false);
            var walker = new VisualTreeWalker(App.Content, analyzer);

            walker.ScanVisualTree();

            Assert.AreEqual(1, visitedItems.Count, "Should have exactly one element.");
        }

        private static void VerifyElementsVisitedCount(FrameworkElement root, int visitedNoteCount)
        {
            App.Content = root;

            var visitedItems = new List<FrameworkElement>();
            var analyzer = new CountingElementAnalyzer(visitedItems);
            var walker = new VisualTreeWalker(root);
            walker.Analyzers.Add(analyzer);

            walker.ScanVisualTree();

            Assert.AreEqual(visitedNoteCount, visitedItems.Count, "Should have visited every element exactly once.");
        }
    }
}
