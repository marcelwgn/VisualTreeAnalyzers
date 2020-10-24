using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// This rule checks if the passed element is expected to be keyboard focusable.
    /// See the <see cref="ExpectKeyboardFocusableFalseRule"/> for the opposite rule.
    /// </summary>
    [AccessibilityRule]
    public sealed class ExpectKeyboardFocusableTrueRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Element should be focusable, but was not.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            var type = peer?.GetAutomationControlType();

            // These types are ignored
            if (type == AutomationControlType.AppBar
                || type == AutomationControlType.Custom
                || type == AutomationControlType.DataItem
                || type == AutomationControlType.Group
                || type == AutomationControlType.Image
                || type == AutomationControlType.Header
                || type == AutomationControlType.List
                || type == AutomationControlType.MenuBar
                || type == AutomationControlType.Pane
                || type == AutomationControlType.ScrollBar 
                || type == AutomationControlType.SemanticZoom
                || type == AutomationControlType.Separator
                || type == AutomationControlType.StatusBar
                || type == AutomationControlType.TitleBar
                || type == AutomationControlType.Thumb
                || type == AutomationControlType.Text
                || type == AutomationControlType.ToolBar
                || type == AutomationControlType.ToolTip
                || type == AutomationControlType.Window)
            {
                return true;
            }

            return !peer.IsEnabled() || peer.IsKeyboardFocusable();
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
