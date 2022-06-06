using Windows.UI.Xaml.Media;

namespace VisualTreeAnalyzers.Snapshot.Exporter
{
    /// <summary>
    /// Default implementation of <see cref="IObjectToStringConverter"/>.
    /// This class contains common representation for objects where ToString() is not a helpful representation such as <see cref="SolidColorBrush"/>.
    /// Otherwise, ToString of that object will be returned.
    /// </summary>
    public sealed class StandardObjectToStringConverter : IObjectToStringConverter
    {

        /// <inheritdoc/>
        public string GetStringRepresentation(object obj)
        {
            if(obj is SolidColorBrush solidColorBrush)
            {
                // SolidColorBrush should return its color
                return solidColorBrush.Color.ToString();
            }

            // Nothing matched, just return default.
            return obj?.ToString();
        }
    }
}
