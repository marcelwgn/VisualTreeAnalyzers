# Getting started
The VisualTreeAnalyzers package contains multiple different functionalities for different kinds of visual tree processing. Below are a few different ways to use the VisualTreeAnalyzers package.
The package is available on [NuGet.org](https://www.nuget.org/packages/VisualTreeAnalyzers/) or download it with Visual Studio by searching for "VisualTreeAnalyzers" in the NuGet package manager.

### Using the Accessibility Analzyer
To scan the current page for accessibility issues such as missing element names, you can leverage the [AccessibilityAnalyzer](xref:VisualTreeAnalyzers.Accessibility.AccessibilityAnalyzer). Below is an example of integrating the AccessibilityAnalyzer into an existing page:

```c# 
public CustomPage()
{
    this.InitializeComponents();

    // Wait for the page to be loaded to have the visual tree be complete.
    this.Loaded += PageLoaded;
}

private void PageLoaded(object sender, RoutedEventArgs e)
{
    // Create a new analyzer.
    var analyzer = new AccessibilityAnalyzer();
    // Associate analyzer with a VisualTreeWalker.
    var walker = new VisualTreeWalker(this, analyzer);
    // Optional: Specify that scanning should only be done when a debugger is attached:
    walker.OnlyWithDebugger = true;
    // Scan the current page.
    walker.ScanVisualTree();
}
```

Note that the VisualTreeAnalyzers only perform a scan once, similar to [Axe.Windows](https://github.com/microsoft/axe-windows). To rerun tests after a certain amount of time, you can use the [VisualTreeWalkerTimer](xref:VisualTreeAnalyzers.Core.VisualTreeWalkerTimer):

```c#
// Create a new analyzer.
var analyzer = new AccessibilityAnalyzer();
// Associate analyzer with a VisualTreeWalker.
var walker = new VisualTreeWalker(this, analyzer);
// Scan tree once.
walker.ScanVisualTree();

// Create a new timer with default interval of 5 seconds.
timer = new VisualTreeWalkerTimer(walker);
// Start timer.
timer.Start();
```

### Creating visual tree snapshots
To take a snapshot of an element and its visual tree with the standard options, you need to create a new [ElementSnapshotCreator](xref:VisualTreeAnalyzers.Snapshot.ElementSnapshotCreator) object. The standard options include a list of commmon properties to include to make it easier getting started with visual tree snapshotting.

Then you can just call `CreateSnapshot`to generate the snapshot object for the visual tree:

```c#
var element = ...; // The element you want a snapshot of

// Create a new snapshot creator object
var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, element);

// Get snapshot
var elementSnapshot = snapshotCreator.CreateSnapshot();

// Get a formatted XML export ...
var xmlExport = new XmlExporter().CreateFormattedXMLString(elementSnapshot, /* Show null/empty values */ false, /* Include namespaces */ false);

// ... or get a formatted JSON export
var jsonExport = new JsonExporter().CreateFormattedJSONString(elementSnapshot, /* Show null/empty values */ false, /* Include namespaces */ false);
```

To learn more about snapshot customization and exporting snapshots into other formats, see [snapshot testing](./snapshot-testing.md).
