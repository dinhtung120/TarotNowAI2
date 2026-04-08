

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

// Unit test cho handler gửi đơn đăng ký Reader.
public class SubmitReaderRequestCommandHandlerTests
{
    // Mock user repo để kiểm soát trạng thái user/role.
    private readonly Mock<IUserRepository> _mockUserRepo;
    // Mock reader request repo để kiểm soát đơn gần nhất và thao tác tạo mới.
    private readonly Mock<IReaderRequestRepository> _mockReaderReqRepo;
    // Handler cần kiểm thử.
    private readonly SubmitReaderRequestCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho SubmitReaderRequestCommandHandler.
    /// Luồng dùng mock repos để cô lập validation rule của đăng ký Reader.
    /// </summary>
    public SubmitReaderRequestCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockReaderReqRepo = new Mock<IReaderRequestRepository>();
        _handler = new SubmitReaderRequestCommandHandler(_mockUserRepo.Object, _mockReaderReqRepo.Object);
    }

    /// <summary>
    /// Xác nhận user không tồn tại trả NotFoundException.
    /// Luồng này ngăn tạo request reader cho user không hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận user inactive/banned bị từ chối đăng ký Reader.
    /// Luồng này bảo vệ rule yêu cầu tài khoản phải active.
    /// </summary>
    [Fact]
    public async Task Handle_UserInactive_ThrowsBadRequestException()
    {
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(command.UserId, UserStatus.Banned, UserRole.User);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Tài khoản chưa được kích hoạt hoặc đã bị khóa", ex.Message);
    }

    /// <summary>
    /// Xác nhận user đã có role đặc biệt không được đăng ký Reader.
    /// Luồng này tránh trùng vai trò và sai quy trình duyệt.
    /// </summary>
    [Fact]
    public async Task Handle_UserAlreadyHasRole_ThrowsBadRequestException()
    {
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(command.UserId, UserStatus.Active, UserRole.TarotReader);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Bạn đã có vai trò đặc biệt, không cần đăng ký Reader", ex.Message);
    }

    /// <summary>
    /// Xác nhận user có đơn pending hiện tại không được nộp thêm đơn mới.
    /// Luồng này bảo vệ idempotency nghiệp vụ theo trạng thái đơn gần nhất.
    /// </summary>
    [Fact]
    public async Task Handle_UserHasPendingRequest_ThrowsBadRequestException()
    {
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(command.UserId, UserStatus.Active, UserRole.User);
        var pendingReq = new ReaderRequestDto { Status = ReaderApprovalStatus.Pending };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockReaderReqRepo.Setup(x => x.GetLatestByUserIdAsync(command.UserId.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(pendingReq);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Bạn đã có đơn đang chờ", ex.Message);
    }

    /// <summary>
    /// Xác nhận request hợp lệ tạo ReaderRequest mới trạng thái Pending.
    /// Luồng này kiểm tra AddAsync nhận đúng UserId và status mặc định.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_CreatesReaderRequestDto()
    {
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid(), IntroText = "Hello" };
        var user = CreateUser(command.UserId, UserStatus.Active, UserRole.User);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockReaderReqRepo.Setup(x => x.GetLatestByUserIdAsync(command.UserId.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync((ReaderRequestDto)null!);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        _mockReaderReqRepo.Verify(x => x.AddAsync(It.Is<ReaderRequestDto>(r => r.UserId == command.UserId.ToString() && r.Status == ReaderApprovalStatus.Pending), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Tạo user test theo trạng thái và role yêu cầu.
    /// Luồng helper này giảm lặp setup entity cho nhiều test case.
    /// </summary>
    private static User CreateUser(Guid id, string status, string role)
    {
        var type = typeof(User);
        var constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).FirstOrDefault();
        var user = (User)constructor!.Invoke(null);
        // Gán trực tiếp thuộc tính cốt lõi để mô phỏng user ở các trạng thái khác nhau.
        type.GetProperty("Id")?.SetValue(user, id);
        type.GetProperty("Status")?.SetValue(user, status);
        type.GetProperty("Role")?.SetValue(user, role);
        return user;
    }

    /// <summary>
    /// Xác nhận đơn bị reject trước đó được phép nộp lại.
    /// Luồng này đảm bảo rule re-submit hợp lệ sau khi bị từ chối.
    /// </summary>
    [Fact]
    public async Task Handle_PreviouslyRejected_AllowsReSubmit()
    {
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid(), IntroText = "Try again" };
        var user = CreateUser(command.UserId, UserStatus.Active, UserRole.User);
        var rejectedReq = new ReaderRequestDto { Status = ReaderApprovalStatus.Rejected };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockReaderReqRepo.Setup(x => x.GetLatestByUserIdAsync(command.UserId.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(rejectedReq);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        _mockReaderReqRepo.Verify(x => x.AddAsync(It.Is<ReaderRequestDto>(r => r.Status == ReaderApprovalStatus.Pending), It.IsAny<CancellationToken>()), Times.Once);
    }
}
