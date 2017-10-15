using System;
using System.Linq;
using GForum.Common.Enums;
using GForum.Data.Contracts;
using GForum.Data.Models;
using Moq;
using NUnit.Framework;

namespace GForum.Services.Tests
{
    [TestFixture]
    public class VoteServiceTests
    {
        [Test]
        public void GetUserVoteTypeForPost_ShouldReturnCorrectVoteType()
        {
            // Arrange
            var vote = new Vote
            {
                UserId = "userid",
                PostId = Guid.NewGuid(),
                VoteType = VoteType.Upvote,
            };

            var postsRepoMock = new Mock<IRepository<Post>>();

            var votesRepoMock = new Mock<IRepository<Vote>>();
            votesRepoMock.Setup(x => x.QueryAll).Returns(new Vote[] { vote }.AsQueryable());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var voteService = new VoteService(unitOfWorkMock.Object, votesRepoMock.Object, postsRepoMock.Object);

            // Act
            var result = voteService.GetUserVoteTypeForPost((Guid)vote.PostId, "userid");

            // Assert
            Assert.AreEqual(VoteType.Upvote, result);
        }

        [Test]
        public void GetUserVoteTypeForPost_ShouldReturnTypeNoneIfVoteDoesNotExist()
        {
            // Arrange
            var postsRepoMock = new Mock<IRepository<Post>>();
            var votesRepoMock = new Mock<IRepository<Vote>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var voteService = new VoteService(unitOfWorkMock.Object, votesRepoMock.Object, postsRepoMock.Object);

            // Act
            var result = voteService.GetUserVoteTypeForPost(Guid.Empty, string.Empty);

            // Assert
            Assert.AreEqual(VoteType.None, result);
        }

        [Test]
        public void ToggleVote_ShouldAddNewVote_IfUseHasNotVotedOnPost()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid() };

            var postsRepoMock = new Mock<IRepository<Post>>();
            postsRepoMock.Setup(x => x.QueryAll).Returns(new Post[] { post }.AsQueryable());

            var votesRepoMock = new Mock<IRepository<Vote>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var voteService = new VoteService(unitOfWorkMock.Object, votesRepoMock.Object, postsRepoMock.Object);

            // Act
            voteService.ToggleVote(post.Id, "userid", VoteType.Upvote);

            // Assert
            Assert.AreEqual(1, post.VoteCount);
            votesRepoMock.Verify(x => x.Add(It.Is<Vote>(v =>
                v.UserId == "userid" &&
                v.PostId == post.Id &&
                v.VoteType == VoteType.Upvote
            )), Times.Once);
            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
        }

        [Test]
        public void ToggleVote_ShouldRemoveLastUserVote_IfUserHasVotedWithTheSameVoteType()
        {
            // Arrange
            var post = new Post
            {
                Id = Guid.NewGuid(),
                VoteCount = 1,
            };
            var vote = new Vote
            {
                PostId = post.Id,
                UserId = "userid",
                VoteType = VoteType.Upvote
            };

            var postsRepoMock = new Mock<IRepository<Post>>();
            postsRepoMock.Setup(x => x.QueryAll).Returns(new Post[] { post }.AsQueryable());

            var votesRepoMock = new Mock<IRepository<Vote>>();
            votesRepoMock.Setup(x => x.QueryAll).Returns(new Vote[] { vote }.AsQueryable());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var voteService = new VoteService(unitOfWorkMock.Object, votesRepoMock.Object, postsRepoMock.Object);

            // Act
            voteService.ToggleVote(post.Id, "userid", VoteType.Upvote);

            // Assert
            Assert.AreEqual(0, post.VoteCount);
            votesRepoMock.Verify(x => x.Remove(It.Is<Vote>(v =>
                v.UserId == "userid" &&
                v.PostId == post.Id
            )), Times.Once);
            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
        }

        [Test]
        public void ToggleVote_ShouldRemoveTheOldVoteAddTheNewVote_IfUserHasVoted_ButNowVotesWithDeifferentVoteType()
        {
            // Arrange
            var post = new Post
            {
                Id = Guid.NewGuid(),
                VoteCount = 1,
            };
            var vote = new Vote
            {
                PostId = post.Id,
                UserId = "userid",
                VoteType = VoteType.Upvote
            };

            var postsRepoMock = new Mock<IRepository<Post>>();
            postsRepoMock.Setup(x => x.QueryAll).Returns(new Post[] { post }.AsQueryable());

            var votesRepoMock = new Mock<IRepository<Vote>>();
            votesRepoMock.Setup(x => x.QueryAll).Returns(new Vote[] { vote }.AsQueryable());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var voteService = new VoteService(unitOfWorkMock.Object, votesRepoMock.Object, postsRepoMock.Object);

            // Act
            voteService.ToggleVote(post.Id, "userid", VoteType.Downvote);

            // Assert
            Assert.AreEqual(-1, post.VoteCount);

            votesRepoMock.Verify(x => x.Remove(It.Is<Vote>(v =>
                v.UserId == "userid" &&
                v.PostId == post.Id &&
                v.VoteType == VoteType.Upvote
            )), Times.Once);

            votesRepoMock.Verify(x => x.Add(It.Is<Vote>(v =>
                v.UserId == "userid" &&
                v.PostId == post.Id &&
                v.VoteType == VoteType.Downvote
            )), Times.Once);

            unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
        }

        [Test]
        public void ToggleVote_ShouldDoNothing_IfPostDoesntExist()
        {
            // Arrange
            var postsRepoMock = new Mock<IRepository<Post>>();
            var votesRepoMock = new Mock<IRepository<Vote>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var voteService = new VoteService(unitOfWorkMock.Object, votesRepoMock.Object, postsRepoMock.Object);

            // Act
            voteService.ToggleVote(Guid.Empty, "userid", VoteType.Downvote);

            // Assert
            postsRepoMock.VerifyGet(x => x.QueryAll, Times.Once);
            votesRepoMock.VerifyGet(x => x.QueryAll, Times.Never);
            unitOfWorkMock.Verify(x => x.Complete(), Times.Never);
        }
    }
}