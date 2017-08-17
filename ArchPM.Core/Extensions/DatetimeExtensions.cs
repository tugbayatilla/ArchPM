using System;

namespace ArchPM.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatetimeExtensions
    {
        /// <summary>
        /// Returns the number of milliseconds since Jan 1, 1970
        /// This method can be used for Json Datetime
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">dt</exception>
        public static Double ToJsonDateTime(this DateTime dt)
        {
            if (dt == DateTime.MinValue || dt == DateTime.MaxValue)
                throw new ArgumentOutOfRangeException("dt");

            DateTime startPointDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan ts = new TimeSpan(dt.Ticks - startPointDateTime.Ticks);
            return ts.TotalMilliseconds;
        }

        /// <summary>
        /// To the database acceptable.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static DateTime? ToSqlDbAcceptable(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                return ToSqlDbAcceptable(dt.Value);
            }
            else
                return null;
        }

        /// <summary>
        /// To the database acceptable.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static DateTime? ToSqlDbAcceptable(this DateTime dt)
        {
            if (dt.Year <= 1900)
                return null;

            return dt;
        }

        /// <summary>
        /// To the database default string.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static String ToDbDefaultString(this DateTime dt)
        {
            if (dt == DateTime.MinValue || dt == DateTime.MaxValue)
                throw new ArgumentOutOfRangeException("dt");

            String result = String.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}",
                            dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            return result;
        }

        /// <summary>
        /// To the database default string.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static String ToDbDefaultString(this DateTime? dt)
        {
            dt.NotNull();

            return ToDbDefaultString(dt.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static String ToShortDateString(this DateTime? dt, String defaultValue = "")
        {
            String result = defaultValue;
            if (dt.HasValue)
                result = dt.Value.ToShortDateString();

            return result;
        }

    }
}
