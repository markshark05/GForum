using System.Web;
using System.Web.Mvc;
using GForum.Web.Helpers;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Helpers
{
    [TestFixture]
    public class MarkdownToHtmlHelperTests
    {
        [Test]
        public void ExpectToConvertMarkdownToHtmlAndEscapeHtml()
        {
            // Arrange
            var input = "**bold** *italic* <script>escaped</script>";
            var expected = "<p><strong>bold</strong> <em>italic</em> &lt;script&gt;escaped&lt;/script&gt;</p>\n";

            var mockViewContext = new Mock<ViewContext>();
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(mockViewContext.Object, mockViewDataContainer.Object);

            // Act
            var result = MarkdownToHtmlHelper.MarkdownToHtml(htmlHelper, input);

            // Assert
            Assert.IsInstanceOf<IHtmlString>(result);
            Assert.AreEqual(expected, result.ToString());
        }
    }
}
