using System.Web.Routing;
using NUnit.Framework;

namespace GForum.Web.Tests.App_Start
{
    [TestFixture]
    public class RouteConfigTests
    {
        [Test]
        public void RegisterRoutes_ShouldEnforceLowerCaseRoutes()
        {
            // Arrange
            var routes = new RouteCollection();

            // Act
            RouteConfig.RegisterRoutes(routes);

            // Assert
            Assert.IsTrue(routes.LowercaseUrls);
        }
    }
}
