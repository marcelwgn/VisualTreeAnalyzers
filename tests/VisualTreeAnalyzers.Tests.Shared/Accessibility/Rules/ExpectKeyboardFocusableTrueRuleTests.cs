using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using VisualTreeAnalyzers.Tests.UWP.Accessibility.TestPeers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace VisualTreeAnalyzers.Tests.UWP.Accessibility.Rules
{
    [TestClass]
    public class ExpectKeyboardFocusableTrueRuleTests
    {
        [UITestMethod]
        public void VerifyCommonControlsComply()
        {
            TestElement(new Button());
            TestElement(new TextBlock());
            TestElement(new TextBox());
            TestElement(new ScrollViewer());
            TestElement(new SemanticZoom());
            TestElement(new AutoSuggestBox());
            TestElement(new MenuBar());
            TestElement(new AppBarButton());
            TestElement(new Thumb());
            TestElement(new GridView());
            TestElement(new ScrollBar());
            TestElement(new Button() { IsEnabled = false });

            void TestElement(FrameworkElement element)
            {
                App.Content = element;
                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);

                var rule = new ExpectKeyboardFocusableTrueRule();

                Assert.IsTrue(rule.IsValid(element, peer));
            }
        }

        [UITestMethod]
        public void VerifyFaultyControlsFails()
        {
            TestElement(new Button());
            TestElement(new TextBox());
            TestElement(new AppBarButton());

            void TestElement(FrameworkElement element)
            {
                App.Content = element;
                var elementPeer = FrameworkElementAutomationPeer.CreatePeerForElement(element);
                var peer = new ControlTypeFocusPeer(element,elementPeer.GetAutomationControlType(),false);

                var rule = new ExpectKeyboardFocusableTrueRule();

                Assert.IsFalse(rule.IsValid(element, peer));
            }
        }
    }
}
