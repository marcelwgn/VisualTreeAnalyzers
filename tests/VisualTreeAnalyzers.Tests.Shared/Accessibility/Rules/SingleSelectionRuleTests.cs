using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using VisualTreeAnalyzers.Tests.UWP.Accessibility.Rules.TestPeers;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.UWP.Accessibility.Rules
{
    [TestClass]
    public partial class SingleSelectionRuleTests
    {

        [UITestMethod]
        public void VerifyNoSelectionPeerPasses()
        {
            var button = new Button();
            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(button);

            var rule = new SingleSelectionRule();

            Assert.IsTrue(rule.IsValid(button, peer));
        }

        [UITestMethod]
        public void VerifyMultiSelectionIgnored()
        {
            var button = new Button();
            var peer = new SelectionPeer(button, 2)
            {
                CanSelectMultiple = true
            };

            var rule = new SingleSelectionRule();

            Assert.IsTrue(rule.IsValid(button, peer));
        }

        [UITestMethod]
        public void VerifySelectionCountCorrectlyClassified()
        {
            VerifyCount(0, true);
            VerifyCount(1, true);
            VerifyCount(2, false);
            VerifyCount(3, false);

            void VerifyCount(int count, bool shouldPass)
            {
                var button = new Button();
                var peer = new SelectionPeer(button, count);

                var rule = new SingleSelectionRule();

                Assert.AreEqual(shouldPass, rule.IsValid(button, peer));
            }
        }
    }
}
