using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ArchPM.Core.Extensions;

namespace ArchPM.Core.Tests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void FindPropertyGivenAllUpperWithUnderscoreStringWhenValidThenReturnsPropertyInfo()
        {
            var property = Utils.FindProperty<AllLowerWithUnderscoreClass>("HANEDE_CALISAN_KISI_SAYISI");

            Assert.AreEqual("hanede_calisan_kisi_sayisi", property.Name);
        }

        [TestMethod]
        public void FindProperty_FindProperty_FromAllLowerToAllUpper_WithoutSearchFormat()
        {
            var property = Utils.FindProperty<AllUpperWithUnderscoreClass>("hanede_calisan_kisi_sayisi");

            Assert.AreEqual("HANEDE_CALISAN_KISI_SAYISI", property.Name);
        }

        [TestMethod]
        public void FindProperty_FindProperty_FromPascalToAllUpper_WithoutSearchFormat()
        {
            var property = Utils.FindProperty<AllUpperWithUnderscoreClass>("HanedeCalisanKisiSayisi");

            Assert.AreEqual("HANEDE_CALISAN_KISI_SAYISI", property.Name);
        }

        [TestMethod]
        public void FindProperty_FindProperty_FromAllUpperToPascal_WithoutSearchFormat()
        {
            var property = Utils.FindProperty<PascalCaseClass>("HANEDE_CALISAN_KISI_SAYISI");

            Assert.AreEqual("HanedeCalisanKisiSayisi", property.Name);
        }

        [TestMethod]
        public void CopyTo_AllUpperToPascal()
        {
            AllUpperWithUnderscoreClass from = new AllUpperWithUnderscoreClass();
            from.HANEDE_CALISAN_KISI_SAYISI = 99;


            //PascalCaseClass to = ObjectCopy.CopyTo<AllUpperWithUnderscoreClass, PascalCaseClass>(from);

            //Assert.AreEqual(99, to.HanedeCalisanKisiSayisi);
        }

        [TestMethod]
        public void SplitByAllUpperWithUnderscoreTest()
        {
            var result = "HIZMET_ALIMI".SplitBy("_");

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("HIZMET"));
            Assert.IsTrue(result.Contains("ALIMI"));
        }

        [TestMethod]
        public void SplitByUppercaseCamelCaseTest()
        {
            var result = "hizmetAlimi".SplitByUppercase();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("hizmet"));
            Assert.IsTrue(result.Contains("Alimi"));
        }

        [TestMethod]
        public void SplitByUppercasePascalCaseTest()
        {
            var result = "HizmetAlimi".SplitByUppercase();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("Hizmet"));
            Assert.IsTrue(result.Contains("Alimi"));
        }



        //[TestMethod]
        //public void buildAllUpperWithUnderscorTestGivensplittedAllUpper()
        //{
        //    var result = ObjectCopy.buildWordsWithUnderscore(new List<string>() { "HIZMET", "ALIMI" });
        //    Assert.AreEqual("HIZMET_ALIMI", result);
        //}

        //[TestMethod]
        //public void buildAllUpperWithUnderscorTestGivensplittedAllLower()
        //{
        //    var result = ObjectCopy.buildWordsWithUnderscore(new List<string>() { "hizmet", "alimi" });
        //    Assert.AreEqual("hizmet_alimi", result);
        //}

        //[TestMethod]
        //public void buildCamelCaseTestGivenSplittedAllLower()
        //{
        //    var result = ObjectCopy.buildWordsCamelCase(new List<string>() { "hizmet", "alimi" });
        //    Assert.AreEqual("hizmetAlimi", result);
        //}

        //[TestMethod]
        //public void buildPascalCaseTestSplittedAllUpper()
        //{
        //    var result = ObjectCopy.buildWordsPascalCase(new List<string>() { "HIZMET", "ALIMI" });
        //    Assert.AreEqual("HizmetAlimi", result);
        //}

        //[TestMethod]
        //public void buildPascalCaseTestSplittedAllLower()
        //{
        //    var result = ObjectCopy.buildWordsPascalCase(new List<string>() { "hizmet", "alimi" });
        //    Assert.AreEqual("HizmetAlimi", result);
        //}


        //[TestMethod]
        //public void GetPossibleNamesAllUpperStartsWithUnderscore1Word()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("_SICIL");

        //    Assert.AreEqual(5, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesAllLowerStartsWithUnderscore1Word()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("_sicil");

        //    Assert.AreEqual(5, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesAllUpperWithUnderscore4Words()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("SICIL_ID_BASVURAN_TEST");

        //    Assert.AreEqual(648, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesAllLowerWithUnderscore4Words()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("sicil_id_basvuran_test");

        //    Assert.AreEqual(648, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesAllUpperWithUnderscore3Words()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("SICIL_ID_BASVURAN");

        //    Assert.AreEqual(108, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesAllLowerWithUnderscore3Words()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("sicil_id_basvuran");

        //    Assert.AreEqual(108, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesAllUpper()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("SICILIDBASVURAN");

        //    Assert.AreEqual(3, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesAllLower()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("sicilidbasvuran");

        //    Assert.AreEqual(3, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesCamelCase2Words()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("firstName");

        //    Assert.AreEqual(18, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesPascalCase2Words()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("FirstName");

        //    Assert.AreEqual(18, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesTests1WordAllLower()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("firstname");

        //    Assert.AreEqual(3, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesTests2WordsSecondStartsUpperWithUnderscore()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("first_name");

        //    Assert.AreEqual(18, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesTests2WordsFirstStartsUpperWithUnderscore()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("First_name");

        //    Assert.AreEqual(18, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesTests2WordsWithUnderscore()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("first_Name");

        //    Assert.AreEqual(18, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesPascalCase3words()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("FirstNameTest");

        //    Assert.AreEqual(108, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesCamelCase3words()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("firstNameTest");

        //    Assert.AreEqual(108, result.Count);
        //}

        //[TestMethod]
        //public void GetPossibleNamesTests4WordsAllUppercaseWithUnderscore()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("HANEDE_CALISAN_KISI_SAYISI");

        //    Assert.IsTrue(result.Contains("HanedeCalisanKisiSayisi"));
        //}

        //[TestMethod]
        //public void GetPossibleNamesTests4WordsCamelCase()
        //{
        //    List<String> result = ObjectCopy.splitUnkown("HanedeCalisanKisiSayisi");

        //    Assert.IsTrue(result.Contains("HANEDE_CALISAN_KISI_SAYISI"));
        //}

        //[TestMethod]
        //public void CopyToGivenNullableInt32ToNullableInt32()
        //{
        //    AllUpperWithUnderscoreClass c1 = new AllUpperWithUnderscoreClass();
        //    c1.HANEDE_CALISAN_KISI_SAYISI = 10;

        //    var c2 = c1.CopyTo<CamelCaseClass>();

        //    Assert.AreEqual(10, c2.hanedeCalisanKisiSayisi);
        //}

    }
}
