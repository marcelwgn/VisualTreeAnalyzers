using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualTreeAnalyzers.Snapshot;
using VisualTreeAnalyzers.Snapshot.Exporter;
using VisualTreeAnalyzers.Tests.TestInfra.DemoVisualTrees;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Tests.Snapshot.Exporter
{
    [TestClass]
    public class JsonExporterTests
    {
        [UITestMethod]
        public void VerifySingleWUXCControlIsCorrectDefaultOptions()
        {
            App.Content = new TextBlock() { Text = "Some text", Foreground = new SolidColorBrush(Colors.DarkGreen) };
            var exporter = new JsonExporter();
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var export = exporter.CreateJsonObject(snapshotCreator.CreateSnapshot(), true, false);
            Assert.IsNotNull(export);

            Assert.AreEqual("TextBlock", export.GetNamedString("type"));

            var jsonAsText = export.Stringify();
            Assert.IsTrue(jsonAsText.Contains("\"Visibility\":\"Visible\""));
            Assert.IsTrue(jsonAsText.Contains("\"Margin\":\"0,0,0,0\""));
            Assert.IsTrue(jsonAsText.Contains("\"Padding\":\"0,0,0,0\""));
            Assert.IsTrue(jsonAsText.Contains("\"Background\":\"[null]\""));
            Assert.IsTrue(jsonAsText.Contains($"\"Foreground\":\"{Colors.DarkGreen}\""));
        }

        [UITestMethod]
        public void VerifySingleWUXCControlIsCorrectNoConverterNoNullValues()
        {
            App.Content = new TextBlock() { Text = "Some text", Foreground = new SolidColorBrush(Colors.DarkGreen) };
            var exporter = new JsonExporter(null);
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var export = exporter.CreateJsonObject(snapshotCreator.CreateSnapshot(), false, true);
            Assert.IsNotNull(export);

            Assert.AreEqual("Windows.UI.Xaml.Controls.TextBlock", export.GetNamedString("type"));

            var jsonAsText = export.Stringify();
            Assert.IsTrue(jsonAsText.Contains("\"Visibility\":\"Visible\""));
            Assert.IsTrue(jsonAsText.Contains("\"Margin\":\"0,0,0,0\""));
            Assert.IsTrue(jsonAsText.Contains("\"Padding\":\"0,0,0,0\""));
            Assert.IsFalse(jsonAsText.Contains("\"Background\"")); // Background should not be present in the export as it was not set.
            Assert.IsTrue(jsonAsText.Contains($"\"Foreground\":\"Windows.UI.Xaml.Media.SolidColorBrush\""));
        }

        [UITestMethod]
        public void VerifySinglePageJSONExport()
        {
            App.Content = new SimplePage();
            var exporter = new JsonExporter(new StandardObjectToStringConverter());
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var export = exporter.CreateJsonObject(snapshotCreator.CreateSnapshot(), false, true);
            Assert.IsNotNull(export);

            var jsonAsText = export.Stringify();

            Assert.IsTrue(jsonAsText.Contains("VisualTreeAnalyzers.Tests.TestInfra.DemoVisualTrees.SimplePage"));
            Assert.IsTrue(jsonAsText.Contains("\"type\":\"Windows.UI.Xaml.Controls.Grid\",\"Name\":\"RootGrid\""));
        }

        [UITestMethod]
        public void VerifyButtonFormattedJSONExportWithNamespaces()
        {
            App.Content = new Button() { 
                RequestedTheme = Windows.UI.Xaml.ElementTheme.Light
            };
            var exporter = new JsonExporter(new StandardObjectToStringConverter());
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var exportFormatted = exporter.CreateFormattedJSONString(snapshotCreator.CreateSnapshot(), false, true);

            var expected =
@"{
    ""type"": ""Windows.UI.Xaml.Controls.Button"",
    ""Visibility"": ""Visible"",
    ""Margin"": ""0,0,0,0"",
    ""Padding"": ""8,4,8,5"",
    ""Background"": ""#33000000"",
    ""BorderBrush"": ""#00FFFFFF"",
    ""BorderThickness"": ""2,2,2,2"",
    ""Foreground"": ""#FF000000"",
    ""children"": [
        {
            ""type"": ""Windows.UI.Xaml.Controls.ContentPresenter"",
            ""Name"": ""ContentPresenter"",
            ""Visibility"": ""Visible"",
            ""Margin"": ""0,0,0,0"",
            ""Padding"": ""8,4,8,5"",
            ""Background"": ""#33000000"",
            ""BorderBrush"": ""#00FFFFFF"",
            ""BorderThickness"": ""2,2,2,2"",
            ""Foreground"": ""#FF000000"",
            ""children"": []
        }
    ]
}";
            Assert.AreEqual(expected, exportFormatted);
        }

        [UITestMethod]
        public void VerifyButtonFormattedJSONExportWithoutNamespaces()
        {
            App.Content = new Button()
            {
                RequestedTheme = Windows.UI.Xaml.ElementTheme.Light
            };
            var exporter = new JsonExporter(new StandardObjectToStringConverter());
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var exportFormatted = exporter.CreateFormattedJSONString(snapshotCreator.CreateSnapshot(), false, false);

            var expected =
@"{
    ""type"": ""Button"",
    ""Visibility"": ""Visible"",
    ""Margin"": ""0,0,0,0"",
    ""Padding"": ""8,4,8,5"",
    ""Background"": ""#33000000"",
    ""BorderBrush"": ""#00FFFFFF"",
    ""BorderThickness"": ""2,2,2,2"",
    ""Foreground"": ""#FF000000"",
    ""children"": [
        {
            ""type"": ""ContentPresenter"",
            ""Name"": ""ContentPresenter"",
            ""Visibility"": ""Visible"",
            ""Margin"": ""0,0,0,0"",
            ""Padding"": ""8,4,8,5"",
            ""Background"": ""#33000000"",
            ""BorderBrush"": ""#00FFFFFF"",
            ""BorderThickness"": ""2,2,2,2"",
            ""Foreground"": ""#FF000000"",
            ""children"": []
        }
    ]
}";

            Assert.AreEqual(expected, exportFormatted);
        }

        [UITestMethod]
        public void VerifyButtonFormattedJSONExportWithoutNamespacesWithNullValues()
        {
            App.Content = new Button()
            {
                RequestedTheme = Windows.UI.Xaml.ElementTheme.Light
            };
            var exporter = new JsonExporter(new StandardObjectToStringConverter());
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, App.Content);

            var exportFormatted = exporter.CreateFormattedJSONString(snapshotCreator.CreateSnapshot(), true, false);

            var expected =
@"{
    ""type"": ""Button"",
    ""Name"": """",
    ""Visibility"": ""Visible"",
    ""Margin"": ""0,0,0,0"",
    ""Padding"": ""8,4,8,5"",
    ""Background"": ""#33000000"",
    ""BorderBrush"": ""#00FFFFFF"",
    ""BorderThickness"": ""2,2,2,2"",
    ""Foreground"": ""#FF000000"",
    ""children"": [
        {
            ""type"": ""ContentPresenter"",
            ""Name"": ""ContentPresenter"",
            ""Visibility"": ""Visible"",
            ""Margin"": ""0,0,0,0"",
            ""Padding"": ""8,4,8,5"",
            ""Background"": ""#33000000"",
            ""BorderBrush"": ""#00FFFFFF"",
            ""BorderThickness"": ""2,2,2,2"",
            ""Foreground"": ""#FF000000"",
            ""children"": []
        }
    ]
}";

            Assert.AreEqual(expected, exportFormatted);
        }
    }
}
