using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchPM.QueryBuilder.SqlQueryGenerator.Tests.Model;

namespace ArchPM.QueryBuilder.SqlQueryGenerator.Tests
{
    [TestClass]
    public class SelectTests
    {
        [TestMethod]
        public void CountWithAlias()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => p.Birth).Count<Person>(p => new { MY_COUNT = p.Id }).ToString();

            Assert.AreEqual("SELECT t0.Birth, COUNT(t0.Id) AS [MY_COUNT] FROM Person AS t0", query);
        }

        [TestMethod]
        public void CountWithDefaultAlias()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p=>p.Birth).Count<Person>(p => p.Id).ToString();

            Assert.AreEqual("SELECT t0.Birth, COUNT(t0.Id) AS [COUNT] FROM Person AS t0", query);
        }

        [TestMethod]
        public void CountAll()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Count<Person>().ToString();

            Assert.AreEqual("SELECT COUNT(*) AS [COUNT] FROM Person AS t0", query);
        }

        [TestMethod]
        public void CountSpecific()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Count<Person>(p => p.Id).ToString();

            Assert.AreEqual("SELECT COUNT(t0.Id) AS [COUNT] FROM Person AS t0", query);
        }

        [TestMethod]
        public void SelectStarFromPersonWithDefaultTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0", query);
        }

        [TestMethod]
        public void SelectSingleFieldFromPersonWithDefaultTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => p.Id).ToString();

            Assert.AreEqual("SELECT t0.Id FROM Person AS t0", query);
        }

        [TestMethod]
        public void SelectMultipleFieldsFromPersonWithDefaultTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => new { p.Id, p.Id2 }).ToString();

            Assert.AreEqual("SELECT t0.Id, t0.Id2 FROM Person AS t0", query);
        }

        [TestMethod]
        public void SelectMultipleFieldsFromPersonWithNewTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>(p => new { p.Id, p.Id2 })
                .ChangeTableName<Person>("NewTableName")
                .ToString();

            Assert.AreEqual("SELECT t0.Id, t0.Id2 FROM NewTableName AS t0", query);
        }

        [TestMethod]
        public void SelectLookLikeMultipleFieldsButSelectOnlyOneFieldFromPersonWithDefaultTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => new { p.Id }).ToString();

            Assert.AreEqual("SELECT t0.Id FROM Person AS t0", query);
        }

        [TestMethod]
        public void SelectNullFromPersonWithDefaultTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => null).ToString();
            Assert.AreEqual("SELECT t0.* FROM Person AS t0", query);
        }

        [TestMethod]
        public void MultipleSelectStarFromPersonWithDefaultTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => p.Id).Select<Address>(p => p.Id2).ToString();

            Assert.AreEqual("SELECT t0.Id, t1.Id2 FROM Person AS t0, Address AS t1", query);
        }


        [TestMethod]
        public void MultipleSelectMultipleFieldsFromPersonWithDefaultTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => new { p.Id, p.Id2 }).Select<Address>(p => new { p.Id, p.Id2 }).ToString();

            Assert.AreEqual("SELECT t0.Id, t0.Id2, t1.Id, t1.Id2 FROM Person AS t0, Address AS t1", query);
        }

        [TestMethod]
        public void MultipleSelectMultipleFieldsFromPersonWithNewTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => new { p.Id, p.Id2 })
                                  .ChangeTableName<Person>("NewTableName")
                                  .Select<Address>(p => new { p.Id, p.Id2 })
                                  .ChangeTableName<Address>("NewTableName2")
                                  .ToString();

            Assert.AreEqual("SELECT t0.Id, t0.Id2, t1.Id, t1.Id2 FROM NewTableName AS t0, NewTableName2 AS t1", query);
        }

        [TestMethod]
        public void MultipleSelectLookLikeMultipleFieldsButSelectOnlyOneFieldFromPersonWithDefaultTablename()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => new { p.Id }).Select<Address>(p => new { AddressId = p.Id }).ToString();

            Assert.AreEqual("SELECT t0.Id, t1.Id AS [AddressId] FROM Person AS t0, Address AS t1", query);
        }


        [TestMethod]
        public void SelectSingleFieldWithAlias()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => new { MyAlias = p.Id }).ToString();

            Assert.AreEqual("SELECT t0.Id AS [MyAlias] FROM Person AS t0", query);
        }

    }
}
