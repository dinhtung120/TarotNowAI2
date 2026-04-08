

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

// Unit test cho handler cập nhật trạng thái online/offline/busy của Reader.
public class UpdateReaderStatusCommandHandlerTests
{
    // Mock profile repo để điều khiển trạng thái profile trước/sau update.
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    // Handler cần kiểm thử.
    private readonly UpdateReaderStatusCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho UpdateReaderStatusCommandHandler.
    /// Luồng dùng mock repository để kiểm thử validation status.
    /// </summary>
    public UpdateReaderStatusCommandHandlerTests()
    {
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _handler = new UpdateReaderStatusCommandHandler(_mockProfileRepo.Object);
    }

    /// <summary>
    /// Xác nhận status không hợp lệ bị từ chối ngay.
    /// Luồng này đảm bảo handler không truy cập repository khi đầu vào sai.
    /// </summary>
    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    public async Task Handle_InvalidStatus_ThrowsBadRequestException(string invalidStatus)
    {
        var command = new UpdateReaderStatusCommand { UserId = Guid.NewGuid(), Status = invalidStatus };

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không hợp lệ", ex.Message);
        _mockProfileRepo.Verify(x => x.GetByUserIdAsync(It.IsAny<string>(), default), Times.Never);
    }

    /// <summary>
    /// Xác nhận status Busy hợp lệ sẽ được cập nhật thành công.
    /// Luồng này kiểm tra cập nhật profile và gọi UpdateAsync đúng một lần.
    /// </summary>
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

    /// <summary>
    /// Xác nhận profile không tồn tại trả NotFoundException.
    /// Luồng này chặn cập nhật trạng thái cho reader chưa có profile.
    /// </summary>
    [Fact]
    public async Task Handle_ProfileNotFound_ThrowsNotFoundException()
    {
        var command = new UpdateReaderStatusCommand { UserId = Guid.NewGuid(), Status = ReaderOnlineStatus.Busy };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.UserId.ToString(), default)).ReturnsAsync((ReaderProfileDto)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận status Online không cho cập nhật thủ công.
    /// Luồng này bảo vệ rule Online được hệ thống điều khiển tự động.
    /// </summary>
    [Fact]
    public async Task Handle_OnlineStatus_ThrowsBadRequestException()
    {
        var userId = Guid.NewGuid();
        var command = new UpdateReaderStatusCommand { UserId = userId, Status = ReaderOnlineStatus.Online };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("cập nhật tự động", ex.Message);
        _mockProfileRepo.Verify(x => x.GetByUserIdAsync(It.IsAny<string>(), default), Times.Never);
        _mockProfileRepo.Verify(x => x.UpdateAsync(It.IsAny<ReaderProfileDto>(), default), Times.Never);
    }

    /// <summary>
    /// Xác nhận status Offline hợp lệ được cập nhật thành công.
    /// Luồng này kiểm tra chuyển trạng thái từ Busy về Offline.
    /// </summary>
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
