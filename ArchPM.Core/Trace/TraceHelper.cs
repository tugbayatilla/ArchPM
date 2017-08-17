using System;
using ArchPM.Core.Extensions;

namespace ArchPM.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class TraceHelper
    {
        /// <summary>
        /// Traces the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>message itself</returns>
        public static String TraceException(Exception ex)
        {
            ex.NotNull("ex argument is null!");

            String recMsg = ex.RecursiveMessage();
            String message = prepareMessage(recMsg);

            System.Diagnostics.Trace.TraceError(message);
            System.Diagnostics.Trace.Flush();

            return message;
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>message itself</returns>
        [Obsolete("use TraceDebug instead!")]
        public static String WriteLine(String message, params Object[] args)
        {
            message.NotNullOrEmpty("message is null or empty");

            message = prepareMessage(message);
            System.Diagnostics.Trace.WriteLine(String.Format(message, args));
            System.Diagnostics.Debug.WriteLine(String.Format(message, args));

            return message;
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>message itself</returns>
        public static String TraceDebug(String message)
        {
            message.NotNullOrEmpty("message is null or empty");

            message = prepareMessage(message);
            System.Diagnostics.Trace.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);

            return message;
        }

        /// <summary>
        /// Prepares the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        internal static String prepareMessage(String message)
        {
            var messageStr = String.Format("[{0:dd.MM.yyyy HH:mm:ss.ffff}]::{1}", DateTime.Now, message);
            return messageStr;

        }

    }

}
