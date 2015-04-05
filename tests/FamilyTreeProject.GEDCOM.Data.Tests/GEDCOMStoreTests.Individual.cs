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
using FamilyTreeProject.GEDCOM.Data.Tests.Common;
using FamilyTreeProject.GEDCOM.Tests.Common;
using FamilyTreeProject.TestUtilities;

using NUnit.Framework;

namespace FamilyTreeProject.GEDCOM.Data.Tests
{
    public partial class GEDCOMStoreTests
    {
        private readonly string FirstName = IndividualsResources.FirstName;
        private readonly Sex IndividualsSex = (IndividualsResources.IndividualsSex == "Male") ? Sex.Male : Sex.Female;
        private readonly string LastName = IndividualsResources.LastName;

        #region AddIndividual

        [Test]
        public void GEDCOMStore_AddIndividual_Should_Throw_On_Null_Individual()
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", "NoRecords"), testFile);

            //Assert
            Assert.Throws<ArgumentNullException>(() => db.AddIndividual(null));
        }

        [Test]
        [TestCase("NoRecords", 1)]
        [TestCase("OneIndividual", 2)]
        [TestCase("TwoIndividuals", 3)]
        public void GEDCOMStore_AddIndividual_Should_Insert_The_Individual_Into_The_DataContext(string fileName, int recordCount)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);
            Individual newIndividual = CreateTestIndividual();

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChangesAsync();

            //Assert
            Assert.AreEqual(recordCount, db.Individuals.Count());
        }

        [Test]
        [TestCase("NoRecords", 1)]
        [TestCase("OneIndividual", 2)]
        [TestCase("TwoIndividuals", 3)]
        public void GEDCOMStore_AddIndividual_Should_Insert_The_Individual_Into_The_Document(string fileName, int recordCount)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);
            Individual newIndividual = CreateTestIndividual();

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChangesAsync();

            //Assert
            Assert.AreEqual(recordCount, GetIndividualCount(testFile));
        }

        [Test]
        [TestCase("NoRecords", 1)]
        [TestCase("OneIndividual", 2)]
        [TestCase("TwoIndividuals", 3)]
        public void GEDCOMStore_AddIndividual_Should_Return_The_Id_Of_The_Individual(string fileName, int recordId)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);
            Individual newIndividual = CreateTestIndividual();

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChangesAsync();

            //Assert
            Assert.AreEqual(recordId, newIndividual.Id);
        }

        [Test]
        [TestCase("ThreeIndividuals", 4, 1, 0, "FourIndividuals_AddIndividualAddFamilyAddHusband")]
        [TestCase("ThreeIndividuals", 4, 0, 2, "FourIndividuals_AddIndividualAddFamilyAddWife")]
        public void GEDCOMStore_AddIndividual_Should_Create_Family_If_Father_Or_Mother_And_Family_Does_Not_Exist(string fileName, int id, int fatherId, int motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);

            Individual newIndividual = CreateTestIndividual(id);
            newIndividual.FatherId = fatherId;
            newIndividual.MotherId = motherId;

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChangesAsync();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("ThreeIndividuals_OneFamily", 3, 1, 2, "ThreeIndividuals_OneFamily_AddIndividual")]
        [TestCase("ThreeIndividuals_OneFamilyHusband", 3, 1, 0, "ThreeIndividuals_OneFamilyHusband_AddIndividual")]
        [TestCase("ThreeIndividuals_OneFamilyWife", 3, 0, 2, "ThreeIndividuals_OneFamilyWife_AddIndividual")]
        public void GEDCOMStore_AddIndividual_Should_Update_Family_If_Father_Or_Mother_And_Family_Exists(string fileName, int idToUpdate, int fatherId, int motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "AddIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);

            Individual newIndividual = CreateTestIndividual();
            newIndividual.FatherId = fatherId;
            newIndividual.MotherId = motherId;

            //Act
            db.AddIndividual(newIndividual);
            db.SaveChangesAsync();

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
            var db = CreateStore(String.Format("{0}.ged", "NoRecords"), testFile);
            Individual individual = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => db.DeleteIndividual(individual));
        }

        [Test]
        [TestCase("OneIndividual", 1, 0)]
        [TestCase("TwoIndividuals", 1, 1)]
        [TestCase("TwoIndividuals", 2, 1)]
        public void GEDCOMStore_DeleteIndividual_Should_Remove_The_Individual_From_The_DataContext(string fileName, int idToDelete, int recordCount)
        {
            //Arrange
            const string testFile = "DeleteIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);
            Individual individual = db.Individuals.Where(ind => ind.Id == idToDelete).SingleOrDefault();

            //Act
            db.DeleteIndividual(individual);
            db.SaveChangesAsync();

            //Assert
            Assert.AreEqual(recordCount, db.Individuals.Count());
        }

        [Test]
        [TestCase("OneIndividual", 1, 0)]
        [TestCase("TwoIndividuals", 1, 1)]
        [TestCase("TwoIndividuals", 2, 1)]
        public void GEDCOMStore_DeleteIndividual_Should_Remove_The_Individual_From_The_Document(string fileName, int idToDelete, int recordCount)
        {
            //Arrange
            const string testFile = "DeleteIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);
            Individual individual = CreateTestIndividual(idToDelete);

            //Act
            db.DeleteIndividual(individual);
            db.SaveChangesAsync();

            //Assert
            Assert.AreEqual(recordCount, GetIndividualCount(testFile));
        }

        [Test]
        [TestCase("FiveIndividuals_ThreeFamilies", 4, "FiveIndividuals_ThreeFamilies_DeleteChild")]
        [TestCase("FiveIndividuals_ThreeFamilies", 2, "FiveIndividuals_ThreeFamilies_DeleteWife")]
        [TestCase("FiveIndividuals_ThreeFamilies", 1, "FiveIndividuals_ThreeFamilies_DeleteHusband")]
        public void GEDCOMStore_DeleteIndividual_Should_Remove_The_Individual_From_Any_Families_In_The_Document(string fileName, int idToDelete, string updatedFileName)
        {
            //Arrange
            const string testFile = "DeleteIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);
            Individual individual = CreateTestIndividual(idToDelete);
            if (individual.Id == 2)
            {
                individual.Sex = Sex.Female;
            }

            //Act
            db.DeleteIndividual(individual);
            db.SaveChangesAsync();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("OneIndividual", 2)]
        [TestCase("TwoIndividuals", 3)]
        public void GEDCOMStore_DeleteIndividual_Should_Throw_If_Individual_Not_In_Document(string fileName, int idToDelete)
        {
            //Arrange
            const string testFile = "DeleteIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);
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
            var db = CreateStore(String.Format("{0}.ged", "NoRecords"), testFile);
            Individual individual = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => db.UpdateIndividual(individual));
        }

        [Test]
        [TestCase("OneIndividual", 1, "OneIndividual_UpdateIndividual")]
        [TestCase("TwoIndividuals", 2, "TwoIndividuals_UpdateIndividual")]
        public void GEDCOMStore_UpdateIndividual_Should_Update_Properties_Of_The_Individual(string fileName, int idToUpdate, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);

            Individual updateIndividual = db.Individuals.Where(ind => ind.Id == idToUpdate).Single();
            updateIndividual.FirstName = TestConstants.IND_UpdateFirstName;
            updateIndividual.LastName = TestConstants.IND_UpdateLastName;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChangesAsync();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("OneIndividual", 2, 1)]
        [TestCase("TwoIndividuals", 3, 2)]
        public void GEDCOMStore_UpdateIndividual_Should_Throw_If_Individual_Not_In_Document(string fileName, int idToUpdate, int recordCount)
        {
            //Arrange
            string testFile = "UpdateIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);
            Individual individual = CreateTestIndividual(idToUpdate);

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => db.UpdateIndividual(individual));
        }

        [Test]
        [TestCase("ThreeIndividuals", 3, 1, 0, "ThreeIndividuals_UpdateIndividualAddFamilyAddHusband")]
        [TestCase("ThreeIndividuals", 3, 0, 2, "ThreeIndividuals_UpdateIndividualAddFamilyAddWife")]
        public void GEDCOMStore_UpdateIndividual_Should_Create_Family_If_Father_Or_Mother_And_Family_Does_Not_Exist(string fileName, int idToUpdate, int fatherId, int motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);

            Individual updateIndividual = db.Individuals.Where(ind => ind.Id == idToUpdate).Single();
            updateIndividual.FatherId = fatherId;
            updateIndividual.MotherId = motherId;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChangesAsync();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("ThreeIndividuals_OneFamily", 3, 1, 2, "ThreeIndividuals_OneFamily_UpdateIndividual")]
        [TestCase("ThreeIndividuals_OneFamilyHusband", 3, 1, 0, "ThreeIndividuals_OneFamilyHusband_UpdateIndividual")]
        [TestCase("ThreeIndividuals_OneFamilyWife", 3, 0, 2, "ThreeIndividuals_OneFamilyWife_UpdateIndividual")]
        public void GEDCOMStore_UpdateIndividual_Should_Update_Family_If_Father_Or_Mother_And_Family_Exists(string fileName, int idToUpdate, int fatherId, int motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);

            Individual updateIndividual = db.Individuals.Where(ind => ind.Id == idToUpdate).Single();
            updateIndividual.FatherId = fatherId;
            updateIndividual.MotherId = motherId;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChangesAsync();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("FiveIndividuals_OneFamily", 4, 3, 2, "FiveIndividuals_UpdateFather_NewFamily")]
        [TestCase("FiveIndividuals_OneFamily", 4, 1, 5, "FiveIndividuals_UpdateMother_NewFamily")]
        public void GEDCOMStore_UpdateIndividual_Should_Add_Family_If_Father_Or_Mother_Is_Changed_And_New_Family_Does_Not_Exist(string fileName, int idToUpdate, int fatherId, int motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);

            Individual updateIndividual = db.Individuals.Where(ind => ind.Id == idToUpdate).Single();
            updateIndividual.FatherId = fatherId;
            updateIndividual.MotherId = motherId;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChangesAsync();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        [Test]
        [TestCase("FiveIndividuals_ThreeFamilies", 4, 3, 2, "FiveIndividuals_ThreeFamilies_UpdateFather")]
        [TestCase("FiveIndividuals_ThreeFamilies", 4, 1, 5, "FiveIndividuals_ThreeFamilies_UpdateMother")]
        public void GEDCOMStore_UpdateIndividual_Should_Update_Family_If_Father_Or_Mother_Is_Changed_And_New_Family_Exists(string fileName, int idToUpdate, int fatherId, int motherId, string updatedFileName)
        {
            //Arrange
            const string testFile = "UpdateIndividual.ged";
            var db = CreateStore(String.Format("{0}.ged", fileName), testFile);

            Individual updateIndividual = db.Individuals.Where(ind => ind.Id == idToUpdate).Single();
            updateIndividual.FatherId = fatherId;
            updateIndividual.MotherId = motherId;

            //Act
            db.UpdateIndividual(updateIndividual);
            db.SaveChangesAsync();

            //Assert
            GEDCOMAssert.IsValidOutput(GetEmbeddedFileString(updatedFileName), GetFileString(testFile));
        }

        #endregion

        #region Other Helpers

        private Individual CreateTestIndividual()
        {
            return CreateTestIndividual(-1);
        }

        private Individual CreateTestIndividual(int id)
        {
            // Create a test individual
            var newIndividual = new Individual
            {
                Id = id,
                FirstName = FirstName,
                LastName = LastName,
                Sex = IndividualsSex,
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