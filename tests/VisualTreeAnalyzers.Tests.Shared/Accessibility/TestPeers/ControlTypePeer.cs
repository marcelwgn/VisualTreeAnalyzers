using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Tests.UWP.Accessibility.TestPeers
{
    internal class ControlTypePeer : FrameworkElementAutomationPeer
    {
        protected readonly AutomationControlType returnType;

        public ControlTypePeer(FrameworkElement owner, AutomationControlType type) : base(owner)
        {
            returnType = type;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return returnType;
        }
    }
}
