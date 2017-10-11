using System.Web.Mvc;
using GForum.Web.Contracts.Identity;
using GForum.Web.Controllers;
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
    }
}
