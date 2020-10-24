using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.Accessibility.Rules.TestPeers
{
    class SelectionPeer : FrameworkElementAutomationPeer, ISelectionProvider
    {
        private int selectionCount;

        public SelectionPeer(FrameworkElement owner, int selectedItemCount) : base(owner)
        {
            selectionCount = selectedItemCount;
        }

        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if(patternInterface == PatternInterface.Selection)
            {
                return this;
            }
            return base.GetPatternCore(patternInterface);
        }

        public IRawElementProviderSimple[] GetSelection()
        {
            var array = new IRawElementProviderSimple[selectionCount];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = ProviderFromPeer(CreatePeerForElement(new Button()));
            }
            return array;
        }

        public bool CanSelectMultiple;

        public bool IsSelectionRequired;

        bool ISelectionProvider.CanSelectMultiple => CanSelectMultiple;

        bool ISelectionProvider.IsSelectionRequired => IsSelectionRequired;
    }
}
