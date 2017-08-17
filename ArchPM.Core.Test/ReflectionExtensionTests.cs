using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using ArchPM.Core.Extensions;
using ArchPM.Core.Extensions.ReflectionExtensions;
using ArchPM.Core.Extensions.ObjectExtensions;

namespace ArchPM.Core.Tests
{
    [TestClass]
    public class ReflectionExtensionTests
    {
        [TestMethod]
        public void FillRandomDataGivenEntityWhenInistantiateThenReturnsEntityWithFilledProperties()
        {
            Person p = new Person();
            p.FillDummyData();

            Assert.AreEqual("test", p.Name, "Name");
            Assert.AreEqual("test", p.Name2, "Name2");
            Assert.AreEqual(new DateTime(2000,1,1), p.Birth, "Birth");
            Assert.AreEqual(new DateTime(2000,1,1), p.Birth2, "Birth2");
            Assert.AreEqual(Fears.Dark, p.Fear, "Fear");
            Assert.AreEqual(Fears.Dark, p.Fear2, "Fear2");
            Assert.AreEqual(Genders.Male, p.Gender, "Gender");
            Assert.AreEqual(Genders.Male, p.Gender2, "Gender2");
            Assert.AreEqual(99, p.Height, "Height");
            Assert.AreEqual(99, p.Height2, "Height2");
            Assert.AreEqual(99, p.Id, "Id");
            Assert.AreEqual(99, p.Id2, "Id2");
            Assert.AreEqual(true, p.IsFriendly, "IsFriendly");
            Assert.AreEqual(true, p.IsFriendly2, "IsFriendly2");
            Assert.AreEqual("test", p.OnlyRead, "OnlyRead");
        }

    }
}
