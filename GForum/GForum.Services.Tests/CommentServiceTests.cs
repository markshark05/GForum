using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models;
using Moq;
using NUnit.Framework;

namespace GForum.Services.Tests
{
    [TestFixture]
    class CommentServiceTests
    {
        [Test]
        public void AddCommentToPost_ShouldCallRepositoryAddAndUnitOFWorkComplete()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid() };
            var comment = new Comment
            {
                PostId = post.Id,
                AuthorId = "userid",
                Content = "content",
            };

            var repositoryMock = new Mock<IRepository<Comment>>();
            var postRepositoryMock = new Mock<IRepository<Post>>();
            postRepositoryMock.Setup(x => x.QueryAll).Returns(new[] { post }.AsQueryable());
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new CommentService(unitOfWorkMock.Object, repositoryMock.Object, postRepositoryMock.Object);
            
            // Act
            postService.AddCommentToPost((Guid)comment.PostId, comment.AuthorId, comment.Content);

            // Assert
            repositoryMock.Verify(x => x.Add(It.Is<Comment>(c =>
                c.PostId == comment.PostId &&
                c.AuthorId == comment.AuthorId &&
                c.Content == comment.Content)), Times.Once);
            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
            Assert.AreEqual(post.CommentCount, 1);
        }

        [Test]
        public void AddCommentToPost_ShouldDoNothingIfPostDoesntExist()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Comment>>();
            var postRepositoryMock = new Mock<IRepository<Post>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new CommentService(unitOfWorkMock.Object, repositoryMock.Object, postRepositoryMock.Object);

            // Act
            postService.AddCommentToPost(Guid.NewGuid(), "id", "content");

            // Assert
            unitOfWorkMock.Verify(x => x.Complete(), Times.Never);
        }
    }
}
