using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// Verifies that a container with single selection has at most one item selected.
    /// A failure for this rule might occur if the peer of the element does not properly list the selection.
    /// </summary>
    [AccessibilityRule]
    public sealed class SingleSelectionRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Parent with SingleSelection should not have multiple items selected.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            if(peer?.GetPattern(PatternInterface.Selection) is ISelectionProvider provider)
            {
                if(provider.CanSelectMultiple == false)
                {
                    var selection = provider.GetSelection();
                    return selection == null || selection.Length < 2;
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
