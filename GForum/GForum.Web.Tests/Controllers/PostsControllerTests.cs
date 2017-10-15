using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using AutoMapper;
using GForum.Data.Models;
using GForum.Services.Contracts;
using GForum.Web.Contracts.Identity;
using GForum.Web.Controllers;
using GForum.Web.Models.Comments;
using GForum.Web.Models.Posts;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Controllers
{
    [TestFixture]
    public class PostsControllerTests
    {
        [Test]
        public void Post_ShouldReturnNotFound_WhenPostIsNull()
        {
            // Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var postServiceMock = new Mock<IPostService>();
            var voteServiceMock = new Mock<IVoteService>();
            var commentServiceMock = new Mock<ICommentService>();
            var userManagerMock = new Mock<IApplicationUserManager>();
            var mapperMock = new Mock<IMapper>();

            var controller = new PostsController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                commentServiceMock.Object,
                userManagerMock.Object,
                mapperMock.Object);

            // Act
            var result = controller.Post(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void SubmitGet_ShouldCallCategoryServiceForCategoryAndReturnView()
        {
            // Arrange
            var postServiceMock = new Mock<IPostService>();
            var voteServiceMock = new Mock<IVoteService>();
            var commentServiceMock = new Mock<ICommentService>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new[] { new Category() }.AsQueryable());

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<PostSubmitViewModel>(It.IsAny<object>()))
                .Returns(new PostSubmitViewModel());

            var controller = new PostsController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                commentServiceMock.Object,
                userManagerMock.Object,
                mapperMock.Object);

            // Act
            var result = controller.Submit(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            categoryServiceMock.Verify(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);
        }

        [Test]
        public void SubmitGet_ShouldReturnNotFound_WhenCategoryIsNull()
        {
            // Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var postServiceMock = new Mock<IPostService>();
            var voteServiceMock = new Mock<IVoteService>();
            var commentServiceMock = new Mock<ICommentService>();
            var userManagerMock = new Mock<IApplicationUserManager>();
            var mapperMock = new Mock<IMapper>();

            var controller = new PostsController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                commentServiceMock.Object,
                userManagerMock.Object,
                mapperMock.Object);

            // Act
            var result = controller.Submit(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void SubmitPost_CallPostServiceSubmit()
        {
            // Arrange
            var voteServiceMock = new Mock<IVoteService>();
            var commentServiceMock = new Mock<ICommentService>();
            var userManagerMock = new Mock<IApplicationUserManager>();
            var mapperMock = new Mock<IMapper>();

            var postServiceMock = new Mock<IPostService>();
            postServiceMock
                .Setup(x => x.Submit(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(new Post());

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(new[] { new Category() }.AsQueryable());

            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.SetupGet(x => x.Identity).Returns(identityMock.Object);
            var contextMock = new Mock<ControllerContext>();
            contextMock.SetupGet(p => p.HttpContext.User).Returns(principalMock.Object);

            var controller = new PostsController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                commentServiceMock.Object,
                userManagerMock.Object,
                mapperMock.Object)
            {
                ControllerContext = contextMock.Object
            };

            var model = new PostSubmitViewModel
            {
                CategoryId = Guid.NewGuid(),
                Title = "title",
                Content = "content"
            };

            // Act
            var result = controller.Submit(model);

            // Assert
            postServiceMock.Verify(x => x.Submit(
                    It.Is<Guid>(y => y == model.CategoryId),
                    It.IsAny<string>(),
                    It.Is<string>(y => y == model.Title),
                    It.Is<string>(y => y == model.Content)));
        }

        [Test]
        public void Comment_ShouldCallPostServiceGetById()
        {
            // Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var postServiceMock = new Mock<IPostService>();
            var voteServiceMock = new Mock<IVoteService>();
            var commentServiceMock = new Mock<ICommentService>();
            var userManagerMock = new Mock<IApplicationUserManager>();
            var mapperMock = new Mock<IMapper>();

            var controller = new PostsController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                commentServiceMock.Object,
                userManagerMock.Object,
                mapperMock.Object);
            var model = new CommentAddViewModel();

            // Act
            var result = controller.Comment(model);

            // Assert
            postServiceMock.Verify(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);
        }
    }
}
