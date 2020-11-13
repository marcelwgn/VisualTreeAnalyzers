using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using VisualTreeAnalyzers.Accessibility;
using VisualTreeAnalyzers.Accessibility.Rules;
using VisualTreeAnalyzers.Core;
using VisualTreeAnalyzers.Tests.DemoVisualTrees;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace VisualTreeAnalyzers.Tests.Accessibility
{
    [TestClass]
    public class AccessibilityAnalyzerTests
    {

        [UITestMethod]
        public void VerifyNamesGetChecked()
        {
            VerifyControlMarked(new Button(), true);
            VerifyControlMarked(new Button() { Content = "Text" }, false);
            VerifyControlMarked(new TextBlock(), true);
            VerifyControlMarked(new TextBlock() { Text = "" }, true);
            VerifyControlMarked(new TextBox() { Header = "Text" }, false);
            VerifyControlMarked(new StackPanel(), false);
            VerifyControlMarked(new Rectangle(), false);

            void VerifyControlMarked(FrameworkElement element, bool shouldBeMarked)
            {
                App.Content = element;
                ToolTipService.SetToolTip(element, "");

                new AccessibilityAnalyzer().Analyze(element);

                Assert.AreEqual(shouldBeMarked, IsMarkedProblematic(element));
            }
        }

        [UITestMethod]
        public void VerifyRemovingRulesWorks()
        {
            var button = new Button();

            var analyzer = new AccessibilityAnalyzer();
            analyzer.AccessibilityRules.Clear();
            analyzer.Analyze(button);

            Assert.AreEqual(false, IsMarkedProblematic(button));
        }

        [UITestMethod]
        public void VerifyPerformanceRepeated()
        {
            Stopwatch sw = new Stopwatch();
            var element = new Button();
            App.Content = element;
            var analyzer = new AccessibilityAnalyzer();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                analyzer.Analyze(element);
            }

            sw.Stop();
            // Verify that scanning 1000 items doesn't take too long.
            Assert.IsTrue(sw.ElapsedMilliseconds < 1000);
            Logger.LogMessage("Elapsed time: " + sw.ElapsedMilliseconds.ToString());
        }

        [UITestMethod]
        public void VerifyPerformanceTreeWalker()
        {
            Stopwatch sw = new Stopwatch();
            App.Content = new PageWithFlatAndNestedLayout();
            var analyzer = new AccessibilityAnalyzer();
            var walker = new VisualTreeWalker(App.Content, analyzer);

            sw.Start();

            walker.ScanVisualTree();

            sw.Stop();
            Assert.IsTrue(sw.ElapsedMilliseconds < 2000);
            Logger.LogMessage("Elapsed time: " + sw.ElapsedMilliseconds.ToString());
        }

        [UITestMethod]
        public void ClearsStateCorrectly()
        {
            var mainButton = new Button();
            AutomationProperties.SetLandmarkType(mainButton, AutomationLandmarkType.Main);

            var analyzer = new AccessibilityAnalyzer();
            analyzer.AccessibilityRules.Clear();
            analyzer.AccessibilityRules.Add(new LandmarkTypeMainOnceRule());
            analyzer.Analyze(mainButton);
            analyzer.Analyze(mainButton);
            Assert.IsTrue(IsMarkedProblematic(mainButton));

            mainButton = new Button();
            AutomationProperties.SetLandmarkType(mainButton, AutomationLandmarkType.Main);
            Assert.IsFalse(IsMarkedProblematic(mainButton));

            analyzer.ResetState();
            analyzer.Analyze(mainButton);
            Assert.IsFalse(IsMarkedProblematic(mainButton));
        }

        private static bool IsMarkedProblematic(FrameworkElement element)
        {
            // If the inaccessible element is a control, we will give it a red border
            if (element is Control control)
            {
                if (control.BorderBrush is SolidColorBrush brush)
                {
                    return brush.Color == Colors.Red;
                }
                return false;
            }
            else
            {
                return !string.IsNullOrEmpty((ToolTipService.GetToolTip(element) as string));
            }
        }

    }
}
