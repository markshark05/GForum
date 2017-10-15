using System;
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
        public void Create_ShouldCallRepositoryAddAndUnitOFWorkComplete()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Category>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new CategoryService(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            var result = postService.Create("userid", "title");

            // Assert
            repositoryMock.Verify(x => x.Add(It.Is<Category>(c => 
                c.AuthorId == "userid" &&
                c.Title == "title" &&
                c.Id != default(Guid))), Times.Once);
        }
    }
}
