using System;
using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// This is an accessibility rule checking if the name of an element does not include the type of the datacontext.
    /// </summary>
    [AccessibilityRule]
    public sealed class ControlNameNotIncludeTypeRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Name should not be type name or datacontext type name.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            var name = peer?.GetName();
            // Null or empty string is not handled by this rule, so it complies.
            if(string.IsNullOrEmpty(name))
            {
                return true;
            }

            var automationType = peer?.GetAutomationControlType();
            // These control types are hard to name without their type and custom types are hard to justify.
            // Also descriptions that are fairly long are usually descriptive enough.
            if(automationType == AutomationControlType.AppBar ||
                automationType == AutomationControlType.Custom ||
                automationType == AutomationControlType.Header ||
                automationType == AutomationControlType.MenuBar||
                automationType == AutomationControlType.SemanticZoom ||
                automationType == AutomationControlType.StatusBar || 
                automationType == AutomationControlType.ToolBar || 
                automationType == AutomationControlType.Text ||
                name.Length > 40)
            {
                return true;
            }

            // Check if control or automation control type is included.
            if(name.Contains(element?.GetType().ToString(), StringComparison.OrdinalIgnoreCase) ||
                name.Contains(peer?.GetAutomationControlType().ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Check if the name is the datacontext name.
            return !name.Equals(element?.DataContext?.GetType().ToString(), StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
