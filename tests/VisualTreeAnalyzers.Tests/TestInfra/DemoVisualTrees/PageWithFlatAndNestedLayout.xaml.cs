using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VisualTreeAnalyzers.Tests.TestInfra.DemoVisualTrees
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageWithFlatAndNestedLayout : Page
    {
        public PageWithFlatAndNestedLayout()
        {
            this.InitializeComponent();

            for (int i = 0; i < 300; i++)
            {
                var lastElement = new Button();
                var rootButton = lastElement;
                for (int j = 0; j < 10; j++)
                {
                    lastElement.Content = new Button();
                    lastElement = lastElement.Content as Button;
                }

                RootPanel.Children.Add(rootButton);
            }
        }
    }

}
