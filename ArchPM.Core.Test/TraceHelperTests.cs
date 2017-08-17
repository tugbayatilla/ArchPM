using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace ArchPM.Core.Test
{
    [TestClass]
    public class TraceHelperTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TraceExceptionGivenExceptionWhenNullThenThrowsArgumentNullException()
        {
            TraceHelper.TraceException(null);
        }

        [TestMethod]
        public void TraceExceptionGivenExceptionWhenExceptionWithHelloWorldThenTraceTextWriter()
        {
            String uniqueText = String.Format("UFN_{0}", Utils.CreateUniqueNumber());
            Trace.Listeners.Add(new EventLogTraceListener("TraceHelper"));
            Exception ex = new Exception(uniqueText);
            String message = TraceHelper.TraceException(ex);


            EventLog myLog = new EventLog();
            myLog.Log = "Application";
            myLog.Source = "TraceHelper";
            var lastEntry = myLog.Entries[myLog.Entries.Count-1];

            Assert.AreEqual(message, lastEntry.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLineGivenStringWhenEmptyThenThrowsArgumentNullException()
        {
            TraceHelper.WriteLine("");
        }

        [TestMethod]
        public void WriteLineGivenMessageWhenNotEmptyThenReturnsMessageContainingGivenMessage()
        {
            String message = TraceHelper.WriteLine("Hello World");
            Assert.IsTrue(message.Contains("Hello World"));
        }


    }
}
