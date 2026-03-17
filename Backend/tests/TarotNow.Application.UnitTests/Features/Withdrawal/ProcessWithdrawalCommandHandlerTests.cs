using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Withdrawal;

public class ProcessWithdrawalCommandHandlerTests
{
    private readonly Mock<IWithdrawalRepository> _mockWithdrawalRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IMfaService> _mockMfaService;
    private readonly ProcessWithdrawalCommandHandler _handler;

    public ProcessWithdrawalCommandHandlerTests()
    {
        _mockWithdrawalRepo = new Mock<IWithdrawalRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();

        _handler = new ProcessWithdrawalCommandHandler(
            _mockWithdrawalRepo.Object,
            _mockWalletRepo.Object,
            _mockUserRepo.Object,
            _mockMfaService.Object);
    }

    private User CreateValidAdmin()
    {
        var userType = typeof(User);
        var admin = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        userType.GetProperty("MfaEnabled")!.SetValue(admin, true);
        userType.GetProperty("MfaSecretEncrypted")!.SetValue(admin, "admin_secret");
        return admin;
    }

    private WithdrawalRequest CreatePendingRequest(Guid requestId)
    {
        var reqType = typeof(WithdrawalRequest);
        var req = (WithdrawalRequest)Activator.CreateInstance(reqType, nonPublic: true)!;
        reqType.GetProperty("Id")!.SetValue(req, requestId);
        reqType.GetProperty("Status")!.SetValue(req, "pending");
        reqType.GetProperty("AmountDiamond")!.SetValue(req, 100L);
        reqType.GetProperty("UserId")!.SetValue(req, Guid.NewGuid());
        return req;
    }

    [Fact]
    public async Task Handle_Approve_UpdatesStatusAndAuditTrail()
    {
        var command = new ProcessWithdrawalCommand { RequestId = Guid.NewGuid(), AdminId = Guid.NewGuid(), Action = "approve", MfaCode = "123", AdminNote = "ok" };
        var admin = CreateValidAdmin();
        var request = CreatePendingRequest(command.RequestId);

        _mockWithdrawalRepo.Setup(x => x.GetByIdAsync(command.RequestId, default)).ReturnsAsync(request);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.AdminId, default)).ReturnsAsync(admin);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("decrypted");
        _mockMfaService.Setup(x => x.VerifyCode("decrypted", command.MfaCode)).Returns(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("approved", request.Status);
        Assert.Equal(command.AdminId, request.AdminId);
        Assert.Equal(command.AdminNote, request.AdminNote);
        Assert.NotNull(request.ProcessedAt);

        _mockWalletRepo.Verify(x => x.CreditAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null, null, default), Times.Never);
        _mockWithdrawalRepo.Verify(x => x.UpdateAsync(request, default), Times.Once);
        _mockWithdrawalRepo.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_Reject_RefundsDiamondAndUpdatesStatus()
    {
        var command = new ProcessWithdrawalCommand { RequestId = Guid.NewGuid(), AdminId = Guid.NewGuid(), Action = "reject", MfaCode = "123", AdminNote = "fraud" };
        var admin = CreateValidAdmin();
        var request = CreatePendingRequest(command.RequestId);

        _mockWithdrawalRepo.Setup(x => x.GetByIdAsync(command.RequestId, default)).ReturnsAsync(request);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.AdminId, default)).ReturnsAsync(admin);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("decrypted");
        _mockMfaService.Setup(x => x.VerifyCode("decrypted", command.MfaCode)).Returns(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("rejected", request.Status);
        Assert.Equal(command.AdminId, request.AdminId);
        Assert.Equal(command.AdminNote, request.AdminNote);

        _mockWalletRepo.Verify(x => x.CreditAsync(request.UserId, "diamond", "withdrawal_refund", 100, "withdrawal_request", request.Id.ToString(), It.IsAny<string>(), null, $"wd_refund_{request.Id}", default), Times.Once);
        _mockWithdrawalRepo.Verify(x => x.UpdateAsync(request, default), Times.Once);
        _mockWithdrawalRepo.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    /// <summary>
    /// TEST CASE: Request không tồn tại → NotFoundException.
    ///
    /// Khi nào xảy ra?
    /// → RequestId sai hoặc đã bị xóa.
    /// </summary>
    [Fact]
    public async Task Handle_RequestNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ProcessWithdrawalCommand
        {
            RequestId = Guid.NewGuid(),
            AdminId = Guid.NewGuid(),
            Action = "approve",
            MfaCode = "123"
        };

        _mockWithdrawalRepo.Setup(x => x.GetByIdAsync(command.RequestId, default))
            .ReturnsAsync((WithdrawalRequest)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: Action không hợp lệ (không phải approve/reject) → BadRequestException.
    ///
    /// Tại sao cần validate?
    /// → Chỉ chấp nhận "approve" hoặc "reject".
    ///   Nếu không validate: action "delete" hoặc rau rác → undefined behavior.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidAction_ThrowsBadRequest()
    {
        // Arrange
        var command = new ProcessWithdrawalCommand
        {
            RequestId = Guid.NewGuid(),
            AdminId = Guid.NewGuid(),
            Action = "delete", // Invalid!
            MfaCode = "123"
        };

        // Act & Assert — reject ngay, không cần query DB
        await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: Request đã được xử lý (status != pending) → BadRequestException.
    ///
    /// Tại sao cần test?
    /// → Double-process protection: admin không thể approve/reject 2 lần.
    ///   Nếu bug: approve 2 lần → không ảnh hưởng.
    ///   Nếu bug: reject 2 lần → refund Diamond 2 lần → tổn thất tài chính!
    /// </summary>
    [Fact]
    public async Task Handle_AlreadyProcessed_ThrowsBadRequest()
    {
        // Arrange — request đã approved
        var command = new ProcessWithdrawalCommand
        {
            RequestId = Guid.NewGuid(),
            AdminId = Guid.NewGuid(),
            Action = "approve",
            MfaCode = "123"
        };
        var admin = CreateValidAdmin();
        var request = CreatePendingRequest(command.RequestId);
        request.GetType().GetProperty("Status")!.SetValue(request, "approved"); // Đã xử lý!

        _mockWithdrawalRepo.Setup(x => x.GetByIdAsync(command.RequestId, default)).ReturnsAsync(request);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.AdminId, default)).ReturnsAsync(admin);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", command.MfaCode)).Returns(true);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("approved", ex.Message);
    }

    /// <summary>
    /// TEST CASE: Admin MFA code sai → BadRequestException.
    ///
    /// Tại sao cần test?
    /// → BR Phase 2.5: Admin BẮT BUỘC MFA trước admin actions.
    ///   Bảo vệ: nếu admin account bị hack, attacker không thể
    ///   approve withdrawal giả mà không có MFA device.
    /// </summary>
    [Fact]
    public async Task Handle_AdminMfaInvalid_ThrowsBadRequest()
    {
        // Arrange
        var command = new ProcessWithdrawalCommand
        {
            RequestId = Guid.NewGuid(),
            AdminId = Guid.NewGuid(),
            Action = "approve",
            MfaCode = "wrong"
        };
        var admin = CreateValidAdmin();
        var request = CreatePendingRequest(command.RequestId);

        _mockWithdrawalRepo.Setup(x => x.GetByIdAsync(command.RequestId, default)).ReturnsAsync(request);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.AdminId, default)).ReturnsAsync(admin);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", "wrong")).Returns(false); // Sai code!

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA", ex.Message);
    }
}
