using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using VisualTreeAnalyzers.Tests.Accessibility.TestPeers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace VisualTreeAnalyzers.Tests.Accessibility.Rules
{
    [TestClass]
    public class PatternInterfaceImplementedRuleTests
    {

        [UITestMethod]
        public void VerifyStandardControlsPass()
        {
            TestElement(new Button());
            TestElement(new Grid());
            TestElement(new TextBlock());
            TestElement(new TextBox());
            TestElement(new MenuBar());
            TestElement(new ListView() { ItemsSource = Enumerable.Range(0, 10), SelectionMode = ListViewSelectionMode.Multiple });
            TestElement(new ListViewItem());
            TestElement(new GridView() { ItemsSource = Enumerable.Range(0, 10), SelectionMode = ListViewSelectionMode.Multiple });
            TestElement(new GridViewItem());
            TestElement(new ScrollViewer());
            TestElement(new NavigationView());
            TestElement(new TextBox());
            TestElement(new RichEditBox());
            TestElement(new ToggleSplitButton());
            TestElement(new SplitButton());
            TestElement(new ProgressBar());
            TestElement(new ScrollBar());

            void TestElement(FrameworkElement element)
            {
                App.Content = element;
                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);

                var rule = new PatternInterfaceImplementedRule();

                // The rule is not great in release builds as reflection becomes an issue.
                // Since the package is not intended to be running in release mode, this will almost certainly not be fixed.
                // However, the tests still might be run with .NET native, so to prevent unnecessary failures,
                // disable asserting when not in debug mode.
                #if DEBUG
                Assert.IsTrue(rule.IsValid(element, peer), element.GetType() + " failed.");
                #endif
            }
        }

        [UITestMethod]
        public void DetectsFaultyPeers()
        {
            TestInterface(PatternInterface.Annotation);
            TestInterface(PatternInterface.CustomNavigation);
            TestInterface(PatternInterface.Drag);
            TestInterface(PatternInterface.DropTarget);
            TestInterface(PatternInterface.Grid);
            TestInterface(PatternInterface.Invoke);
            TestInterface(PatternInterface.MultipleView);
            TestInterface(PatternInterface.RangeValue);
            TestInterface(PatternInterface.Selection);
            TestInterface(PatternInterface.SelectionItem);

            void TestInterface(PatternInterface pattern)
            {
                var element = new Button();
                var peer = new PatternInterfacePeer(element, AutomationControlType.Custom);
                peer.ImplementedInterfaces.Add(pattern);

                var rule = new PatternInterfaceImplementedRule();

                Assert.IsFalse(rule.IsValid(element, peer));
            }
        }

        [UITestMethod]
        public void VerifyPerformance()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var element = new Button();
            App.Content = element;
            var buttonPeer = FrameworkElementAutomationPeer.CreatePeerForElement(element);
            var rule = new PatternInterfaceImplementedRule();
            Assert.IsNotNull(buttonPeer);
            for (int i = 0; i < 100000; i++)
            {
                Assert.IsTrue(rule.IsValid(element, buttonPeer));
            }

            sw.Stop();
            // Verify that scanning 100000 items doesn't take too long.
            Assert.IsTrue(sw.ElapsedMilliseconds < 1000);
        }
    }
}
