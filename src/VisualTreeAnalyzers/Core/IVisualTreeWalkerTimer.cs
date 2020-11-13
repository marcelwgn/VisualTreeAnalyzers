
namespace VisualTreeAnalyzers.Core
{
    /// <summary>
    /// Default interface for <see cref="VisualtreeWalker"/> timer.
    /// </summary>
    public interface IVisualTreeWalkerTimer
    {
        /// <summary>
        /// Starts this timer.
        /// </summary>
        void StartTimer();

        /// <summary>
        /// Stops this time.
        /// </summary>
        void StopTimer();
    }
}
