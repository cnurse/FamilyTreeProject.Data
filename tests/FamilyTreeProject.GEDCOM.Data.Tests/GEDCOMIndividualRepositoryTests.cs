//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using FamilyTreeProject.Data.GEDCOM;
using Moq;
using Naif.Core.Caching;
using NUnit.Framework;

namespace FamilyTreeProject.GEDCOM.Data.Tests
{
    [TestFixture]
    public class GEDCOMIndividualRepositoryTests
    {
        [Test]
        public void Constructor_Throws_On_Null_Database()
        {
            //Arrange
            IGEDCOMStore database = null;

            //Act

            //Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new GEDCOMIndividualRepository(database));
        }

        [Test]
        public void Add_Throws_On_Null_Individual()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var rep = new GEDCOMIndividualRepository(mockStore.Object);

            //Act, Assert
            Assert.Throws<ArgumentNullException>(() => rep.Add(null));
        }

        [Test]
        public void Add_Calls_Store_AddIndividual()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var rep = new GEDCOMIndividualRepository(mockStore.Object);
            var individual = new Individual();

            //Act
            rep.Add(individual);

            //Assert
            mockStore.Verify(s => s.AddIndividual(individual));
        }

        [Test]
        public void Delete_Throws_On_Null_Individual()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var rep = new GEDCOMIndividualRepository(mockStore.Object);

            //Act, Assert
            Assert.Throws<ArgumentNullException>(() => rep.Delete(null));
        }

        [Test]
        public void Delete_Calls_Store_DeleteIndividual()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var rep = new GEDCOMIndividualRepository(mockStore.Object);
            var individual = new Individual();

            //Act
            rep.Delete(individual);

            //Assert
            mockStore.Verify(s => s.DeleteIndividual(individual));
        }

        [Test]
        public void GetAll_Calls_Store_Individuals()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            mockStore.Setup(s => s.Individuals).Returns(() => new List<Individual>());
            var rep = new GEDCOMIndividualRepository(mockStore.Object);

            //Act
            var individuals = rep.GetAll();

            //Assert
            mockStore.Verify(s => s.Individuals);
        }

        [Test]
        public void Update_Throws_On_Null_Individual()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var rep = new GEDCOMIndividualRepository(mockStore.Object);

            //Act, Assert
            Assert.Throws<ArgumentNullException>(() => rep.Update(null));
        }

        [Test]
        public void Update_Calls_Store_UpdateIndividual()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var rep = new GEDCOMIndividualRepository(mockStore.Object);
            var individual = new Individual();

            //Act
            rep.Update(individual);

            //Assert
            mockStore.Verify(s => s.UpdateIndividual(individual));
        }
    }
}
