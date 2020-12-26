using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.TestInfra.DemoVisualTrees
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageWithFlatStructure : Page
    {
        public PageWithFlatStructure()
        {
            this.InitializeComponent();
            for (int i = 0; i < 100; i++)
            {
                RootStackPanel.Children.Add(new Button() { Content = i });
            }
        }
    }
}
