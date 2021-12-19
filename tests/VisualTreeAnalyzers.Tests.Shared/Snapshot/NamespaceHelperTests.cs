using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisualTreeAnalyzers.Snapshot;

namespace VisualTreeAnalyzers.Tests.UWP.Snapshot
{
    [TestClass]
    public class NamespaceHelperTests
    {
        [DataTestMethod]
        [DataRow("test", "")]
        [DataRow("test.test", "test")]
        [DataRow("test.test.test", "test.test")]
        [DataRow("Windows.UI.Xaml.Controls.TextBlock", "Windows.UI.Xaml.Controls")]
        public void VerifyNamespaceExtraction(string fullName, string expectedNamespace)
        {
            Assert.AreEqual(expectedNamespace, NamespaceHelper.GetNamespace(fullName));
        }

        [DataTestMethod]
        [DataRow("test1", "test1")]
        [DataRow("test.test2", "test2")]
        [DataRow("test.test.test3", "test3")]
        [DataRow("Windows.UI.Xaml.Controls.TextBlock", "TextBlock")]
        public void VerifyTypeNameExtraction(string fullName, string expectedNamespace)
        {
            Assert.AreEqual(expectedNamespace, NamespaceHelper.GetTypeName(fullName));
        }

    }
}
