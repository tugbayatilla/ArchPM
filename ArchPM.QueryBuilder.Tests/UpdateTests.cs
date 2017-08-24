using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchPM.QueryBuilder.Tests.Model;

namespace ArchPM.QueryBuilder.Tests
{
    [TestClass]
    public class UpdateTests
    {
        [TestMethod]
        public void UpdateAllFields()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person p = new Person();
            p.Birth = DateTime.Now;
            p.Birth2 = null;
            p.Fear = Fears.Alone;
            p.Fear2 = null;
            p.Gender = Genders.Male;
            p.Gender2 = Genders.Other;
            p.Height = 190;
            p.Height2 = 200;
            p.Id = 1;
            p.Id2 = null;
            p.IsFriendly = true;
            p.IsFriendly2 = false;
            p.Myclass = new MyClass();
            p.Myclass2 = null;
            p.MyInterface = new MyInterfaceClass();
            p.MyInterface2 = null;
            p.Name = "test name";
            p.Name2 = null;
            p.Salary = 13.3M;
            p.Salary2 = null;
            p.Weight = 30;
            p.Weight2 = null;

            String query = builder.Update<Person>(p).Where<Person>(x => x.Id == 1).ToString();

            // Assert.AreEqual("UPDATE Person SET [Id] = 1 ,[Id2] = NULL ,[Height] = 190 ,[Height2] = 200 ,[Weight] = 30 ,[Weight2] = NULL ,[Name] = 'test name' ,[Name2] = NULL ,[Salary] = 13.3 ,[Salary2] = NULL ,[IsFriendly] = 1 ,[IsFriendly2] = 0 ,[MyInterface] = '' ,[MyInterface2] = '' ,[Birth] = '2017-03-01 16:17:27' ,[Birth2] = NULL WHERE \tId = 1", query);
        }


        [TestMethod]
        public void UpdateSingleFieldAsInt32()
        {
            var builder = new QBuilder(new TSqlQueryGenerator());
            Person person = new Person();
            person.Id = 1;

            String query = builder.Update<Person>(person, p => p.Id, IncludeExclude.Include).Where<Person>(x => x.Id == 1).ToString();

            Assert.AreEqual("UPDATE t0 SET [Id] = 1 FROM Person AS t0 WHERE t0.Id = 1", query);
        }

    }
}
