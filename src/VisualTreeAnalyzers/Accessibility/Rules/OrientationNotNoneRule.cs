using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// Verifies that the scrollable controls have an orientation.
    /// </summary>
    [AccessibilityRule]
    public sealed class OrientationNotNoneRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Orientation should be set on Scrollbars and Tabs";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            if(peer?.GetAutomationControlType() == AutomationControlType.ScrollBar
                || peer?.GetAutomationControlType() == AutomationControlType.Tab)
            {
                return peer?.GetOrientation() != AutomationOrientation.None;
            }
            return true;
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
