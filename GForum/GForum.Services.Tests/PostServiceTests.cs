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
        private string guidPattern = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

        [Test]
        public void GetAll_ShouldReturnQueriableFromGivenRepository()
        {
            // Arrange
            var posts = new Post[] {
                new Post(),
                new Post()
            }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Post>>();
            repositoryMock.Setup(x => x.Query).Returns(posts);

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new PostService(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = postService.GetAll();

            // Assert
            CollectionAssert.AreEqual(result, posts);
        }

        [Test]
        public void GetByID_ShouldReturnQueriableContainingCurrectElement()
        {
            // Arrange
            var guid1 = new Guid(this.guidPattern.Replace('x', '1'));
            var guid2 = new Guid(this.guidPattern.Replace('x', '2'));

            var expectedPost = new Post() { Id = guid1 };
            var posts = new Post[] {
                expectedPost,
                new Post() { Id = guid2 }
            }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Post>>();
            repositoryMock.Setup(x => x.Query).Returns(posts);

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new PostService(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = postService.GetById(guid1);

            // Assert
            Assert.Contains(expectedPost, result.ToList());
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetByID_ShouldReturnEmptyQueriable_WhenIdDoesNotExist()
        {
            // Arrange
            var guid1 = new Guid(this.guidPattern.Replace('x', '1'));
            var guid2 = new Guid(this.guidPattern.Replace('x', '2'));

            var expectedPost = new Post() { Id = guid1 };
            var posts = new Post[] {
                expectedPost,
                new Post() { Id = guid2 }
            }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Post>>();
            repositoryMock.Setup(x => x.Query).Returns(posts);

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new PostService(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = postService.GetById(new Guid());

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void Submit_ShouldCallRepositoryAddWithCorrectPostValuesAndUnitOfWorkComplete()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Post>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var postService = new PostService(repositoryMock.Object, unitOfWorkMock.Object);

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
            var guid1 = new Guid(this.guidPattern.Replace('x', '1'));

            var post = new Post
            {
                Id = guid1,
                Title = "title",
                Content = "content"
            };
            var posts = new Post[] { post }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Post>>();
            repositoryMock.Setup(x => x.Query).Returns(posts);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var postService = new PostService(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            postService.Edit(guid1, "newcontent");

            // Assert
            Assert.AreEqual(post.Content, "newcontent");
            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
        }

        [Test]
        public void Delete_ShouldCallRepositoryRemoveAndUnitOfWorkComplete()
        {
            // Arrange
            var guid1 = new Guid(this.guidPattern.Replace('x', '1'));

            var post = new Post { Id = guid1 };
            var posts = new Post[] { post }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Post>>();
            repositoryMock.Setup(x => x.Query).Returns(posts);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var postService = new PostService(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            postService.Delete(guid1);

            // Assert
            repositoryMock.Verify(x => x.Remove(It.Is<Post>(p => p.Id == guid1)));
            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
        }
    }
}
