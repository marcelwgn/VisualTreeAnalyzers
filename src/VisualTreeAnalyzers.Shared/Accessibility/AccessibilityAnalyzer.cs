using System;
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
        /// Gets the amount of rule violations found by the <see cref="AccessibilityAnalyzer"/>.
        /// </summary>
        /// <param name="element">The object whose count will be determined.</param>
        /// <returns>The number of issues that were found by the last scan with the <see cref="AccessibilityAnalyzer"/></returns>
        public static int GetAccessibilityAnalyzerViolationCount(DependencyObject element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            return (int)element.GetValue(accessibilityAnalyzerViolationCountProperty);
        }

        /// <summary>
        /// Sets the amount of rule violations found by the <see cref="AccessibilityAnalyzer"/>.
        /// </summary>
        /// <param name="element">The object whose count will be set.</param>
        /// <param name="value">The number of rule issues found.</param>
        private static void SetAccessibilityAnalyzerViolationCount(DependencyObject element, int value)
        {
            element.SetValue(accessibilityAnalyzerViolationCountProperty, value);
        }

        /// <summary>
        /// Dependency property that can be used to read (and set) the number of issues found by the <see cref="AccessibilityAnalyzer"/>.
        /// </summary>
        private static readonly DependencyProperty accessibilityAnalyzerViolationCountProperty =
            DependencyProperty.RegisterAttached("AccessibilityAnalyzerViolationCount", typeof(int), typeof(AccessibilityAnalyzer), new PropertyMetadata(0));

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

        private bool forceScan;
        private SolidColorBrush errorBrush = new SolidColorBrush() { Color = Colors.Red };
        private Thickness errorBrushThickness = new Thickness(2);

        /// <summary>
        /// Creates a new AccessibilityAnalyzer object. The default AccessibilityAnalyzer loads all accessibility rules included by this library.
        /// To improve performance, the analyzer will ignore items that already violated rules in a previous scan.
        /// </summary>
        public AccessibilityAnalyzer() : this(false) { }

        /// <summary>
        /// Creates a new AccessibilityAnalyzer object. The default AccessibilityAnalyzer loads all accessibility rules included by this library.
        /// To improve performance, the analyzer can ignore items that already violated rules in a previous scan by setting set ignorePreviousScans to true.
        /// </summary>
        /// <param name="ignorePreviousScans">Indicates whether items that already violated rules will be ignored.</param>
        public AccessibilityAnalyzer(bool ignorePreviousScans)
        {
            AccessibilityRules = RuleScanner.GetRules(typeof(AccessibilityRule));
            forceScan = ignorePreviousScans;
        }

        /// <inheritdoc/>
        public void Analyze(FrameworkElement element)
        {
            // Skip if element is not in content tree.
            if (AutomationProperties.GetAccessibilityView(element) == AccessibilityView.Raw)
            {
                return;
            }

            // Skip if element was already visited.
            if (GetAccessibilityAnalyzerViolationCount(element) != 0 && !forceScan)
            {
                return;
            }

            if (FrameworkElementAutomationPeer.CreatePeerForElement(element) is FrameworkElementAutomationPeer peer)
            {
                int failureCount = 0;
                foreach (IRule rule in AccessibilityRules)
                {
                    if (!rule.IsValid(element, peer))
                    {
                        failureCount++;
                        MarkElement(element, rule.FailureMessage);
                    }
                }

                SetAccessibilityAnalyzerViolationCount(element, failureCount);
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
