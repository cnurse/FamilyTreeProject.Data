//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FamilyTreeProject.GEDCOM.Data.Tests.Common;
using NUnit.Framework;

// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable UseStringInterpolation

namespace FamilyTreeProject.Data.GEDCOM.Tests
{
    [TestFixture]
    public partial class GEDCOMStoreTests : GEDCOMTestBase
    {
        private readonly string _treeId = IndividualsResources.TreeId;

        #region Protected Properties

        protected override string EmbeddedFilePath => "FamilyTreeProject.Data.GEDCOM.Tests.TestFiles";

        protected override string FilePath
        {
            get
            {
                return SharedResources.GEDCOMTestFilePath;
            }
        }

        #endregion

        [Test]
        public void GEDCOMStore_Constructor_Throws_On_Empty_Path()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => { new GEDCOMStore(""); });
        }

        [Test]
        [TestCase("NoRecords", 0)]
        [TestCase("OneIndividual", 1)]
        [TestCase("TwoIndividuals", 2)]
        public void GEDCOMStore_Constructor_Loads_Individuals_Property(string fileName, int recordCount)
        {
            //Arrange
            const string testFile = "Constructor.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);

            var inds = db.Individuals;
            Assert.AreEqual(recordCount, inds.Count);
        }

        [Test]
        [TestCase("NoRecords", 0)]
        [TestCase("OneFamily", 1)]
        [TestCase("TwoFamilies", 2)]
        public void GEDCOMStore_Constructor_Loads_Families_Property(string fileName, int recordCount)
        {
            //Arrange
            const string testFile = "Constructor.ged";
            var db = CreateStore(string.Format("{0}.ged", fileName), testFile);

            var families = db.Families;
            Assert.AreEqual(recordCount, families.Count);
        }

        [Test]
        public void GEDCOMStore_Constructor_Creates_Family_Links()
        {
            //Arrange
            const string testFile = "Constructor.ged";
            const string fileName = "BindingTest";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);

            //Act
            var testIndividual = db.Individuals.SingleOrDefault(ind => ind.Id == "1");

            //Assert
            if (testIndividual != null)
            {
                Assert.AreEqual("John", testIndividual.FirstName);
                Assert.AreEqual("Smith", testIndividual.LastName);
                Assert.AreEqual("2", testIndividual.FatherId);
                Assert.AreEqual("3", testIndividual.MotherId);
            }
        }

        #region Other Helpers

        protected override Stream GetEmbeddedFileStream(string fileName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(GetEmbeddedFileName(fileName));
        }

        private GEDCOMStore CreateStore(string file, string test)
        {
            string fileName = Path.Combine(FilePath, file);
            string testFile = Path.Combine(FilePath, test);
            File.Copy(fileName, testFile, true);

            return new GEDCOMStore(testFile);
        }
        #endregion
    }
}