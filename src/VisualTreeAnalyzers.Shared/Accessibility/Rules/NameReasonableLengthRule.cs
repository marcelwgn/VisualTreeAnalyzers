using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// Determines whether the name of a control has a reasonable length, that is, it is not to short and not too long.
    /// </summary>
    [AccessibilityRule]
    public sealed class NameReasonableLengthRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Name should have a reasonable length. This issue might also be caused by icons being used as content or name.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            // This only cares about the length.
            // If a name should be null or not is handled by another rule.
            if(peer?.GetName() is string name)
            {
                return name.Length != 1 && name.Length <= 512;
            }
            return true;
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
