using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using System.Reflection;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

public class SubmitReaderRequestCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IReaderRequestRepository> _mockReaderReqRepo;
    private readonly SubmitReaderRequestCommandHandler _handler;

    public SubmitReaderRequestCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockReaderReqRepo = new Mock<IReaderRequestRepository>();
        _handler = new SubmitReaderRequestCommandHandler(_mockUserRepo.Object, _mockReaderReqRepo.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UserInactive_ThrowsBadRequestException()
    {
        // Arrange
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(command.UserId, UserStatus.Banned, UserRole.User);
        
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Tài khoản chưa được kích hoạt hoặc đã bị khóa", ex.Message);
    }

    [Fact]
    public async Task Handle_UserAlreadyHasRole_ThrowsBadRequestException()
    {
        // Arrange
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(command.UserId, UserStatus.Active, UserRole.TarotReader);
        
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Bạn đã có vai trò đặc biệt, không cần đăng ký Reader", ex.Message);
    }

    [Fact]
    public async Task Handle_UserHasPendingRequest_ThrowsBadRequestException()
    {
        // Arrange
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(command.UserId, UserStatus.Active, UserRole.User);
        var pendingReq = new ReaderRequestDto { Status = ReaderApprovalStatus.Pending };
        
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockReaderReqRepo.Setup(x => x.GetLatestByUserIdAsync(command.UserId.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(pendingReq);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Bạn đã có đơn đang chờ", ex.Message);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesReaderRequestDto()
    {
        // Arrange
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid(), IntroText = "Hello" };
        var user = CreateUser(command.UserId, UserStatus.Active, UserRole.User);
        
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockReaderReqRepo.Setup(x => x.GetLatestByUserIdAsync(command.UserId.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync((ReaderRequestDto)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockReaderReqRepo.Verify(x => x.AddAsync(It.Is<ReaderRequestDto>(r => r.UserId == command.UserId.ToString() && r.Status == ReaderApprovalStatus.Pending), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    // Helper function
    private static User CreateUser(Guid id, string status, string role)
    {
        var type = typeof(User);
        var constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).FirstOrDefault();
        var user = (User)constructor!.Invoke(null);
        
        type.GetProperty("Id")?.SetValue(user, id);
        type.GetProperty("Status")?.SetValue(user, status);
        type.GetProperty("Role")?.SetValue(user, role);
        return user;
    }

    /// <summary>
    /// TEST CASE: User đã bị reject trước đó, nộp đơn lại → cho phép.
    ///
    /// Tại sao test này quan trọng?
    /// → Handler chỉ chặn khi latestRequest.Status == Pending.
    ///   Nếu Status == Rejected → user có thể nộp đơn mới.
    ///   UX: không khóa vĩnh viễn user bị reject.
    /// </summary>
    [Fact]
    public async Task Handle_PreviouslyRejected_AllowsReSubmit()
    {
        // Arrange — user đã bị reject trước đó
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid(), IntroText = "Try again" };
        var user = CreateUser(command.UserId, UserStatus.Active, UserRole.User);
        var rejectedReq = new ReaderRequestDto { Status = ReaderApprovalStatus.Rejected };

        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockReaderReqRepo.Setup(x => x.GetLatestByUserIdAsync(command.UserId.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(rejectedReq);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert — cho phép submit lại
        Assert.True(result);
        _mockReaderReqRepo.Verify(x => x.AddAsync(
            It.Is<ReaderRequestDto>(r => r.Status == ReaderApprovalStatus.Pending),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
