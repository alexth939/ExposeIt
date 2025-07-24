using System;
using ExposeIt.Editor;
using ExposeIt.Runtime.Carriers;

namespace ExposeIt.Examples
{
    /// <summary>
    /// Displays a live countdown label in the Play Mode Zone until a fixed deadline.
    /// <para>
    /// Updates the displayed time every second, showing days, hours, minutes, and seconds remaining.<br/>
    /// If the deadline is reached or passed, the Unity Editor will exit immediately.
    /// </para>
    /// </summary>
    /// <remarks>
    /// - Uses <see cref="IPlayModeZoneUpdatableLabelProvider"/> to provide dynamic text updates.<br/>
    /// - The update interval is set to 1000ms (1 second).<br/>
    /// - Be cautious: this forcibly closes the Unity Editor if the countdown reaches zero,
    /// which may cause unsaved work to be lost.
    /// </remarks>

    //[PlayModeZoneElementProvider(10)]
    public class PlayModeZoneExample4 : IPlayModeZoneUpdatableLabelProvider
    {
        private static DateTime DeadlineDate = new(2035, 7, 19, 23, 54, 0);

        public UpdatableLabelCarrier ProvideElement() => new()
        {
            UpdateIntervalInMS = 1000,
            TextGetter = TextGetter,
        };

        private string TextGetter()
        {
            var timeLast = DeadlineDate - DateTime.Now;

            if(timeLast.TotalSeconds <= 0)
                UnityEditor.EditorApplication.Exit(0);

            return $"Time Left: {timeLast.Days} days, {timeLast.Hours:00}:{timeLast.Minutes:00}:{timeLast.Seconds:00}";
        }
    }
}
