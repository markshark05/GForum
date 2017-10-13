using System.Web.Mvc;
using GForum.Data.Models;
using GForum.Web.Contracts.Identity;
using GForum.Web.Controllers;
using GForum.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void LoginGet_ShouldReturnView()
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
        public void LoginPost_ShouldCallSignInMethodWithCorectArgs()
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

            var model = new LoginViewModel
            {
                Username = "username",
                Password = "passowrd",
                RememberMe = true,
            };

            // Act
            var result = controller.Login(model, "").Result;

            // Assert
            signInManagerMock.Verify(x => x.PasswordSignInAsync(
                It.Is<string>(y => y == model.Username),
                It.Is<string>(y => y == model.Password),
                It.Is<bool>(y => y == model.RememberMe),
                It.IsAny<bool>()));
        }

        [Test]
        public void LoginPost_ShouldReDisplayFormIfThereAreModelErrors()
        {
            // Arrange
            var userManagerMock = new Mock<IApplicationUserManager>();
            var authManager = new Mock<IAuthenticationManager>();
            var signInManagerMock = new Mock<IApplicationSignInManager>();

            var controller = new AccountController(
                userManagerMock.Object,
                signInManagerMock.Object,
                authManager.Object)
            {
                Url = new Mock<UrlHelper>().Object
            };
            controller.ModelState.AddModelError("", "");

            var model = new LoginViewModel();

            // Act
            var result = controller.Login(model, "").Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void LoginPost_ShouldReDisplayFormIfThereIsASignInError()
        {
            // Arrange
            var userManagerMock = new Mock<IApplicationUserManager>();
            var authManager = new Mock<IAuthenticationManager>();

            var signInManagerMock = new Mock<IApplicationSignInManager>();
            signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInStatus.Failure);

            var controller = new AccountController(
                userManagerMock.Object,
                signInManagerMock.Object,
                authManager.Object)
            {
                Url = new Mock<UrlHelper>().Object
            };

            var model = new LoginViewModel();

            // Act
            var result = controller.Login(model, "").Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void RegisterGet_ShouldReturnView()
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
        public void RegisterPost_ShouldCallCreateAsyncWithCorectArgs()
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

            var model = new RegisterViewModel
            {
                Username = "username",
                Password = "passowrd",
                ConfirmPassword = "password",
            };

            // Act
            var result = controller.Register(model).Result;

            // Assert
            userManagerMock.Verify(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(),
                It.Is<string>(y => y == model.Password)));
        }

        [Test]
        public void RegisterPost_ShouldAddErrorsWhenNeeded()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var authManager = new Mock<IAuthenticationManager>();

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed("error"));

            var controller = new AccountController(
                userManagerMock.Object,
                signInManagerMock.Object,
                authManager.Object)
            {
                Url = new Mock<UrlHelper>().Object
            };

            var model = new RegisterViewModel();

            // Act
            var result = controller.Register(model).Result;

            // Assert
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void LogOff_ShouldCallCreateSignOutAndRedirect()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var authManagerMock = new Mock<IAuthenticationManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var controller = new AccountController(
                userManagerMock.Object,
                signInManagerMock.Object,
                authManagerMock.Object);

            // Act
            var result = controller.LogOff();

            // Assert
            authManagerMock.Verify(x => x.SignOut(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void GetEmail_ShouldCallGetEmailAsync()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var authManagerMock = new Mock<IAuthenticationManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var controller = new AccountController(
                userManagerMock.Object,
                signInManagerMock.Object,
                authManagerMock.Object);

            // Act
            var result = controller.GetEmail("id");

            // Assert
            userManagerMock.Verify(x => x.GetEmailAsync(It.Is<string>(s => s == "id")), Times.Once);
        }
    }
}
