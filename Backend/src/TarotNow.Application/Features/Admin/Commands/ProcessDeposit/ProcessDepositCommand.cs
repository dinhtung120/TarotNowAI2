

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

        public string Action { get; set; } = "approve";

        public string? TransactionId { get; set; }
}

public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, bool>
{
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

        
        if (order == null || order.Status != "Pending") return false;

        var action = request.Action?.Trim().ToLowerInvariant();
        if (action != "approve" && action != "reject")
        {
            throw new BadRequestException("Action phải là 'approve' hoặc 'reject'.");
        }

        
        var txnId = ResolveTransactionId(request.TransactionId, action);

        if (action == "approve")
        {
            

            
            await _walletRepository.CreditAsync(
                userId: order.UserId,
                currency: CurrencyType.Diamond,
                type: TransactionType.Deposit,
                amount: order.DiamondAmount,            
                referenceSource: "DepositOrder",
                referenceId: order.Id.ToString(),
                description: $"Approved deposit order {order.Id} (+{order.DiamondAmount} Diamond)",
                idempotencyKey: $"deposit_approve_{order.Id}", 
                cancellationToken: cancellationToken
            );

            
            order.MarkAsSuccess(txnId, "Admin Manual Approval");
        }
        else
        {
            
            order.MarkAsFailed(txnId);
        }

        
        await _depositOrderRepository.UpdateAsync(order, cancellationToken);

        return true;
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
