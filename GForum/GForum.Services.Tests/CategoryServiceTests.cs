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

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IRepository<Category>>();
            repositoryMock.Setup(x => x.Query).Returns(categories);

            var categoryService = new CategoryService(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = categoryService.GetAll();

            // Assert
            CollectionAssert.AreEqual(result, categories);
        }

        [Test]
        public void GetByID_ShouldReturnQueriableContainingOnlyCurrectElement()
        {
            // Arrange
            var guidPattern = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            var guid1 = new Guid(guidPattern.Replace('x', '1'));
            var guid2 = new Guid(guidPattern.Replace('x', '2'));

            var expectedCategory = new Category { Id = guid1 };

            var categories = new Category[] {
                new Category { Id = guid2},
                expectedCategory,
            }.AsQueryable();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IRepository<Category>>();
            repositoryMock.Setup(x => x.Query).Returns(categories);

            var categoryService = new CategoryService(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = categoryService.GetById(guid1);

            // Assert
            Assert.Contains(expectedCategory, result.ToList());
            Assert.AreEqual(1, result.Count());
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

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IRepository<Category>>();
            repositoryMock.Setup(x => x.Query).Returns(categories);

            var categoryService = new CategoryService(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = categoryService.GetById(new Guid());

            // Assert
            Assert.IsEmpty(result);
        }
    }
}
