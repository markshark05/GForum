using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using GForum.Data.Models;
using GForum.Services.Contracts;
using GForum.Web.Controllers;
using GForum.Web.Models.Posts;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void Index_ShouldCallGetRecentAndTopRated()
        {
            // Arrange
            var postServiceMock = new Mock<IPostService>();
            var voteServiceMock = new Mock<IVoteService>();
            var mapperMock = new Mock<IMapper>();

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.IsAuthenticated).Returns(false);

            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity.Name).Returns(string.Empty);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.User).Returns(userMock.Object);

            var controller = new HomeController(
                postServiceMock.Object,
                voteServiceMock.Object,
                mapperMock.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            postServiceMock.Verify(x => x.GetRecent(It.IsAny<int>()), Times.Once);
            postServiceMock.Verify(x => x.GetTopRated(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Index_ShouldCallVoteServiceWhenAuthenticated()
        {
            // Arrange
            var posts = new[] { new Post() };
            var postServiceMock = new Mock<IPostService>();
            postServiceMock
                .Setup(x => x.GetTopRated(It.IsAny<int>()))
                .Returns(posts.AsQueryable());
            postServiceMock
                .Setup(x => x.GetRecent(It.IsAny<int>()))
                .Returns(posts.AsQueryable());

            var voteServiceMock = new Mock<IVoteService>();
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<PostViewModel>(It.IsAny<object>()))
                .Returns(new PostViewModel());

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.IsAuthenticated).Returns(true);

            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity.Name).Returns(string.Empty);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.User).Returns(userMock.Object);

            var controller = new HomeController(
                postServiceMock.Object,
                voteServiceMock.Object,
                mapperMock.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            voteServiceMock.Verify(x => x.GetUserVoteTypeForPost(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void About_ShouldReturnViewResult()
        {
            // Arrange
            var postServiceMock = new Mock<IPostService>();
            var voteServiceMock = new Mock<IVoteService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new HomeController(
                postServiceMock.Object,
                voteServiceMock.Object,
                mapperMock.Object);

            // Act
            var result = controller.About();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
