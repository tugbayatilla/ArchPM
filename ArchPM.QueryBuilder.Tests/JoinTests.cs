using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


namespace ArchPM.QueryBuilder.Tests
{
    [TestClass]
    public class JoinTests
    {
        [TestMethod]
        public void JoinOneTimeWithOneParameter()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => p.Id).Join<Person, Address>((a, b) => a.Id == b.Id).ToString();

            Assert.AreEqual("SELECT t0.Id FROM Person AS t0 LEFT JOIN Address AS t1 ON t0.Id = t1.Id", query);
        }

        [TestMethod]
        public void JoinMultipleWithOneParameter()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .ToString();

            Assert.AreEqual("SELECT t0.Id FROM Person AS t0 LEFT JOIN Address AS t1 ON t0.Id = t1.Id LEFT JOIN Address AS t1 ON t0.Id = t1.Id", query);
        }

        [TestMethod]
        public void JoinMultipleWithOneParameterWithWhereOneParameter()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .Where<Person>(p => p.Salary == 256)
                .ToString();

            Assert.AreEqual("SELECT t0.Id FROM Person AS t0 LEFT JOIN Address AS t1 ON t0.Id = t1.Id LEFT JOIN Address AS t1 ON t0.Id = t1.Id WHERE t0.Salary = '256'", query);
        }

        [TestMethod]
        public void JoinMultipleWithOneParameterWithWhereOneParameterWithMultipleSelects()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>(p => p.Id)
                .Select<Address>(p => new { p.Id, p.Description } )
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .Where<Person>(p => p.Salary == 256)
                .ToString();

            Assert.AreEqual("SELECT t0.Id, t1.Id, t1.Description FROM Person AS t0 LEFT JOIN Address AS t1 ON t0.Id = t1.Id WHERE t0.Salary = '256'", query);
        }

    }
}
