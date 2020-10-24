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

    // Scan the current page.
    walker.ScanVisualTree();
}
```

Note that currently, the VisualTreeAnalyzers only perform a scan once, similar to Axe.Windows. If the visual tree changed, you will need to rerun the scan.