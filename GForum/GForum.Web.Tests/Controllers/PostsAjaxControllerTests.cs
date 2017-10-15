using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web.Mvc;
using GForum.Common.Enums;
using GForum.Data.Models;
using GForum.Services.Contracts;
using GForum.Web.Contracts.Identity;
using GForum.Web.Controllers;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Controllers
{
    [TestFixture]
    public class PostsAjaxControllerTests
    {
        [Test]
        public void Vote_ShouldCallPostServiceToggleVote()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid() };

            var categoryServiceMock = new Mock<ICategoryService>();
            var postServiceMock = new Mock<IPostService>();
            postServiceMock
                .Setup(x => x.GetById(It.Is<Guid>(g => g == post.Id), It.IsAny<bool>()))
                .Returns(new Post[] { post }.AsQueryable());

            var voteServiceMock = new Mock<IVoteService>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.SetupGet(x => x.Identity).Returns(identityMock.Object);
            var contextMock = new Mock<ControllerContext>();
            contextMock.SetupGet(p => p.HttpContext.User).Returns(principalMock.Object);

            var controller = new PostsAjaxController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                userManagerMock.Object)
            {
                ControllerContext = contextMock.Object
            };

            // Act
            controller.Vote(post.Id, VoteType.Upvote);

            // Assert
            voteServiceMock.Verify(x => x.ToggleVote(
                It.Is<Guid>(g => g == post.Id),
                It.IsAny<string>(),
                It.Is<VoteType>(v => v == VoteType.Upvote)));
        }

        [Test]
        public void Edit_ShouldCallPostServiceEdit()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid() };

            var categoryServiceMock = new Mock<ICategoryService>();
            var postServiceMock = new Mock<IPostService>();
            postServiceMock
                .Setup(x => x.GetById(It.Is<Guid>(g => g == post.Id), It.IsAny<bool>()))
                .Returns(new Post[] { post }.AsQueryable());

            var voteServiceMock = new Mock<IVoteService>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.SetupGet(x => x.Identity).Returns(identityMock.Object);
            var contextMock = new Mock<ControllerContext>();
            contextMock.SetupGet(p => p.HttpContext.User).Returns(principalMock.Object);

            var controller = new PostsAjaxController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                userManagerMock.Object)
            {
                ControllerContext = contextMock.Object
            };

            // Act
            controller.Edit(post.Id, "newcontent");

            // Assert
            postServiceMock.Verify(x => x.Edit(
                It.Is<Guid>(g => g == post.Id),
                It.IsAny<string>()));
        }

        [Test]
        public void Edit_ShouldReturnForbiddenIfUserIsNotTheAuthor()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid(), AuthorId = "userid" };

            var categoryServiceMock = new Mock<ICategoryService>();
            var postServiceMock = new Mock<IPostService>();
            postServiceMock
                .Setup(x => x.GetById(It.Is<Guid>(g => g == post.Id), It.IsAny<bool>()))
                .Returns(new Post[] { post }.AsQueryable());

            var voteServiceMock = new Mock<IVoteService>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.SetupGet(x => x.Identity).Returns(identityMock.Object);
            var contextMock = new Mock<ControllerContext>();
            contextMock.SetupGet(p => p.HttpContext.User).Returns(principalMock.Object);

            var controller = new PostsAjaxController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                userManagerMock.Object)
            {
                ControllerContext = contextMock.Object
            };

            // Act
            var result = controller.Edit(post.Id, "newcontent");

            // Assert
            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual((int)HttpStatusCode.Forbidden, (result as HttpStatusCodeResult).StatusCode);
        }

        [Test]
        public void Delete_ShouldCallPostServiceDelete()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid() };

            var categoryServiceMock = new Mock<ICategoryService>();
            var postServiceMock = new Mock<IPostService>();
            postServiceMock
                .Setup(x => x.GetById(It.Is<Guid>(g => g == post.Id), It.IsAny<bool>()))
                .Returns(new Post[] { post }.AsQueryable());

            var voteServiceMock = new Mock<IVoteService>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.SetupGet(x => x.Identity).Returns(identityMock.Object);
            var contextMock = new Mock<ControllerContext>();
            contextMock.SetupGet(p => p.HttpContext.User).Returns(principalMock.Object);

            var controller = new PostsAjaxController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                userManagerMock.Object)
            {
                ControllerContext = contextMock.Object
            };

            // Act
            controller.Delete(post.Id);

            // Assert
            postServiceMock.Verify(x => x.Delete(It.Is<Guid>(g => g == post.Id)));
        }

        [Test]
        public void Delete_ShouldReturnForbiddenIfUserIsNotTheAuthor()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid(), AuthorId = "userid" };

            var categoryServiceMock = new Mock<ICategoryService>();
            var postServiceMock = new Mock<IPostService>();
            postServiceMock
                .Setup(x => x.GetById(It.Is<Guid>(g => g == post.Id), It.IsAny<bool>()))
                .Returns(new Post[] { post }.AsQueryable());

            var voteServiceMock = new Mock<IVoteService>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.SetupGet(x => x.Identity).Returns(identityMock.Object);
            var contextMock = new Mock<ControllerContext>();
            contextMock.SetupGet(p => p.HttpContext.User).Returns(principalMock.Object);

            var controller = new PostsAjaxController(
                categoryServiceMock.Object,
                postServiceMock.Object,
                voteServiceMock.Object,
                userManagerMock.Object)
            {
                ControllerContext = contextMock.Object
            };

            // Act
            var result = controller.Delete(post.Id);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual((int)HttpStatusCode.Forbidden, (result as HttpStatusCodeResult).StatusCode);
        }
    }
}
