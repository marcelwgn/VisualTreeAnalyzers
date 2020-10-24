using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace VisualTreeAnalyzers.Tests.Accessibility.Rules
{
    [TestClass]
    public class ControlNonEmptyNameRuleTests
    {
        [UITestMethod]
        public void VerifyEmptyAutomationNameIsMarked()
        {
            VerifyControlMarked(new Button());
            VerifyControlMarked(new TextBlock());
            VerifyControlMarked(new TextBlock() { Text = "" });
            VerifyControlMarked(new TextBox() { Header = "" });

            void VerifyControlMarked(FrameworkElement element)
            {
                App.Content = element;

                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);
                var rule = new ControlNonEmptyNameRule();
                Assert.IsFalse(rule.IsValid(element, peer));
            }
        }

        [UITestMethod]
        public void VerifyNonFocusableControlIsNotMarked()
        {
            var button = new Button()
            {
                IsTabStop = false
            };

            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(button);
            var rule = new ControlNonEmptyNameRule();
            Assert.IsTrue(rule.IsValid(button, peer));
        }


        [UITestMethod]
        public void VerifyElementsWithAutomationNameNotMarked()
        {
            VerifyControlMarked(new Button() { Content = "Text" });
            VerifyControlMarked(new TextBlock() { Text = "Text" });
            VerifyControlMarked(new TextBox() { PlaceholderText = "Text" });
            VerifyControlMarked(new TextBox() { Header = "Text" });

            void VerifyControlMarked(FrameworkElement element)
            {
                App.Content = element;

                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);
                var rule = new ControlNonEmptyNameRule();
                Assert.IsTrue(rule.IsValid(element, peer));
            }
        }

        [UITestMethod]
        public void VerifyNonFocusableElementsShoudNotBeMarked()
        {

            VerifyControlNotMarked(new StackPanel());
            VerifyControlNotMarked(new Rectangle());
            VerifyControlNotMarked(new Grid());
            VerifyControlNotMarked(new Canvas());

            void VerifyControlNotMarked(FrameworkElement element)
            {
                App.Content = element;

                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);
                var rule = new ControlNonEmptyNameRule();
                Assert.IsTrue(rule.IsValid(element, peer));
            }
        }
    }
}
