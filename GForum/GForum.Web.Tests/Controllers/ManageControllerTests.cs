using System.Security.Principal;
using System.Web.Mvc;
using GForum.Web.Contracts.Identity;
using GForum.Web.Controllers;
using GForum.Web.Models.Manage;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using static GForum.Web.Controllers.ManageController;

namespace GForum.Web.Tests.Controllers
{
    [TestFixture]
    public class ManageControllerTests
    {
        [Test]
        public void Index_ShouldReturnViewResult()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var controller = new ManageController(userManagerMock.Object, signInManagerMock.Object);

            // Act
            var result = controller.Index(null);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        [TestCase(ManageMessageId.Error, "error")]
        [TestCase(ManageMessageId.ChnageEmailSuccess, "email")]
        [TestCase(ManageMessageId.ChangePasswordSuccess, "password")]
        [TestCase(null, "")]
        public void Index_ShouldDisplayMessage(ManageMessageId messageId, string expected)
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var controller = new ManageController(userManagerMock.Object, signInManagerMock.Object);

            // Act
            var result = controller.Index(messageId);

            // Assert
            StringAssert.Contains(expected, controller.ViewBag.StatusMessage);
        }

        [Test]
        public void ChangePassword_ShouldReturnViewResult()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var controller = new ManageController(userManagerMock.Object, signInManagerMock.Object);

            // Act
            var result = controller.ChangePassword();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ChangePasswordPost_ShouldCallChangePasswordAsync()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var model = new ChangePasswordViewModel
            {
                OldPassword = "oldpass",
                NewPassword = "pass",
                ConfirmPassword = "pass",
            };

            var controllerContextMock = new Mock<ControllerContext>();
            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(x => x.Identity).Returns(identityMock.Object);
            controllerContextMock.Setup(x => x.HttpContext.User).Returns(principalMock.Object);

            var controller = new ManageController(userManagerMock.Object, signInManagerMock.Object)
            {
                ControllerContext = controllerContextMock.Object
            };

            // Act
            var result = controller.ChangePassword(model);

            // Assert
            userManagerMock.Verify(x => x.ChangePasswordAsync(
                It.IsAny<string>(),
                It.Is<string>(y => y == model.OldPassword),
                It.Is<string>(y => y == model.NewPassword)), Times.Once);
        }

        [Test]
        public void ChangeEmail_ShouldReturnViewResult()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var controllerContextMock = new Mock<ControllerContext>();
            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(x => x.Identity).Returns(identityMock.Object);
            controllerContextMock.Setup(x => x.HttpContext.User).Returns(principalMock.Object);

            var controller = new ManageController(userManagerMock.Object, signInManagerMock.Object)
            {
                ControllerContext = controllerContextMock.Object
            };

            // Act
            var result = controller.ChangeEmail().Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ChangeEmailPost_ShouldCallSetEmailAsync()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var model = new ChangeEmailViewModel
            {
                CurrentEmail = "current@email.com",
                NewEmail = "new@email.com",
            };

            var controllerContextMock = new Mock<ControllerContext>();
            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(x => x.Identity).Returns(identityMock.Object);
            controllerContextMock.Setup(x => x.HttpContext.User).Returns(principalMock.Object);

            var controller = new ManageController(userManagerMock.Object, signInManagerMock.Object)
            {
                ControllerContext = controllerContextMock.Object
            };

            // Act
            var result = controller.ChangeEmail(model);

            // Assert
            userManagerMock.Verify(x => x.SetEmailAsync(
                It.IsAny<string>(),
                It.Is<string>(y => y == model.NewEmail)), Times.Once);
        }

        [Test]
        public void ChangeEmailPost_ShouldReturnView_IfModelIsInvalid()
        {
            // Arrange
            var signInManagerMock = new Mock<IApplicationSignInManager>();
            var userManagerMock = new Mock<IApplicationUserManager>();

            var model = new ChangeEmailViewModel
            {
                CurrentEmail = "current@email.com",
                NewEmail = "new@email.com",
            };

            var controller = new ManageController(userManagerMock.Object, signInManagerMock.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var result = controller.ChangeEmail(model).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
