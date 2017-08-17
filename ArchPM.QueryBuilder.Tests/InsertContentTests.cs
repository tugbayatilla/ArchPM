using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchPM.QueryBuilder.Tests.Model;
using System.Linq;


namespace ArchPM.QueryBuilder.Tests
{
    [TestClass]
    public class InsertContentTests
    {
        [TestMethod]
        public void InsertContentIncludeSingleField()
        {
            var builder = new QBuilder();
            Person p = new Person();

            var content = builder.Insert(p, x => x.Name).InsertContent;

            Assert.AreEqual(1, content.Fields.Count);
            Assert.IsTrue(content.Fields.Any(x => x.Name == "Name"));
        }

        [TestMethod]
        public void InsertContentExcludeSingleField()
        {
            var builder = new QBuilder();
            Person p = new Person();

            var content = builder.Insert(p, x => x.Name, IncludeExclude.Exclude).InsertContent;

            Assert.AreEqual(17, content.Fields.Count);
            Assert.IsFalse(content.Fields.Any(x => x.Name == "Name"));
        }

        [TestMethod]
        public void InsertContentExcludeMultipleFields()
        {
            var builder = new QBuilder();
            Person p = new Person();

            var content = builder.Insert(p, x => new { x.Name, x.Birth, x.Gender }, IncludeExclude.Exclude).InsertContent;

            Assert.AreEqual(15, content.Fields.Count);
            Assert.IsFalse(content.Fields.Any(x => x.Name == "Name"));
            Assert.IsFalse(content.Fields.Any(x => x.Name == "Birth"));
            Assert.IsFalse(content.Fields.Any(x => x.Name == "Gender"));
        }

    }
}
