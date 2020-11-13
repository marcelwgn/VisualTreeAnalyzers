using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
            var gridViewItem = new GridViewItem();
            AutomationProperties.SetName(gridViewItem, "Windows.UI.Xaml.Controls.Button");
            var content = new Button()
            {
                DataContext = new Button()
            };
            gridViewItem.Content = content;
            gridViewItem.DataContext = null;
            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(gridViewItem);
            App.Content = gridViewItem;

            var rule = new ListItemNameNotEqualDataContextRule();

            Assert.IsFalse(rule.IsValid(gridViewItem, peer));
        }

        [UITestMethod]
        public void VerifyDataContextOfDirectChildOfListViewItemWithNullDataContext()
        {
            var listViewItem = new ListViewItem();
            AutomationProperties.SetName(listViewItem, "Windows.UI.Xaml.Controls.Button");
            var content = new Button()
            {
                DataContext = new Button()
            };
            listViewItem.Content = content;
            listViewItem.DataContext = null;

            App.Content = listViewItem;

            var directChild = VisualTreeHelper.GetChild(listViewItem, 0) as FrameworkElement;
            directChild.DataContext = null;
            directChild = VisualTreeHelper.GetChild(directChild, 0) as FrameworkElement;
            directChild.DataContext = null;

            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(listViewItem);

            var rule = new ListItemNameNotEqualDataContextRule();

            Assert.IsTrue(rule.IsValid(listViewItem, peer));
        }

        [UITestMethod]
        public void VerifyDataContextOfDirectChildOfGridViewItemWithNullDataContext()
        {
            var gridViewItem = new GridViewItem();
            AutomationProperties.SetName(gridViewItem, "Windows.UI.Xaml.Controls.Button");
            var content = new Button()
            {
                DataContext = new Button()
            };
            gridViewItem.Content = content;
            gridViewItem.DataContext = null;

            App.Content = gridViewItem;

            var directChild = VisualTreeHelper.GetChild(gridViewItem, 0) as FrameworkElement;
            directChild.DataContext = null;
            directChild = VisualTreeHelper.GetChild(directChild, 0) as FrameworkElement;
            directChild.DataContext = null;

            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(gridViewItem);

            var rule = new ListItemNameNotEqualDataContextRule();

            Assert.IsTrue(rule.IsValid(gridViewItem, peer));
        }
    }
}
