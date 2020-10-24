using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Tests.Accessibility.TestPeers
{
    class ControlTypeFocusPeer : ControlTypePeer
    {
        private bool isFocusable;

        public ControlTypeFocusPeer(FrameworkElement owner, AutomationControlType type, bool focusable) : base(owner,type)
        {
            isFocusable = focusable;
        }

        protected override bool IsKeyboardFocusableCore()
        {
            return isFocusable;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return returnType;
        }
    }
}
