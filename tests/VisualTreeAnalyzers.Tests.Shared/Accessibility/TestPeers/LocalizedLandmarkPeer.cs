using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace VisualTreeAnalyzers.Tests.UWP.Accessibility.TestPeers
{
    class LocalizedLandmarkPeer : ControlTypePeer
    {
        private readonly string landmarkType;

        public LocalizedLandmarkPeer(FrameworkElement owner, string type) : base(owner, AutomationControlType.Custom)
        {
            landmarkType = type;
        }

        protected override AutomationLandmarkType GetLandmarkTypeCore()
        {
            return AutomationLandmarkType.Custom;
        }

        public new string GetLocalizedLandmarkType()
        {
            return landmarkType;
        }
    }
}
