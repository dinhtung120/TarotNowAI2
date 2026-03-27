/*
 * FILE: UpdateReaderStatusCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler chuyển đổi trạng thái online của Reader.
 *
 *   CÁC TEST CASE (4 scenarios):
 *   1. Handle_InvalidStatus_ThrowsBadRequestException: [Theory] "busy","invalid","" → 400
 *   2. Handle_ProfileNotFound_ThrowsNotFoundException: chưa approved → 404
 *   3. Handle_ValidAcceptingStatus_UpdatesSuccessfully: set AcceptingQuestions → OK
 *   4. Handle_ValidOfflineStatus_UpdatesSuccessfully: set Offline → OK
 *
 *   TRẠNG THÁI HỢP LỆ: online | offline | busy
 *   → online, busy = gate check cho CreateConversation
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

/// <summary>
/// Test update reader status: enum validation, profile existence, state transition.
/// </summary>
public class UpdateReaderStatusCommandHandlerTests
{
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    private readonly UpdateReaderStatusCommandHandler _handler;

    public UpdateReaderStatusCommandHandlerTests()
    {
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _handler = new UpdateReaderStatusCommandHandler(_mockProfileRepo.Object);
    }

    /// <summary>Trạng thái không hợp lệ → BadRequest + không gọi DB.</summary>
    [Theory]
    [InlineData("busy")]
    [InlineData("invalid")]
    [InlineData("")]
    public async Task Handle_InvalidStatus_ThrowsBadRequestException(string invalidStatus)
    {
        var command = new UpdateReaderStatusCommand { UserId = Guid.NewGuid(), Status = invalidStatus };

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không hợp lệ", ex.Message);
        _mockProfileRepo.Verify(x => x.GetByUserIdAsync(It.IsAny<string>(), default), Times.Never); // Không gọi DB
    }

    [Fact]
    public async Task Handle_ValidBusyStatus_UpdatesSuccessfully()
    {
        var userId = Guid.NewGuid();
        var command = new UpdateReaderStatusCommand { UserId = userId, Status = ReaderOnlineStatus.Busy };
        var existingProfile = new ReaderProfileDto { UserId = userId.ToString(), Status = ReaderOnlineStatus.Online };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default)).ReturnsAsync(existingProfile);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(ReaderOnlineStatus.Busy, existingProfile.Status);
        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }

    /// <summary>Profile chưa tồn tại → NotFoundException.</summary>
    [Fact]
    public async Task Handle_ProfileNotFound_ThrowsNotFoundException()
    {
        var command = new UpdateReaderStatusCommand { UserId = Guid.NewGuid(), Status = ReaderOnlineStatus.Online };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.UserId.ToString(), default)).ReturnsAsync((ReaderProfileDto)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Set Online → OK (gate check cho chat).</summary>
    [Fact]
    public async Task Handle_ValidOnlineStatus_UpdatesSuccessfully()
    {
        // Ghi chú: Command giờ chỉ nhận offline/busy. 
        // Nhưng nếu override manual ở unit test thì Online vẫn hợp lệ theo rules cũ
        var userId = Guid.NewGuid();
        var command = new UpdateReaderStatusCommand { UserId = userId, Status = ReaderOnlineStatus.Online };
        var existingProfile = new ReaderProfileDto { UserId = userId.ToString(), Status = ReaderOnlineStatus.Offline };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default)).ReturnsAsync(existingProfile);

        // Act & Assert (hiện tại validation controller chặn online, nhưng handler vẫn cho phép nếu qua lọt)
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(ReaderOnlineStatus.Online, existingProfile.Status);
        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }

    /// <summary>Set Offline → OK (ẩn khỏi directory).</summary>
    [Fact]
    public async Task Handle_ValidOfflineStatus_UpdatesSuccessfully()
    {
        var userId = Guid.NewGuid();
        var command = new UpdateReaderStatusCommand { UserId = userId, Status = ReaderOnlineStatus.Offline };
        var existingProfile = new ReaderProfileDto { UserId = userId.ToString(), Status = ReaderOnlineStatus.Busy };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default)).ReturnsAsync(existingProfile);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(ReaderOnlineStatus.Offline, existingProfile.Status);
        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }
}
