using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Snapshot;
using VisualTreeAnalyzers.Snapshot.Exporter;
using VisualTreeAnalyzers.Tests.TestInfra.DemoVisualTrees;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Tests.Snapshot.Exporter
{
    [TestClass]
    public class XMLExporterTests
    {
        [UITestMethod]
        public void VerifySingleWUXCControlIsCorrectDefaultOptions()
        {
            App.Content = new TextBlock() { Text = "Some text", Foreground = new SolidColorBrush(Colors.DarkGreen) };
            var exporter = new XMLExporter();
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var export = exporter.CreateXMLDocument(snapshotCreator.CreateSnapshot(), true, false);
            Assert.IsNotNull(export);

            Assert.AreEqual("TextBlock", export.FirstChild.NodeName);
            Assert.AreEqual(null, export.FirstChild.Prefix);

            var xmlAsText = export.GetXml();
            Assert.IsTrue(xmlAsText.Contains("Visibility=\"Visible\""));
            Assert.IsTrue(xmlAsText.Contains("Margin=\"0,0,0,0\""));
            Assert.IsTrue(xmlAsText.Contains("Padding=\"0,0,0,0\""));
            Assert.IsTrue(xmlAsText.Contains("Background=\"[null]\""));
            Assert.IsTrue(xmlAsText.Contains($"Foreground=\"{Colors.DarkGreen}\""));
        }

        [UITestMethod]
        public void VerifySingleWUXCControlIsCorrectNoConverterNoNullValues()
        {
            App.Content = new TextBlock() { Text = "Some text", Foreground = new SolidColorBrush(Colors.DarkGreen) };
            var exporter = new XMLExporter(null);
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var export = exporter.CreateXMLDocument(snapshotCreator.CreateSnapshot(), false, true);
            Assert.IsNotNull(export);

            Assert.AreEqual("Windows.UI.Xaml.Controls.TextBlock", export.FirstChild.NodeName);
            Assert.AreEqual(null, export.FirstChild.Prefix);

            var xmlAsText = export.GetXml();
            Assert.IsTrue(xmlAsText.Contains("Visibility=\"Visible\""));
            Assert.IsTrue(xmlAsText.Contains("Margin=\"0,0,0,0\""));
            Assert.IsTrue(xmlAsText.Contains("Padding=\"0,0,0,0\""));
            Assert.IsFalse(xmlAsText.Contains("Background")); // Background should not be present in the export as it was not set.
            Assert.IsTrue(xmlAsText.Contains($"Foreground=\"Windows.UI.Xaml.Media.SolidColorBrush\""));
        }

        [UITestMethod]
        public void VerifySinglePageXMLExport()
        {
            App.Content = new SimplePage();
            var exporter = new XMLExporter(new StandardObjectToStringConverter());
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var export = exporter.CreateXMLDocument(snapshotCreator.CreateSnapshot(), false, true);
            Assert.IsNotNull(export);

            var xmlAsText = export.GetXml();

            Assert.IsTrue(xmlAsText.Contains("VisualTreeAnalyzers.Tests.TestInfra.DemoVisualTrees.SimplePage"));
            Assert.IsTrue(xmlAsText.Contains($"Windows.UI.Xaml.Controls.Grid Name=\"RootGrid\""));
        }

        [UITestMethod]
        public void VerifySimplePageFormattedXMLExport()
        {
            App.Content = new SimplePage();
            var exporter = new XMLExporter(new StandardObjectToStringConverter());
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var exportFormatted = exporter.CreateFormattedXMLString(snapshotCreator.CreateSnapshot(), false, true);

            Assert.IsTrue(exportFormatted.Contains("<Windows.UI.Xaml.Controls.Grid Name=\"RootGrid\" Visibility=\"Visible\""));
        }
    }
}
