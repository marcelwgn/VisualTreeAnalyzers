using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.Accessibility.Rules
{
    [TestClass]
    public class ListItemNameNotEqualDataContextRuleTests
    {

        [UITestMethod]
        public void VerifyDataContextOfDirectChildOfListViewItem()
        {
            var listViewItem = new ListViewItem();
            AutomationProperties.SetName(listViewItem, "Windows.UI.Xaml.Controls.Button");
            var content = new Button()
            {
                DataContext = new Button()
            };
            listViewItem.Content = content;
            listViewItem.DataContext = null;
            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(listViewItem);
            App.Content = listViewItem;

            var rule = new ListItemNameNotEqualDataContextRule();

            Assert.IsFalse(rule.IsValid(listViewItem, peer));
        }

        [UITestMethod]
        public void VerifyDataContextOfDirectChildOfGridViewItem()
        {
            var listViewItem = new GridViewItem();
            AutomationProperties.SetName(listViewItem, "Windows.UI.Xaml.Controls.Button");
            var content = new Button()
            {
                DataContext = new Button()
            };
            listViewItem.Content = content;
            listViewItem.DataContext = null;
            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(listViewItem);
            App.Content = listViewItem;

            var rule = new ListItemNameNotEqualDataContextRule();

            Assert.IsFalse(rule.IsValid(listViewItem, peer));
        }
    }
}
