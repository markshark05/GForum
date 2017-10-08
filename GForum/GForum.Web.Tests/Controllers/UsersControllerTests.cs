using System.Linq;
using System.Web.Mvc;
using GForum.Data.Models;
using GForum.Web.Controllers;
using GForum.Web.IdentityConfig;
using GForum.Web.Models.Users;
using Microsoft.AspNet.Identity;
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
            AutoMapperConfig.Configure();
            var user = new ApplicationUser { UserName = "admin" };
            var users = new ApplicationUser[] { user }.AsQueryable();
            var storeMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<ApplicationUserManager>(storeMock.Object);
            userManagerMock.Setup(x => x.Users).Returns(users);

            var controller = new UsersController(userManagerMock.Object);

            // Act
            var result = controller.UserProfile("admin") as ViewResult;

            // Assert
            Assert.AreEqual((result.Model as UserViewModel).UserName, "admin");
        }

        [Test]
        public void UserProfile_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            AutoMapperConfig.Configure();
            var user = new ApplicationUser { UserName = "admin" };
            var users = new ApplicationUser[] { user }.AsQueryable();
            var storeMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<ApplicationUserManager>(storeMock.Object);
            userManagerMock.Setup(x => x.Users).Returns(users);

            var controller = new UsersController(userManagerMock.Object);

            // Act
            var result = controller.UserProfile("not_admin");

            // Assert
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }
    }
}
