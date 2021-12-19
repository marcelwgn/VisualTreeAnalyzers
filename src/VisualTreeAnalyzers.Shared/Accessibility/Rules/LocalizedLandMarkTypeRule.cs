using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// This rule checks whether the localized landmark type is specified if the element has a landmark type.
    /// </summary>
    [AccessibilityRule]
    public sealed class LocalizedLandMarkTypeRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "Localized LandMarkType should not be null or empty.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            return peer?.GetLandmarkType() == AutomationLandmarkType.None || !string.IsNullOrEmpty(peer?.GetLocalizedLandmarkType());
        }

        /// <inheritdoc/>
        public void ResetState() { }
    }
}
