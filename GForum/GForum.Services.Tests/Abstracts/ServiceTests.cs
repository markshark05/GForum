using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Services.Tests.Abstracts.Fakes;
using Moq;
using NUnit.Framework;

namespace GForum.Services.Tests.Abstracts
{
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void GetAll_ShouldCallQueryAll()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Entitity_Fake>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new Service_Fake(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            var result = postService.GetAll();

            // Assert
            repositoryMock.Verify(x => x.QueryAll, Times.Once);
        }

        [Test]
        public void GetAll_true_ShouldCallQueryAllWithDeleted()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Entitity_Fake>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new Service_Fake(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            var result = postService.GetAll(true);

            // Assert
            repositoryMock.Verify(x => x.QueryAllWithDeletd, Times.Once);
        }

        [Test]
        public void GetByID_ShouldCallQueryAllAndFilterById()
        {
            // Arrange
            var expectedEntity = new Entitity_Fake() { Id = Guid.NewGuid() };
            var posts = new[] {
                expectedEntity,
                new Entitity_Fake { Id = Guid.NewGuid() }
            }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Entitity_Fake>>();
            repositoryMock.Setup(x => x.QueryAll).Returns(posts);

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new Service_Fake(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            var result = postService.GetById(expectedEntity.Id);

            // Assert
            repositoryMock.Verify(x => x.QueryAll, Times.Once);
            Assert.Contains(expectedEntity, result.ToList());
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetByID_true_ShouldCallQueryAllWithDeletedAndFilterById()
        {
            // Arrange
            var expectedEntity = new Entitity_Fake() { Id = Guid.NewGuid() };
            var entities = new[] {
                expectedEntity,
                new Entitity_Fake { Id = Guid.NewGuid() }
            }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Entitity_Fake>>();
            repositoryMock.Setup(x => x.QueryAllWithDeletd).Returns(entities);

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new Service_Fake(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            var result = postService.GetById(expectedEntity.Id, true);

            // Assert
            repositoryMock.Verify(x => x.QueryAllWithDeletd, Times.Once);
            Assert.Contains(expectedEntity, result.ToList());
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Delete_ShouldCallRepositoryRemoveAndSaveChanges()
        {
            // Arrange
            var entity = new Entitity_Fake { Id = Guid.NewGuid() };
            var entities = new[] {
                entity,
                new Entitity_Fake { Id = Guid.NewGuid() }
            }.AsQueryable();

            var repositoryMock = new Mock<IRepository<Entitity_Fake>>();
            repositoryMock.SetupGet(x => x.QueryAll).Returns(entities);
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var postService = new Service_Fake(unitOfWorkMock.Object, repositoryMock.Object);

            // Act
            postService.Delete(entity.Id);

            // Assert
            repositoryMock.Verify(x => x.Remove(It.Is<Entitity_Fake>(y => y == entity)), Times.Once);
            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
        }
    }
}
