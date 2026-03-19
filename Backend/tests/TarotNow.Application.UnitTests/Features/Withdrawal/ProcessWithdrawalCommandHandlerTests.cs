/*
 * FILE: ProcessWithdrawalCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler Admin xử lý yêu cầu rút tiền (approve/reject).
 *
 *   CÁC TEST CASE (5 scenarios):
 *   1. Handle_Approve_UpdatesStatusAndAuditTrail:
 *      → Approve → status=approved + AdminId + AdminNote + ProcessedAt
 *   2. Handle_Reject_RefundsDiamondAndUpdatesStatus:
 *      → Reject → status=rejected + CreditAsync refund Diamond
 *   3. Handle_RequestNotFound_ThrowsNotFoundException: requestId sai → 404
 *   4. Handle_InvalidAction_ThrowsBadRequest: action != approve/reject → 400
 *   5. Handle_AlreadyProcessed_ThrowsBadRequest: double-process → 400
 *   6. Handle_AdminMfaInvalid_ThrowsBadRequest: Admin MFA sai → 400
 *
 *   BẢO MẬT: Admin BẮT BUỘC MFA cho process withdrawal (BR Phase 2.5)
 *   AUDIT: AdminId, AdminNote, ProcessedAt để truy vết
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Withdrawal;

/// <summary>
/// Test process withdrawal: approve/reject, MFA, double-process guard, refund.
/// </summary>
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
            _mockWithdrawalRepo.Object, _mockWalletRepo.Object,
            _mockUserRepo.Object, _mockMfaService.Object);
    }

    /* Helper: tạo Admin hợp lệ (MFA bật) */
    private User CreateValidAdmin()
    {
        var userType = typeof(User);
        var admin = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        userType.GetProperty("MfaEnabled")!.SetValue(admin, true);
        userType.GetProperty("MfaSecretEncrypted")!.SetValue(admin, "admin_secret");
        return admin;
    }

    /* Helper: tạo WithdrawalRequest pending */
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

    /// <summary>Approve → audit trail (AdminId, Note, ProcessedAt).</summary>
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
        Assert.NotNull(request.ProcessedAt);
        _mockWalletRepo.Verify(x => x.CreditAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null, null, default), Times.Never);
    }

    /// <summary>Reject → refund Diamond + audit trail.</summary>
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
        _mockWalletRepo.Verify(x => x.CreditAsync(request.UserId, "diamond", "withdrawal_refund", 100, "withdrawal_request", request.Id.ToString(), It.IsAny<string>(), null, $"wd_refund_{request.Id}", default), Times.Once);
    }

    /// <summary>RequestId sai → NotFoundException.</summary>
    [Fact]
    public async Task Handle_RequestNotFound_ThrowsNotFoundException()
    {
        var command = new ProcessWithdrawalCommand { RequestId = Guid.NewGuid(), AdminId = Guid.NewGuid(), Action = "approve", MfaCode = "123" };
        _mockWithdrawalRepo.Setup(x => x.GetByIdAsync(command.RequestId, default)).ReturnsAsync((WithdrawalRequest)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Action không hợp lệ → BadRequest.</summary>
    [Fact]
    public async Task Handle_InvalidAction_ThrowsBadRequest()
    {
        var command = new ProcessWithdrawalCommand { RequestId = Guid.NewGuid(), AdminId = Guid.NewGuid(), Action = "delete", MfaCode = "123" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Double-process (đã approved) → BadRequest.</summary>
    [Fact]
    public async Task Handle_AlreadyProcessed_ThrowsBadRequest()
    {
        var command = new ProcessWithdrawalCommand { RequestId = Guid.NewGuid(), AdminId = Guid.NewGuid(), Action = "approve", MfaCode = "123" };
        var admin = CreateValidAdmin();
        var request = CreatePendingRequest(command.RequestId);
        request.GetType().GetProperty("Status")!.SetValue(request, "approved");
        _mockWithdrawalRepo.Setup(x => x.GetByIdAsync(command.RequestId, default)).ReturnsAsync(request);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.AdminId, default)).ReturnsAsync(admin);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", command.MfaCode)).Returns(true);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("approved", ex.Message);
    }

    /// <summary>Admin MFA sai → BadRequest.</summary>
    [Fact]
    public async Task Handle_AdminMfaInvalid_ThrowsBadRequest()
    {
        var command = new ProcessWithdrawalCommand { RequestId = Guid.NewGuid(), AdminId = Guid.NewGuid(), Action = "approve", MfaCode = "wrong" };
        var admin = CreateValidAdmin();
        var request = CreatePendingRequest(command.RequestId);
        _mockWithdrawalRepo.Setup(x => x.GetByIdAsync(command.RequestId, default)).ReturnsAsync(request);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.AdminId, default)).ReturnsAsync(admin);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", "wrong")).Returns(false);
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA", ex.Message);
    }
}
