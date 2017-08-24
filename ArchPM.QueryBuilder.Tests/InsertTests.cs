using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


namespace ArchPM.QueryBuilder.Tests
{
    [TestClass]
    public class InsertTests
    {
        [TestMethod]
        public void InsertIntoStringValueAsString()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Name = "my name";

            String query = builder.Insert(p, x => x.Name).ToString();

            Assert.AreEqual("INSERT INTO Person (Name) VALUES ('my name');", query);
        }

        [TestMethod]
        public void InsertIntoStringValueAsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Name2 = null;

            String query = builder.Insert(p, x => x.Name2).ToString();

            Assert.AreEqual("INSERT INTO Person (Name2) VALUES (NULL);", query);
        }

        [TestMethod]
        public void InsertIntoBooleanValueAsTrue()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.IsFriendly = true;

            String query = builder.Insert(p, x => x.IsFriendly).ToString();

            Assert.AreEqual("INSERT INTO Person (IsFriendly) VALUES (1);", query);
        }

        [TestMethod]
        public void InsertIntoBooleanValueAsFalse()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.IsFriendly = false;

            String query = builder.Insert(p, x => x.IsFriendly).ToString();

            Assert.AreEqual("INSERT INTO Person (IsFriendly) VALUES (0);", query);
        }

        [TestMethod]
        public void InsertIntoNullableBooleanValueAsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.IsFriendly2 = null;

            String query = builder.Insert(p, x => x.IsFriendly2).ToString();

            Assert.AreEqual("INSERT INTO Person (IsFriendly2) VALUES (NULL);", query);
        }

        [TestMethod]
        public void InsertIntoInt32ValueAs99()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Id = 99;

            String query = builder.Insert(p, x => x.Id).ToString();

            Assert.AreEqual("INSERT INTO Person (Id) VALUES (99);", query);
        }

        [TestMethod]
        public void InsertIntoNullableInt32ValueAsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Id2 = null;

            String query = builder.Insert(p, x => x.Id2).ToString();

            Assert.AreEqual("INSERT INTO Person (Id2) VALUES (NULL);", query);
        }

        [TestMethod]
        public void InsertIntoInt16ValueAs1()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Weight = 1;

            String query = builder.Insert(p, x => x.Weight).ToString();

            Assert.AreEqual("INSERT INTO Person (Weight) VALUES (1);", query);
        }

        [TestMethod]
        public void InsertIntoNullableInt16ValueAsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Weight2 = null;

            String query = builder.Insert(p, x => x.Weight2).ToString();

            Assert.AreEqual("INSERT INTO Person (Weight2) VALUES (NULL);", query);
        }

        [TestMethod]
        public void InsertIntoInt64ValueAs1000000()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Height = 1000000;

            String query = builder.Insert(p, x => x.Height).ToString();

            Assert.AreEqual("INSERT INTO Person (Height) VALUES (1000000);", query);
        }

        [TestMethod]
        public void InsertIntoNullableInt64ValueAsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Height2 = null;

            String query = builder.Insert(p, x => x.Height2).ToString();

            Assert.AreEqual("INSERT INTO Person (Height2) VALUES (NULL);", query);
        }

        [TestMethod]
        public void InsertIntoNullableDecimalValueAsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Salary2 = null;

            String query = builder.Insert(p, x => x.Salary2).ToString();

            Assert.AreEqual("INSERT INTO Person (Salary2) VALUES (NULL);", query);
        }

        [TestMethod]
        public void InsertIntoDecimalValueAs123Point45()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());//todo:decimal seperator yazilmasi lazim. language ile degisiyor. 123,45 ya da 123.45 oluyor.
            //CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator
            //Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator
            /*
             NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            value.ToString(nfi);
             */


            Person p = new Person();
            p.Salary = 123.45M;

            String query = builder.Insert(p, x => x.Salary).ToString();

            Assert.AreEqual("INSERT INTO Person (Salary) VALUES ('123.45');", query);
        }


        [TestMethod]
        public void InsertIntoDatetimeValueAsDMYFormat03041981()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Birth = new DateTime(1981, 4, 3);

            String query = builder.Insert(p, x => x.Birth).ToString();

            Assert.AreEqual("INSERT INTO Person (Birth) VALUES ('1981-04-03 00:00:00');", query);
        }

        [TestMethod]
        public void InsertIntoNullableDatetimeValueAsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Birth2 = null;

            String query = builder.Insert(p, x => x.Birth2).ToString();

            Assert.AreEqual("INSERT INTO Person (Birth2) VALUES (NULL);", query);
        }

        [TestMethod]
        public void InsertIntoByteEnumValueAs1()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Fear = Fears.Alone; //1

            String query = builder.Insert(p, x => x.Fear).ToString();

            Assert.AreEqual("INSERT INTO Person (Fear) VALUES (1);", query);
        }

        [TestMethod]
        public void InsertIntoNullableByteEnumValueAsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Fear2 = null;

            String query = builder.Insert(p, x => x.Fear2).ToString();

            Assert.AreEqual("INSERT INTO Person (Fear2) VALUES (NULL);", query);
        }

        [TestMethod]
        public void InsertIntoEnumValueAs1()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Gender = Genders.Female; //1

            String query = builder.Insert(p, x => x.Gender).ToString();

            Assert.AreEqual("INSERT INTO Person (Gender) VALUES (1);", query);
        }

        [TestMethod]
        public void InsertIntoNullableEnumValueAsNull()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Gender2 = null;

            String query = builder.Insert(p, x => x.Gender2).ToString();

            Assert.AreEqual("INSERT INTO Person (Gender2) VALUES (NULL);", query);
        }


        [TestMethod]
        public void InsertIntoAllFields()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            SmallTable t = new SmallTable();
            t.Id = 1;
            t.Name = "name field";
            t.Salary = 123.45M;
            t.Id2 = 2;
            t.Id42 = 42;

            String query = builder.Insert(t).ToString();

            Assert.AreEqual("INSERT INTO SmallTable (Id42,Id2,Id,Name,Salary) VALUES (42,2,1,'name field','123.45');", query);
        }

        [TestMethod]
        public void InsertIntoAllFieldsWithNewTableName()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            SmallTable t = new SmallTable();
            t.Id = 1;
            t.Name = "name field";
            t.Salary = 123.45M;
            t.Id2 = 2;
            t.Id42 = 42;

            String query = builder.Insert<SmallTable>(t).ChangeTableName<SmallTable>("newTableName").ToString();

            Assert.AreEqual("INSERT INTO newTableName (Id42,Id2,Id,Name,Salary) VALUES (42,2,1,'name field','123.45');", query);
        }

        [TestMethod]
        public void InsertIntoAllFieldsExceptIdField()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            SmallTable t = new SmallTable();
            t.Id = 1;
            t.Name = "name field";
            t.Salary = 123.45M;
            t.Id2 = 2;
            t.Id42 = 42;

            String query = builder.Insert(t, p => p.Id, IncludeExclude.Exclude).ToString();

            Assert.AreEqual("INSERT INTO SmallTable (Id42,Id2,Name,Salary) VALUES (42,2,'name field','123.45');", query);
        }

        [TestMethod]
        public void InsertIntoAllFieldsReturningScopeIdentity()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            SmallTable t = new SmallTable();
            t.Id = 1;
            t.Name = "name field";
            t.Salary = 123.45M;
            t.Id2 = 2;
            t.Id42 = 42;

            String query = builder.Insert(t).ReturnScopeIdentity<SmallTable>(p => p.Id).ToString();

            Assert.AreEqual("INSERT INTO SmallTable (Id42,Id2,Name,Salary) VALUES (42,2,'name field','123.45'); SELECT SCOPE_IDENTITY();", query);
        }

        [TestMethod]
        public void InsertIntoAllFieldsExceptIdFieldReturningScopeIdentity()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            SmallTable t = new SmallTable();
            t.Id = 1;
            t.Name = "name field";
            t.Salary = 123.45M;
            t.Id2 = 2;
            t.Id42 = 42;

            String query = builder
                .Insert(t, p => p.Id, IncludeExclude.Exclude)
                .ReturnScopeIdentity<SmallTable>(p => p.Id).ToString();

            Assert.AreEqual("INSERT INTO SmallTable (Id42,Id2,Name,Salary) VALUES (42,2,'name field','123.45'); SELECT SCOPE_IDENTITY();", query);

        }


    }
}
