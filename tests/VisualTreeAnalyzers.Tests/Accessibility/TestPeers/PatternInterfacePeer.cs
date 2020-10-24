using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Tests.Accessibility.TestPeers
{
    class PatternInterfacePeer : ControlTypePeer
    {

        public List<PatternInterface> ImplementedInterfaces = new List<PatternInterface>();

        public PatternInterfacePeer(FrameworkElement owner, AutomationControlType type) : base(owner, type)
        {
        }

        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if(ImplementedInterfaces.Contains(patternInterface))
            {
                return this;
            }
            return null;
        }

    }
}
