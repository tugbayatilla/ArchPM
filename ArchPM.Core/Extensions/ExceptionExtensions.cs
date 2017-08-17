using System;
using System.Collections.Generic;

namespace ArchPM.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns the number of milliseconds since Jan 1, 1970
        /// This method can be used for Json Datetime
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static String RecursiveMessage(this Exception ex, Boolean showMessageTypeAsHeader = true)
        {
            String message = "";
            if (showMessageTypeAsHeader)
                message = String.Format("[{0}]:", ex.GetType().Name);

            message += ex.Message;

            if (ex.InnerException != null)
                message += String.Concat("\r\n ", RecursiveMessage(ex.InnerException, showMessageTypeAsHeader));

            return message;
        }

        public static IEnumerable<Exception> RecursiveExceptions(this Exception ex)
        {
            if (ex != null)
                yield return ex;
            if (ex.InnerException != null)
            {
                foreach (var item in RecursiveExceptions(ex.InnerException))
                {
                    yield return item;
                }

            }

            yield break;
        }


    }
}
