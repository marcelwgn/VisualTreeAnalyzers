using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// This rule checks if the passed element is expected to not be keyboard focusable.
    /// See the <see cref="ExpectKeyboardFocusableTrueRule"/> for the opposite rule.
    /// </summary>
    [AccessibilityRule]
    public sealed class ExpectKeyboardFocusableFalseRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "This control shouldn't be keyboard focusable.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            var type = peer?.GetAutomationControlType();
            if(type == AutomationControlType.Header
                || type == AutomationControlType.SemanticZoom
                || type == AutomationControlType.ScrollBar)
            {
                return !peer.IsKeyboardFocusable();
            }
            return true;
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
