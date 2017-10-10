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
        private string guidPattern = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

        [Test]
        public void GetUserVoteTypeForPost_ShouldReturnCorrectVoteType()
        {
            // Arrange
            var postId = new Guid(this.guidPattern.Replace('x', '1'));
            var vote = new Vote
            {
                UserId = "userid",
                PostId = postId,
                VoteType = VoteType.Upvote,
            };

            var postsRepoMock = new Mock<IRepository<Post>>();

            var votesRepoMock = new Mock<IRepository<Vote>>();
            votesRepoMock.Setup(x => x.Query).Returns(new Vote[] { vote }.AsQueryable());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var voteService = new VoteService(unitOfWorkMock.Object, postsRepoMock.Object, votesRepoMock.Object);

            // Act
            var result = voteService.GetUserVoteTypeForPost(postId, "userid");

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
            var voteService = new VoteService(unitOfWorkMock.Object, postsRepoMock.Object, votesRepoMock.Object);

            // Act
            var result = voteService.GetUserVoteTypeForPost(Guid.Empty, string.Empty);

            // Assert
            Assert.AreEqual(VoteType.None, result);
        }

        [Test]
        public void ToggleVote_ShouldAddNewVote_IfUseHasNotVotedOnPost()
        {
            // Arrange
            var post = new Post { Id = new Guid(this.guidPattern.Replace('x', '1')) };

            var postsRepoMock = new Mock<IRepository<Post>>();
            postsRepoMock.Setup(x => x.Query).Returns(new Post[] { post }.AsQueryable());

            var votesRepoMock = new Mock<IRepository<Vote>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var voteService = new VoteService(unitOfWorkMock.Object, postsRepoMock.Object, votesRepoMock.Object);

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
                Id = new Guid(this.guidPattern.Replace('x', '1')),
                VoteCount = 1,
            };
            var vote = new Vote
            {
                PostId = post.Id,
                UserId = "userid",
                VoteType = VoteType.Upvote
            };

            var postsRepoMock = new Mock<IRepository<Post>>();
            postsRepoMock.Setup(x => x.Query).Returns(new Post[] { post }.AsQueryable());

            var votesRepoMock = new Mock<IRepository<Vote>>();
            votesRepoMock.Setup(x => x.Query).Returns(new Vote[] { vote }.AsQueryable());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var voteService = new VoteService(unitOfWorkMock.Object, postsRepoMock.Object, votesRepoMock.Object);

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
                Id = new Guid(this.guidPattern.Replace('x', '1')),
                VoteCount = 1,
            };
            var vote = new Vote
            {
                PostId = post.Id,
                UserId = "userid",
                VoteType = VoteType.Upvote
            };

            var postsRepoMock = new Mock<IRepository<Post>>();
            postsRepoMock.Setup(x => x.Query).Returns(new Post[] { post }.AsQueryable());

            var votesRepoMock = new Mock<IRepository<Vote>>();
            votesRepoMock.Setup(x => x.Query).Returns(new Vote[] { vote }.AsQueryable());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var voteService = new VoteService(unitOfWorkMock.Object, postsRepoMock.Object, votesRepoMock.Object);

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
    }
}