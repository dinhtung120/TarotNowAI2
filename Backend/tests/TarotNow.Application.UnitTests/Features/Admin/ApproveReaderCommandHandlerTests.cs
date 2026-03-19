/*
 * FILE: ApproveReaderCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler Admin duyệt/từ chối yêu cầu Reader.
 *
 *   CÁC TEST CASE:
 *   1. Handle_InvalidAction_ThrowsBadRequest: action không hợp lệ → 400
 *   2. Handle_RequestNotFound_ThrowsNotFound: request ID không tồn tại → 404
 *   3. Handle_RequestNotPending_ThrowsBadRequest: request đã xử lý rồi → 400
 *   4. Handle_UserNotFound_ThrowsNotFound: User bị xóa → 404
 *   5. Handle_ActionReject_UpdatesUserAndRequest: reject → status=Rejected, KHÔNG tạo profile
 *   6. Handle_ActionApprove_UpdatesUser_CreatesProfile: approve → role=TarotReader, tạo profile MongoDB
 *
 *   PATTERN:
 *   → Moq mock các repositories → test handler logic thuần túy
 *   → Reflection tạo User (vì constructor private/protected cho EF Core)
 *   → Verify: kiểm tra mock được gọi đúng số lần, đúng tham số
 */

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

/// <summary>
/// Test Admin approve/reject Reader request — RBAC + state machine.
/// </summary>
public class ApproveReaderCommandHandlerTests
{
    private readonly Mock<IReaderRequestRepository> _mockReqRepo;
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly ApproveReaderCommandHandler _handler;

    public ApproveReaderCommandHandlerTests()
    {
        _mockReqRepo = new Mock<IReaderRequestRepository>();
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _handler = new ApproveReaderCommandHandler(_mockReqRepo.Object, _mockProfileRepo.Object, _mockUserRepo.Object);
    }

    /// <summary>Action không hợp lệ (vd: "invalid") → BadRequestException.</summary>
    [Fact]
    public async Task Handle_InvalidAction_ThrowsBadRequest()
    {
        var command = new ApproveReaderCommand { Action = "invalid" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Request ID không tồn tại → NotFoundException (404).</summary>
    [Fact]
    public async Task Handle_RequestNotFound_ThrowsNotFound()
    {
        var command = new ApproveReaderCommand { RequestId = "test_id", Action = "approve" };
        _mockReqRepo.Setup(x => x.GetByIdAsync("test_id", CancellationToken.None)).ReturnsAsync((ReaderRequestDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Request đã xử lý (Approved) → không cho xử lý lần 2 → BadRequest.</summary>
    [Fact]
    public async Task Handle_RequestNotPending_ThrowsBadRequest()
    {
        var command = new ApproveReaderCommand { RequestId = "test_id", Action = "approve" };
        var req = new ReaderRequestDto { Status = ReaderApprovalStatus.Approved };
        _mockReqRepo.Setup(x => x.GetByIdAsync("test_id", CancellationToken.None)).ReturnsAsync(req);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>User bị xóa/không tồn tại → NotFoundException.</summary>
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
    /// REJECT: status → Rejected, KHÔNG tạo ReaderProfile.
    /// Verify: UpdateAsync gọi 1 lần, AddAsync profile KHÔNG được gọi.
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

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(ReaderApprovalStatus.Rejected, req.Status);
        Assert.Equal("Rejected", req.AdminNote);
        _mockReqRepo.Verify(x => x.UpdateAsync(req, CancellationToken.None), Times.Once);
        _mockUserRepo.Verify(x => x.UpdateAsync(It.Is<User>(u => u.ReaderStatus == ReaderApprovalStatus.Rejected), CancellationToken.None), Times.Once);
        _mockProfileRepo.Verify(x => x.AddAsync(It.IsAny<ReaderProfileDto>(), CancellationToken.None), Times.Never); // KHÔNG tạo profile
    }
    
    /// <summary>
    /// APPROVE: status → Approved, role → TarotReader, TẠO ReaderProfile mới.
    /// Verify: user.Role = TarotReader, profile AddAsync gọi 1 lần.
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

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(ReaderApprovalStatus.Approved, req.Status);
        Assert.Equal("OK", req.AdminNote);
        Assert.Equal(UserRole.TarotReader, user.Role);
        _mockReqRepo.Verify(x => x.UpdateAsync(req, CancellationToken.None), Times.Once);
        _mockUserRepo.Verify(x => x.UpdateAsync(It.Is<User>(u => u.Role == UserRole.TarotReader), CancellationToken.None), Times.Once);
        _mockProfileRepo.Verify(x => x.AddAsync(It.Is<ReaderProfileDto>(p => p.UserId == userId.ToString()), CancellationToken.None), Times.Once);
    }
    
    /// <summary>Helper: tạo User qua reflection (EF Core entity có constructor protected).</summary>
    private static User CreateUser(Guid id, string status, string role)
    {
        var type = typeof(User);
        var constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).FirstOrDefault();
        var user = (User)constructor!.Invoke(new object[0]);
        
        type.GetProperty("Id")?.SetValue(user, id);
        type.GetProperty("Status")?.SetValue(user, status);
        type.GetProperty("Role")?.SetValue(user, role);
        return user;
    }
}
