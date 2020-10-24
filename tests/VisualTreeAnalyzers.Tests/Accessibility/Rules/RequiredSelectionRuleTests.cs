using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using VisualTreeAnalyzers.Tests.Accessibility.Rules.TestPeers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.Accessibility.Rules
{
    [TestClass]
    public class RequiredSelectionRuleTests
    {
        [UITestMethod]
        public void VerifyNoItemSelectedFails()
        {
            var button = new Button();
            var peer = new SelectionPeer(button, 0)
            {
                IsSelectionRequired = true
            };
            var rule = new RequiredSelectionRule();

            Assert.IsFalse(rule.IsValid(button,peer));
        }


        [UITestMethod]
        public void VerifyOneItemSelectedComplies()
        {
            var button = new Button();
            var peer = new SelectionPeer(button, 1)
            {
                IsSelectionRequired = true
            };
            var rule = new RequiredSelectionRule();

            Assert.IsTrue(rule.IsValid(button, peer));
        }

    }
}
