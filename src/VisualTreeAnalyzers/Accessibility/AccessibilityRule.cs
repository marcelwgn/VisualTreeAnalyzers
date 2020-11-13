using System;

namespace VisualTreeAnalyzers.Accessibility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class AccessibilityRule : Attribute
    {
    }
}
