using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchPM.QueryBuilder.SqlQueryGenerator.Tests.Model;
using System.Linq;
using System.Linq.Expressions;


namespace ArchPM.QueryBuilder.SqlQueryGenerator.Tests
{
    [TestClass]
    public class PagingTests
    {
        [TestMethod]
        public void Paging()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => p.Id).Where<Person>(p => p.Id == 1).Paging<Person>(5, 10, p => p.Id).ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id Asc) AS trow,  t0.Id FROM Person AS t0 WHERE t0.Id = 1) AS t99  WHERE t99.trow BETWEEN 5 AND 15 ORDER BY t99.Id Asc", query);
        }

        [TestMethod]
        public void PagingDynamicOrderBy()
        {
            ParameterExpression pe = Expression.Parameter(typeof(Person), "p");
            var propertyInfo = ArchPM.Core.Utils.FindProperty<Person>("id");
            Expression property = Expression.Property(pe, propertyInfo);
            Expression<Func<Person, Object>> predicate = p => property;

            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => p.Id).Where<Person>(p => p.Id == 1).Paging<Person>(5, 10, predicate).ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id Asc) AS trow,  t0.Id FROM Person AS t0 WHERE t0.Id = 1) AS t99  WHERE t99.trow BETWEEN 5 AND 15 ORDER BY t99.Id Asc", query);
        }

        [TestMethod]
        public void PagingSingle()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            
            String query = builder.Select<Person>().Paging<Person>(10, 20, x => x.Id)
                .ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id Asc) AS trow,  t0.* FROM Person AS t0) AS t99  WHERE t99.trow BETWEEN 10 AND 30 ORDER BY t99.Id Asc", query);
        }

        [TestMethod]
        public void PagingOrderBy()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Paging<Person>(10, 20, x => x.Gender)
                .ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Gender Asc) AS trow,  t0.* FROM Person AS t0) AS t99  WHERE t99.trow BETWEEN 10 AND 30 ORDER BY t99.Gender Asc", query);
        }

        [TestMethod]
        public void PagingOrderByDesc()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Paging<Person>(10, 20, x => x.Gender, OrderByDirections.Desc)
                .ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Gender Desc) AS trow,  t0.* FROM Person AS t0) AS t99  WHERE t99.trow BETWEEN 10 AND 30 ORDER BY t99.Gender Desc", query);
        }


    }
}
