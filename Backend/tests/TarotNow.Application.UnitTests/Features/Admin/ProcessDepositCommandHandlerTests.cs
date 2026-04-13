

using Moq;
using TarotNow.Application.Features.Admin.Commands.ProcessDeposit;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Admin;

// Unit test cho luồng xử lý duyệt/từ chối deposit của admin.
public class ProcessDepositCommandHandlerTests
{
    // Mock repository deposit order để điều khiển trạng thái đơn nạp.
    private readonly Mock<IDepositOrderRepository> _mockDepositOrderRepository;
    // Mock wallet repository để kiểm tra side-effect cộng kim cương.
    private readonly Mock<IWalletRepository> _mockWalletRepository;
    // Mock domain event publisher để xác nhận có publish MoneyChanged event.
    private readonly Mock<IDomainEventPublisher> _mockDomainEventPublisher;
    // Handler cần kiểm thử.
    private readonly ProcessDepositCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho ProcessDepositCommandHandler.
    /// Luồng tiêm mock giúp kiểm thử logic xử lý mà không phụ thuộc DB thật.
    /// </summary>
    public ProcessDepositCommandHandlerTests()
    {
        _mockDepositOrderRepository = new Mock<IDepositOrderRepository>();
        _mockWalletRepository = new Mock<IWalletRepository>();
        _mockDomainEventPublisher = new Mock<IDomainEventPublisher>();
        _handler = new ProcessDepositCommandHandler(
            _mockDepositOrderRepository.Object,
            _mockWalletRepository.Object,
            _mockDomainEventPublisher.Object);
    }

    /// <summary>
    /// Xác nhận approve với transactionId rỗng sẽ tự sinh mã thủ công.
    /// Luồng này đảm bảo business rule luôn lưu transactionId hợp lệ khi admin duyệt tay.
    /// </summary>
    [Fact]
    public async Task Handle_Approve_WithWhitespaceTransactionId_GeneratesManualTransactionId()
    {
        var userId = Guid.NewGuid();
        var order = new DepositOrder(userId, amountVnd: 100_000, diamondAmount: 100);

        // Setup đầy đủ luồng load đơn, update đơn và credit ví.
        _mockDepositOrderRepository
            .Setup(x => x.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _mockDepositOrderRepository
            .Setup(x => x.UpdateAsync(order, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockWalletRepository
            .Setup(x => x.CreditAsync(
                userId, CurrencyType.Diamond, TransactionType.Deposit,
                order.DiamondAmount, "DepositOrder", order.Id.ToString(),
                It.IsAny<string>(), null, $"deposit_approve_{order.Id}",
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Truyền transactionId whitespace để ép handler vào nhánh auto-generate.
        var result = await _handler.Handle(new ProcessDepositCommand
        {
            DepositId = order.Id,
            Action = "approve",
            TransactionId = "   "
        }, CancellationToken.None);

        // Kỳ vọng trạng thái success và transactionId được sinh đúng prefix.
        Assert.True(result);
        Assert.Equal("Success", order.Status);
        Assert.False(string.IsNullOrWhiteSpace(order.TransactionId));
        Assert.StartsWith("ADMIN_APPROVE_", order.TransactionId);
        _mockWalletRepository.Verify(x => x.CreditAsync(
            userId, CurrencyType.Diamond, TransactionType.Deposit,
            order.DiamondAmount, "DepositOrder", order.Id.ToString(),
            It.IsAny<string>(), null, $"deposit_approve_{order.Id}",
            It.IsAny<CancellationToken>()), Times.Once);
        _mockDomainEventPublisher.Verify(x => x.PublishAsync(
            It.IsAny<TarotNow.Domain.Events.MoneyChangedDomainEvent>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Xác nhận approve với transactionId có khoảng trắng sẽ được trim đúng.
    /// Luồng này kiểm tra handler không làm mất transactionId do admin cung cấp.
    /// </summary>
    [Fact]
    public async Task Handle_Approve_WithProvidedTransactionId_UsesTrimmedValue()
    {
        var order = new DepositOrder(Guid.NewGuid(), amountVnd: 100_000, diamondAmount: 100);

        _mockDepositOrderRepository
            .Setup(x => x.GetByIdAsync(order.Id, It.IsAny<CancellationToken>())).ReturnsAsync(order);
        _mockDepositOrderRepository
            .Setup(x => x.UpdateAsync(order, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _mockWalletRepository
            .Setup(x => x.CreditAsync(
                order.UserId, CurrencyType.Diamond, TransactionType.Deposit,
                order.DiamondAmount, "DepositOrder", order.Id.ToString(),
                It.IsAny<string>(), null, $"deposit_approve_{order.Id}",
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Truyền transactionId có khoảng trắng đầu/cuối để test normalize.
        await _handler.Handle(new ProcessDepositCommand
        {
            DepositId = order.Id,
            Action = "approve",
            TransactionId = "  TXN-123  "
        }, CancellationToken.None);

        Assert.Equal("Success", order.Status);
        Assert.Equal("TXN-123", order.TransactionId);
        _mockDomainEventPublisher.Verify(x => x.PublishAsync(
            It.IsAny<TarotNow.Domain.Events.MoneyChangedDomainEvent>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Xác nhận reject với transactionId rỗng sẽ tự sinh mã và không cộng ví.
    /// Luồng này bảo vệ nhánh từ chối không phát sinh side-effect tài chính sai.
    /// </summary>
    [Fact]
    public async Task Handle_Reject_WithWhitespaceTransactionId_GeneratesManualTransactionId()
    {
        var order = new DepositOrder(Guid.NewGuid(), amountVnd: 100_000, diamondAmount: 100);

        _mockDepositOrderRepository
            .Setup(x => x.GetByIdAsync(order.Id, It.IsAny<CancellationToken>())).ReturnsAsync(order);
        _mockDepositOrderRepository
            .Setup(x => x.UpdateAsync(order, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(new ProcessDepositCommand
        {
            DepositId = order.Id,
            Action = "reject",
            TransactionId = ""
        }, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("Failed", order.Status);
        Assert.False(string.IsNullOrWhiteSpace(order.TransactionId));
        Assert.StartsWith("ADMIN_REJECT_", order.TransactionId);

        // Reject không được credit ví trong bất kỳ trường hợp nào.
        _mockWalletRepository.Verify(x => x.CreditAsync(
            It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Never);
        _mockDomainEventPublisher.Verify(x => x.PublishAsync(
            It.IsAny<TarotNow.Domain.Events.MoneyChangedDomainEvent>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}
