using System.Text.Json;
using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public class ResolveDisputeCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }
    public Guid AdminId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? AdminNote { get; set; }
}

public class ResolveDisputeCommandHandler : IRequestHandler<ResolveDisputeCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public ResolveDisputeCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<bool> Handle(ResolveDisputeCommand request, CancellationToken cancellationToken)
    {
        var action = request.Action?.Trim().ToLowerInvariant();
        if (action != "release" && action != "refund")
            throw new BadRequestException("Action phải là 'release' hoặc 'refund'.");

        var auditMetadata = BuildResolveAuditMetadata(request.AdminId, action, request.AdminNote);

        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await _financeRepo.GetItemForUpdateAsync(request.ItemId, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy câu hỏi.");
            if (item.Status != QuestionItemStatus.Disputed)
                throw new BadRequestException("Câu hỏi không ở trạng thái dispute.");
            if (item.ReleasedAt != null || item.RefundedAt != null)
                throw new BadRequestException("Dispute này đã được settle trước đó.");

            if (action == "release")
            {
                var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);
                var readerAmount = item.AmountDiamond - fee;

                await _walletRepo.ReleaseAsync(
                    item.PayerId, item.ReceiverId, readerAmount,
                    referenceSource: "admin_dispute_resolve",
                    referenceId: item.Id.ToString(),
                    description: $"Admin {request.AdminId} resolve: release {readerAmount}💎",
                    metadataJson: auditMetadata,
                    idempotencyKey: $"settle_release_{item.Id}",
                    cancellationToken: transactionCt);

                if (fee > 0)
                {
                    await _walletRepo.ConsumeAsync(
                        item.PayerId, fee,
                        referenceSource: "platform_fee",
                        referenceId: item.Id.ToString(),
                        description: $"Admin {request.AdminId} settle fee {fee}💎",
                        metadataJson: auditMetadata,
                        idempotencyKey: $"settle_fee_{item.Id}",
                        cancellationToken: transactionCt);
                }

                item.Status = QuestionItemStatus.Released;
                item.ReleasedAt = DateTime.UtcNow;
            }
            else
            {
                await _walletRepo.RefundAsync(
                    item.PayerId, item.AmountDiamond,
                    referenceSource: "admin_dispute_resolve",
                    referenceId: item.Id.ToString(),
                    description: $"Admin {request.AdminId} resolve: refund {item.AmountDiamond}💎",
                    metadataJson: auditMetadata,
                    idempotencyKey: $"settle_refund_{item.Id}",
                    cancellationToken: transactionCt);

                item.Status = QuestionItemStatus.Refunded;
                item.RefundedAt = DateTime.UtcNow;
            }

            await _financeRepo.UpdateItemAsync(item, transactionCt);

            var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null)
            {
                session.TotalFrozen -= item.AmountDiamond;
                if (session.TotalFrozen < 0) session.TotalFrozen = 0;
                await _financeRepo.UpdateSessionAsync(session, transactionCt);
            }

            await _financeRepo.SaveChangesAsync(transactionCt);
        }, cancellationToken);

        return true;
    }

    private static string BuildResolveAuditMetadata(Guid adminId, string action, string? adminNote)
    {
        var normalizedAdminNote = string.IsNullOrWhiteSpace(adminNote)
            ? null
            : adminNote.Trim();

        if (normalizedAdminNote is { Length: > 500 })
        {
            normalizedAdminNote = normalizedAdminNote[..500];
        }

        return JsonSerializer.Serialize(new
        {
            adminId,
            action,
            adminNote = normalizedAdminNote
        });
    }
}
