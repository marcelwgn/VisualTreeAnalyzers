using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Core
{
    /// <summary>
    /// This is the standard interface for rule objects to be used by the different analyzers.
    /// Every rule must provide a failure message that analyzers can use to notify users why this element failed the given rule.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Checks if the element and peer are valid for this rule.
        /// </summary>
        /// <param name="element">The element to check.</param>
        /// <param name="peer">The peer to check.</param>
        /// <returns>True if the element and peer are valid for this rule, false otherwise,.</returns>
        bool IsValid(FrameworkElement element, AutomationPeer peer);

        /// <summary>
        /// Returns the failure message for this rule.
        /// </summary>
        /// <returns>The failure message.</returns>
        string FailureMessage { get; }

        /// <summary>
        /// Resets the state of this rule. This method will usually be called by analyzers when a new scan has been started.
        /// </summary>
        void ResetState();
    }
}
