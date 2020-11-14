using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Core;
using VisualTreeAnalyzers.Tests.DemoVisualTrees;
using VisualTreeAnalyzers.Tests.TestImplementations;
using VisualTreeAnalyzers.Tests.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.Core
{
    [TestClass]
    public class VisualTreeWalkerTests
    {
        [TestMethod]
        [Timeout(10000)]
        public void VerifyAllElementsVisitedSimplePage()
        {
            RunOnUIThread.Execute(() =>
            {
                VerifyElementsVisitedCount(new SimplePage(), 3);
            });
        }

        [TestMethod]
        [Timeout(10000)]
        public void VerifyAllElementsVisitedFlatStructure()
        {
            RunOnUIThread.Execute(() =>
            {
                VerifyElementsVisitedCount(new PageWithFlatStructure(), 302);
            });
        }

        [TestMethod]
        [Timeout(10000)]
        public void VerifyAllElementsVisitedDeepNesting()
        {
            RunOnUIThread.Execute(() =>
            {
                VerifyElementsVisitedCount(new PageWithDeepNesting(), 203);
            });
        }

        [TestMethod]
        [Timeout(10000)]
        public void VerifyAllElementsVisitedFlatAndNested()
        {
            RunOnUIThread.Execute(() =>
            {
                VerifyElementsVisitedCount(new PageWithFlatAndNestedLayout(), 6602);
            });
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
