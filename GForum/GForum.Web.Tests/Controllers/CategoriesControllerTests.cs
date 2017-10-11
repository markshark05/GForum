using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using GForum.Common.Enums;
using GForum.Data.Models;
using GForum.Services.Contracts;
using GForum.Web.Contracts.Identity;
using GForum.Web.Controllers;
using GForum.Web.Models.Forum;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Controllers
{
    [TestFixture]
    public class CategoriesControllerTests
    {
        [Test]
        public void Index_ShouldReturnAllAsignAllCaetgoriesReturnedFromService()
        {
            // Arrange
            var categories = new Category[]
            {
                new Category { Id = Guid.NewGuid() },
                new Category { Id = Guid.NewGuid() }
            };

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock
                .Setup(x => x.GetAll())
                .Returns(categories.AsQueryable());

            var voteServiceMock = new Mock<IVoteService>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<CategoryViewModel>(It.IsAny<Category>()))
                .Returns<Category>(x => new CategoryViewModel { Id = x.Id });

            var controller = new CategoriesController(
                categoryServiceMock.Object,
                voteServiceMock.Object,
                userManagerMock.Object,
                mapperMock.Object);

            // Act
            var result = ((controller.Index() as ViewResult).Model as IEnumerable<CategoryViewModel>);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(result.First().Id, categories.First().Id);
        }

        [Test]
        public void Category_ShouldReturnNotFoundIfCategoryDoesNotExist()
        {
            // Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var voteServiceMock = new Mock<IVoteService>();
            var userManagerMock = new Mock<IApplicationUserManager>();
            var mapperMock = new Mock<IMapper>();

            var controller = new CategoriesController(
                categoryServiceMock.Object,
                voteServiceMock.Object,
                userManagerMock.Object,
                mapperMock.Object);

            // Act
            var result = controller.Category(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Category_ShouldCallGetUserVoteTypeForPostForEachPostInCategory()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid() };
            var categories = new Category[]
            {
                new Category { Posts = new Post[] { post, post, post } }
            };

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(categories.AsQueryable());

            var voteServiceMock = new Mock<IVoteService>();
            voteServiceMock
                .Setup(x => x.GetUserVoteTypeForPost(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(VoteType.None);

            var userManagerMock = new Mock<IApplicationUserManager>();

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<CategoryWithPostsViewModel>(It.IsAny<Category>()))
                .Returns<Category>(x =>
                    new CategoryWithPostsViewModel
                    {
                        Posts = x.Posts.Select(y => new PostViewModel { Id = y.Id })
                    });

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.IsAuthenticated).Returns(true);

            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.Identity.Name).Returns(string.Empty);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.User).Returns(userMock.Object);

            var controller = new CategoriesController(
                categoryServiceMock.Object,
                voteServiceMock.Object,
                userManagerMock.Object,
                mapperMock.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            // Act
            var result = controller.Category(post.Id);

            // Assert
            voteServiceMock.Verify(x => x.GetUserVoteTypeForPost(It.Is<Guid>(i => i == post.Id), It.IsAny<string>()), Times.Exactly(3));
        }
    }
}
