using NUnit.Framework;
using System;
using Microsoft.EntityFrameworkCore;
using Moq;

// ReSharper disable ObjectCreationAsStatement

namespace FamilyTreeProject.Data.EntityFramework.Tests
{
    [TestFixture]
    class EFUnitOfWorkTests
    {
        [Test]
        public void Constructor_Overload_Throws_On_Null_Database()
        {
            //Arrange
            FamilyTreeContext database = null;

            //Act

            //Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new EFUnitOfWork(database));
        }

        [Test]
        public void Constructor_Overload_Throws_On_Null_Options()
        {
            //Arrange
            DbContextOptions<FamilyTreeContext> options = null;

            //Act

            //Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new EFUnitOfWork(options));
        }

        [Test]
        public void Commit_Calls_SaveChanges()
        {
            //Arrange
            var mockContext = new Mock<FamilyTreeContext>();
            var unitOfWork = new EFUnitOfWork(mockContext.Object);

            //Act
            unitOfWork.Commit();

            //Assert
            mockContext.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Test]
        public void GetRepository_Returns_Repository()
        {
            //Arrange
            var mockContext = new Mock<FamilyTreeContext>();
            var unitOfWork = new EFUnitOfWork(mockContext.Object);

            //Act
            var rep = unitOfWork.GetRepository<Individual>();

            //Assert
            Assert.IsInstanceOf<IRepository<Individual>>(rep);
        }
    }
}
