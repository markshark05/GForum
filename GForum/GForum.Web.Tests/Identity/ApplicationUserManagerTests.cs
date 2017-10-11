using GForum.Data.Models;
using GForum.Web.Identity;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Identity
{
    [TestFixture]
    public class ApplicationUserManagerTests
    {
        [Test]
        public void Constructor_Should_SetCorrectProperties()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            // Act
            var userManager = new ApplicationUserManager(userStoreMock.Object);

            // Assert 
            Assert.IsNotNull(userManager.UserValidator);
            Assert.IsNotNull(userManager.PasswordValidator);
        }
    }
}
