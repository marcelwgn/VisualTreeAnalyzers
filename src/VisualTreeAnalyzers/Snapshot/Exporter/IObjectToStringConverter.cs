namespace VisualTreeAnalyzers.Snapshot.Exporter
{
    /// <summary>
    /// Interface defining operations needed for object to string converter objects.
    /// </summary>
    public interface IObjectToStringConverter
    {
        /// <summary>
        /// Gets a string representation of the object that can be used to present the object.
        /// </summary>
        /// <param name="obj">The object to get a presentation of.</param>
        /// <returns>The string representation</returns>
        string GetStringRepresentation(object obj);
    }
}