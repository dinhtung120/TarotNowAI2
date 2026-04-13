

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Withdrawal;

// Unit test cho handler duyệt/từ chối yêu cầu rút tiền bởi admin.
public class ProcessWithdrawalCommandHandlerTests
{
    // Mock withdrawal repo để kiểm soát trạng thái request rút.
    private readonly Mock<IWithdrawalRepository> _mockWithdrawalRepo;
    // Mock wallet repo để kiểm tra nhánh refund khi reject.
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    // Mock user repo để lấy tài khoản admin xử lý.
    private readonly Mock<IUserRepository> _mockUserRepo;
    // Mock MFA service để xác thực admin.
    private readonly Mock<IMfaService> _mockMfaService;
    // Mock publisher để xác nhận phát domain event khi refund.
    private readonly Mock<IDomainEventPublisher> _mockDomainEventPublisher;
    // Handler cần kiểm thử.
    private readonly ProcessWithdrawalCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho ProcessWithdrawalCommandHandler.
    /// Luồng dùng mock dependencies để cô lập logic xử lý trạng thái withdrawal request.
    /// </summary>
    public ProcessWithdrawalCommandHandlerTests()
    {
        _mockWithdrawalRepo = new Mock<IWithdrawalRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();
        _mockDomainEventPublisher = new Mock<IDomainEventPublisher>();
        _handler = new ProcessWithdrawalCommandHandler(
            _mockWithdrawalRepo.Object, _mockWalletRepo.Object,
            _mockUserRepo.Object, _mockMfaService.Object, _mockDomainEventPublisher.Object);
    }

    /// <summary>
    /// Tạo user admin hợp lệ có bật MFA.
    /// Luồng helper này giảm lặp setup cho các test xử lý withdrawal.
    /// </summary>
    private User CreateValidAdmin()
    {
        var userType = typeof(User);
        var admin = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        userType.GetProperty("MfaEnabled")!.SetValue(admin, true);
        userType.GetProperty("MfaSecretEncrypted")!.SetValue(admin, "admin_secret");
        return admin;
    }

    /// <summary>
    /// Tạo request rút tiền pending mẫu.
    /// Luồng helper này chuẩn hóa dữ liệu nền để tập trung assert hành vi handler.
    /// </summary>
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

    /// <summary>
    /// Xác nhận approve cập nhật status và audit trail, không refund ví.
    /// Luồng này kiểm tra nhánh duyệt thành công của admin.
    /// </summary>
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

        // Thực thi approve và xác nhận request được cập nhật đúng trạng thái.
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("approved", request.Status);
        Assert.Equal(command.AdminId, request.AdminId);
        Assert.NotNull(request.ProcessedAt);
        _mockWalletRepo.Verify(x => x.CreditAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null, null, default), Times.Never);
        _mockDomainEventPublisher.Verify(
            x => x.PublishAsync(It.IsAny<MoneyChangedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Xác nhận reject sẽ refund diamond cho user và cập nhật trạng thái rejected.
    /// Luồng này kiểm tra side-effect tài chính khi từ chối yêu cầu rút.
    /// </summary>
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

        // Thực thi reject và assert refund được gọi đúng thông số.
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("rejected", request.Status);
        _mockWalletRepo.Verify(x => x.CreditAsync(request.UserId, CurrencyType.Diamond, TransactionType.WithdrawalRefund, 100, "withdrawal_request", request.Id.ToString(), It.IsAny<string>(), null, $"wd_refund_{request.Id}", default), Times.Once);
        _mockDomainEventPublisher.Verify(
            x => x.PublishAsync(
                It.Is<MoneyChangedDomainEvent>(eventData =>
                    eventData.UserId == request.UserId
                    && eventData.Currency == CurrencyType.Diamond
                    && eventData.ChangeType == TransactionType.WithdrawalRefund
                    && eventData.DeltaAmount == request.AmountDiamond),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Xác nhận request không tồn tại trả NotFoundException.
    /// Luồng này chặn thao tác duyệt/từ chối trên id không hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_RequestNotFound_ThrowsNotFoundException()
    {
        var command = new ProcessWithdrawalCommand { RequestId = Guid.NewGuid(), AdminId = Guid.NewGuid(), Action = "approve", MfaCode = "123" };
        _mockWithdrawalRepo.Setup(x => x.GetByIdAsync(command.RequestId, default)).ReturnsAsync((WithdrawalRequest)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận action không hợp lệ bị từ chối.
    /// Luồng này bảo vệ whitelist action approve/reject.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidAction_ThrowsBadRequest()
    {
        var command = new ProcessWithdrawalCommand { RequestId = Guid.NewGuid(), AdminId = Guid.NewGuid(), Action = "delete", MfaCode = "123" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận request đã xử lý trước đó không thể xử lý lại.
    /// Luồng này bảo vệ idempotency và audit trail của quy trình withdrawal.
    /// </summary>
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

    /// <summary>
    /// Xác nhận MFA admin sai sẽ bị từ chối.
    /// Luồng này bảo vệ thao tác xử lý rút tiền bằng xác thực lớp hai.
    /// </summary>
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
