using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System;
using System.Collections.Generic;
using VisualTreeAnalyzers.Snapshot;
using VisualTreeAnalyzers.Tests.TestInfra.DemoVisualTrees;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Tests.Snapshot
{
    [TestClass]
    public class ElementSnapshotCreatorTests
    {

        private static readonly IList<string> testPropertyNames = new string[]
        {
                "Visibility",
                "ActualWidth",
                "ActualHeight",
                "Background",
                "Foreground",
                "Text"
        };

        [UITestMethod]
        public void VerifyCreatesSimplePageTreeCorrectly()
        {
            var page = new SimplePage();
            App.Content = page;

            var snapshotCreator = new ElementSnapshotCreator(testPropertyNames, page);

            var pageSnapshot = snapshotCreator.CreateSnapshot();
            Assert.AreEqual(testPropertyNames, pageSnapshot.PropertyNames);
            Assert.AreEqual("SimplePageName", pageSnapshot.ElementName);
            Assert.AreEqual("VisualTreeAnalyzers.Tests.TestInfra.DemoVisualTrees.SimplePage", pageSnapshot.FullTypeName);
            Assert.AreEqual(1, pageSnapshot.Children.Count);

            var gridSnapshot = pageSnapshot.Children[0];
            Assert.AreEqual(testPropertyNames, gridSnapshot.PropertyNames);
            Assert.AreEqual("RootGrid", gridSnapshot.ElementName);
            Assert.AreEqual("Windows.UI.Xaml.Controls.Grid", gridSnapshot.FullTypeName);
            Assert.AreEqual(1, gridSnapshot.Children.Count);
            Assert.AreEqual("#FFD3D3D3", (gridSnapshot.PropertyValues[3] as SolidColorBrush).Color.ToString());

            var textBlockSnapshot = gridSnapshot.Children[0];
            Assert.AreEqual(testPropertyNames, textBlockSnapshot.PropertyNames);
            Assert.AreEqual("ExampleTextBlock", textBlockSnapshot.ElementName);
            Assert.AreEqual("Windows.UI.Xaml.Controls.TextBlock", textBlockSnapshot.FullTypeName);
            Assert.AreEqual(0, textBlockSnapshot.Children.Count);
            Assert.AreEqual("#FF006400", (textBlockSnapshot.PropertyValues[4] as SolidColorBrush).Color.ToString());
            Assert.AreEqual("Some text", textBlockSnapshot.PropertyValues[5]);
        }

        [UITestMethod]
        public void VerifyChildrenCountCorrect()
        {
            var panel = new StackPanel();
            var childCount = new Random().Next(3, 15);
            for (int i = 0; i < childCount; i++)
            {
                panel.Children.Add(new TextBlock());
            }
            App.Content = panel;
            var snapshotCreator = new ElementSnapshotCreator(new List<string>(), panel);

            var snapshot = snapshotCreator.CreateSnapshot();

            Assert.AreEqual(childCount, snapshot.Children.Count);
            for (int i = 0; i < childCount; i++)
            {
                Assert.AreEqual(0, snapshot.Children[i].Children.Count);
            }
        }

        [UITestMethod]
        public void ValidatesInput()
        {
            var element = new Button();

            Assert.IsNotNull(new ElementSnapshotCreator(
                    new List<string>()
                    {
                        "Visibility"
                    }, null)
                );

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new ElementSnapshotCreator(null, element);
            });
        }
    }
}
