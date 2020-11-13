using System.Collections.Generic;
using System.Linq;
using AnalyzersSampleApp.DemoTypes;
using VisualTreeAnalyzers.Accessibility;
using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AnalyzersSampleApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccessibilityDemoPage : Page
    {
        private readonly List<NumberItem> numbers = new List<NumberItem>();

        private VisualTreeWalkerTimer timer;

        public AccessibilityDemoPage()
        {
            foreach(int i in Enumerable.Range(0,100))
            {
                numbers.Add(new NumberItem(i));
            }

            InitializeComponent();

            Loaded += AccessibilityDemoPage_Loaded;
        }

        private void AccessibilityDemoPage_Loaded(object sender, RoutedEventArgs e)
        {
            var analyzer = new AccessibilityAnalyzer();
            var walker = new VisualTreeWalker(this, analyzer);

            walker.ScanVisualTree();

            timer = new VisualTreeWalkerTimer(walker);
            timer.StartTimer();
        }

        private void AddInaccessibleControlButton_Click(object sender, RoutedEventArgs e)
        {

            ButtonCollection.Children.Add(
                new Button()
                {
                    Width = 100,
                    Height = 30
                }
            );
        }
        private void AddAccessibleControlButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonCollection.Children.Add(
                new Button()
                {
                    Width = 100,
                    Height = 30,
                    Content = "button"
                }
            );
        }
    }
}
