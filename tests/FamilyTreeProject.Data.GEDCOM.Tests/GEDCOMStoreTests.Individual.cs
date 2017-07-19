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
using FamilyTreeProject.Common;
using FamilyTreeProject.Core;
using FamilyTreeProject.GEDCOM;
using FamilyTreeProject.GEDCOM.Tests.Common;
using FamilyTreeProject.TestUtilities;
using NUnit.Framework;

namespace FamilyTreeProject.Data.GEDCOM.Tests
{
    public partial class GEDCOMStoreTests
    {
        private readonly string _firstName = IndividualsResources.FirstName;
        private readonly Sex _individualsSex = (IndividualsResources.IndividualsSex == "Male") ? Sex.Male : Sex.Female;
        private readonly string _lastName = IndividualsResources.LastName;

        #region AddIndividual

        [Test]
        public void GEDCOMStore_AddIndividual_Should_Throw_On_Null_Individual()
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore("NoRecords.ged", testFile);

            //Assert
            Assert.Throws<ArgumentNullException>(() => db.AddIndividual(null));
        }

        [Test]
        [TestCase("NoRecords", 1)]
        [TestCase("OneIndividual", 2)]
        [TestCase("TwoIndividuals", 3)]
        public void GEDCOMStore_AddIndividual_Should_Insert_The_Individual_Into_The_UnitOfWork(string fileName, int recordCount)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);
            Individual newIndividual = CreateTestIndividual();

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChanges();

            //Assert
            Assert.AreEqual(recordCount, db.Individuals.Count);
        }

        [Test]
        [TestCase("NoRecords", 1)]
        [TestCase("OneIndividual", 2)]
        [TestCase("TwoIndividuals", 3)]
        public void GEDCOMStore_AddIndividual_Should_Insert_The_Individual_Into_The_Document(string fileName, int recordCount)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);
            Individual newIndividual = CreateTestIndividual();

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChanges();

            //Assert
            Assert.AreEqual(recordCount, GetIndividualCount(testFile));
        }

        [Test]
        [TestCase("NoRecords", "1")]
        [TestCase("OneIndividual", "2")]
        [TestCase("TwoIndividuals", "3")]
        public void GEDCOMStore_AddIndividual_Should_Return_The_Id_Of_The_Individual(string fileName, string recordId)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);
            Individual newIndividual = CreateTestIndividual();

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChanges();

            //Assert
            Assert.AreEqual(recordId, newIndividual.Id);
        }

        [Test]
        [TestCase("ThreeIndividuals", "4", "1", "", "FourIndividuals_AddIndividualAddFamilyAddHusband")]
        [TestCase("ThreeIndividuals", "4", "", "2", "FourIndividuals_AddIndividualAddFamilyAddWife")]
        public void GEDCOMStore_AddIndividual_Should_Create_Family_If_Father_Or_Mother_And_Family_Does_Not_Exist(string fileName, string id, string fatherId, string motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);

            Individual newIndividual = CreateTestIndividual(id);
            newIndividual.FatherId = fatherId;
            newIndividual.MotherId = motherId;

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChanges();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("ThreeIndividuals_OneFamily","3", "1", "2", "ThreeIndividuals_OneFamily_AddIndividual")]
        [TestCase("ThreeIndividuals_OneFamilyHusband", "3", "1", "", "ThreeIndividuals_OneFamilyHusband_AddIndividual")]
        [TestCase("ThreeIndividuals_OneFamilyWife", "3", "", "2", "ThreeIndividuals_OneFamilyWife_AddIndividual")]
        public void GEDCOMStore_AddIndividual_Should_Update_Family_If_Father_Or_Mother_And_Family_Exists(string fileName, string idToUpdate, string fatherId, string motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);

            Individual newIndividual = CreateTestIndividual();
            newIndividual.FatherId = fatherId;
            newIndividual.MotherId = motherId;

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChanges();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        #endregion

        #region DeleteIndividual

        [Test]
        public void GEDCOMStore_DeleteIndividual_Should_Throw_On_Null_Individual()
        {
            //Arrange
            string testFile = "DeleteIndividual.ged";
            var db = CreateStore("NoRecords.ged", testFile);

            //Assert
            Assert.Throws<ArgumentNullException>(() => db.DeleteIndividual(null));
        }

        [Test]
        [TestCase("OneIndividual", "1", 0)]
        [TestCase("TwoIndividuals", "1", 1)]
        [TestCase("TwoIndividuals", "2", 1)]
        public void GEDCOMStore_DeleteIndividual_Should_Remove_The_Individual_From_The_UnitOfWork(string fileName, string idToDelete, int recordCount)
        {
            //Arrange
            const string testFile = "DeleteIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);
            Individual individual = db.Individuals.SingleOrDefault(ind => ind.Id == idToDelete);

            //Act
            db.DeleteIndividual(individual);
            db.SaveChanges();

            //Assert
            Assert.AreEqual(recordCount, db.Individuals.Count);
        }

        [Test]
        [TestCase("OneIndividual", "1", 0)]
        [TestCase("TwoIndividuals", "1", 1)]
        [TestCase("TwoIndividuals", "2", 1)]
        public void GEDCOMStore_DeleteIndividual_Should_Remove_The_Individual_From_The_Document(string fileName, string idToDelete, int recordCount)
        {
            //Arrange
            const string testFile = "DeleteIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);
            Individual individual = CreateTestIndividual(idToDelete);

            //Act
            db.DeleteIndividual(individual);
            db.SaveChanges();

            //Assert
            Assert.AreEqual(recordCount, GetIndividualCount(testFile));
        }

        [Test]
        [TestCase("FiveIndividuals_ThreeFamilies", "4", "FiveIndividuals_ThreeFamilies_DeleteChild")]
        [TestCase("FiveIndividuals_ThreeFamilies", "2", "FiveIndividuals_ThreeFamilies_DeleteWife")]
        [TestCase("FiveIndividuals_ThreeFamilies", "1", "FiveIndividuals_ThreeFamilies_DeleteHusband")]
        public void GEDCOMStore_DeleteIndividual_Should_Remove_The_Individual_From_Any_Families_In_The_Document(string fileName, string idToDelete, string updatedFileName)
        {
            //Arrange
            const string testFile = "DeleteIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);
            Individual individual = CreateTestIndividual(idToDelete);
            if (individual.Id == "2")
            {
                individual.Sex = Sex.Female;
            }

            //Act
            db.DeleteIndividual(individual);
            db.SaveChanges();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("OneIndividual", "2")]
        [TestCase("TwoIndividuals", "3")]
        public void GEDCOMStore_DeleteIndividual_Should_Throw_If_Individual_Not_In_Document(string fileName, string idToDelete)
        {
            //Arrange
            const string testFile = "DeleteIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);
            Individual individual = CreateTestIndividual(idToDelete);

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => db.DeleteIndividual(individual));
        }

        #endregion

        #region UpdateIndividual

        [Test]
        public void GEDCOMStore_UpdateIndividual_Should_Throw_On_Null_Individual()
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore("NoRecords.ged", testFile);

            //Assert
            Assert.Throws<ArgumentNullException>(() => db.UpdateIndividual(null));
        }

        [Test]
        [TestCase("OneIndividual", "1", "OneIndividual_UpdateIndividual")]
        [TestCase("TwoIndividuals", "1", "TwoIndividuals_UpdateIndividual")]
        public void GEDCOMStore_UpdateIndividual_Should_Update_Properties_Of_The_Individual(string fileName, string idToUpdate, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);

            Individual updateIndividual = db.Individuals.Single(ind => ind.Id == idToUpdate);
            updateIndividual.FirstName = TestConstants.IND_UpdateFirstName;
            updateIndividual.LastName = TestConstants.IND_UpdateLastName;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChanges();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("OneIndividual", "2", 1)]
        [TestCase("TwoIndividuals", "3", 2)]
        public void GEDCOMStore_UpdateIndividual_Should_Throw_If_Individual_Not_In_Document(string fileName, string idToUpdate, int recordCount)
        {
            //Arrange
            string testFile = "UpdateIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);
            Individual individual = CreateTestIndividual(idToUpdate);

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => db.UpdateIndividual(individual));
        }

        [Test]
        [TestCase("ThreeIndividuals", "3", "1","", "ThreeIndividuals_UpdateIndividualAddFamilyAddHusband")]
        [TestCase("ThreeIndividuals", "3", "", "2", "ThreeIndividuals_UpdateIndividualAddFamilyAddWife")]
        public void GEDCOMStore_UpdateIndividual_Should_Create_Family_If_Father_Or_Mother_And_Family_Does_Not_Exist(string fileName, string idToUpdate, string fatherId, string motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);

            Individual updateIndividual = db.Individuals.Single(ind => ind.Id == idToUpdate);
            updateIndividual.FatherId = fatherId;
            updateIndividual.MotherId = motherId;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChanges();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("ThreeIndividuals_OneFamily", "3", "1", "2", "ThreeIndividuals_OneFamily_UpdateIndividual")]
        [TestCase("ThreeIndividuals_OneFamilyHusband", "3", "1", "", "ThreeIndividuals_OneFamilyHusband_UpdateIndividual")]
        [TestCase("ThreeIndividuals_OneFamilyWife", "3", "", "2", "ThreeIndividuals_OneFamilyWife_UpdateIndividual")]
        public void GEDCOMStore_UpdateIndividual_Should_Update_Family_If_Father_Or_Mother_And_Family_Exists(string fileName, string idToUpdate, string fatherId, string motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);

            Individual updateIndividual = db.Individuals.Single(ind => ind.Id == idToUpdate);
            updateIndividual.FatherId = fatherId;
            updateIndividual.MotherId = motherId;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChanges();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("FiveIndividuals_OneFamily", "4", "3", "2", "FiveIndividuals_UpdateFather_NewFamily")]
        [TestCase("FiveIndividuals_OneFamily", "4", "1", "5", "FiveIndividuals_UpdateMother_NewFamily")]
        public void GEDCOMStore_UpdateIndividual_Should_Add_Family_If_Father_Or_Mother_Is_Changed_And_New_Family_Does_Not_Exist(string fileName, string idToUpdate, string fatherId, string motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);

            Individual updateIndividual = db.Individuals.Single(ind => ind.Id == idToUpdate);
            updateIndividual.FatherId = fatherId;
            updateIndividual.MotherId = motherId;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChanges();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("FiveIndividuals_ThreeFamilies", "4", "3", "2", "FiveIndividuals_ThreeFamilies_UpdateFather")]
        [TestCase("FiveIndividuals_ThreeFamilies", "4", "1", "5", "FiveIndividuals_ThreeFamilies_UpdateMother")]
        public void GEDCOMStore_UpdateIndividual_Should_Update_Family_If_Father_Or_Mother_Is_Changed_And_New_Family_Exists(string fileName, string idToUpdate, string fatherId, string motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore($"{fileName}.ged", testFile);

            Individual updateIndividual = db.Individuals.Single(ind => ind.Id == idToUpdate);
            updateIndividual.FatherId = fatherId;
            updateIndividual.MotherId = motherId;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChanges();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        #endregion

        #region Other Helpers

        private Individual CreateTestIndividual()
        {
            return CreateTestIndividual(String.Empty);
        }

        private Individual CreateTestIndividual(string id)
        {
            // Create a test individual
            var newIndividual = new Individual
            {
                Id = id,
                FirstName = _firstName,
                LastName = _lastName,
                Sex = _individualsSex,
                TreeId = _treeId
            };

            // Return the individual
            return newIndividual;
        }

        private int GetIndividualCount(string file)
        {
            string fileName = Path.Combine(FilePath, file);
            Stream testStream = new FileStream(fileName, FileMode.Open);
            var doc = new GEDCOMDocument();
            doc.Load(testStream);

            return doc.IndividualRecords.Count;
        }
        #endregion
    }
}