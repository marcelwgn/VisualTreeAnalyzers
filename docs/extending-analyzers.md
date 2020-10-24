# Extending analyzers

VisualTreeAnalyzers allows the authoring of new analyzers and rules and integrating them into the existing APIs.
For that, existing classes have ways to add new rules and work with self authored analyzers.
Below are examples showing how to extend the functionality of the VisualTreeAnalyzers.

### Adding additional accessibility rules
The [AccessibilityAnalyzer](xref:VisualTreeAnalyzers.Accessibility.AccessibilityAnalyzer) exposes the list of rules used for analyzing and allows adding new rules to an existing analyzer object.
To add a new rule, you will need to implement the [IRule](xref:VisualTreeAnalyzers.Core.IRule) interface:

```c#
class CustomAccessibilityRule : IRule
{
    public string FailureMessage => "Element should ....";

    public bool IsValid(FrameworkElement element, AutomationPeer peer)
    {
        bool elementIsValid = true;
        // Process element
        return elementIsValid;
    }

    public void ResetState() 
    { 
        // Reset state of this rule in case it needs broader context of the visual tree.
        // An example of that, is the LandmarkTypeOnceRule which counts the number of all elements encountered
        // with the LandmarkType=Main.
    }
}
```

After authoring a new rule, you can add the new rule to an existing [AccessibilityAnalyzer](xref:VisualTreeAnalyzers.Accessibility.AccessibilityAnalyzer) object:

```c#
var analyzer = new AccessibilityAnalyzer();
analyzer.AccessibilityRules.Add(new CustomAccessibilityRule());
```

### Authoring a new analyzer
To scan the visual tree for specific issues or patterns, in some cases, it can be easier to write a new analyzer instead of extending existing ones.
The VisualTreeWalker is able to work with any object implementing the [IElementAnalyzer](xref:VisualTreeAnalyzers.Core.IElementAnalyzer) interface.

Below is the skeleton of an analyzer:

```c#
public class CustomAnalyzer : IElementAnalyzer
{
    private SolidColorBrush markingBrush = new SolidColorBrush(Colors.Yellow);
    private Thickness markingBorderThickness = new Thickness(1);

    private IList<IRule> rules { get; }

    /// <summary>
    /// Creates a new CustomAnalyzer.
    /// </summary>
    public CustomAnalyzer()
    {
        // Add all rules that should be used for this analyzer in here.
    }

    public void Analyze(FrameworkElement element)
    {
        foreach (IRule rule in rules)
        {
            // If your rules don't need the AutomationPeer, you can just pass null for the peer
            if (!rule.IsValid(element, null))
            {
                MarkElement(element, rule.FailureMessage);
            }
        }
    }

    public bool ShouldVisitChildren(FrameworkElement element)
    {
        // Determine whether to visit the children of this element. 
        return true;
    }

    public void ResetState()
    {
        // This calls ClearState on all rules
        AnalyzerHelper.ClearStates(rules);
    }

    private void MarkElement(FrameworkElement element, string message)
    {
        AnalyzerHelper.MarkElement(element, markingBrush, markingBorderThickness, message);
    }
}
```