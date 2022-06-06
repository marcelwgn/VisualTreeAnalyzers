using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Core
{
    /// <summary>
    /// The AnalyzerHelper class contains helper functions that are used by all analyzers.
    /// </summary>
    public static class AnalyzerHelper
    {
        /// <summary>
        /// Marks the specified <see cref="FrameworkElement"/> with a border and a border-thickness if available or sets the foreground to the specified color.
        /// This function will handle the required logic to determine which way can be used to highlight this element.
        /// In addition to that, the tooltip of the specified <see cref="FrameworkElement"/> to include the specified message for easier use of analyzer warnings/errors.
        /// </summary>
        /// <param name="element">The element that will be marked.</param>
        /// <param name="color">The border color or foreground brush depending on the type of control.</param>
        /// <param name="thickness">The thickness used to draw a border.</param>
        /// <param name="message">The message that will appended to the tooltip.</param>
        public static void MarkElement(FrameworkElement element, SolidColorBrush color, Thickness thickness, string message)
        {
            if (element is Control control)
            {
                control.BorderBrush = color;
                control.BorderThickness = thickness;
            }
            else if (element is TextBlock textBlock)
            {
                textBlock.Foreground = color;
            }
            else if (element is RichTextBlock richTextBlock)
            {
                richTextBlock.Foreground = color;
            }
            else
            {
                // Not a control nor text element.
                // Use reflection to check if we have a border or foreground.
                if (element?.GetType().GetProperty("BorderBrush") is PropertyInfo borderBrush)
                {
                    borderBrush.SetValue(element, color);
                    if (element?.GetType().GetProperty("BorderThickness") is PropertyInfo borderThickness)
                    {
                        borderThickness.SetValue(element, thickness);
                    }
                }
                else if (element?.GetType().GetProperty("Foreground") is PropertyInfo foreground)
                {
                    foreground.SetValue(element, color);
                }
            }
            if (ToolTipService.GetToolTip(element) is string lastMessage)
            {
                ToolTipService.SetToolTip(element, lastMessage + "\n" + message);
            }
            else
            {
                ToolTipService.SetToolTip(element, message);
            }
        }

        /// <summary>
        /// This method will call ResetState on all rules in the list.
        /// </summary>
        /// <param name="rules">A list of rules whose state is to be cleared.</param>
        public static void ClearStates(IList<IRule> rules)
        {
            if(rules != null)
            {
                foreach(var rule in rules)
                {
                    if(rule != null)
                    {
                        rule.ResetState();
                    }
                }
            }
        }
    }
}
