using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchPM.QueryBuilder.SqlQueryGenerator.Tests.Model;
using System.Linq;
using ArchPM.Core.Enums;
using ArchPM.Core.Exceptions;

namespace ArchPM.QueryBuilder.SqlQueryGenerator.Tests
{
    [TestClass]
    public class WhereTests
    {
        [TestMethod]
        public void WhereContainsEnumsCastToInt32()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            var list = EnumManager<Genders>.GetList().Select(p => p.Value);

            String query = builder.Select<Person>().Where<Person>(p => list.Contains((Int32)p.Gender)).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Gender IN (0,1,2)", query);
        }

        [TestMethod]
        public void WhereAndAndOr()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.IsFriendly && p.IsFriendly2.HasValue && (p.Id == 1 || p.Id2 == 2)).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly = 1 AND t0.IsFriendly2 IS NOT NULL AND (t0.Id = 1 OR t0.Id2 = 2)", query);
        }

        [TestMethod]
        public void WhereOrElse()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.IsFriendly || p.IsFriendly2.HasValue).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly = 1 OR t0.IsFriendly2 IS NOT NULL", query);
        }

        [TestMethod]
        public void WhereNotBooleanWithoutEquals()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => !p.IsFriendly && !p.IsFriendly2.HasValue).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly != 1 AND t0.IsFriendly2 IS NULL", query);
        }

        [TestMethod]
        public void WhereSingleIntValueNot()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Id != 1).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id != 1", query);
        }

        [TestMethod]
        public void WhereSingleNullableIntValueIsNotNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Id2 != null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NOT NULL", query);
        }

        [TestMethod]
        public void WhereSingleNullableIntValueNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Id2 == null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NULL", query);
        }

        [TestMethod]
        public void WhereSingleNullableIntHasValue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Id2.HasValue).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NOT NULL", query);
        }

        [TestMethod]
        public void WhereSingleNullableIntHasNotValue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => !p.Id2.HasValue).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NULL", query);
        }

        [TestMethod]
        public void WhereSingleIntValue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Id == 1).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id = 1", query);
        }

        [TestMethod]
        public void SelectMultipleFieldFromPersonWithDefaultTablenameAndWhereIdEquals1()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>(p => new { p.Id, p.Id2 }).Where<Person>(p => p.Id == 1).ToString();

            Assert.AreEqual("SELECT t0.Id, t0.Id2 FROM Person AS t0 WHERE t0.Id = 1", query);
        }

        [TestMethod]
        public void WhereSingleStringValue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Name == "my name is").ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Name = 'my name is'", query);
        }

        [TestMethod]
        public void WhereSingleStringIsNullValue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Name == null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Name IS NULL", query);
        }

        [TestMethod]
        public void WhereSingleStringIsStringIsNullOrEmpty()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => String.IsNullOrEmpty(p.Name)).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE (t0.Name IS NULL OR t0.Name = '')", query);
        }

        [TestMethod]
        public void WhereSingleIntegerWithParameter()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Int32 id = 1;

            String query = builder.Select<Person>().Where<Person>(p => p.Id == id).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id = 1", query);
        }

        [TestMethod]
        public void WhereSingleStringContainsInStringList()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            List<String> stringList = new List<string>();
            stringList.Add("123");
            stringList.Add("aBc");
            stringList.Add("Test123");

            String query = builder.Select<Person>().Where<Person>(p => stringList.Contains(p.Name)).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Name IN ('123','aBc','Test123')", query);
        }

        [TestMethod]
        public void WhereSingleStringContainsInInt32AsEnumerable()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            List<Int32> stringList = new List<Int32>();
            stringList.Add(1);
            stringList.Add(2);
            stringList.Add(3);

            String query = builder.Select<Person>().Where<Person>(p => stringList.AsEnumerable().Contains(p.Id)).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id IN (1,2,3)", query);
        }

        [TestMethod]
        public void WhereSingleStringContainsInInt32IEnumerable()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            List<Int32> stringList = new List<Int32>();
            stringList.Add(1);
            stringList.Add(2);
            stringList.Add(3);

            IEnumerable<Int32> xx = stringList.AsEnumerable();

            String query = builder.Select<Person>().Where<Person>(p => xx.Contains(p.Id)).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id IN (1,2,3)", query);
        }

        [TestMethod]
        public void WhereSingleStringNotContainsInInt32IEnumerable()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            List<Int32> stringList = new List<Int32>();
            stringList.Add(1);
            stringList.Add(2);
            stringList.Add(3);

            IEnumerable<Int32> xx = stringList.AsEnumerable();

            String query = builder.Select<Person>().Where<Person>(p => !xx.Contains(p.Id)).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id NOT IN (1,2,3)", query);
        }

        [TestMethod]
        public void WhereMultibleIntegerParameters()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Id == 1 && p.Id2 == 2).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id = 1 AND t0.Id2 = 2", query);
        }

        [TestMethod]
        public void WhereMultibleIntegerParametersWithOneOfThemNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Id == 1 && p.Id2 == null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id = 1 AND t0.Id2 IS NULL", query);
        }

        [TestMethod]
        public void WhereMultibleIntegerParametersWithOneOfThemNotNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder.Select<Person>().Where<Person>(p => p.Id == 1 && p.Id2 != null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id = 1 AND t0.Id2 IS NOT NULL", query);
        }

        [TestMethod]
        public void WhereSingleIntegerParameterComingFromInterface()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            IMyInterface interfaceClass = new MyInterfaceClass();
            interfaceClass.Id = 13;

            String query = builder.Select<MyInterfaceClass>().Where<MyInterfaceClass>(p => p.Id == interfaceClass.Id).ToString();

            Assert.AreEqual("SELECT t0.* FROM MyInterfaceClass AS t0 WHERE t0.Id = 13", query);
        }

        [TestMethod]
        public void WhereMultipleIntegerParametersComingFromInterface()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            IMyInterface interfaceClass = new MyInterfaceClass();
            interfaceClass.Id = 13;

            String query = builder.Select<MyInterfaceClass>().Where<MyInterfaceClass>(p => p.Id == interfaceClass.Id && p.Id != interfaceClass.Id).ToString();

            Assert.AreEqual("SELECT t0.* FROM MyInterfaceClass AS t0 WHERE t0.Id = 13 AND t0.Id != 13", query);
        }

        [TestMethod]
        public void WhereMultipleIntegerParametersComingFromBaseClass()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            SmallTableInherited inheritedClass = new SmallTableInherited();
            inheritedClass.Id2 = 13;

            String query = builder.Select<SmallTableInherited>().Where<SmallTableInherited>(p => p.Id2 == inheritedClass.Id2).ToString();

            Assert.AreEqual("SELECT t0.* FROM SmallTableInherited AS t0 WHERE t0.Id2 = 13", query);
        }

        [TestMethod]
        public void WhereMultipleIntegerParametersComingFromBaseClassWith2Parameters()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            SmallTableInherited inheritedClass = new SmallTableInherited();
            inheritedClass.Id2 = 13;
            inheritedClass.Id42 = 42;

            String query = builder.Select<SmallTableInherited>().Where<SmallTableInherited>(p => p.Id2 == inheritedClass.Id2 && p.Id42 == inheritedClass.Id42).ToString();

            Assert.AreEqual("SELECT t0.* FROM SmallTableInherited AS t0 WHERE t0.Id2 = 13 AND t0.Id42 = 42", query);
        }

        //[TestMethod]
        //[ExpectedException(typeof(QueryBuilderException))]
        //public void WhereMultipleTimes()
        //{
        //    var builder = new QBuilder(new TSqlQueryGenerator());

        //    String query = builder
        //        .Select<Person>()
        //        .Select<Address>()
        //        .Where<Person, Address>((a, b) => a.Id == b.Id)
        //        .Where<Person, Address>((a, b) => a.Height == b.Id2).ToString();

        //}

        [TestMethod]
        public void ChangeTableNameWithUsingWhere()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .ChangeTableName<Person>("Person2")
                .Where<Person>(p => p.Id == 1).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person2 AS t0 WHERE t0.Id = 1", query);

        }

        [TestMethod]
        public void WhereStringAndEnum()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Name == "12345678901" && p.Fear == Fears.Cat).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Name = '12345678901' AND t0.Fear = 2", query);

        }

        [TestMethod]
        public void WhereEnumEqualsValue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Fear == Fears.Cat).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Fear = 2", query);

        }

        [TestMethod]
        public void WhereEnumNotEqualsValue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Fear != Fears.Cat).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Fear != 2", query);

        }

        [TestMethod]
        public void WhereEnumNotEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Fear2 != null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Fear2 IS NOT NULL", query);

        }

        [TestMethod]
        public void WhereEnumEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Fear2 == null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Fear2 IS NULL", query);

        }

        [TestMethod]
        public void WhereIntEquals1()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Id == 1).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id = 1", query);

        }

        [TestMethod]
        public void WhereIntNotEquals1()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Id != 1).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id != 1", query);

        }

        [TestMethod]
        public void WhereIntEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Id2 == null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NULL", query);

        }

        [TestMethod]
        public void WhereIntNotEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Id2 != null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NOT NULL", query);
        }

        [TestMethod]
        public void WhereDatetimeEqualsDate()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            DateTime birth = new DateTime(1981, 4, 3);

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Birth == birth).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Birth = '1981-04-03 00:00:00'", query);
        }

        [TestMethod]
        public void WhereDatetimeNotEqualsDate()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            DateTime birth = new DateTime(1981, 4, 3);

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Birth != birth).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Birth != '1981-04-03 00:00:00'", query);
        }

        [TestMethod]
        public void WhereDatetimeNotEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Birth2 != null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Birth2 IS NOT NULL", query);
        }

        [TestMethod]
        public void WhereDatetimeEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Birth2 == null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Birth2 IS NULL", query);
        }

        [TestMethod]
        public void WhereDecimalEquals1Point1()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Salary == 1.1M).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Salary = '1.1'", query);
        }

        [TestMethod]
        public void WhereDecimalNotEquals1Point1()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Salary != 1.1M).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Salary != '1.1'", query);
        }

        [TestMethod]
        public void WhereDecimalNotEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Salary2 != null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Salary2 IS NOT NULL", query);
        }

        [TestMethod]
        public void WhereDecimalEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.Salary2 == null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Salary2 IS NULL", query);
        }

        [TestMethod]
        public void WhereBooleanEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly2 == null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly2 IS NULL", query);
        }

        [TestMethod]
        public void WhereBooleanNotEqualsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly2 == null).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly2 IS NULL", query);
        }

        [TestMethod]
        public void WhereBooleanEqualsTrueItself()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly = 1", query);
        }

        [TestMethod]
        public void WhereBooleanEqualsFalseItself()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => !p.IsFriendly).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly != 1", query);
        }

        [TestMethod]
        public void WhereBooleanEqualsTrue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly == true).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly = 1", query);
        }

        [TestMethod]
        public void WhereBooleanEqualsFalse()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly == false).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly = 0", query);
        }

        [TestMethod]
        public void WhereBooleanNotEqualsFalse()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly != false).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly != 0", query);
        }

        [TestMethod]
        public void WhereBooleanNotEqualsTrue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());

            String query = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly != true).ToString();

            Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly != 1", query);
        }


    }
}
