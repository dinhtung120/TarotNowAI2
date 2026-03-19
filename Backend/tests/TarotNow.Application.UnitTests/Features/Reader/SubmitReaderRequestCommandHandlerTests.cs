/*
 * FILE: SubmitReaderRequestCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler nộp đơn đăng ký làm Reader (Tarot Reader).
 *
 *   CÁC TEST CASE (6 scenarios):
 *   1. Handle_UserNotFound_ThrowsNotFoundException: userId sai → 404
 *   2. Handle_UserInactive_ThrowsBadRequestException: user bị ban/chưa kích hoạt → 400
 *   3. Handle_UserAlreadyHasRole_ThrowsBadRequestException: đã có role đặc biệt → 400
 *   4. Handle_UserHasPendingRequest_ThrowsBadRequestException: đơn đang chờ → 400
 *   5. Handle_ValidRequest_CreatesReaderRequestDto: tạo đơn Pending → chờ Admin
 *   6. Handle_PreviouslyRejected_AllowsReSubmit: đã bị reject → cho nộp lại
 *
 *   UX: User bị reject có thể nộp lại (không khóa vĩnh viễn)
 */

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

/// <summary>
/// Test submit reader request: status/role validation, re-submit after rejection.
/// </summary>
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

    /// <summary>UserId sai → NotFoundException.</summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>User bị ban/chưa kích hoạt → BadRequest.</summary>
    [Fact]
    public async Task Handle_UserInactive_ThrowsBadRequestException()
    {
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(command.UserId, UserStatus.Banned, UserRole.User);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Tài khoản chưa được kích hoạt hoặc đã bị khóa", ex.Message);
    }

    /// <summary>Đã có role đặc biệt (TarotReader/Admin) → BadRequest.</summary>
    [Fact]
    public async Task Handle_UserAlreadyHasRole_ThrowsBadRequestException()
    {
        var command = new SubmitReaderRequestCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(command.UserId, UserStatus.Active, UserRole.TarotReader);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Bạn đã có vai trò đặc biệt, không cần đăng ký Reader", ex.Message);
    }

    /// <summary>Đơn Pending đang chờ → BadRequest (chỉ 1 đơn tại 1 thời điểm).</summary>
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

    /// <summary>Happy path: tạo đơn Pending → chờ Admin duyệt.</summary>
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

    /* Helper: tạo User với Id, Status, Role bằng reflection */
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

    /// <summary>Đã bị reject trước đó → cho phép nộp lại (UX tốt).</summary>
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
