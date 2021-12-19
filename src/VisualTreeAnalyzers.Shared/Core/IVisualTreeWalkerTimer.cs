
namespace VisualTreeAnalyzers.Core
{
    /// <summary>
    /// Default interface for <see cref="VisualTreeWalkerTimer"/> timer.
    /// </summary>
    public interface IVisualTreeWalkerTimer
    {
        /// <summary>
        /// Starts this timer.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this time.
        /// </summary>
        void Stop();
    }
}
