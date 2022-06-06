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
    public class LandmarkTypeMainOnceRuleTests
    {
        [UITestMethod]
        public void VerifyTreeComplyBehavior()
        {
            var noMainPeer = FrameworkElementAutomationPeer.CreatePeerForElement(new Button());
            var mainButton = new Button();
            AutomationProperties.SetLandmarkType(mainButton, AutomationLandmarkType.Main);
            var mainPeer = FrameworkElementAutomationPeer.CreatePeerForElement(mainButton);
            var rule = new LandmarkTypeMainOnceRule();

            Assert.IsTrue(rule.IsValid(null, noMainPeer));

            Assert.IsTrue(rule.IsValid(null, mainPeer));

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(rule.IsValid(null, noMainPeer));
            }
        }

        [UITestMethod]
        public void VerifyFailingTreeFails()
        {
            var noMainPeer = FrameworkElementAutomationPeer.CreatePeerForElement(new Button());
            var mainButton = new Button();
            AutomationProperties.SetLandmarkType(mainButton, AutomationLandmarkType.Main);
            var mainPeer = FrameworkElementAutomationPeer.CreatePeerForElement(mainButton);
            var rule = new LandmarkTypeMainOnceRule();

            Assert.IsTrue(rule.IsValid(null, noMainPeer));

            Assert.IsTrue(rule.IsValid(null, mainPeer));

            Assert.IsTrue(rule.IsValid(null, noMainPeer));

            Assert.IsFalse(rule.IsValid(null, mainPeer));

            Assert.IsTrue(rule.IsValid(null, noMainPeer));

            var stackPanel = new StackPanel();
            AutomationProperties.SetLandmarkType(stackPanel, AutomationLandmarkType.Main);
        }

        [UITestMethod]
        public void ClearsStateCorrectly()
        {
            var mainButton = new Button();
            AutomationProperties.SetLandmarkType(mainButton, AutomationLandmarkType.Main);
            var mainPeer = FrameworkElementAutomationPeer.CreatePeerForElement(mainButton);
            var rule = new LandmarkTypeMainOnceRule();

            Assert.IsTrue(rule.IsValid(mainButton, mainPeer));
            Assert.IsFalse(rule.IsValid(mainButton, mainPeer));

            rule.ResetState();
            Assert.IsTrue(rule.IsValid(mainButton, mainPeer));
        }
    }
}
