using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Tests.Utils
{
    // Class take from WinUI repository. Original source code:
    // https://github.com/microsoft/microsoft-ui-xaml/blob/master/test/MUXControlsTestApp/Utilities/RunOnUIThread.cs
    static class RunOnUIThread
    {
        public static void Execute(Action action)
        {
            Exception exception = null;
            var dispatcher = CoreApplication.MainView.Dispatcher;
            if (dispatcher.HasThreadAccess)
            {
                action();
            }
            else
            {
                // We're not on the UI thread, queue the work. Make sure that the action is not run until
                // the splash screen is dismissed (i.e. that the window content is present).
                var workComplete = new AutoResetEvent(false);
                // If the Splash screen dismissal happens on the UI thread, run the action right now.
                if (dispatcher.HasThreadAccess)
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        exception = e;
                        throw;
                    }
                    finally // Unblock calling thread even if action() throws
                    {
                        workComplete.Set();
                    }
                }
                else
                {
                    // Otherwise queue the work to the UI thread and then set the completion event on that thread.
                    var ignore = dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            try
                            {
                                action();
                            }
                            catch (Exception e)
                            {
                                exception = e;
                                throw;
                            }
                            finally // Unblock calling thread even if action() throws
                            {
                                workComplete.Set();
                            }
                        });
                }

                workComplete.WaitOne();
                if (exception != null)
                {
                    Assert.Fail("Exception thrown by action on the UI thread: " + exception.ToString());
                }

                workComplete.Dispose();
            }
        }

        public static void WaitMilliSeconds(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public static void WaitForTick()
        {
            var renderingEvent = new ManualResetEvent(false);

            EventHandler<object> renderingHandler = (object sender, object args) =>
            {
                renderingEvent.Set();
            };

            Execute(() =>
            {
                CompositionTarget.Rendering += renderingHandler;
            });

            renderingEvent.WaitOne();

            Execute(() =>
            {
                CompositionTarget.Rendering -= renderingHandler;
            });

            renderingEvent.Dispose();
        }
    }
}
