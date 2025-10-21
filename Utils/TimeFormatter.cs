namespace PomodoroApp.Utils
{
    /// <summary>
    /// Utility class for formatting time values
    /// </summary>
    public static class TimeFormatter
    {
        /// <summary>
        /// Formats seconds into MM:SS format
        /// </summary>
        /// <param name="totalSeconds">Total seconds to format</param>
        /// <returns>Formatted time string in MM:SS format</returns>
        public static string FormatTime(int totalSeconds)
        {
            if (totalSeconds < 0)
            {
                totalSeconds = 0;
            }

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        /// <summary>
        /// Formats seconds into a human-readable format (e.g., "5 minutes", "1 minute 30 seconds")
        /// </summary>
        /// <param name="totalSeconds">Total seconds to format</param>
        /// <returns>Human-readable time string</returns>
        public static string FormatTimeHumanReadable(int totalSeconds)
        {
            if (totalSeconds < 0)
            {
                return "0 seconds";
            }

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            if (minutes == 0)
            {
                return seconds == 1 ? "1 second" : $"{seconds} seconds";
            }

            string minutesPart = minutes == 1 ? "1 minute" : $"{minutes} minutes";

            if (seconds == 0)
            {
                return minutesPart;
            }

            string secondsPart = seconds == 1 ? "1 second" : $"{seconds} seconds";
            return $"{minutesPart} {secondsPart}";
        }

        /// <summary>
        /// Converts minutes to seconds
        /// </summary>
        /// <param name="minutes">Number of minutes</param>
        /// <returns>Total seconds</returns>
        public static int MinutesToSeconds(int minutes)
        {
            return minutes * 60;
        }

        /// <summary>
        /// Converts seconds to minutes (rounded down)
        /// </summary>
        /// <param name="seconds">Total seconds</param>
        /// <returns>Number of minutes</returns>
        public static int SecondsToMinutes(int seconds)
        {
            return seconds / 60;
        }
    }
}
