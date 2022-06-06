using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.UWP.Accessibility.Rules
{
    [TestClass]
    public class ControlNameNotIncludeTypeRuleTests
    {

        [UITestMethod]
        public void VerifyCorrectNamesComply()
        {
            ProcessElement(new TextBlock() { Text = "Some text" });

            var button = new Button();
            AutomationProperties.SetName(button, "Some text");
            ProcessElement(button);

            AutomationProperties.SetName(button, "Some very very long text " +
                "that contains the word button but that is fine as we are descriptive enough.");
            ProcessElement(button);

            ProcessElement(new TextBox() { Header = "Some text" });

            void ProcessElement(FrameworkElement element)
            {
                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);

                var rule = new ControlNameNotIncludeTypeRule();

                Assert.IsTrue(rule.IsValid(element, peer));
            }
        }

        [UITestMethod]
        public void VerifyFaultyElementsFail()
        {

            var button = new Button()
            {
                DataContext = "SomeText"
            };

            ProcessElement(button, new CustomPeer(button, ReturnProperty.ElementDatacontext));
            ProcessElement(button, new CustomPeer(button, ReturnProperty.ElementType));
            ProcessElement(button, new CustomPeer(button, ReturnProperty.PeerControlType));

            void ProcessElement(FrameworkElement element, FrameworkElementAutomationPeer peer)
            {
                var rule = new ControlNameNotIncludeTypeRule();

                Assert.IsFalse(rule.IsValid(element, peer));
            }
        }

        [UITestMethod]
        public void VerifyEmptyStringComplies()
        {
            var peer = new CustomPeer(new Button(), ReturnProperty.EmptyString);
            var rule = new ControlNameNotIncludeTypeRule();

            Assert.IsTrue(rule.IsValid(new Button(), peer));
        }

        enum ReturnProperty
        {
            ElementType,
            ElementDatacontext,
            PeerControlType,
            EmptyString
        }

        class CustomPeer : FrameworkElementAutomationPeer
        {
            private ReturnProperty returnProperty;

            public CustomPeer(FrameworkElement owner, ReturnProperty nameRoute) : base(owner)
            {
                returnProperty = nameRoute;
            }

            protected override string GetNameCore()
            {
                switch (returnProperty)
                {
                    case ReturnProperty.ElementType:
                        return Owner.GetType().ToString();
                    case ReturnProperty.ElementDatacontext:
                        return (Owner as FrameworkElement).DataContext.GetType().ToString();
                    case ReturnProperty.PeerControlType:
                        return GetAutomationControlType().ToString();
                    case ReturnProperty.EmptyString:
                        return "";
                }
                return base.GetNameCore();
            }

            protected override AutomationControlType GetAutomationControlTypeCore()
            {
                return AutomationControlType.Button;
            }
        }
    }
}
