

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Withdrawal;

// Unit test cho handler tạo yêu cầu rút tiền.
public class CreateWithdrawalCommandHandlerTests
{
    // Mock withdrawal repo để kiểm tra validate trùng ngày và lưu request.
    private readonly Mock<IWithdrawalRepository> _mockWithdrawalRepo;
    // Mock wallet repo để xác nhận debit kim cương.
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    // Mock user repo để điều khiển dữ liệu user rút tiền.
    private readonly Mock<IUserRepository> _mockUserRepo;
    // Mock MFA service để kiểm tra xác thực trước khi rút.
    private readonly Mock<IMfaService> _mockMfaService;
    // Mock transaction coordinator để chạy transactional flow inline.
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    // Mock publisher để xác nhận emit MoneyChangedDomainEvent.
    private readonly Mock<IDomainEventPublisher> _mockDomainEventPublisher;
    // Handler cần kiểm thử.
    private readonly CreateWithdrawalCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho CreateWithdrawalCommandHandler.
    /// Luồng setup coordinator inline giúp test deterministic và dễ assert side-effect.
    /// </summary>
    public CreateWithdrawalCommandHandlerTests()
    {
        _mockWithdrawalRepo = new Mock<IWithdrawalRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockDomainEventPublisher = new Mock<IDomainEventPublisher>();
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));
        _handler = new CreateWithdrawalCommandHandler(
            _mockWithdrawalRepo.Object, _mockWalletRepo.Object,
            _mockUserRepo.Object, _mockMfaService.Object, _mockTransactionCoordinator.Object, _mockDomainEventPublisher.Object);
    }

    /// <summary>
    /// Tạo user reader hợp lệ với MFA bật và số dư ví đủ.
    /// Luồng helper này giảm lặp setup user cho nhiều kịch bản rút tiền.
    /// </summary>
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

    /// <summary>
    /// Xác nhận amount nhỏ hơn 50 diamond bị từ chối.
    /// Luồng này bảo vệ rule mức rút tối thiểu.
    /// </summary>
    [Fact]
    public async Task Handle_LessThan50Diamond_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", It.IsAny<string>())).Returns(true);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 49, MfaCode = "123", IdempotencyKey = "test_key" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("tối thiểu là 50", ex.Message);
    }

    /// <summary>
    /// Xác nhận KYC chưa approved thì không cho tạo yêu cầu rút.
    /// Luồng này bảo vệ yêu cầu tuân thủ trước khi rút tiền.
    /// </summary>
    [Fact]
    public async Task Handle_KycNotApproved_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        user.GetType().GetProperty("ReaderStatus")!.SetValue(user, "pending");
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", It.IsAny<string>())).Returns(true);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "123", IdempotencyKey = "test_key" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("KYC", ex.Message);
    }

    /// <summary>
    /// Xác nhận cùng ngày đã có request pending thì không cho tạo thêm.
    /// Luồng này tránh spam nhiều yêu cầu rút trong một business day.
    /// </summary>
    [Fact]
    public async Task Handle_SecondRequestSameDay_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", It.IsAny<string>())).Returns(true);
        _mockWithdrawalRepo.Setup(x => x.HasPendingRequestTodayAsync(userId, It.IsAny<DateOnly>(), default)).ReturnsAsync(true);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "123", IdempotencyKey = "test_key" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("hôm nay", ex.Message);
    }

    /// <summary>
    /// Xác nhận request hợp lệ sẽ debit ví và lưu withdrawal request pending.
    /// Luồng này kiểm tra cả id trả về lẫn các trường fee/net amount.
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
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 100, MfaCode = "123", BankName = "VCB", BankAccountName = "Name", BankAccountNumber = "12345", IdempotencyKey = "test_key" };

        // Thực thi handler và xác nhận debit + persist request đúng dữ liệu.
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedId, result);
        _mockWalletRepo.Verify(x => x.DebitAsync(userId, "diamond", "withdrawal", 100, "withdrawal_request", It.IsAny<string>(), It.IsAny<string>(), null, "withdrawal_test_key", default), Times.Once);
        _mockDomainEventPublisher.Verify(
            x => x.PublishAsync(It.IsAny<MoneyChangedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockWithdrawalRepo.Verify(x => x.AddAsync(It.Is<WithdrawalRequest>(r => r.UserId == userId && r.AmountDiamond == 100 && r.NetAmountVnd == 90000 && r.FeeVnd == 10000 && r.Status == "pending"), default), Times.Once);
    }

    /// <summary>
    /// Xác nhận user không tồn tại trả NotFoundException.
    /// Luồng này chặn thao tác rút tiền với user id không hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        var command = new CreateWithdrawalCommand { UserId = Guid.NewGuid(), AmountDiamond = 50, MfaCode = "123", IdempotencyKey = "test_key" };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync((User)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận MFA chưa bật thì không cho rút tiền.
    /// Luồng này bảo vệ lớp xác thực thứ hai cho giao dịch nhạy cảm.
    /// </summary>
    [Fact]
    public async Task Handle_MfaNotEnabled_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        user.GetType().GetProperty("MfaEnabled")!.SetValue(user, false);
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "123", IdempotencyKey = "test_key" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA", ex.Message);
    }

    /// <summary>
    /// Xác nhận mã MFA sai thì yêu cầu rút bị từ chối.
    /// Luồng này đảm bảo xác thực MFA là điều kiện bắt buộc trước debit ví.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidMfaCode_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", "wrong_code")).Returns(false);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "wrong_code", IdempotencyKey = "test_key" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA", ex.Message);
    }

    /// <summary>
    /// Xác nhận số dư không đủ sẽ bị từ chối.
    /// Luồng này bảo vệ kiểm tra balance trước khi trừ ví.
    /// </summary>
    [Fact]
    public async Task Handle_InsufficientBalance_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        typeof(UserWallet).GetProperty("DiamondBalance")!.SetValue(user.Wallet, 30);
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", It.IsAny<string>())).Returns(true);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 100, MfaCode = "123", IdempotencyKey = "test_key" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Số dư", ex.Message);
    }

    /// <summary>
    /// Xác nhận thông tin ngân hàng rỗng bị từ chối.
    /// Luồng này đảm bảo dữ liệu chuyển khoản đầu ra luôn đầy đủ.
    /// </summary>
    [Fact]
    public async Task Handle_EmptyBankInfo_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateValidReader();
        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret(It.IsAny<string>())).Returns("secret");
        _mockMfaService.Setup(x => x.VerifyCode("secret", "123")).Returns(true);
        _mockWithdrawalRepo.Setup(x => x.HasPendingRequestTodayAsync(userId, It.IsAny<DateOnly>(), default)).ReturnsAsync(false);
        var command = new CreateWithdrawalCommand { UserId = userId, AmountDiamond = 50, MfaCode = "123", BankName = "", BankAccountName = "Name", BankAccountNumber = "12345", IdempotencyKey = "test_key" };
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("ngân hàng", ex.Message);
    }
}
