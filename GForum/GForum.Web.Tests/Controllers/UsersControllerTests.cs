using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GForum.Data.Models;
using GForum.Web.Contracts.Identity;
using GForum.Web.Controllers;
using GForum.Web.Models.Users;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        [Test]
        public void UserProfile_ShouldSetUsername_WhenUserExists()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "admin" };

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock
                .Setup(x => x.Users)
                .Returns(new ApplicationUser[] { user }.AsQueryable());

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<UserViewModel>(It.IsAny<ApplicationUser>()))
                .Returns<ApplicationUser>(u => new UserViewModel { UserName = u.UserName });

            var controller = new UsersController(userManagerMock.Object, mapperMock.Object);

            // Act
            var result = controller.UserProfile("admin") as ViewResult;

            // Assert
            Assert.AreEqual((result.Model as UserViewModel).UserName, "admin");
        }

        [Test]
        public void UserProfile_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "admin" };

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock
                .Setup(x => x.Users)
                .Returns(new ApplicationUser[] { user }.AsQueryable());

            var mapperMock = new Mock<IMapper>();
            
            var controller = new UsersController(userManagerMock.Object, mapperMock.Object);

            // Act
            var result = controller.UserProfile("not_admin");

            // Assert
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }
    }
}
