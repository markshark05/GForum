using System.Net;
using System.Web;
using System.Web.Mvc;
using GForum.Web.Helpers;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Helpers
{
    [TestFixture]
    public class AjaxAuthorizeAttributeTests
    {
        private class AjaxAuthorizeAttribute_Fake : AjaxAuthorizeAttribute
        {
            public void HandleUnauthorizedRequest_Test(AuthorizationContext context)
            {
                base.HandleUnauthorizedRequest(context);
            }
        }

        [Test]
        public void HandleUnauthorizedRequest_ShouldReturnJsonReponse_WhenReqeustIsAjax()
        {
            // Arrange
            var httpRequestMock = new Mock<HttpRequestBase>();
            httpRequestMock
                .SetupGet(x => x.Headers)
                .Returns(new WebHeaderCollection { { "X-Requested-With", "XMLHttpRequest" } });

            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var authContextMock = new Mock<AuthorizationContext>();
            authContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

            var attribute = new AjaxAuthorizeAttribute_Fake();

            // Act
            attribute.HandleUnauthorizedRequest_Test(authContextMock.Object);

            // Assert
            Assert.IsInstanceOf<JsonResult>(authContextMock.Object.Result);
        }

        [Test]
        public void HandleUnauthorizedRequest_ShouldReturnOtherReponse_WhenReqeustIsNotAjax()
        {
            // Arrange
            var httpRequestMock = new Mock<HttpRequestBase>();

            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var authContextMock = new Mock<AuthorizationContext>();
            authContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

            var attribute = new AjaxAuthorizeAttribute_Fake();

            // Act
            attribute.HandleUnauthorizedRequest_Test(authContextMock.Object);

            // Assert
            Assert.IsNotInstanceOf<JsonResult>(authContextMock.Object.Result);
        }
    }
}
