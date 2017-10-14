using System.Web;
using System.Web.Mvc;
using GForum.Web.Helpers;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Helpers
{
    [TestFixture]
    public class GravatarHtmlHelperTests
    {
        [Test]
        public void Expect_ToReturnCorrectImageTagWithDefaultImage()
        {
            // Arrange
            var email = "test@test.com";
            var emailHash = "b642b4217b34b1e8d3bd915fc65c4452";

            var mockViewContext = new Mock<ViewContext>();
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(mockViewContext.Object, mockViewDataContainer.Object);

            // Act
            var result = GravatarHtmlHelper.GravatarImage(htmlHelper,
                email, 
                defaultImage: GravatarHtmlHelper.DefaultImage.Retro);

            // Assert
            Assert.IsInstanceOf<HtmlString>(result);
            StringAssert.Contains(emailHash, result.ToString());
            StringAssert.Contains("d=retro", result.ToString());
        }

        [Test]
        public void Expect_ToReturnCorrectImageTagWithCustomImage()
        {
            // Arrange
            var email = "test@test.com";
            var emailHash = "b642b4217b34b1e8d3bd915fc65c4452";

            var mockViewContext = new Mock<ViewContext>();
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(mockViewContext.Object, mockViewDataContainer.Object);

            // Act
            var result = GravatarHtmlHelper.GravatarImage(htmlHelper,
                email,
                defaultImageUrl: "test.jpg");

            // Assert
            Assert.IsInstanceOf<HtmlString>(result);
            StringAssert.Contains(emailHash, result.ToString());
            StringAssert.Contains("d=test.jpg", result.ToString());
        }

        [Test]
        public void Expect_ToForceDefaultImg_WhenGIvenParam()
        {
            // Arrange
            var mockViewContext = new Mock<ViewContext>();
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(mockViewContext.Object, mockViewDataContainer.Object);

            // Act
            var result = GravatarHtmlHelper.GravatarImage(htmlHelper, string.Empty, forceDefaultImage: true);

            // Assert
            Assert.IsInstanceOf<HtmlString>(result);
            StringAssert.Contains("f=y", result.ToString());
        }

        [Test]
        public void Expect_ToSwitchToUnsecure_WhenGivenParam()
        {
            // Arrange
            var mockViewContext = new Mock<ViewContext>();
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(mockViewContext.Object, mockViewDataContainer.Object);

            // Act
            var result = GravatarHtmlHelper.GravatarImage(htmlHelper, string.Empty, forceSecureRequest: false);

            // Assert
            Assert.IsInstanceOf<HtmlString>(result);
            StringAssert.DoesNotContain("https", result.ToString());
        }
    }
}
