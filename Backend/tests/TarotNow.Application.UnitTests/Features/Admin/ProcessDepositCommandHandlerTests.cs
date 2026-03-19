/*
 * FILE: ProcessDepositCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler Admin xử lý đơn nạp tiền (approve/reject).
 *
 *   CÁC TEST CASE:
 *   1. Handle_Approve_WithWhitespaceTransactionId_GeneratesManualTransactionId:
 *      → Admin approve nhưng không nhập transactionId → tự sinh "ADMIN_APPROVE_..."
 *   2. Handle_Approve_WithProvidedTransactionId_UsesTrimmedValue:
 *      → Admin nhập "  TXN-123  " → trim → "TXN-123"
 *   3. Handle_Reject_WithWhitespaceTransactionId_GeneratesManualTransactionId:
 *      → Admin reject → status="Failed", transactionId="ADMIN_REJECT_...", KHÔNG credit
 *
 *   KIỂM TRA:
 *   → Approve: Credit Diamond vào ví User + đánh dấu order "Success"
 *   → Reject: KHÔNG credit + đánh dấu order "Failed"
 *   → TransactionId auto-generate khi Admin không nhập (whitespace/empty)
 */

using Moq;
using TarotNow.Application.Features.Admin.Commands.ProcessDeposit;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Admin;

/// <summary>
/// Test Admin approve/reject deposit: credit ví, transactionId auto-generate.
/// </summary>
public class ProcessDepositCommandHandlerTests
{
    private readonly Mock<IDepositOrderRepository> _mockDepositOrderRepository;
    private readonly Mock<IWalletRepository> _mockWalletRepository;
    private readonly ProcessDepositCommandHandler _handler;

    public ProcessDepositCommandHandlerTests()
    {
        _mockDepositOrderRepository = new Mock<IDepositOrderRepository>();
        _mockWalletRepository = new Mock<IWalletRepository>();
        _handler = new ProcessDepositCommandHandler(_mockDepositOrderRepository.Object, _mockWalletRepository.Object);
    }

    /// <summary>
    /// Approve + transactionId trống → tự sinh "ADMIN_APPROVE_{orderId}".
    /// Verify: status=Success, CreditAsync gọi 1 lần.
    /// </summary>
    [Fact]
    public async Task Handle_Approve_WithWhitespaceTransactionId_GeneratesManualTransactionId()
    {
        var userId = Guid.NewGuid();
        var order = new DepositOrder(userId, amountVnd: 100_000, diamondAmount: 100);

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

        var result = await _handler.Handle(new ProcessDepositCommand
        {
            DepositId = order.Id,
            Action = "approve",
            TransactionId = "   " // Trống → auto-generate
        }, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("Success", order.Status);
        Assert.False(string.IsNullOrWhiteSpace(order.TransactionId));
        Assert.StartsWith("ADMIN_APPROVE_", order.TransactionId);
        _mockWalletRepository.Verify(x => x.CreditAsync(
            userId, CurrencyType.Diamond, TransactionType.Deposit,
            order.DiamondAmount, "DepositOrder", order.Id.ToString(),
            It.IsAny<string>(), null, $"deposit_approve_{order.Id}",
            It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Approve + transactionId có giá trị → trim whitespace + dùng nguyên.
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

        await _handler.Handle(new ProcessDepositCommand
        {
            DepositId = order.Id,
            Action = "approve",
            TransactionId = "  TXN-123  " // Có khoảng trắng → trim
        }, CancellationToken.None);

        Assert.Equal("Success", order.Status);
        Assert.Equal("TXN-123", order.TransactionId); // Đã trim
    }

    /// <summary>
    /// Reject → status="Failed", transactionId="ADMIN_REJECT_...", KHÔNG credit ví.
    /// Verify: CreditAsync KHÔNG được gọi.
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
            TransactionId = "" // Trống → auto-generate
        }, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("Failed", order.Status);
        Assert.False(string.IsNullOrWhiteSpace(order.TransactionId));
        Assert.StartsWith("ADMIN_REJECT_", order.TransactionId);
        // KHÔNG credit ví khi reject
        _mockWalletRepository.Verify(x => x.CreditAsync(
            It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}
