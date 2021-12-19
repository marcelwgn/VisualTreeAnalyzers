using System;
using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// The purpose of this rule is to check whether the ListViewItems and GridViewItems have a valid name and are not using the type of the datacontext.
    /// </summary>
    [AccessibilityRule]
    public sealed class ListItemNameNotEqualDataContextRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Name should not equal data type of collection items.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            if(peer?.GetAutomationControlType() != AutomationControlType.ListItem
                || VisualTreeHelper.GetChildrenCount(element) == 0)
            {
                return true;
            }

            if(element is ListViewItem)
            {
                var child = VisualTreeHelper.GetChild(element,0) as FrameworkElement;

                if(child?.DataContext == null)
                {
                    child = VisualTreeHelper.GetChild(child, 0) as FrameworkElement;
                }
                if(child.DataContext == null)
                {
                    return true;
                }
                return !peer.GetName().Contains(child?.DataContext?.GetType().ToString(), StringComparison.InvariantCultureIgnoreCase);
            }
            else if(element is GridViewItem)
            {
                var child = VisualTreeHelper.GetChild(element, 0) as FrameworkElement;

                if (child?.DataContext == null)
                {
                    child = VisualTreeHelper.GetChild(child, 0) as FrameworkElement;
                }
                if (child.DataContext == null)
                {
                    return true;
                }
                return !peer.GetName().Contains(child?.DataContext?.GetType().ToString(), StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return true;
            }
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
