using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GForum.Services.Contracts;
using GForum.Web.Areas.Admin.Controllers;
using GForum.Web.Areas.Admin.Models.Categories;
using Moq;
using NUnit.Framework;

namespace GForum.Web.Tests.Areas.Admin.Controllers
{
    [TestFixture]
    public class CategoriesControllerTests
    {
        [Test]
        public void Index_ShouldReturnViewModelAndCallSericeIncludingDeleted()
        {
            // Arrange
            var categoryService = new Mock<ICategoryService>();
            var controller = new CategoriesController(categoryService.Object);

            // Act
             var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            categoryService.Verify(x => x.GetAll(It.Is<bool>(y => y == true)));
        }


        [Test]
        public void Add_ShouldCallCategoryServiceCreate()
        {
            // Arrange
            var categoryService = new Mock<ICategoryService>();

            var identityMock = new Mock<IIdentity>();
            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(x => x.Identity).Returns(identityMock.Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principalMock.Object);
            var contextMock = new Mock<ControllerContext>();
            contextMock.Setup(x => x.HttpContext).Returns(httpContext.Object);

            var controller = new CategoriesController(categoryService.Object)
            {
                ControllerContext = contextMock.Object
            };

            var model = new CategoryAddViewModel
            {
                Title = "title"
            };

            // Act
            var result = controller.Add(model);

            // Assert
            categoryService.Verify(x => x.Create(It.IsAny<string>(), It.Is<string>(y => y == model.Title)));
        }

        [Test]
        public void Delete_ShouldCallCategoryServiceDelete()
        {
            // Arrange
            var categoryService = new Mock<ICategoryService>();
            var controller = new CategoriesController(categoryService.Object);
            var guid = Guid.NewGuid();

            // Act
            var result = controller.Delete(guid);

            // Assert
            categoryService.Verify(x => x.Delete(It.Is<Guid>(y => y == guid)));
        }

        [Test]
        public void Restore_ShouldCallCategoryServiceRestore()
        {
            // Arrange
            var categoryService = new Mock<ICategoryService>();
            var controller = new CategoriesController(categoryService.Object);
            var guid = Guid.NewGuid();

            // Act
            var result = controller.Restore(guid);

            // Assert
            categoryService.Verify(x => x.Restore(It.Is<Guid>(y => y == guid)));
        }
    }
}
