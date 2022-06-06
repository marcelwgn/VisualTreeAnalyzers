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
    public partial class ExpectKeyboardFocusableFalseRuleTests
    {

        [UITestMethod]
        public void VerifyElementsComply()
        {
            TestElement(new Button());
            TestElement(new TextBlock());
            TestElement(new ScrollBar());
            TestElement(new ScrollViewer());
            TestElement(new SemanticZoom());

            void TestElement(FrameworkElement element)
            {
                App.Content = element;
                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);

                var rule = new ExpectKeyboardFocusableFalseRule();

                Assert.IsTrue(rule.IsValid(element, peer));
            }
        }

        [UITestMethod]
        public void VerifyElementsFail()
        {
            TestElement(AutomationControlType.ScrollBar);
            TestElement(AutomationControlType.Header);
            TestElement(AutomationControlType.SemanticZoom);

            void TestElement(AutomationControlType type)
            {
                var peer = new ControlTypeFocusPeer(new Button(),type,true);

                var rule = new ExpectKeyboardFocusableFalseRule();

                Assert.IsFalse(rule.IsValid(new Button(), peer));
            }
        }
    }
}
