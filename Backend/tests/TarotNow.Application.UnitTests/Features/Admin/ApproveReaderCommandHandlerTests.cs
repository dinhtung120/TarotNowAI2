

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Admin.Commands.ApproveReader;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using System.Reflection;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Admin;

// Unit test cho luồng duyệt/từ chối yêu cầu reader.
public class ApproveReaderCommandHandlerTests
{
    // Mock repo request reader để kiểm soát trạng thái đầu vào/đầu ra.
    private readonly Mock<IReaderRequestRepository> _mockReqRepo;
    // Mock repo profile reader để xác nhận nhánh tạo profile khi approve.
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    // Mock repo user để kiểm tra cập nhật role/status.
    private readonly Mock<IUserRepository> _mockUserRepo;
    // Handler cần kiểm thử.
    private readonly ApproveReaderCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho test ApproveReaderCommandHandler.
    /// Luồng tiêm đầy đủ mock dependencies để cô lập logic handler.
    /// </summary>
    public ApproveReaderCommandHandlerTests()
    {
        _mockReqRepo = new Mock<IReaderRequestRepository>();
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _handler = new ApproveReaderCommandHandler(_mockReqRepo.Object, _mockProfileRepo.Object, _mockUserRepo.Object);
    }

    /// <summary>
    /// Xác nhận action không hợp lệ bị từ chối bằng BadRequestException.
    /// Luồng này bảo vệ business rule chỉ cho phép approve/reject.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidAction_ThrowsBadRequest()
    {
        var command = new ApproveReaderCommand { Action = "invalid" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận request không tồn tại trả về NotFoundException.
    /// Luồng này đảm bảo handler không cập nhật dữ liệu khi thiếu request nguồn.
    /// </summary>
    [Fact]
    public async Task Handle_RequestNotFound_ThrowsNotFound()
    {
        var command = new ApproveReaderCommand { RequestId = "test_id", Action = "approve" };
        _mockReqRepo.Setup(x => x.GetByIdAsync("test_id", CancellationToken.None)).ReturnsAsync((ReaderRequestDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận request không ở trạng thái pending bị từ chối.
    /// Luồng bảo vệ rule chỉ xử lý duyệt khi yêu cầu còn chờ.
    /// </summary>
    [Fact]
    public async Task Handle_RequestNotPending_ThrowsBadRequest()
    {
        var command = new ApproveReaderCommand { RequestId = "test_id", Action = "approve" };
        var req = new ReaderRequestDto { Status = ReaderApprovalStatus.Approved };
        _mockReqRepo.Setup(x => x.GetByIdAsync("test_id", CancellationToken.None)).ReturnsAsync(req);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận user không tồn tại trả về NotFoundException.
    /// Luồng này chặn trạng thái dữ liệu mồ côi giữa request và user.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFound()
    {
        var userIdStr = Guid.NewGuid().ToString();
        var command = new ApproveReaderCommand { RequestId = "test_id", Action = "approve" };
        var req = new ReaderRequestDto { Status = ReaderApprovalStatus.Pending, UserId = userIdStr };
        _mockReqRepo.Setup(x => x.GetByIdAsync("test_id", CancellationToken.None)).ReturnsAsync(req);
        _mockUserRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync((User)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận nhánh reject cập nhật đúng request và trạng thái reader của user.
    /// Luồng này cũng kiểm tra không tạo profile khi từ chối.
    /// </summary>
    [Fact]
    public async Task Handle_ActionReject_UpdatesUserAndRequest()
    {
        var userId = Guid.NewGuid();
        var command = new ApproveReaderCommand { RequestId = "req_1", Action = "reject", AdminNote = "Rejected" };
        var req = new ReaderRequestDto { Id = "req_1", Status = ReaderApprovalStatus.Pending, UserId = userId.ToString() };
        var user = CreateUser(userId, UserStatus.Active, UserRole.User);

        _mockReqRepo.Setup(x => x.GetByIdAsync("req_1", CancellationToken.None)).ReturnsAsync(req);
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, CancellationToken.None)).ReturnsAsync(user);

        // Thực thi nhánh reject và xác nhận trạng thái cập nhật đúng.
        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(ReaderApprovalStatus.Rejected, req.Status);
        Assert.Equal("Rejected", req.AdminNote);
        _mockReqRepo.Verify(x => x.UpdateAsync(req, CancellationToken.None), Times.Once);
        _mockUserRepo.Verify(x => x.UpdateAsync(It.Is<User>(u => u.ReaderStatus == ReaderApprovalStatus.Rejected), CancellationToken.None), Times.Once);
        _mockProfileRepo.Verify(x => x.AddAsync(It.IsAny<ReaderProfileDto>(), CancellationToken.None), Times.Never);
    }

    /// <summary>
    /// Xác nhận nhánh approve cập nhật role user và tạo reader profile khi chưa có.
    /// Luồng kiểm tra đủ cả cập nhật request lẫn side-effect tạo profile.
    /// </summary>
    [Fact]
    public async Task Handle_ActionApprove_UpdatesUser_CreatesProfile()
    {
        var userId = Guid.NewGuid();
        var command = new ApproveReaderCommand { RequestId = "req_1", Action = "approve", AdminNote = "OK", AdminId = Guid.NewGuid() };
        var req = new ReaderRequestDto { Id = "req_1", Status = ReaderApprovalStatus.Pending, UserId = userId.ToString() };
        var user = CreateUser(userId, UserStatus.Active, UserRole.User);

        _mockReqRepo.Setup(x => x.GetByIdAsync("req_1", CancellationToken.None)).ReturnsAsync(req);
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, CancellationToken.None)).ReturnsAsync(user);
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), CancellationToken.None)).ReturnsAsync((ReaderProfileDto)null!);

        // Thực thi nhánh approve và xác nhận role/profile được cập nhật đúng.
        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(ReaderApprovalStatus.Approved, req.Status);
        Assert.Equal("OK", req.AdminNote);
        Assert.Equal(UserRole.TarotReader, user.Role);
        _mockReqRepo.Verify(x => x.UpdateAsync(req, CancellationToken.None), Times.Once);
        _mockUserRepo.Verify(x => x.UpdateAsync(It.Is<User>(u => u.Role == UserRole.TarotReader), CancellationToken.None), Times.Once);
        _mockProfileRepo.Verify(x => x.AddAsync(It.Is<ReaderProfileDto>(p => p.UserId == userId.ToString()), CancellationToken.None), Times.Once);
    }

    /// <summary>
    /// Tạo thực thể User phục vụ test bằng reflection.
    /// Luồng này cho phép khởi tạo user theo trạng thái mong muốn mà không phụ thuộc constructor domain cụ thể.
    /// </summary>
    private static User CreateUser(Guid id, string status, string role)
    {
        var type = typeof(User);
        var constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).FirstOrDefault();
        var user = (User)constructor!.Invoke(new object[0]);

        // Gán các thuộc tính cốt lõi để mô phỏng trạng thái user trước khi handler xử lý.
        type.GetProperty("Id")?.SetValue(user, id);
        type.GetProperty("Status")?.SetValue(user, status);
        type.GetProperty("Role")?.SetValue(user, role);
        return user;
    }
}
