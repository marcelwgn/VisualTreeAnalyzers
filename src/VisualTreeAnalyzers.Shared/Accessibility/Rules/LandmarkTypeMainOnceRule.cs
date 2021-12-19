using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// This rule checks if only one element with the landmark type "Main" was encountered.
    /// </summary>
    [AccessibilityRule]
    public sealed class LandmarkTypeMainOnceRule : IRule
    {
        private int landmarkTypeMainCount;

        /// <inheritdoc/>
        public string FailureMessage => "Only one element should have the landmark type main.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            if(peer?.GetLandmarkType() == AutomationLandmarkType.Main)
            {
                landmarkTypeMainCount++;
                return !(landmarkTypeMainCount > 1);
            }
            return true;
        }

        /// <inheritdoc/>
        public void ResetState() 
        {
            landmarkTypeMainCount = 0;
        }
    }
}
