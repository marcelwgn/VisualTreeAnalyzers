using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// This rule verifies that a container with required selection has an element selected.
    /// A failure for this rule might occur if the peer of the element does not properly list the selection.
    /// </summary>
    [AccessibilityRule]
    public sealed class RequiredSelectionRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Required selection must have one item selected.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            if (peer?.GetPattern(PatternInterface.Selection) is ISelectionProvider provider)
            {
                if (provider.IsSelectionRequired)
                {
                    return provider.GetSelection().Length > 0;
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
