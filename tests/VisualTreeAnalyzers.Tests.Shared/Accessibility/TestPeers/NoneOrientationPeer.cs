using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Tests.UWP.Accessibility.TestPeers
{
    class NoneOrientationPeer : ControlTypePeer

    {

        public NoneOrientationPeer(FrameworkElement owner, AutomationControlType type) : base(owner,type)
        {
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return returnType;
        }

        protected override AutomationOrientation GetOrientationCore()
        {
            return AutomationOrientation.None;
        }
    }
}
