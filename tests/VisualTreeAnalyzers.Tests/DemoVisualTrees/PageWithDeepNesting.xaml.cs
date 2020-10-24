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

namespace VisualTreeAnalyzers.Tests.DemoVisualTrees
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageWithDeepNesting : Page
    {
        public PageWithDeepNesting()
        {
            this.InitializeComponent();

            var lastElement = RootButton;
            for(int i=0;i<100;i++)
            {
                lastElement.Content = new Button();
                lastElement = lastElement.Content as Button;
            }

        }
    }
}
