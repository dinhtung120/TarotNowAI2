/*
 * FILE: CreateWithdrawalCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler tạo yêu cầu rút tiền (Withdrawal Request).
 *
 *   CÁC TEST CASE (8 scenarios):
 *   1. Handle_LessThan50Diamond_ThrowsBadRequest: số lượng < 50 → 400
 *   2. Handle_KycNotApproved_ThrowsBadRequest: chưa approve KYC → 400
 *   3. Handle_SecondRequestSameDay_ThrowsBadRequest: đã rút hôm nay → 400 (1 lần/ngày)
 *   4. Handle_ValidRequest_DebitsDiamondAndSavesRequest:
 *      → Debit Diamond + tính fee 10% + lưu request pending
 *   5. Handle_UserNotFound_ThrowsNotFoundException: userId sai → 404
 *   6. Handle_MfaNotEnabled_ThrowsBadRequest: chưa bật MFA → 400
 *   7. Handle_InvalidMfaCode_ThrowsBadRequest: TOTP code sai → 400
 *   8. Handle_InsufficientBalance_ThrowsBadRequest: số dư không đủ → 400
 *   9. Handle_EmptyBankInfo_ThrowsBadRequest: thông tin ngân hàng trống → 400
 *
 *   FEE: NetAmountVnd = (amount * 1000) - 10%, FeeVnd = 10%
 *   BẢO MẬT: MFA bắt buộc trước khi rút tiền (BR Phase 2.5)
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Withdrawal;

/// <summary>
/// Test create withdrawal: MFA, KYC, balance, fee calculation, daily limit.
/// </summary>
public class CreateWithdrawalCommandHandlerTests
{
    private readonly Mock<IWithdrawalRepository> _mockWithdrawalRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IMfaService> _mockMfaService;
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly CreateWithdrawalCommandHandler _handler;

    public CreateWithdrawalCommandHandlerTests()
    {
        _mockWithdrawalRepo = new Mock<IWithdrawalRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));
        _handler = new CreateWithdrawalCommandHandler(
            _mockWithdrawalRepo.Object, _mockWalletRepo.Object,
            _mockUserRepo.Object, _mockMfaService.Object, _mockTransactionCoordinator.Object);
    }

    /* Helper: tạo Reader hợp lệ (MFA bật, KYC approved, balance 100) */
    private User CreateValidReader()
    {
        var userType = typeof(User);
        var user = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        userType.GetProperty("MfaEnabled")!.SetValue(user, true);
        userType.GetProperty("MfaSecretEncrypted")!.SetValue(user, "encrypted_secret");
        userType.GetProperty("ReaderStatus")!.DeclaringType!.GetProperty("ReaderStatus")!.SetValue(user, "approved", null);
        var walletType = typeof(UserWallet);
        var wallet = Activator.CreateInstance(walletType, nonPublic: true)!;
        walletType.GetProperty("DiamondBalance")!.DeclaringType!.GetProperty("DiamondBalance")!.SetValue(wallet, 100, null);
        userType.GetProperty("Wallet")!.DeclaringType!.GetProperty("Wallet")!.SetValue(user, wallet, null);
        return user;
    }

    /// <summary>Số lượng < 50 → BadRequest (tối thiểu 50 Diamond).</summary>
    [Fact]
    public async Task Handle_LessThan50Diamond_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", It.IsAny<string>())).Returns(true);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 49, MfaCode = "123" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("tối thiểu là 50", ex.Message);
    }

    /// <summary>KYC chưa approved → BadRequest.</summary>
    [Fact]
    public async Task Handle_KycNotApproved_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        user.GetType().GetProperty("ReaderStatus")!.SetValue(user, "pending");
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", It.IsAny<string>())).Returns(true);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "123" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("KYC", ex.Message);
    }

    /// <summary>Đã rút hôm nay → BadRequest (1 lần/ngày).</summary>
    [Fact]
    public async Task Handle_SecondRequestSameDay_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", It.IsAny<string>())).Returns(true);
        _mockWithdrawalRepo.Setup(x => x.HasPendingRequestTodayAsync(userId, It.IsAny<DateOnly>(), default)).ReturnsAsync(true);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "123" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("hôm nay", ex.Message);
    }

    /// <summary>
    /// Happy path: Debit 100 Diamond, NetAmountVnd=90000, FeeVnd=10000.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_DebitsDiamondAndSavesRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", "123")).Returns(true);
        _mockWithdrawalRepo.Setup(x => x.HasPendingRequestTodayAsync(userId, It.IsAny<DateOnly>(), default)).ReturnsAsync(false);
        var expectedId = Guid.NewGuid();
        _mockWithdrawalRepo.Setup(x => x.AddAsync(It.IsAny<WithdrawalRequest>(), default))
            .Callback<WithdrawalRequest, CancellationToken>((r, c) => typeof(WithdrawalRequest).GetProperty("Id")!.SetValue(r, expectedId))
            .Returns(Task.CompletedTask);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 100, MfaCode = "123", BankName = "VCB", BankAccountName = "Name", BankAccountNumber = "12345" };

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedId, result);
        _mockWalletRepo.Verify(x => x.DebitAsync(userId, "diamond", "withdrawal", 100, "withdrawal_request", null, It.IsAny<string>(), null, null, default), Times.Once);
        _mockWithdrawalRepo.Verify(x => x.AddAsync(It.Is<WithdrawalRequest>(r => r.UserId == userId && r.AmountDiamond == 100 && r.NetAmountVnd == 90000 && r.FeeVnd == 10000 && r.Status == "pending"), default), Times.Once);
    }

    /// <summary>UserId sai → NotFoundException.</summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        var command = new CreateWithdrawalCommand { UserId = Guid.NewGuid(), AmountDiamond = 50, MfaCode = "123" };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync((User)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>MFA chưa bật → BadRequest (bắt buộc cho withdrawal).</summary>
    [Fact]
    public async Task Handle_MfaNotEnabled_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        user.GetType().GetProperty("MfaEnabled")!.SetValue(user, false);
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "123" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA", ex.Message);
    }

    /// <summary>TOTP code sai → BadRequest.</summary>
    [Fact]
    public async Task Handle_InvalidMfaCode_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", "wrong_code")).Returns(false);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "wrong_code" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA", ex.Message);
    }

    /// <summary>Số dư không đủ → BadRequest.</summary>
    [Fact]
    public async Task Handle_InsufficientBalance_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        typeof(UserWallet).GetProperty("DiamondBalance")!.SetValue(user.Wallet, 30);
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", It.IsAny<string>())).Returns(true);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 100, MfaCode = "123" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Số dư", ex.Message);
    }

    /// <summary>Thông tin ngân hàng trống → BadRequest.</summary>
    [Fact]
    public async Task Handle_EmptyBankInfo_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", "123")).Returns(true);
        _mockWithdrawalRepo.Setup(x => x.HasPendingRequestTodayAsync(userId, It.IsAny<DateOnly>(), default)).ReturnsAsync(false);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "123", BankName = "", BankAccountName = "Name", BankAccountNumber = "12345" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("ngân hàng", ex.Message);
    }
}
