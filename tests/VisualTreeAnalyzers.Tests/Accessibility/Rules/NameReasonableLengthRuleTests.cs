using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Accessibility.Rules;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Tests.Accessibility.Rules
{
    [TestClass]
    public class NameReasonableLengthRuleTests
    {

        [UITestMethod]
        public void VerifyNameBehaviors()
        {
            TestString(null, true);
            TestString("", true);
            TestString("\uE700", false);
            TestString("Text", true);
            // This should be around 55 characters.
            TestString("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. " +
                "Aenean commodo ligula eget dolor. Aenean massa. " +
                "Cum sociis natoque penatibus et magnis dis parturient montes, " +
                "nascetur ridiculus mus. Donec quam felis, ultricies nec, " +
                "pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. " +
                "Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. " +
                "In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. " +
                "Nullam dictum felis eu pede mollis pretium. Integer tincidunt. " +
                "Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate", false);


            void TestString(string name, bool complies)
            {
                var element = new Button();
                element.SetValue(AutomationProperties.NameProperty,name);
                var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);

                var rule = new NameReasonableLengthRule();

                Assert.AreEqual(complies, rule.IsValid(element, peer));
            }
        }
    }
}
