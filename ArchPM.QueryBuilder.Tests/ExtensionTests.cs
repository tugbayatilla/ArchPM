//using System;
//using System.Text;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ArchPM.QueryBuilder.SqlQueryGenerator.Tests.Model;
//using System.Linq;
//using System.Linq.Expressions;

//namespace ArchPM.QueryBuilder.SqlQueryGenerator.Tests
//{
//    [TestClass]
//    public class ExtensionTests
//    {
//        [TestMethod]
//        public void OrElse()
//        {
//            Expression<Func<SmallTable, bool>> exp = p => p.Id == 1;

//            exp = exp.OrElse(prop => prop.Id2 == 2);

//            var builder = new QBuilder(new TSqlQueryGenerator());
//            var query = builder.Select<SmallTable>().Where(exp).ToString();

//            Assert.AreEqual("SELECT t0.* FROM SmallTable AS t0 WHERE (t0.Id = 1 OR t0.Id2 = 2)", query);

//        }

//        [TestMethod]
//        public void AndAlso()
//        {
//            Expression<Func<SmallTable, bool>> exp = p => p.Id == 1;

//            exp = exp.AndAlso(prop => prop.Id2 == 2);

//            var builder = new QBuilder(new TSqlQueryGenerator());
//            var query = builder.Select<SmallTable>().Where(exp).ToString();

//            Assert.AreEqual("SELECT t0.* FROM SmallTable AS t0 WHERE t0.Id = 1 AND t0.Id2 = 2", query);
            
//        }

//        [TestMethod]
//        public void AndAlsoOrElse()
//        {
//            Expression<Func<SmallTable, bool>> exp = p => p.Id == 1;
//            Expression<Func<SmallTable, bool>> left = prop => prop.Id2 == 2;
//            Expression<Func<SmallTable, bool>> right = prop => prop.Id2 == 3;

//            Expression<Func<SmallTable, bool>> exp3 = left.OrElse(right);

//            exp = exp.AndAlso(exp3);

//            var builder = new QBuilder(new TSqlQueryGenerator());
//            var query = builder.Select<SmallTable>().Where(exp).ToString();

//            Assert.AreEqual("SELECT t0.* FROM SmallTable AS t0 WHERE t0.Id = 1 AND (t0.Id2 = 2 OR t0.Id2 = 3)", query);

//        }

//        [TestMethod]
//        public void ContainsOrElse()
//        {
//            List<Int32> idList = new List<int>() { 1,2,3 };
//            Expression<Func<SmallTable, bool>> exp = p => p.Id == 1;
//            Expression<Func<SmallTable, bool>> result = exp.OrElse(prop => idList.Contains(prop.Id));


//            var builder = new QBuilder(new TSqlQueryGenerator());
//            var query = builder.Select<SmallTable>().Where(result).ToString();

//            Assert.AreEqual("SELECT t0.* FROM SmallTable AS t0 WHERE (t0.Id = 1 OR t0.Id IN (1,2,3))", query);

//        }


//    }
//}
