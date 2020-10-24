using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTreeAnalyzers.Accessibility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class AccessibilityRule : Attribute
    {
    }
}
