using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Community.Commands.ResolvePostReport;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Community.Commands;

public class ResolvePostReportCommandHandlerRequestedDomainEventHandlerTests
{
    [Fact]
    public async Task Handle_FreezeAccount_ShouldLockPostAuthorAndRevokeSessions()
    {
        var reportRepository = new Mock<IReportRepository>();
        var postRepository = new Mock<ICommunityPostRepository>();
        var userRepository = new Mock<IUserRepository>();
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var authSessionRepository = new Mock<IAuthSessionRepository>();

        var reportId = "report-001";
        var postId = "post-001";
        var targetUserId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var user = new User(
            email: "reported@test.local",
            username: "reported-user",
            passwordHash: "hash",
            displayName: "Reported User",
            dateOfBirth: new DateTime(1994, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            hasConsented: true);
        typeof(User).GetProperty("Id")?.SetValue(user, targetUserId);
        user.Activate();

        reportRepository
            .Setup(x => x.GetByIdAsync(reportId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReportDto
            {
                Id = reportId,
                TargetType = "post",
                TargetId = postId,
                Status = PostReportStatus.Pending
            });
        postRepository
            .Setup(x => x.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CommunityPostDto
            {
                Id = postId,
                AuthorId = targetUserId.ToString("D"),
                IsDeleted = false
            });
        userRepository
            .Setup(x => x.GetByIdAsync(targetUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        reportRepository
            .Setup(x => x.ResolvePostReportWithPostMutationAsync(
                It.Is<PostReportResolutionMutation>(m =>
                    m.ReportId == reportId
                    && m.PostId == postId
                    && m.RemovePost == false
                    && m.Result == ModerationResult.FreezeAccount),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new ResolvePostReportCommandHandlerRequestedDomainEventHandler(
            postRepository.Object,
            reportRepository.Object,
            userRepository.Object,
            refreshTokenRepository.Object,
            authSessionRepository.Object,
            Mock.Of<TarotNow.Application.Interfaces.DomainEvents.IEventHandlerIdempotencyService>());

        var result = await handler.Handle(new ResolvePostReportCommand
        {
            ReportId = reportId,
            AdminId = adminId,
            Result = ModerationResult.FreezeAccount
        }, CancellationToken.None);

        Assert.True(result);
        userRepository.Verify(
            x => x.UpdateAsync(It.Is<User>(candidate =>
                candidate.Id == targetUserId
                && candidate.Status == UserStatus.Locked), It.IsAny<CancellationToken>()),
            Times.Once);
        refreshTokenRepository.Verify(x => x.RevokeAllByUserIdAsync(targetUserId, It.IsAny<CancellationToken>()), Times.Once);
        authSessionRepository.Verify(x => x.RevokeAllByUserAsync(targetUserId, It.IsAny<CancellationToken>()), Times.Once);
        reportRepository.Verify(
            x => x.ResolvePostReportWithPostMutationAsync(It.IsAny<PostReportResolutionMutation>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
