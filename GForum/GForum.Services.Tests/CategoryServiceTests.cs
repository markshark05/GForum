using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models;
using Moq;
using NUnit.Framework;

namespace GForum.Services.Tests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        [Test]
        public void GetAll_ShouldReturnQueriableFromGivenRepository()
        {
            // Arrange
            var categories = new Category[] {
                new Category(),
                new Category()
            }.AsQueryable();
            var repositoryMock = new Mock<IRepository<Category>>();
            repositoryMock.Setup(x => x.Query).Returns(categories);
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var categoryService = new CategoryService(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            var result = categoryService.GetAll();

            // Assert
            CollectionAssert.AreEqual(result, categories);
        }

        [Test]
        public void GetByID_ShouldReturnQueriableContainingCurrectElement()
        {
            // Arrange
            var guidPattern = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            var guid1 = new Guid(guidPattern.Replace('x', '1'));
            var guid2 = new Guid(guidPattern.Replace('x', '2'));

            var expectedResult = new Category { Id = guid1 };

            var categories = new Category[] {
                new Category { Id = guid2},
                expectedResult,
            }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Category>>();
            repositoryMock.Setup(x => x.Query).Returns(categories);

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var categoryService = new CategoryService(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            var result = categoryService.GetById(guid1);

            // Assert
            Assert.Contains(expectedResult, result.ToList());
        }

        [Test]
        public void GetByID_ShouldReturnEmptyQueriable_WhenIdDoesNotExist()
        {
            // Arrange
            var guidPattern = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            var guid1 = new Guid(guidPattern.Replace('x', '1'));
            var guid2 = new Guid(guidPattern.Replace('x', '2'));

            var categories = new Category[] {
                new Category { Id = guid1},
                new Category { Id = guid2},
            }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Category>>();
            repositoryMock.Setup(x => x.Query).Returns(categories);

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var categoryService = new CategoryService(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            var result = categoryService.GetById(new Guid());

            // Assert
            Assert.IsEmpty(result);
        }
    }
}
