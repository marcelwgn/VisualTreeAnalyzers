using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class OrientationNotNoneRuleTests
    {
        [UITestMethod]
        public void VerifyNormalElementsComply()
        {
            ProcessElement(new Button());
            ProcessElement(new ScrollBar());
            ProcessElement(new TextBlock());
            ProcessElement(new StackPanel());

            void ProcessElement(FrameworkElement element)
            {
                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);

                var rule = new OrientationNotNoneRule();

                Assert.IsTrue(rule.IsValid(element, peer));
            }
        }

        [UITestMethod]
        public void VerifyMissingOrientationFails()
        {
            TestType(AutomationControlType.ScrollBar);
            TestType(AutomationControlType.Tab);

            void TestType(AutomationControlType elementType)
            {
                var peer = new NoneOrientationPeer(new Button(),elementType);

                var rule = new OrientationNotNoneRule();

                Assert.IsFalse(rule.IsValid(null, peer));
            }
        }

    }
}
