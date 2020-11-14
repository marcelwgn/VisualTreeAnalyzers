using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using VisualTreeAnalyzers.Core;
using VisualTreeAnalyzers.Tests.TestImplementations;
using VisualTreeAnalyzers.Tests.Utils;
using Windows.UI.Xaml.Controls;

namespace VisualTreeAnalyzers.Tests.Core
{
    [TestClass]
    public class VisualTreeWalkerTimerTests
    {
        [TestMethod]
        public void ScansAfterCertainTime()
        {
            var analyzer = new AnalyzeInvokeCountAnalyzer();
            VisualTreeWalker walker = null;
            VisualTreeWalkerTimer timer = null;

            RunOnUIThread.Execute(() =>
            {
                walker = new VisualTreeWalker(new Button(), analyzer);
                timer = new VisualTreeWalkerTimer(walker, 10);
                timer.StartTimer();
            });

            RunOnUIThread.WaitMilliSeconds(100);

            Logger.LogMessage("Analyzer count: " + analyzer.AnalyzeCount);
            Assert.IsTrue(analyzer.AnalyzeCount >= 1);
        }

        [TestMethod]
        public void ScansMultipleTimes()
        {
            var analyzer = new AnalyzeInvokeCountAnalyzer();
            VisualTreeWalker walker = null;
            VisualTreeWalkerTimer timer = null;

            RunOnUIThread.Execute(() =>
            {
                walker = new VisualTreeWalker(new Button(), analyzer);
                timer = new VisualTreeWalkerTimer(walker, 10);
                timer.StartTimer();
            });

            RunOnUIThread.WaitMilliSeconds(500);

            Logger.LogMessage("Analyzer count: " + analyzer.AnalyzeCount);
            Assert.IsTrue(analyzer.AnalyzeCount >= 2);
        }

        [TestMethod]
        public void StoppingTimerStopsTimer()
        {
            var analyzer = new AnalyzeInvokeCountAnalyzer();
            VisualTreeWalker walker = null;
            VisualTreeWalkerTimer timer = null;

            RunOnUIThread.Execute(() =>
            {
                walker = new VisualTreeWalker(new Button(), analyzer);
                timer = new VisualTreeWalkerTimer(walker, 10);
                timer.StartTimer();
            });
            RunOnUIThread.WaitMilliSeconds(100);
            Assert.IsTrue(analyzer.AnalyzeCount >= 1);

            var oldCount = 0;
            RunOnUIThread.Execute(() =>
            {
                timer.StopTimer();
                Logger.LogMessage("Analyzer count before stopping: " + analyzer.AnalyzeCount);
                oldCount = analyzer.AnalyzeCount;
            });

            RunOnUIThread.WaitMilliSeconds(400);
            Logger.LogMessage("Analyzer count after stopping: " + analyzer.AnalyzeCount);
            Assert.IsTrue(analyzer.AnalyzeCount == oldCount);
        }
    }
}
