//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using FamilyTreeProject.Core;
using FamilyTreeProject.TestUtilities.Models;
using Moq;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace FamilyTreeProject.Data.GEDCOM.Tests
{
    [TestFixture]
    public class GEDCOMUnitOfWorkTests
    {
        [Test]
        public void Constructor_Throws_On_Empty_Path()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => new GEDCOMUnitOfWork(String.Empty));
        }

        [Test]
        public void Constructor_Overload_Throws_On_Null_Database()
        {
            //Arrange
            IGEDCOMStore database = null;

            //Act

            //Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new GEDCOMUnitOfWork(database));
        }

        [Test]
        public void Commit_Calls_Store_SaveChanges()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var unitOfWork = new GEDCOMUnitOfWork(mockStore.Object);

            //Act
            unitOfWork.Commit();

            //Assert
            mockStore.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Test]
        public void GetRepository_Throws_If_T_Not_Recognised()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var unitOfWork = new GEDCOMUnitOfWork(mockStore.Object);

            //Act, Assert
            Assert.Throws<NotImplementedException>(() => unitOfWork.GetRepository<Dog>());
        }

        [Test]
        public void GetLinqRepository_Returns_IndividualRepository_If_T_Is_Individual()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var unitOfWork = new GEDCOMUnitOfWork(mockStore.Object);

            //Act
            var rep = unitOfWork.GetRepository<Individual>();

            //Assert
            Assert.IsInstanceOf<GEDCOMIndividualRepository>(rep);
        }

        [Test]
        public void GetLinqRepository_Returns_FamilyRepository_If_T_Is_Family()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var unitOfWork = new GEDCOMUnitOfWork(mockStore.Object);

            //Act
            var rep = unitOfWork.GetRepository<Family>();

            //Assert
            Assert.IsInstanceOf<GEDCOMFamilyRepository>(rep);
        }

        [Test]
        public void GetRepository_Throws_If_T_Is_Neither_Family_Individual()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var unitOfWork = new GEDCOMUnitOfWork(mockStore.Object);

            //Act, Assert
            Assert.Throws<NotImplementedException>(() => unitOfWork.GetRepository<Note>());
        }
    }
}
