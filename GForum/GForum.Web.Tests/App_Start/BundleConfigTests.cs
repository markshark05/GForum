using System.Linq;
using System.Web.Optimization;
using NUnit.Framework;

namespace GForum.Web.Tests.App_Start
{
    [TestFixture]
    public class BundleConfigTests
    {
        [Test]
        public void RegisterBundles_ShouldRegisterRequiredBundles()
        {
            // Arrange
            var bundles = new BundleCollection();

            // Act
            BundleConfig.RegisterBundles(bundles);

            // Assert
            Assert.IsTrue(bundles.Any(x => x.Path == "~/bundles/jquery"));
            Assert.IsTrue(bundles.Any(x => x.Path == "~/bundles/jqueryval"));
            Assert.IsTrue(bundles.Any(x => x.Path == "~/bundles/modernizr"));
            Assert.IsTrue(bundles.Any(x => x.Path == "~/bundles/bootstrap"));
            Assert.IsTrue(bundles.Any(x => x.Path == "~/bundles/bootbox"));
            Assert.IsTrue(bundles.Any(x => x.Path == "~/bundles/toastr"));
            Assert.IsTrue(bundles.Any(x => x.Path == "~/Content/Styles/all"));
        }
    }
}
