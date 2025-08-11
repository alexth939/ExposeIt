using System.Globalization;
using System.IO;
using System.Linq;
using ExposeIt.Editor;
using ExposeIt.Runtime.Carriers;
using UnityEngine;

namespace ExposeIt.Examples
{
    /// <summary>
    /// Displays the total size of the project's <c>Assets</c> folder in the Play Mode Zone.
    /// <para>
    /// Updates every 5 seconds to reflect the current disk usage of all files under the Assets directory.
    /// </para>
    /// </summary>
    /// <remarks>
    /// - Implements <see cref="IPlayModeZoneUpdatableLabelProvider"/> to show dynamic text.<br/>
    /// - The size is automatically formatted into B, KB, MB, or GB units with up to two decimal places.<br/>
    /// - Errors in folder access (e.g. permission issues) are silently ignored, and the value will fall back to 0.
    /// </remarks>

    //[PlayModeZoneElementProvider(11)]
    public class PlayModeZoneExample5 : IPlayModeZoneUpdatableLabelProvider
    {
        public UpdatableLabelCarrier ProvideElement() => new()
        {
            UpdateIntervalInMS = 5000,
            TextGetter = GetText,
        };

        private string GetText()
        {
            string assetsPath = Application.dataPath;
            long totalSize = GetDirectorySize(assetsPath);
            return $"Assets Folder Size: {FormatBytes(totalSize)}";
        }

        private static long GetDirectorySize(string path)
        {
            try
            {
                var directory = new DirectoryInfo(path);

                return directory.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
            }
            catch
            {
                return 0;
            }
        }

        private const int OneKB = 1024;
        private const int OneMB = OneKB * 1024;
        private const int OneGB = OneMB * 1024;

        private string FormatBytes(long bytes)
        {
            (double value, string suffix) result = bytes switch
            {
                < OneKB => (bytes, " B"),
                < OneMB => (bytes / (double)OneKB, " KB"),
                < OneGB => (bytes / (double)OneMB, " MB"),
                _ => (bytes / (double)OneGB, " GB"),
            };

            return result.value.ToString("0.##", CultureInfo.InvariantCulture) + result.suffix;
        }
    }
}
