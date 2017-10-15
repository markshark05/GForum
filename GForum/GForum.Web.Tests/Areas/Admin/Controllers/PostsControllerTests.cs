using System;
using System.Web.Mvc;
using GForum.Services.Contracts;
using GForum.Web.Areas.Admin.Controllers;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Areas.Admin.Controllers
{
    [TestFixture]
    public class PostsControllerTests
    {
        [Test]
        public void Index_ShouldReturnViewModelAndCallPostSericeIncludingDeleted()
        {
            // Arrange
            var postServiceMock = new Mock<IPostService>();
            var controller = new PostsController(postServiceMock.Object);

            // Act
             var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            postServiceMock.Verify(x => x.GetAll(It.Is<bool>(y => y == true)));
        }

        [Test]
        public void Delete_ShouldCallPostServiceDelete()
        {
            // Arrange
            var postServiceMock = new Mock<IPostService>();
            var controller = new PostsController(postServiceMock.Object);
            var guid = Guid.NewGuid();

            // Act
            var result = controller.Delete(guid);

            // Assert
            postServiceMock.Verify(x => x.Delete(It.Is<Guid>(y => y == guid)));
        }

        [Test]
        public void Restore_ShouldCallPostServiceRestore()
        {
            // Arrange
            var postServiceMock = new Mock<IPostService>();
            var controller = new PostsController(postServiceMock.Object);
            var guid = Guid.NewGuid();

            // Act
            var result = controller.Restore(guid);

            // Assert
            postServiceMock.Verify(x => x.Restore(It.Is<Guid>(y => y == guid)));
        }
    }
}
