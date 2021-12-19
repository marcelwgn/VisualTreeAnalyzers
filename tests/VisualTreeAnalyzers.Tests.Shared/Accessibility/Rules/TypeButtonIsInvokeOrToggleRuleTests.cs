using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class TypeButtonIsInvokeOrToggleRuleTests
    {

        [UITestMethod]
        public void VerifyControlsPass()
        {
            TestElement(new Button());
            TestElement(new ToggleButton());
            TestElement(new TextBlock());
            TestElement(new TextBox());
            TestElement(new Grid());

            void TestElement(FrameworkElement element)
            {
                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);

                var rule = new TypeButtonIsInvokeOrToggleRule();

                Assert.IsTrue(rule.IsValid(element, peer));
            }
        }

        [UITestMethod]
        public void FaultyControlFails()
        {
            var button = new Button();
            var peer = new PatternInterfacePeer(button, AutomationControlType.Button);

            var rule = new TypeButtonIsInvokeOrToggleRule();

            Assert.IsFalse(rule.IsValid(button, peer));
        }
    }
}
