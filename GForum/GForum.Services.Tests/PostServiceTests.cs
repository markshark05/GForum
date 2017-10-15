using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models;
using Moq;
using NUnit.Framework;

namespace GForum.Services.Tests
{
    [TestFixture]
    public class PostServiceTests
    {
        [Test]
        public void Submit_ShouldCallRepositoryAddWithCorrectPostValuesAndUnitOfWorkComplete()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Post>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var postService = new PostService(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            var result = postService.Submit(new Guid(), "userId", "title", "content");

            // Assert
            repositoryMock.Verify(x => x.Add(It.Is<Post>(p =>
                p.Title == "title" &&
                p.Content == "content" &&
                p.AuthorId == "userId" &&
                p.VoteCount == 0 &&
                p.Id != default(Guid)
            )), Times.Once);
            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
        }

        [Test]
        public void Edit_ShouldChangePostValuesAndCallUnitOfWorkComplete()
        {
            // Arrange
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = "title",
                Content = "content"
            };
            var posts = new Post[] { post }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Post>>();
            repositoryMock.Setup(x => x.QueryAll).Returns(posts);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var postService = new PostService(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            postService.Edit(post.Id, "newcontent");

            // Assert
            Assert.AreEqual(post.Content, "newcontent");
            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
        }
    }
}
