# Getting started
The VisualTreeAnalyzers package contains multiple different functionalities for different kinds of visual tree processing. Below are a few different ways to use the VisualTreeAnalyzers package.
The package is available on [NuGet.org](https://www.nuget.org/packages/VisualTreeAnalyzers/) or download it with Visual Studio by searching for "VisualTreeAnalyzers".

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