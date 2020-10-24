using System.Collections.Generic;
using VisualTreeAnalyzers.Core;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Accessibility
{
    /// <summary>
    /// The accessibility analyzer provides a range of accessibility rules such as missing names or misaligned control types.
    /// </summary>
    public sealed class AccessibilityAnalyzer : IElementAnalyzer
    {
        /// <summary>
        /// This brush is used to highlight violating elements by either setting the border of foreground.
        /// </summary>
        public SolidColorBrush ErrorBrush
        {
            get => errorBrush;
            set { if (value != null) { errorBrush = value; } }
        }

        /// <summary>
        /// The thickness used to render a border when an element is being highlighted by updating the border.
        /// </summary>
        public Thickness ErrorBrushThickness
        {
            get => errorBrushThickness;
            set { if (value != null) { errorBrushThickness = value; } }
        }

        /// <summary>
        /// The list of all rules this accessibility analyzer uses to analyze and flag elements.
        /// </summary>
        public IList<IRule> AccessibilityRules { get; }

        private SolidColorBrush errorBrush = new SolidColorBrush() { Color = Colors.Red };
        private Thickness errorBrushThickness = new Thickness(2);

        /// <summary>
        /// Creates a new AccessibilityAnalyzer object. The default AccessibilityAnalyzer loads all accessibility rules included by this library.
        /// </summary>
        public AccessibilityAnalyzer()
        {
            AccessibilityRules = RuleScanner.GetRules(typeof(AccessibilityRule));
        }

        /// <inheritdoc/>
        public void Analyze(FrameworkElement element)
        {
            if (AutomationProperties.GetAccessibilityView(element) == AccessibilityView.Raw)
            {
                return;
            }

            if (FrameworkElementAutomationPeer.CreatePeerForElement(element) is FrameworkElementAutomationPeer peer)
            {
                foreach (IRule rule in AccessibilityRules)
                {
                    if (!rule.IsValid(element, peer))
                    {
                        MarkElement(element, rule.FailureMessage);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public bool ShouldVisitChildren(FrameworkElement element)
        {
            if (FrameworkElementAutomationPeer.CreatePeerForElement(element) is FrameworkElementAutomationPeer peer)
            {
                if (peer.GetChildren().Count == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This will clear the state on all accessibility rules associated with this object.
        /// </summary>
        public void ResetState()
        {
            AnalyzerHelper.ClearStates(AccessibilityRules);
        }

        private void MarkElement(FrameworkElement element, string message)
        {
            AnalyzerHelper.MarkElement(element, errorBrush, errorBrushThickness, message);
        }
    }
}
