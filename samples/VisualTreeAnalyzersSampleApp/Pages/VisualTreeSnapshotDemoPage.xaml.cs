using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VisualTreeAnalyzers.Snapshot;
using VisualTreeAnalyzers.Snapshot.Exporter;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AnalyzersSampleApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VisualTreeSnapshotDemoPage : Page
    {
        private ElementSnapshotCreator snapshotCreator;
        private XmlExporter xmlExporter = new XmlExporter();
        private JsonExporter jsonExporter = new JsonExporter();

        public VisualTreeSnapshotDemoPage()
        {
            InitializeComponent();
            
            snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, SampleControlsPanel);

            Loaded += VisualTreeSnapshotDemoPage_Loaded;
        }

        private void VisualTreeSnapshotDemoPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSnapshots_Click(null, null);
        }

        private void UpdateSnapshots_Click(object sender, RoutedEventArgs e)
        {
            var snapshot = snapshotCreator.CreateSnapshot();

            XMLExportTextBlock.Text = xmlExporter.CreateFormattedXMLString(snapshot, ShowNullAndEmptyValuesCheckbox.IsChecked.Value, IncludeNamespacesCheckbox.IsChecked.Value);
            JSONExportTextBlock.Text = jsonExporter.CreateFormattedJSONString(snapshot, ShowNullAndEmptyValuesCheckbox.IsChecked.Value, IncludeNamespacesCheckbox.IsChecked.Value);
        }
    }
}
