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
        public void Create_ShouldCallRepositoryAddAndUnitOFWorkComplete()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Category>>();
            var postRepositoryMock = new Mock<IRepository<Post>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new CategoryService(unitOfWorkMock.Object, repositoryMock.Object, postRepositoryMock.Object);

            // Act
            var result = postService.Create("userid", "title");

            // Assert
            repositoryMock.Verify(x => x.Add(It.Is<Category>(c => 
                c.AuthorId == "userid" &&
                c.Title == "title" &&
                c.Id != default(Guid))), Times.Once);
        }

        [Test]
        public void Delete_ShouldCallRepositoryDeleteForAllPostsAndUnitOFWorkComplete()
        {
            // Arrange
            var posts = new[] { new Post(), new Post(), new Post() };
            var category = new Category { Id = Guid.NewGuid(), Posts = posts };
            var categories = new[] { category }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Category>>();
            repositoryMock.Setup(x => x.QueryAll).Returns(categories);

            var postRepositoryMock = new Mock<IRepository<Post>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new CategoryService(unitOfWorkMock.Object, repositoryMock.Object, postRepositoryMock.Object);

            // Act
            postService.Delete(category.Id);

            // Assert
            postRepositoryMock.Verify(x => x.Remove(It.IsAny<Post>()), Times.Exactly(posts.Length));
            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
        }

        [Test]
        public void Delete_ShouldNotComplete_IfNoCategoriesAreFound()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Category>>();
            var postRepositoryMock = new Mock<IRepository<Post>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new CategoryService(unitOfWorkMock.Object, repositoryMock.Object, postRepositoryMock.Object);

            // Act
            postService.Delete(Guid.Empty);

            // Assert
            unitOfWorkMock.Verify(x => x.Complete(), Times.Never);
        }
    }
}
