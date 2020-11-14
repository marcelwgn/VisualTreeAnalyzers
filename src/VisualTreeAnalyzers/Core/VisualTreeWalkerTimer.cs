using System;
using Windows.UI.Xaml;

namespace VisualTreeAnalyzers.Core
{
    /// <summary>
    /// This timer allows to periodically scan the visual tree of the current root.
    /// Due to performance reasons, listening to changes is no option to have "live" scanning.
    /// </summary>
    public sealed class VisualTreeWalkerTimer : IVisualTreeWalkerTimer
    {

        private readonly DispatcherTimer timer;
        private readonly IVisualTreeWalker visualTreeWalker;

        /// <summary>
        /// Creates a new VisualTreeWalkerTimer with the given <see cref="IVisualTreeWalker"/> as walker that will being used for scanning.
        /// A scan will occur every 5 seconds.
        /// </summary>
        /// <param name="visualTreeWalker">The <see cref="IVisualTreeWalker"/> that will be used for scanning.</param>
        public VisualTreeWalkerTimer(IVisualTreeWalker visualTreeWalker) : this(visualTreeWalker, 5000) { }

        /// <summary>
        /// Creates a new VisualTreeWalkerTimer with the given <see cref="IVisualTreeWalker"/> as walker that will being used for scanning.
        /// The milliSeconds parameter will be used to determine the frequency of the scanning.
        /// </summary>
        /// <param name="visualTreeWalker">The <see cref="IVisualTreeWalker"/> that will be used for scanning.</param>
        /// <param name="milliSeconds">The amount of milliseconds between scans.</param>
        public VisualTreeWalkerTimer(IVisualTreeWalker visualTreeWalker, int milliSeconds)
        {
            if(milliSeconds <= 0)
            {
                throw new ArgumentException("Parameter milliSeconds must be strictly positive.", nameof(milliSeconds));
            }

            this.visualTreeWalker = visualTreeWalker ?? throw new ArgumentNullException(nameof(visualTreeWalker));

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(milliSeconds);
        }

        /// <summary>
        /// Starts this timer.
        /// </summary>
        public void StartTimer()
        {
            timer.Start();
        }

        /// <summary>
        /// Stops this time.
        /// </summary>
        public void StopTimer()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, object e)
        {
            visualTreeWalker.ScanVisualTree();
        }
    }
}
