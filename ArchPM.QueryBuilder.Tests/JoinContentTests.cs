using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ArchPM.QueryBuilder.Tests.Model;


namespace ArchPM.QueryBuilder.Tests
{
    [TestClass]
    public class JoinContentTests
    {
        [TestMethod]
        public void JoinContentsOneJoinContentExist()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .JoinContents;

            Assert.AreEqual(1, contents.Count, "Total count of join operator");
        }

        [TestMethod]
        public void JoinContentsTwoJoinContentCountReturnsTwo()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .Join<Person, SmallTable>((a, b) => a.Id2 == b.Id2)
                .JoinContents;

            Assert.AreEqual(2, contents.Count, "Total count of join operator");
        }

        [TestMethod]
        public void JoinContentsCheckTableInfos()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .JoinContents;

            var content = contents.First();

            Assert.IsTrue(content.LeftTableInfo.CheckTableInfo("Person", "t0", ""), "Left table info is wrong");
            Assert.IsTrue(content.RightTableInfo.CheckTableInfo("Address", "t1", ""), "Right table info is wrong");
        }

        [TestMethod]
        public void JoinContentsCheckPropertiesLikeLeftInnerJoin()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .JoinContents;

            var content = contents.First();
            Assert.AreEqual(JoinTypes.Inner, content.JoinType);
            Assert.AreEqual(JoinDirections.Left, content.JoinDirection);
        }

        [TestMethod]
        public void JoinContentsCheckPropertiesLikeRightOuterJoin()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id, JoinDirections.Right, JoinTypes.Outer)
                .JoinContents;

            var content = contents.First();
            Assert.AreEqual(JoinTypes.Outer, content.JoinType);
            Assert.AreEqual(JoinDirections.Right, content.JoinDirection);
        }

        [TestMethod]
        public void JoinContentsItemsCountEqualsToThree()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .JoinContents;

            var content = contents.First();
            Assert.AreEqual(3, content.Items.Count);

        }

        [TestMethod]
        public void JoinContentsCheckItemsContent()
        {
            var builder = new QBuilder();

            var contents = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id2)
                .JoinContents;

            var content = contents.First();
            var tuble = content.Items.Check("Id", "=", "Id2");
            Assert.AreEqual(tuble.Item3, content.Items.Count);
            Assert.IsTrue(tuble.Item1, String.Format("Value: {0}", tuble.Item2));

        }

        [TestMethod]
        public void JoinContentsTwoJoinContentExist()
        {
            var builder = new QBuilder();

            var contents = builder
                 .Select<Person>(p => p.Id)
                 .Join<Person, Address>((a, b) => a.Id == b.Id)
                 .Join<Person, Address>((a, b) => a.Id == b.Id)
                 .JoinContents;

            Assert.AreEqual(2, contents.Count, "Total count of join operator");
        }

    }
}
