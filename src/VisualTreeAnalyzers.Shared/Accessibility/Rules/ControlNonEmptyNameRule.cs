using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// This rule checks if the controls has a non empty name if it has a peer.
    /// </summary>
    [AccessibilityRule]
    public sealed class ControlNonEmptyNameRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Name should not be empty or null.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            if(peer != null)
            {
                if(peer.GetAutomationControlType() == AutomationControlType.Text 
                    || peer.GetAutomationControlType() == AutomationControlType.Image
                    || peer.IsKeyboardFocusable())
                {
                    return !string.IsNullOrWhiteSpace(peer.GetName());
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
