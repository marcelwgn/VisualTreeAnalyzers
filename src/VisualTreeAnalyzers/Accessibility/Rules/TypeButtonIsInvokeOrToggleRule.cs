using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// This rule verifies that elements with automation type Button are either toggleable or invokable.
    /// </summary>
    [AccessibilityRule]
    public sealed class TypeButtonIsInvokeOrToggleRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Button control should be invokable or toggleable";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            if(peer?.GetAutomationControlType() == AutomationControlType.Button)
            {
                return peer.GetPattern(PatternInterface.Invoke) != null 
                    || peer.GetPattern(PatternInterface.Toggle) != null;
            }
            return true;
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
