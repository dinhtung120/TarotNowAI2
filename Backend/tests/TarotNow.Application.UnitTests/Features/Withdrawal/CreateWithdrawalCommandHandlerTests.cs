using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Withdrawal;

public class CreateWithdrawalCommandHandlerTests
{
    private readonly Mock<IWithdrawalRepository> _mockWithdrawalRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IMfaService> _mockMfaService;
    private readonly CreateWithdrawalCommandHandler _handler;

    public CreateWithdrawalCommandHandlerTests()
    {
        _mockWithdrawalRepo = new Mock<IWithdrawalRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();

        _handler = new CreateWithdrawalCommandHandler(
            _mockWithdrawalRepo.Object,
            _mockWalletRepo.Object,
            _mockUserRepo.Object,
            _mockMfaService.Object);
    }

    private User CreateValidReader()
    {
        var userType = typeof(User);
        var user = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        
        // MfaEnabled and MfaSecretEncrypted are properties
        userType.GetProperty("MfaEnabled")!.SetValue(user, true);
        userType.GetProperty("MfaSecretEncrypted")!.SetValue(user, "encrypted_secret");
        
        // Use reflection to set properties with private setters
        userType.GetProperty("ReaderStatus")!.DeclaringType!.GetProperty("ReaderStatus")!.SetValue(user, "approved", null);

        // Map Wallet for DiamondBalance
        var walletType = typeof(UserWallet);
        var wallet = Activator.CreateInstance(walletType, nonPublic: true)!;
        walletType.GetProperty("DiamondBalance")!.DeclaringType!.GetProperty("DiamondBalance")!.SetValue(wallet, 100, null);
        userType.GetProperty("Wallet")!.DeclaringType!.GetProperty("Wallet")!.SetValue(user, wallet, null);
        
        return user;
    }

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

        var command = new CreateWithdrawalCommand 
        { 
            UserId = userId, 
            AmountDiamond = 100, 
            MfaCode = "123",
            BankName = "VCB",
            BankAccountName = "Name",
            BankAccountNumber = "12345"
        };
        
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedId, result);

        // Verify debit diamond with amount 100
        _mockWalletRepo.Verify(x => x.DebitAsync(userId, "diamond", "withdrawal", 100, "withdrawal_request", null, It.IsAny<string>(), null, null, default), Times.Once);

        // Verify save
        _mockWithdrawalRepo.Verify(x => x.AddAsync(It.Is<WithdrawalRequest>(r => 
            r.UserId == userId && 
            r.AmountDiamond == 100 && 
            r.NetAmountVnd == 90000 && // 100*1000 - 10%
            r.FeeVnd == 10000 &&
            r.Status == "pending"), default), Times.Once);
        
        _mockWithdrawalRepo.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    /// <summary>
    /// TEST CASE: User không tồn tại → NotFoundException.
    ///
    /// Khi nào xảy ra?
    /// → UserId sai hoặc user đã bị xóa khỏi hệ thống.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new CreateWithdrawalCommand { UserId = Guid.NewGuid(), AmountDiamond = 50, MfaCode = "123" };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: MFA chưa bật → BadRequestException.
    ///
    /// Tại sao cần MFA cho withdrawal?
    /// → BR Phase 2.5: Reader và Admin BẮT BUỘC MFA trước payout.
    ///   Bảo vệ khỏi trường hợp tài khoản bị hack → attacker không thể rút tiền.
    /// </summary>
    [Fact]
    public async Task Handle_MfaNotEnabled_ThrowsBadRequest()
    {
        // Arrange — user có MFA disabled
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        user.GetType().GetProperty("MfaEnabled")!.SetValue(user, false);

        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);

        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "123" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA", ex.Message);
    }

    /// <summary>
    /// TEST CASE: MFA code không hợp lệ (sai hoặc hết hạn) → BadRequestException.
    ///
    /// Tại sao test riêng?
    /// → TOTP code thay đổi mỗi 30 giây. Nếu code hết hạn hoặc sai → reject.
    ///   Brute-force protection: không cho thử code nhiều lần.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidMfaCode_ThrowsBadRequest()
    {
        // Arrange — MFA enabled nhưng code sai
        var userId = Guid.NewGuid();
        var user = CreateValidReader();

        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", "wrong_code")).Returns(false); // Code sai!

        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "wrong_code" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA", ex.Message);
    }

    /// <summary>
    /// TEST CASE: Số dư Diamond không đủ → BadRequestException.
    ///
    /// Tại sao quan trọng?
    /// → Handler phải check balance TRƯỚC khi gọi DebitAsync.
    ///   Nếu không: stored proc sẽ throw DB exception (xấu hơn).
    ///   Guard 3 trong handler (line 73-74).
    /// </summary>
    [Fact]
    public async Task Handle_InsufficientBalance_ThrowsBadRequest()
    {
        // Arrange — user chỉ có 30 Diamond nhưng muốn rút 100
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        // Set DiamondBalance = 30 (qua User.Wallet.DiamondBalance)
        var walletType = typeof(UserWallet);
        var wallet = user.Wallet;
        walletType.GetProperty("DiamondBalance")!.SetValue(wallet, 30);

        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", It.IsAny<string>())).Returns(true);

        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 100, MfaCode = "123" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Số dư", ex.Message);
    }

    /// <summary>
    /// TEST CASE: Thông tin ngân hàng trống → BadRequestException.
    ///
    /// Tại sao cần validate?
    /// → Admin cần bank info để chuyển khoản.
    ///   Nếu bank info trống → admin không thể xử lý payout.
    /// </summary>
    [Fact]
    public async Task Handle_EmptyBankInfo_ThrowsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = CreateValidReader();

        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", "123")).Returns(true);
        _mockWithdrawalRepo.Setup(x => x.HasPendingRequestTodayAsync(userId, It.IsAny<DateOnly>(), default)).ReturnsAsync(false);

        var command = new CreateWithdrawalCommand
        {
            UserId = userId,
            AmountDiamond = 50,
            MfaCode = "123",
            BankName = "", // Trống!
            BankAccountName = "Name",
            BankAccountNumber = "12345"
        };
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("ngân hàng", ex.Message);
    }
}
