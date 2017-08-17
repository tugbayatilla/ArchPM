using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchPM.QueryBuilder.Tests.Model;
using System.Linq;
using ArchPM.Core.Enums;
using ArchPM.Core.Exceptions;

namespace ArchPM.QueryBuilder.Tests
{
    [TestClass]
    public class WhereContentTests
    {
        [TestMethod]
        public void WhereContentStringContainsToUpperInvariant()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Name.Contains("a".ToUpperInvariant())).WhereContent;

            var tuble = content.Items.Check("Name", "LIKE", "%A%");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentStringContainsToLowerInvariant()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Name.Contains("A".ToLowerInvariant())).WhereContent;

            var tuble = content.Items.Check("Name", "LIKE", "%a%");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }


        [TestMethod]
        public void WhereContentStringEndsWith()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Name.EndsWith("A")).WhereContent;

            var tuble = content.Items.Check("Name", "LIKE", "%A");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }


        [TestMethod]
        public void WhereContentStringStartsWith()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Name.StartsWith("A")).WhereContent;

            var tuble = content.Items.Check("Name", "LIKE", "A%");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }


        //[TestMethod]
        //[ExpectedException(typeof(QueryBuilderException))]
        //public void WhereContentWhioutSelectThrowsQueryBuilderException()
        //{
        //    var builder = new ArchPmQueryBuilder();

        //    var content = builder.Where<Person>(p => p.Id == 1).WhereContent;
        //}

        [TestMethod]
        public void WhereContentSingleWhereTableInfo()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Id == 1).WhereContent;

            Assert.IsTrue(content.Items.CheckTableInfo("Person", "t0", ""));
        }

        [TestMethod]
        public void WhereContentMultipleWhereTableInfo()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>()
                .Where<SmallTable>(p => p.Id == 1)
                .Where<Person>(p => p.Id == 1)
                .Where<Address>(p => p.Id == 1).WhereContent;

            Assert.IsTrue(content.Items.CheckTableInfo("Person", "t0", ""));
            Assert.IsTrue(content.Items.CheckTableInfo("SmallTable", "t1", ""));
            Assert.IsTrue(content.Items.CheckTableInfo("Address", "t2", ""));
        }

        [TestMethod]
        public void WhereContentNotEqualTo()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Id != 1).WhereContent;

            var tuble = content.Items.Check("Id", "!=", 1);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentOneInt32FieldEqualsTo1()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Id == 1).WhereContent;

            var tuble = content.Items.Check("Id", "=", 1);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentOneNullableInt32FieldEqualsToNull()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Id2 == null).WhereContent;

            var tuble = content.Items.Check("Id2", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentTwoInt32FieldWithAndOperator()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Id == 1 && p.Id2 == 2).WhereContent;

            var tuble = content.Items.Check("Id", "=", 1, "AND", "Id2", "=", 2);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentSingleNullableIntHasValue()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Id2.HasValue).WhereContent;

            var tuble = content.Items.Check("Id2", "IS NOT", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentSingleNullableIntHasNotValue()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => !p.Id2.HasValue).WhereContent;

            var tuble = content.Items.Check("Id2", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentSingleStringValue()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Name == "my name is").WhereContent;

            var tuble = content.Items.Check("Name", "=", "my name is");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentListContainsInt32()
        {
            var builder = new QBuilder();

            var list = new List<Int32>() { 1, 2, 3 };

            var content = builder.Select<Person>().Where<Person>(p => list.Contains(p.Id)).WhereContent;

            var tuble = content.Items.Check("Id", "IN", "(", 1, 2, 3, ")");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereContentListNotContainsInt32()
        {
            var builder = new QBuilder();

            var list = new List<Int32>() { 1, 2, 3 };

            var content = builder.Select<Person>().Where<Person>(p => !list.Contains(p.Id)).WhereContent;

            var tuble = content.Items.Check("Id", "NOT IN", "(", 1, 2, 3, ")");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentListContainsGenderEnums()
        {
            var builder = new QBuilder();

            var list = EnumManager<Genders>.GetList().Select(p => p.Value);

            var content = builder.Select<Person>().Where<Person>(p => list.Contains((Int32)p.Gender)).WhereContent;

            var tuble = content.Items.Check("Gender", "IN", "(", 0, 1, 2, ")");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentBooleanWithoutEquality()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.IsFriendly).WhereContent;

            var tuble = content.Items.Check("IsFriendly", "=", true);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentBooleanNOTWithoutEquality()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => !p.IsFriendly).WhereContent;

            var tuble = content.Items.Check("IsFriendly", "!=", true);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereContentBooleanWithEqualFalse()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.IsFriendly == false).WhereContent;

            var tuble = content.Items.Check("IsFriendly", "=", false);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereContentAndAndOr()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.IsFriendly2.HasValue && (p.Id == 1 || p.Id2 == 2)).WhereContent;

            var tuble = content.Items.Check("IsFriendly2", "IS NOT", null, "AND", "(", "Id", "=", 1, "OR", "Id2", "=", 2, ")");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereOrElse()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.IsFriendly || p.IsFriendly2.HasValue).WhereContent;

            var tuble = content.Items.Check("IsFriendly", "=", true, "OR", "IsFriendly2", "IS NOT", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereNotBooleanWithoutEquals()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => !p.IsFriendly && !p.IsFriendly2.HasValue).WhereContent;

            var tuble = content.Items.Check("IsFriendly", "!=", true, "AND", "IsFriendly2", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }


        [TestMethod]
        public void WhereSingleStringIsNullValue()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Name == null).WhereContent;

            var tuble = content.Items.Check("Name", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereSingleStringIsStringIsNullOrEmpty()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => String.IsNullOrEmpty(p.Name)).WhereContent;

            var tuble = content.Items.Check("(", "Name", "IS", null, "OR", "Name", "=", "", ")");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereSingleIntegerWithParameter()
        {
            var builder = new QBuilder();

            Int32 id = 1;
            var content = builder.Select<Person>().Where<Person>(p => p.Id == id).WhereContent;

            var tuble = content.Items.Check("Id", "=", 1);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereSingleStringContainsInStringList()
        {
            var builder = new QBuilder();

            List<String> stringList = new List<string>();
            stringList.Add("123");
            stringList.Add("aBc");
            stringList.Add("Test123");
            var content = builder.Select<Person>().Where<Person>(p => stringList.Contains(p.Name)).WhereContent;

            var tuble = content.Items.Check("Name", "IN", "(", "123", "aBc", "Test123", ")");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereSingleStringContainsInInt32AsEnumerable()
        {
            var builder = new QBuilder();

            List<Int32> stringList = new List<Int32>();
            stringList.Add(1);
            stringList.Add(2);
            stringList.Add(3);
            var content = builder.Select<Person>().Where<Person>(p => stringList.AsEnumerable().Contains(p.Id)).WhereContent;

            var tuble = content.Items.Check("Id", "IN", "(", 1, 2, 3, ")");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereSingleStringContainsInInt32IEnumerable()
        {
            var builder = new QBuilder();

            List<Int32> list = new List<Int32>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            IEnumerable<Int32> interfaceList = list.AsEnumerable();
            var content = builder.Select<Person>().Where<Person>(p => interfaceList.Contains(p.Id)).WhereContent;


            var tuble = content.Items.Check("Id", "IN", "(", 1, 2, 3, ")");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereSingleStringNotContainsInInt32IEnumerable()
        {
            var builder = new QBuilder();

            List<Int32> stringList = new List<Int32>();
            stringList.Add(1);
            stringList.Add(2);
            stringList.Add(3);

            IEnumerable<Int32> xx = stringList.AsEnumerable();

            var content = builder.Select<Person>().Where<Person>(p => !xx.Contains(p.Id)).WhereContent;

            var tuble = content.Items.Check("Id", "NOT IN", "(", 1, 2, 3, ")");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereMultibleIntegerParameters()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Id == 1 && p.Id2 == 2).WhereContent;

            var tuble = content.Items.Check("Id", "=", 1, "AND", "Id2", "=", 2);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereMultibleIntegerParametersWithOneOfThemNull()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Id == 1 || p.Id2 == null).WhereContent;

            var tuble = content.Items.Check("Id", "=", 1, "OR", "Id2", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereMultibleIntegerParametersWithOneOfThemNotNull()
        {
            var builder = new QBuilder();

            var content = builder.Select<Person>().Where<Person>(p => p.Id == 1 && p.Id2 != null).WhereContent;

            var tuble = content.Items.Check("Id", "=", 1, "AND", "Id2", "IS NOT", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereSingleIntegerParameterComingFromInterface()
        {
            var builder = new QBuilder();

            IMyInterface interfaceClass = new MyInterfaceClass();
            interfaceClass.Id = 13;
            var content = builder.Select<MyInterfaceClass>().Where<MyInterfaceClass>(p => p.Id == interfaceClass.Id).WhereContent;

            var tuble = content.Items.Check("Id", "=", 13);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereMultipleIntegerParametersComingFromInterface()
        {
            var builder = new QBuilder();

            IMyInterface interfaceClass = new MyInterfaceClass();
            interfaceClass.Id = 13;
            var content = builder.Select<MyInterfaceClass>().Where<MyInterfaceClass>(p => p.Id == interfaceClass.Id && p.Id != interfaceClass.Id).WhereContent;

            var tuble = content.Items.Check("Id", "=", 13, "AND", "Id", "!=", 13);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereMultipleIntegerParametersComingFromBaseClass()
        {
            var builder = new QBuilder();

            SmallTableInherited inheritedClass = new SmallTableInherited();
            inheritedClass.Id2 = 13;
            var content = builder.Select<SmallTableInherited>().Where<SmallTableInherited>(p => p.Id2 == inheritedClass.Id2).WhereContent;

            var tuble = content.Items.Check("Id2", "=", 13);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereMultipleIntegerParametersComingFromBaseClassWith2Parameters()
        {
            var builder = new QBuilder();

            SmallTableInherited inheritedClass = new SmallTableInherited();
            inheritedClass.Id2 = 13;
            inheritedClass.Id42 = 42;
            var content = builder.Select<SmallTableInherited>()
                .Where<SmallTableInherited>(p => p.Id2 == inheritedClass.Id2 && p.Id42 == inheritedClass.Id42).WhereContent;

            var tuble = content.Items.Check("Id2", "=", 13, "AND", "Id42", "=", 42);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereMultipleTimes()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Select<Address>()
                .Where<Person, Address>((a, b) => a.Id == b.Id)
                .InsertOperatorBetweenWhereStatements(Operators.OR)
                .Where<Person, Address>((a, b) => a.Height == b.Id2).WhereContent;

            var tuble = content.Items.Check("Id", "=", "Id", "OR", "Height", "=", "Id2");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void ChangeTableNameWithUsingWhere()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .ChangeTableName<Person>("Person2")
                .Where<Person>(p => p.Id == 1)
                .WhereContent;

            Assert.IsTrue(content.Items.CheckTableInfo("Person2", "t0", ""), "Changing table name failed");

            var tuble = content.Items.Check("Id", "=", 1);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereStringAndEnum()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Name == "12345678901" && p.Fear == Fears.Cat)
                .WhereContent;

            var tuble = content.Items.Check("Name", "=", "12345678901", "AND", "Fear", "=", 2);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereEnumEqualsValue()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Fear == Fears.Cat)
                .WhereContent;

            var tuble = content.Items.Check("Fear", "=", 2);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereEnumNotEqualsValue()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Fear != Fears.Cat)
                .WhereContent;

            var tuble = content.Items.Check("Fear", "!=", 2);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereEnumNotEqualsNull()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Fear2 != null)
                .WhereContent;

            var tuble = content.Items.Check("Fear2", "IS NOT", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereEnumEqualsNull()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Fear2 == null)
                .WhereContent;

            var tuble = content.Items.Check("Fear2", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }



        [TestMethod]
        public void WhereIntNotEquals1()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Id != 1)
                .WhereContent;

            var tuble = content.Items.Check("Id", "!=", 1);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereIntEqualsNull()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Id2 == null)
                .WhereContent;

            var tuble = content.Items.Check("Id2", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereIntNotEqualsNull()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Id2 != null)
                .WhereContent;

            var tuble = content.Items.Check("Id2", "IS NOT", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereDatetimeEqualsDate()
        {
            var builder = new QBuilder();

            DateTime birth = new DateTime(1981, 4, 3);
            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Birth == birth)
                .WhereContent;

            var tuble = content.Items.Check("Birth", "=", birth);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereDatetimeNotEqualsDate()
        {
            var builder = new QBuilder();

            DateTime birth = new DateTime(1981, 4, 3);
            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Birth != birth)
                .WhereContent;

            var tuble = content.Items.Check("Birth", "!=", birth);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereDatetimeNotEqualsNull()
        {
            var builder = new QBuilder();

            DateTime? birth = null;
            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Birth2 != birth)
                .WhereContent;

            var tuble = content.Items.Check("Birth2", "IS NOT", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereDatetimeEqualsNull()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Birth2 == null)
                .WhereContent;

            var tuble = content.Items.Check("Birth2", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereDecimalEquals1Point1()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Salary == 1.1M)
                .WhereContent;

            var tuble = content.Items.Check("Salary", "=", 1.1M);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereDecimalNotEquals1Point1()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Salary != 1.1M)
                .WhereContent;

            var tuble = content.Items.Check("Salary", "!=", 1.1M);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereDecimalNotEqualsNull()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Salary2 != null)
                .WhereContent;

            var tuble = content.Items.Check("Salary2", "IS NOT", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereDecimalEqualsNull()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.Salary2 == null)
                .WhereContent;

            var tuble = content.Items.Check("Salary2", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereBooleanEqualsNull()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly2 == null)
                .WhereContent;

            var tuble = content.Items.Check("IsFriendly2", "IS", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereBooleanNotEqualsNull()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly2 != null)
                .WhereContent;

            var tuble = content.Items.Check("IsFriendly2", "IS NOT", null);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereBooleanEqualsTrueItself()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly)
                .WhereContent;

            var tuble = content.Items.Check("IsFriendly", "=", true);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereBooleanEqualsFalseItself()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => !p.IsFriendly)
                .WhereContent;

            var tuble = content.Items.Check("IsFriendly", "!=", true);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereBooleanEqualsTrue()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly == true)
                .WhereContent;

            var tuble = content.Items.Check("IsFriendly", "=", true);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereBooleanEqualsFalse()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly == false)
                .WhereContent;

            var tuble = content.Items.Check("IsFriendly", "=", false);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));
        }

        [TestMethod]
        public void WhereBooleanNotEqualsFalse()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly != false)
                .WhereContent;

            var tuble = content.Items.Check("IsFriendly", "!=", false);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void WhereBooleanNotEqualsTrue()
        {
            var builder = new QBuilder();

            var content = builder
                .Select<Person>()
                .Where<Person>(p => p.IsFriendly != true)
                .WhereContent;

            var tuble = content.Items.Check("IsFriendly", "!=", true);
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }


    }
}
