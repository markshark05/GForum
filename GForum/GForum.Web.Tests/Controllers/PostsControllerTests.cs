using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GForum.Data.Models;
using GForum.Services.Contracts;
using GForum.Web.Contracts.Identity;
using GForum.Web.Controllers;
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
    }
}
