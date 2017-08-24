using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Linq.Expressions;
using ArchPM.Core.Extensions;


namespace ArchPM.QueryBuilder.Tests
{
    [TestClass]
    public class PagingTests
    {
        [TestMethod]
        public void Paging()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => p.Id).Where<Person>(p => p.Id == 1).Paging<Person>(5, 10, p => p.Id).ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id ASC) AS trow, t0.Id  FROM Person AS t0 WHERE t0.Id = 1) AS t99 WHERE t99.trow BETWEEN 50 AND 60 ORDER BY t99.Id ASC", query);
        }

        [TestMethod]
        public void PagingDynamicOrderBy()
        {
            ParameterExpression pe = Expression.Parameter(typeof(Person), "p");
            var propertyInfo = Extend<Person>.GetPropertyInfoByPropertyName("id");
            Expression property = Expression.Property(pe, propertyInfo);
            Expression<Func<Person, Object>> predicate = p => property;

            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => p.Id).Where<Person>(p => p.Id == 1).Paging<Person>(5, 10, predicate).ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id ASC) AS trow, t0.Id  FROM Person AS t0 WHERE t0.Id = 1) AS t99 WHERE t99.trow BETWEEN 50 AND 60 ORDER BY t99.Id ASC", query);
        }

        [TestMethod]
        public void PagingSingle()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            
            String query = builder.Select<Person>().Paging<Person>(10, 20, x => x.Id)
                .ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id ASC) AS trow, t0.* FROM Person AS t0) AS t99 WHERE t99.trow BETWEEN 200 AND 220 ORDER BY t99.Id ASC", query);
        }

        [TestMethod]
        public void PagingOrderBy()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Paging<Person>(10, 20, x => x.Gender)
                .ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Gender ASC) AS trow, t0.* FROM Person AS t0) AS t99 WHERE t99.trow BETWEEN 200 AND 220 ORDER BY t99.Gender ASC", query);
        }

        [TestMethod]
        public void PagingOrderByDesc()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Paging<Person>(10, 20, x => x.Gender, OrderByDirections.Desc)
                .ToString();

            Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Gender DESC) AS trow, t0.* FROM Person AS t0) AS t99 WHERE t99.trow BETWEEN 200 AND 220 ORDER BY t99.Gender DESC", query);
        }


    }
}
