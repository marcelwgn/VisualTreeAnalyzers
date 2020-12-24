using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System;
using System.Collections.Generic;
using VisualTreeAnalyzers.Snapshot;
using VisualTreeAnalyzers.Tests.DemoVisualTrees;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Tests.Snapshot
{
    [TestClass]
    public class ElementSnapshotCreatorTests
    {
        private static readonly IList<DependencyProperty> testDependencyProperties = new List<DependencyProperty>()
            {
                UIElement.VisibilityProperty,
                FrameworkElement.ActualWidthProperty,
                FrameworkElement.ActualHeightProperty,
                Panel.BackgroundProperty,
                TextBlock.ForegroundProperty,
                TextBlock.TextProperty
            };

        private static readonly IList<string> testPropertyNames = new string[]
        {
                "Visibility",
                "ActualWidth",
                "ActualHeight",
                "Background",
                "ForeGround",
                "Text"
        };

        [UITestMethod]
        public void VerifyCreatesSimplePageTreeCorrectly()
        {
            var page = new SimplePage();
            App.Content = page;

            var snapshotCreator = CreateElementSnapshotCreator(page);

            var pageSnapshot = snapshotCreator.CreateTreeSnapshot();
            Assert.AreEqual(testPropertyNames, pageSnapshot.PropertyNames);
            Assert.AreEqual("SimplePageName", pageSnapshot.ElementName);
            Assert.AreEqual(1, pageSnapshot.Children.Count);

            var gridSnapshot = pageSnapshot.Children[0];
            Assert.AreEqual(testPropertyNames, gridSnapshot.PropertyNames);
            Assert.AreEqual("RootGrid", gridSnapshot.ElementName);
            Assert.AreEqual(1, gridSnapshot.Children.Count);
            Assert.AreEqual("#FFD3D3D3", (gridSnapshot.PropertyValues[3] as SolidColorBrush).Color.ToString());

            var textBlockSnapshot = gridSnapshot.Children[0];
            Assert.AreEqual(testPropertyNames, textBlockSnapshot.PropertyNames);
            Assert.AreEqual("ExampleTextBlock", textBlockSnapshot.ElementName);
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
            var snapshotCreator = new ElementSnapshotCreator(panel, new List<DependencyProperty>(), new List<string>());

            var snapshot = snapshotCreator.CreateTreeSnapshot();

            Assert.AreEqual(childCount, snapshot.Children.Count);
            for (int i = 0; i < childCount; i++)
            {
                Assert.AreEqual(0, snapshot.Children[i].Children.Count);
            }
        }

        [UITestMethod]
        public void ValidatesInputs()
        {
            var element = new Button();

            Assert.ThrowsException<ArgumentNullException>(() => {
                _ = new ElementSnapshotCreator(null,
                    new List<DependencyProperty>()
                    {
                        UIElement.VisibilityProperty
                    },
                    new List<string>()
                    {
                        "Visibility"
                    });
            });

            Assert.ThrowsException<ArgumentNullException>(() => {
                _ = new ElementSnapshotCreator(element,
                    null,
                    new List<string>()
                    {
                        "Visibility"
                    });
            });

            Assert.ThrowsException<ArgumentNullException>(() => {
                _ = new ElementSnapshotCreator(element,
                    new List<DependencyProperty>()
                    {
                        UIElement.VisibilityProperty
                    },
                    null);
            });

            Assert.ThrowsException<ArgumentException>(() => {
                _ = new ElementSnapshotCreator(element,
                    new List<DependencyProperty>()
                    {
                        UIElement.VisibilityProperty
                    },
                    new List<string>() { });
            });

            Assert.ThrowsException<ArgumentException>(() => {
                _ = new ElementSnapshotCreator(element,
                    new List<DependencyProperty>()
                    {
                    },
                    new List<string>()
                    {
                        "Visibility"
                    });
            });

        }

        private static ElementSnapshotCreator CreateElementSnapshotCreator(FrameworkElement element)
        {
            return new ElementSnapshotCreator(element, testDependencyProperties, testPropertyNames);
        }
    }
}
