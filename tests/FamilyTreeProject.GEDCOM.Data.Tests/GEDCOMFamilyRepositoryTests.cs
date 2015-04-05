//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using FamilyTreeProject.Data.GEDCOM;
using Moq;
using Naif.Core.Caching;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement

namespace FamilyTreeProject.GEDCOM.Data.Tests
{
    [TestFixture]
    public class GEDCOMFamilyRepositoryTests
    {
        [Test]
        public void Constructor_Throws_On_Null_CacheProvider()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() => new GEDCOMFamilyRepository(mockStore.Object, null));
        }

        [Test]
        public void Constructor_Throws_On_Null_Database()
        {
            //Arrange
            var mockCache = new Mock<ICacheProvider>();
            IGEDCOMStore database = null;

            //Act

            //Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new GEDCOMFamilyRepository(database, mockCache.Object));
        }

        [Test]
        public void Add_Throws_On_Null_Family()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var mockCache = new Mock<ICacheProvider>();
            var rep = new GEDCOMFamilyRepository(mockStore.Object, mockCache.Object);

            //Act, Assert
            Assert.Throws<ArgumentNullException>(() => rep.Add(null));
        }

        [Test]
        public void Add_Calls_Store_AddFamily()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var mockCache = new Mock<ICacheProvider>();
            var rep = new GEDCOMFamilyRepository(mockStore.Object, mockCache.Object);
            var family = new Family();

            //Act
            rep.Add(family);

            //Assert
            mockStore.Verify(s => s.AddFamily(family));
        }

        [Test]
        public void Delete_Throws_On_Null_Family()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var mockCache = new Mock<ICacheProvider>();
            var rep = new GEDCOMFamilyRepository(mockStore.Object, mockCache.Object);

            //Act, Assert
            Assert.Throws<ArgumentNullException>(() => rep.Delete(null));
        }

        [Test]
        public void Delete_Calls_Store_DeleteFamily()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var mockCache = new Mock<ICacheProvider>();
            var rep = new GEDCOMFamilyRepository(mockStore.Object, mockCache.Object);
            var family = new Family();

            //Act
            rep.Delete(family);

            //Assert
            mockStore.Verify(s => s.DeleteFamily(family));
        }

        [Test]
        public void GetAll_Calls_Store_Families()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var mockCache = new Mock<ICacheProvider>();
            var rep = new GEDCOMFamilyRepository(mockStore.Object, mockCache.Object);

            //Act
            var individuals = rep.GetAll();

            //Assert
            mockStore.Verify(s => s.Families);
        }

        [Test]
        public void GetById_Throws()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var mockCache = new Mock<ICacheProvider>();
            var rep = new GEDCOMFamilyRepository(mockStore.Object, mockCache.Object);

            //Act, Assert
            Assert.Throws<NotImplementedException>(() => rep.GetById(-1));
        }

        [Test]
        public void GetByProperty_Throws()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var mockCache = new Mock<ICacheProvider>();
            var rep = new GEDCOMFamilyRepository(mockStore.Object, mockCache.Object);

            //Act, Assert
            Assert.Throws<NotImplementedException>(() => rep.GetByProperty("TreeId", -1));
        }

        [Test]
        public void Update_Throws_On_Null_Family()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var mockCache = new Mock<ICacheProvider>();
            var rep = new GEDCOMFamilyRepository(mockStore.Object, mockCache.Object);

            //Act, Assert
            Assert.Throws<ArgumentNullException>(() => rep.Update(null));
        }

        [Test]
        public void Update_Calls_Store_UpdateFamily()
        {
            //Arrange
            var mockStore = new Mock<IGEDCOMStore>();
            var mockCache = new Mock<ICacheProvider>();
            var rep = new GEDCOMFamilyRepository(mockStore.Object, mockCache.Object);
            var family = new Family();

            //Act
            rep.Update(family);

            //Assert
            mockStore.Verify(s => s.UpdateFamily(family));
        }
    }
}
