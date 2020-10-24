using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using VisualTreeAnalyzers.Tests.Accessibility.TestPeers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.Accessibility.Rules
{
    [TestClass]
    public class LocalizedLandMarkTypeRuleTests
    {
        [UITestMethod]
        public void VerifyFailsForNullLocalizedLandMarkType()
        {
            var rule = new LocalizedLandMarkTypeRule();
            var peer = new LocalizedLandmarkPeer(new Button(), null);

            Assert.IsFalse(rule.IsValid(null, peer));
        }

        [UITestMethod]
        public void VerifyFailsForEmptyLocalizedLandMarkType()
        {
            var rule = new LocalizedLandMarkTypeRule();
            var peer = new LocalizedLandmarkPeer(new Button(), "");

            Assert.IsFalse(rule.IsValid(null, peer));
        }

        [UITestMethod]
        public void VerifySucceedsForExistingLocalizedLandMarkType()
        {
            var rule = new LocalizedLandMarkTypeRule();
            var peer = new LocalizedLandmarkPeer(new Button(), "Text");

            Assert.IsFalse(rule.IsValid(null, peer));
        }

        [UITestMethod]
        public void VerifyEmptyLocalizedLandMarkTypealidWithNoLandmarkType()
        {
            var rule = new LocalizedLandMarkTypeRule();
            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(new Button());

            Assert.IsTrue(rule.IsValid(null, peer));
        }

    }
}
