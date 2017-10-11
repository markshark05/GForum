using System.Web.Mvc;
using GForum.Data.Models;
using GForum.Web.Contracts.Identity;
using GForum.Web.Controllers;
using GForum.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void GetLogin_ShouldReturnView()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();
            var authManager = new Mock<IAuthenticationManager>();

            var controller = new AccountController(
                userManagerMock.Object,
                signInManagerMock.Object,
                authManager.Object);

            // Act
            var result = controller.Login(string.Empty);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void PostLogin_ShouldCallSignInMethodWithCorectArgs()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();
            var authManager = new Mock<IAuthenticationManager>();

            var controller = new AccountController(
                userManagerMock.Object,
                signInManagerMock.Object,
                authManager.Object)
            {
                Url = new Mock<UrlHelper>().Object
            };

            // Act
            var model = new LoginViewModel
            {
                Username = "username",
                Password = "passowrd",
                RememberMe = true,
            };

            var result = controller.Login(model, "").Result;

            // Assert
            signInManagerMock.Verify(x => x.PasswordSignInAsync(
                It.Is<string>(y => y == model.Username),
                It.Is<string>(y => y == model.Password),
                It.Is<bool>(y => y == model.RememberMe),
                It.IsAny<bool>()));
        }

        [Test]
        public void GetRegister_ShouldReturnView()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();
            var authManager = new Mock<IAuthenticationManager>();

            var controller = new AccountController(
                userManagerMock.Object,
                signInManagerMock.Object,
                authManager.Object);

            // Act
            var result = controller.Register();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void PostRegister_ShouldCallCreateAsyncWithCorectArgs()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var authManager = new Mock<IAuthenticationManager>();

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new AccountController(
                userManagerMock.Object,
                signInManagerMock.Object,
                authManager.Object)
            {
                Url = new Mock<UrlHelper>().Object
            };

            // Act
            var model = new RegisterViewModel
            {
                Username = "username",
                Password = "passowrd",
                ConfirmPassword = "password",
            };

            var result = controller.Register(model).Result;

            // Assert
            userManagerMock.Verify(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(),
                It.Is<string>(y => y == model.Password)));
        }
    }
}
