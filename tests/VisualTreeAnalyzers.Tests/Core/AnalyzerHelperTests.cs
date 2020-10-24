using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Core;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Tests.Core
{
    [TestClass]
    public class AnalyzerHelperTests
    {
        public readonly static SolidColorBrush MarkBrush = new SolidColorBrush() { Color = Colors.Red };
        public readonly static Thickness MarkThickness = new Thickness(2);
        
        [UITestMethod]
        public void MarksControlsAppropriately()
        {
            VerifyElement(new Button());
            VerifyElement(new HyperlinkButton());
            VerifyElement(new NavigationView());
            VerifyElement(new MenuBar());
            VerifyElement(new TextBox());
            VerifyElement(new ToggleSwitch());

            void VerifyElement(Control element)
            {
                MarkElement(element);

                Assert.AreEqual(MarkBrush, element.BorderBrush);
                Assert.AreEqual(MarkThickness, element.BorderThickness);
            }
        }

        [UITestMethod]
        public void MarksTextElementsAppropriately()
        {
            VerifyElement(new TextBlock());
            VerifyElement(new RichTextBlock());

            void VerifyElement(FrameworkElement element)
            {
                MarkElement(element);

                if (element?.GetType().GetProperty("Foreground") is PropertyInfo foreground)
                {
                    Assert.AreEqual(foreground.GetValue(element), MarkBrush);
                }
                else
                {
                    Assert.IsTrue(false, "Element should have had a foreground property.");
                }
            }
        }

        [UITestMethod]
        public void MarksNonControlElementsWithBorderAppropriately()
        {
            VerifyElement(new StackPanel());
            VerifyElement(new Grid());
            VerifyElement(new CustomControl1());
            VerifyElement(new CustomControl2());

            void VerifyElement(FrameworkElement element)
            {
                MarkElement(element);

                int foundProperties = 0;
                if (element?.GetType().GetProperty("BorderBrush") is PropertyInfo borderBrush)
                {
                    foundProperties++;
                    Assert.AreEqual(borderBrush.GetValue(element), MarkBrush);
                }
                if (element?.GetType().GetProperty("BorderThickness") is PropertyInfo borderThickness)
                {
                    foundProperties++;
                    Assert.AreEqual(borderThickness.GetValue(element), MarkThickness);
                }

                Assert.IsTrue(foundProperties > 0, "At least one property should have been checked.");
            }
        }


        class CustomControl1 : FrameworkElement
        {
            public SolidColorBrush BorderBrush { get; set; }
        }

        class CustomControl2: FrameworkElement
        {
            public SolidColorBrush BorderBrush { get; set; }
            public Thickness BorderThickness { get; set; }
        }
        public static void MarkElement(FrameworkElement element)
        {
            AnalyzerHelper.MarkElement(element, MarkBrush, MarkThickness, "Marked");
        }
    }
}
