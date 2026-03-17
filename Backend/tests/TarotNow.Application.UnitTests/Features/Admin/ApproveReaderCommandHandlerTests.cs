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

    [Fact]
    public async Task Handle_InvalidAction_ThrowsBadRequest()
    {
        var command = new ApproveReaderCommand { Action = "invalid" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_RequestNotFound_ThrowsNotFound()
    {
        var command = new ApproveReaderCommand { RequestId = "test_id", Action = "approve" };
        _mockReqRepo.Setup(x => x.GetByIdAsync("test_id", CancellationToken.None)).ReturnsAsync((ReaderRequestDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_RequestNotPending_ThrowsBadRequest()
    {
        var command = new ApproveReaderCommand { RequestId = "test_id", Action = "approve" };
        var req = new ReaderRequestDto { Status = ReaderApprovalStatus.Approved };
        _mockReqRepo.Setup(x => x.GetByIdAsync("test_id", CancellationToken.None)).ReturnsAsync(req);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

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
    
    [Fact]
    public async Task Handle_ActionReject_UpdatesUserAndRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new ApproveReaderCommand { RequestId = "req_1", Action = "reject", AdminNote = "Rejected" };
        var req = new ReaderRequestDto { Id = "req_1", Status = ReaderApprovalStatus.Pending, UserId = userId.ToString() };
        var user = CreateUser(userId, UserStatus.Active, UserRole.User);
        
        _mockReqRepo.Setup(x => x.GetByIdAsync("req_1", CancellationToken.None)).ReturnsAsync(req);
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, CancellationToken.None)).ReturnsAsync(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(ReaderApprovalStatus.Rejected, req.Status);
        Assert.Equal("Rejected", req.AdminNote);
        _mockReqRepo.Verify(x => x.UpdateAsync(req, CancellationToken.None), Times.Once);
        _mockUserRepo.Verify(x => x.UpdateAsync(It.Is<User>(u => u.ReaderStatus == ReaderApprovalStatus.Rejected), CancellationToken.None), Times.Once);
        _mockProfileRepo.Verify(x => x.AddAsync(It.IsAny<ReaderProfileDto>(), CancellationToken.None), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ActionApprove_UpdatesUser_CreatesProfile()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new ApproveReaderCommand { RequestId = "req_1", Action = "approve", AdminNote = "OK", AdminId = Guid.NewGuid() };
        var req = new ReaderRequestDto { Id = "req_1", Status = ReaderApprovalStatus.Pending, UserId = userId.ToString() };
        var user = CreateUser(userId, UserStatus.Active, UserRole.User);
        
        _mockReqRepo.Setup(x => x.GetByIdAsync("req_1", CancellationToken.None)).ReturnsAsync(req);
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, CancellationToken.None)).ReturnsAsync(user);
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), CancellationToken.None)).ReturnsAsync((ReaderProfileDto)null!);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(ReaderApprovalStatus.Approved, req.Status);
        Assert.Equal("OK", req.AdminNote);
        Assert.Equal(UserRole.TarotReader, user.Role);
        _mockReqRepo.Verify(x => x.UpdateAsync(req, CancellationToken.None), Times.Once);
        _mockUserRepo.Verify(x => x.UpdateAsync(It.Is<User>(u => u.Role == UserRole.TarotReader), CancellationToken.None), Times.Once);
        _mockProfileRepo.Verify(x => x.AddAsync(It.Is<ReaderProfileDto>(p => p.UserId == userId.ToString()), CancellationToken.None), Times.Once);
    }
    
    // Helper function
    private static User CreateUser(Guid id, string status, string role)
    {
        var type = typeof(User);
        // User class using EF conventions usually has a protected/private parameterless constructor
        var constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).FirstOrDefault();
        var user = (User)constructor!.Invoke(new object[0]);
        
        type.GetProperty("Id")?.SetValue(user, id);
        type.GetProperty("Status")?.SetValue(user, status);
        type.GetProperty("Role")?.SetValue(user, role);
        return user;
    }
}
