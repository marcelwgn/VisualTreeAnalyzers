using System;

namespace VisualTreeAnalyzers.Snapshot
{
    /// <summary>
    /// Helper class to extract namespace/type names from a full type name.
    /// </summary>
    public static class NamespaceHelper
    {
        /// <summary>
        /// Extracts the namespace from a given full typename.
        /// </summary>
        /// <param name="fullTypeName">The full typename.</param>
        /// <returns>The namespace of the type or an empty string if there was no namespace.</returns>
        public static string GetNameSpace(string fullTypeName)
        {
            if (fullTypeName is null) throw new ArgumentNullException(nameof(fullTypeName));
            int splitIndex = fullTypeName.LastIndexOf(".", StringComparison.Ordinal);
            return splitIndex < 1 ? "" : fullTypeName.Substring(0, splitIndex);
        }

        /// <summary>
        /// Extracts the type name from a given full typename.
        /// </summary>
        /// <param name="fullTypeName">The full typename.</param>
        /// <returns>The type name of the given full typename.</returns>
        public static string GetTypeName(string fullTypeName)
        {
            if (fullTypeName is null) throw new ArgumentNullException(nameof(fullTypeName));
            int splitIndex = fullTypeName.LastIndexOf(".", StringComparison.Ordinal) + 1;
            return fullTypeName.Substring(splitIndex, fullTypeName.Length - splitIndex);
        }
    }
}
