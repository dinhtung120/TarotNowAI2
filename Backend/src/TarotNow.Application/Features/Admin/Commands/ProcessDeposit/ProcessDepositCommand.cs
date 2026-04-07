

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Admin.Commands.ProcessDeposit;

public class ProcessDepositCommand : IRequest<bool>
{
    public Guid DepositId { get; set; }

    public string Action { get; set; } = ApproveAction;
    public string? TransactionId { get; set; }

    internal const string ApproveAction = "approve";
    internal const string RejectAction = "reject";
}

public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, bool>
{
    private const string PendingStatus = "Pending";
    private const string DepositReferenceSource = "DepositOrder";
    private const string ManualApprovalNote = "Admin Manual Approval";
    private const string InvalidActionMessage = "Action phải là 'approve' hoặc 'reject'.";

    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IWalletRepository _walletRepository;

    public ProcessDepositCommandHandler(
        IDepositOrderRepository depositOrderRepository,
        IWalletRepository walletRepository)
    {
        _depositOrderRepository = depositOrderRepository;
        _walletRepository = walletRepository;
    }

    public async Task<bool> Handle(ProcessDepositCommand request, CancellationToken cancellationToken)
    {
        var order = await _depositOrderRepository.GetByIdAsync(request.DepositId, cancellationToken);
        if (order == null || order.Status != PendingStatus) return false;

        var action = ValidateAndNormalizeAction(request.Action);
        var txnId = ResolveTransactionId(request.TransactionId, action);

        if (action == ProcessDepositCommand.ApproveAction)
        {
            await ApproveOrderAsync(order, txnId, cancellationToken);
            await _depositOrderRepository.UpdateAsync(order, cancellationToken);
            return true;
        }

        order.MarkAsFailed(txnId);
        await _depositOrderRepository.UpdateAsync(order, cancellationToken);
        return true;
    }

    private static string ValidateAndNormalizeAction(string? action)
    {
        var normalizedAction = action?.Trim().ToLowerInvariant();
        if (normalizedAction == ProcessDepositCommand.ApproveAction ||
            normalizedAction == ProcessDepositCommand.RejectAction)
        {
            return normalizedAction;
        }

        throw new BadRequestException(InvalidActionMessage);
    }

    private async Task ApproveOrderAsync(
        Domain.Entities.DepositOrder order,
        string transactionId,
        CancellationToken cancellationToken)
    {
        await _walletRepository.CreditAsync(
            userId: order.UserId,
            currency: CurrencyType.Diamond,
            type: TransactionType.Deposit,
            amount: order.DiamondAmount,
            referenceSource: DepositReferenceSource,
            referenceId: order.Id.ToString(),
            description: $"Approved deposit order {order.Id} (+{order.DiamondAmount} Diamond)",
            idempotencyKey: $"deposit_approve_{order.Id}",
            cancellationToken: cancellationToken);

        order.MarkAsSuccess(transactionId, ManualApprovalNote);
    }

    private static string ResolveTransactionId(string? requestedTransactionId, string action)
    {
        if (!string.IsNullOrWhiteSpace(requestedTransactionId))
        {
            return requestedTransactionId.Trim();
        }

        return $"ADMIN_{action.ToUpperInvariant()}_{Guid.CreateVersion7():N}";
    }
}
