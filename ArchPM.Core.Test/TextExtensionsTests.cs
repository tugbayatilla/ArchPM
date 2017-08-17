using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ArchPM.Core.Extensions;

namespace ArchPM.Core.Tests
{
    [TestClass]
    public class TextExtensionsTests
    {
        [TestMethod]
        public void IsValidTCKNGivenTcknWhenInValidThenReturnsFalse()
        {
            String tckn = "aaaaaaaaaaa";

            Boolean isValid = tckn.IsValidTCKN();

            Assert.AreEqual(false, isValid);
        }

        [TestMethod]
        public void IsValidTCKNGivenTcknWhenValidThenReturnsTrue()
        {
            String tckn = "58984057220";

            Boolean isValid = tckn.IsValidTCKN();

            Assert.AreEqual(true, isValid);
        }

        [TestMethod]
        public void IsValidTCKNGivenTcknWhenLessNumbersThenReturnsFalse()
        {
            String tckn = "1111111111";

            Boolean isValid = tckn.IsValidTCKN();

            Assert.AreEqual(false, isValid);
        }

        [TestMethod]
        public void IsValidTCKNGivenTcknWhenNullThenReturnsFalse()
        {
            String tckn = null;

            Boolean isValid = tckn.IsValidTCKN();

            Assert.AreEqual(false, isValid);
        }

        [TestMethod]
        public void IsValidTCKNGivenTcknWhenEmptyStringThenReturnsFalse()
        {
            String tckn = "";

            Boolean isValid = tckn.IsValidTCKN();

            Assert.AreEqual(false, isValid);
        }


    }
}
