using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using VisualTreeAnalyzers.Accessibility;
using VisualTreeAnalyzers.Accessibility.Rules;
using VisualTreeAnalyzers.Core;
using VisualTreeAnalyzers.Tests.UWP.TestInfra.DemoVisualTrees;
using VisualTreeAnalyzers.Tests.UWP.TestInfra.Utils;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace VisualTreeAnalyzers.Tests.UWP.Accessibility
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

        [TestMethod]
        [Timeout(10000)]
        public void VerifyPerformanceSameItemWithSkippingBehavior()
        {
            RunOnUIThread.Execute(() =>
            {
                Stopwatch sw = new Stopwatch();
                var element = new Button();
                App.Content = element;
                var analyzer = new AccessibilityAnalyzer(false);
                analyzer.Analyze(element);

                sw.Start();
                for (int i = 0; i < 10000; i++)
                {
                    analyzer.Analyze(element);
                }

                sw.Stop();

                // The item is already flagged, scanning it should take not much time.
                Assert.IsTrue(AccessibilityAnalyzer.GetAccessibilityAnalyzerViolationCount(element) > 0);
                Assert.IsTrue(sw.ElapsedMilliseconds < 200);
                Logger.LogMessage("Elapsed time: " + sw.ElapsedMilliseconds.ToString());
            });
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
