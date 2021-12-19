using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using VisualTreeAnalyzers.Snapshot;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Tests.UWP.Snapshot
{
    [TestClass]
    public class StandardOptionsTests
    {

        [UITestMethod]
        public void VerifyOptionsAreHelpful()
        {
            var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames,null);

            ProcessElement(new TextBlock()
            {
                Foreground = new SolidColorBrush(Colors.DarkGreen)
            });
            ProcessElement(new Button()
            {
                BorderBrush = new SolidColorBrush(Colors.LightGreen),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Colors.DarkGreen),
                Foreground = new SolidColorBrush(Colors.DarkGreen)
            });
            ProcessElement(new Grid()
            {
                BorderBrush = new SolidColorBrush(Colors.LightGreen),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Colors.DarkGreen)
            });
            ProcessElement(new StackPanel()
            {
                BorderBrush = new SolidColorBrush(Colors.LightGreen),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Colors.DarkGreen)
            });
            ProcessElement(new TextBox()
            {
                BorderBrush = new SolidColorBrush(Colors.LightGreen),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Colors.DarkGreen),
                Foreground = new SolidColorBrush(Colors.DarkGreen)
            });
            ProcessElement(new Page()
            {
                BorderBrush = new SolidColorBrush(Colors.LightGreen),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Colors.DarkGreen),
                Foreground = new SolidColorBrush(Colors.DarkGreen)
            });

            void ProcessElement(FrameworkElement element)
            {
                App.Content = element;
                var snapshot = snapshotCreator.CreateSnapshot(element);

                int nonNullCount = 0;
                foreach(var property in snapshot.PropertyValues)
                {
                    if(property != null)
                    {
                        nonNullCount++;
                    }
                }

                Assert.IsTrue(nonNullCount >= 4, $"Non null properties was {nonNullCount} which is to low (less than two).");
            }
        }

    }
}
