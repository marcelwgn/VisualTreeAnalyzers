using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace VisualTreeAnalyzers.Tests.Utils
{
    public static class SampleLoader
    {
        /// <summary>
        /// Returns the string in the file.
        /// </summary>
        /// <param name="fileName">The path of the file.</param>
        /// <returns></returns>
        public static async Task<string> LoadString(string fileName)
        {
            Uri derivedSource = GetDerivedSource(fileName);
            var file = await StorageFile.GetFileFromApplicationUriAsync(derivedSource);
            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Loads the file and returns the root element if it is a FrameworkElement or returns null otherwise.
        /// Throws if XAML is not valid.
        /// </summary>
        /// <param name="fileName">The path of the file.</param>
        /// <returns></returns>
        public static FrameworkElement LoadElement(string filePath)
        {
            var rawText = LoadString(filePath).GetAwaiter().GetResult();
            try
            {
                var xamlResult = XamlReader.LoadWithInitialTemplateValidation(rawText);
                return (FrameworkElement)xamlResult;
            }
            catch (InvalidCastException)
            {
                return null;
            }
        }

        private static Uri GetDerivedSource(string rawSource)
        {
            return new Uri(new Uri("ms-appx:///"), rawSource);
        }
    }
}
