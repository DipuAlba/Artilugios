using System;

namespace SeDipuAlba.Artilugios
{
    /// <summary>
    /// Provides utility methods for converting time formats to DateTime.
    /// Based on ScottCher code: https://stackoverflow.com/a/250400/2126607
    /// </summary>
    public class TimeConversion
    {
        /// <summary>
        /// Converts a Unix timestamp to a DateTime object.
        /// </summary>
        /// <param name="unixTimeStamp">The Unix timestamp to convert, represented as a double. 
        /// This value represents the number of seconds that have elapsed since January 1, 1970 (UTC).</param>
        /// <returns>The converted DateTime object adjusted to local time.</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Define the starting point as January 1, 1970 (the Unix epoch) in UTC.
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            // Add the number of seconds given by unixTimeStamp to the epoch.
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            // Convert to local time from UTC.
            return dateTime;
        }

        /// <summary>
        /// Converts a Java timestamp to a DateTime object.
        /// </summary>
        /// <param name="javaTimeStamp">The Java timestamp to convert, represented as a double.
        /// This value represents the number of milliseconds that have elapsed since January 1, 1970 (UTC).</param>
        /// <returns>The converted DateTime object adjusted to local time.</returns>
        public static DateTime JavaTimeStampToDateTime(double javaTimeStamp)
        {
            // Define the starting point as January 1, 1970 (the Unix epoch) in UTC.
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            // Add the number of milliseconds given by javaTimeStamp to the epoch.
            dateTime = dateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
            // Convert to local time from UTC.
            return dateTime;
        }
    }
}
