using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchPM.QueryBuilder.Tests.Model;
using System.Linq;
using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder.Tests
{
    [TestClass]
    public class SelectContentTests
    {
        [TestMethod]
        public void SelectContentChangeAliasTableInfoCheck()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(null)
                .SetTableAlias<Person>("NewPerson")
                .SelectContents;
            Assert.AreEqual(1, contents.Count, "Total count of select operator");

            var content = contents.First();
            Assert.IsTrue((bool)content.TableInfo.CheckTableInfo("Person", "NewPerson", String.Empty));
        }

        [TestMethod]
        public void SelectContentChangeTabelNameTableInfoCheck()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(null)
                .ChangeTableName<Person>("NewPerson")
                .SelectContents;
            Assert.AreEqual(1, contents.Count, "Total count of select operator");

            var content = contents.First();
            Assert.IsTrue((bool)content.TableInfo.CheckTableInfo("Person", "t0", "NewPerson"));
        }

        [TestMethod]
        public void SelectContentTableInfoNameAliasPrefixCheck()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(null)
                .SetTableAlias<Person>("NewPerson")
                .SelectContents;
            Assert.AreEqual(1, contents.Count, "Total count of select operator");

            var content = contents.First();
            Assert.AreEqual(0, content.Items.Count);
        }

        [TestMethod]
        public void SelectContentNoFieldEntered()
        {
            var builder = new QBuilder();

            var contents = builder.Select<Person>().SelectContents;
            Assert.AreEqual(1, contents.Count, "Total count of select operator");

            var content = contents.First();
            Assert.AreEqual(0, content.Items.Count);
        }

        [TestMethod]
        public void SelectContentOneFieldEntered()
        {
            var builder = new QBuilder();

            var contents = builder.Select<Person>(p => p.Id).SelectContents;
            Assert.AreEqual(1, contents.Count, "Total count of select operator");

            var content = contents.First();
            Assert.AreEqual(1, content.Items.Count);

            var tuble = content.Items.Check("Id");
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void SelectContentThreeFieldsEntered()
        {
            var builder = new QBuilder();

            var contents = builder.Select<Person>(p => new { p.Id, p.Name, p.Birth }).SelectContents;
            Assert.AreEqual(1, contents.Count, "Total count of select operator");

            var content = contents.First();
            Assert.AreEqual(3, content.Items.Count);

            var tuble = content.Items.Check("Id", "Name", "Birth");
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void SelectContentTwoFieldsEnteredWithAlias()
        {
            var builder = new QBuilder();

            var contents = builder.Select<Person>(p => new { NewId = p.Id, NewBirth = p.Birth }).SelectContents;
            Assert.AreEqual(1, contents.Count, "Total count of select operator");

            var content = contents.First();
            Assert.AreEqual(2, content.Items.Count);

            var tuble = content.Items.Check("Id", "Birth");
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));


            Assert.IsTrue(content.Items[0] is FieldContentItem);
            Assert.AreEqual("Id", (content.Items[0] as FieldContentItem).Value);
            Assert.AreEqual("NewId", (content.Items[0] as FieldContentItem).Alias);

            Assert.IsTrue(content.Items[1] is FieldContentItem);

        }


        [TestMethod]
        public void SelectContentSelectTwoTableWithNoFieldEntered()
        {
            var builder = new QBuilder();

            var contents = builder.Select<Person>()
                                  .Select<Address>()
                                  .SelectContents;
            Assert.AreEqual(2, contents.Count, "Total count of select operator");

            Assert.IsTrue((bool)contents[0].TableInfo.CheckTableInfo("Person", "t0",""));
            Assert.IsTrue((bool)contents[1].TableInfo.CheckTableInfo("Address", "t1",""));
        }

        [TestMethod]
        public void SelectContentSelectTwoTableWithOneFieldForEachTable()
        {
            var builder = new QBuilder();

            var contents = builder.Select<Person>(p => p.Id)
                                  .Select<Address>(p => p.Description)
                                  .SelectContents;
            Assert.AreEqual(2, contents.Count, "Total count of select operator");

            var content1 = contents.First();
            var content2 = contents.Last();

            Assert.IsTrue((bool)content1.TableInfo.CheckTableInfo("Person", "t0", ""));
            Assert.AreEqual(1, content1.Items.Count);
            var tuble1 = content1.Items.Check("Id");
            Assert.IsTrue(tuble1.Item1, String.Format("Value: {0}", tuble1.Item2));

           
            Assert.IsTrue((bool)content2.TableInfo.CheckTableInfo("Address", "t1", ""));
            Assert.AreEqual(1, content2.Items.Count);
            var tuble2 = content2.Items.Check("Description");
            Assert.IsTrue(tuble2.Item1, String.Format("Value: {0}", tuble2.Item2));
        }

        [TestMethod]
        public void SelectContentOneFieldToNull()
        {
            var builder = new QBuilder();

            var contents = builder.Select<Person>(p => null)
                                  .SelectContents;
            Assert.AreEqual(1, contents.Count, "Total count of select operator");

            var content = contents.First();

            Assert.IsTrue((bool)content.TableInfo.CheckTableInfo("Person", "t0", ""));
            Assert.AreEqual(0, content.Items.Count);
        }




    }
}
