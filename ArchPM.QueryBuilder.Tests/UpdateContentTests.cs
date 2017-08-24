using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchPM.QueryBuilder.Tests.Model;

namespace ArchPM.QueryBuilder.Tests
{
    [TestClass]
    public class UpdateContentTests
    {
        [TestMethod]
        public void UpdateAllFieldsTestContentCount()
        {
            var builder = new QBuilder();
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

            var content = builder
                .Update<Person>(p)
                .UpdateContent;


            Assert.AreEqual(18, content.Fields.Count);



        }




        //[TestMethod]
        //public void UpdateSingleFieldAsInt32()
        //{
        //    SqlQueryBuilder builder = new SqlQueryBuilder();
        //    Person person = new Person();
        //    person.Id = 1;

        //    String query = builder.Update<Person>(person, p => p.Id, IncludeExclude.Include).Where<Person>(x => x.Id == 1).ToString();

        //    Assert.AreEqual("UPDATE Person SET [Id] = 1 WHERE Id = 1", query);
        //}

    }
}
