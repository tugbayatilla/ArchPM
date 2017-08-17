using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchPM.Core.Extensions;

namespace ArchPM.Core.Test
{
    [TestClass]
    public class DatetimeExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToJsonDateTimeGivenDateTimeWhenInvalidDateThenThrowsArgumentOutOfRangeException()
        {
            DateTime dt = new DateTime();
            var result = dt.ToJsonDateTime();
        }

        [TestMethod]
        public void ToJsonDateTimeGivenDateTimeWhenValidDateThenReturnsTicks()
        {

            DateTime checkDateTime = DateTime.Parse("22 Jan 2013 03:00:00 PM");
            //found at internet
            Double checkDateTimeJsonEqualivent = 1358866800000;

            var jsonDateTime = checkDateTime.ToJsonDateTime();
            Assert.AreEqual(checkDateTimeJsonEqualivent, jsonDateTime);
        }

        [TestMethod]
        public void ToSqlDbAcceptableGivenDateTimeWhenNullThenReturnsNull()
        {
            DateTime? dt = null;
            var result = dt.ToSqlDbAcceptable();

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ToSqlDbAcceptableGivenDateTimeWhenInvalidDateThenReturnsNull()
        {
            DateTime dt = new DateTime();
            var result = dt.ToSqlDbAcceptable();

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDbDefaultStringGivenDateTimeWhenNullThenThrowsArgumentNullException()
        {
            DateTime? dt = null;
            var result = dt.ToDbDefaultString();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToDbDefaultStringGivenDateTimeWhenInvalidThenThrowsArgumentOutOfRangeException()
        {
            DateTime dt = new DateTime();
            var result = dt.ToDbDefaultString();
        }

        [TestMethod]
        public void ToDbDefaultStringGivenDateTimeWhenValidThenReturnsString()
        {
            DateTime dt = DateTime.Parse("01.01.2016 13:15:18");
            var result = dt.ToDbDefaultString();

            Assert.AreEqual("2016-01-01 13:15:18", result);
        }
    }
}
