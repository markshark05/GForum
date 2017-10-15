using System.Web.Mvc;
using GForum.Web.Areas.Admin.Controllers;
using NUnit.Framework;

namespace GForum.Web.Tests.Areas.Admin.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void Index_ShouldReturnView()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
