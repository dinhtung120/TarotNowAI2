

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.OpenDispute;

public class OpenDisputeCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Reason { get; set; } = string.Empty;
}

public class OpenDisputeCommandHandler : IRequestHandler<OpenDisputeCommand, bool>
{
    private const int MinReasonLength = 10;
    private static readonly TimeSpan DisputeWindowDuration = TimeSpan.FromHours(48);
    private const string SessionDisputedStatus = "disputed";

    private readonly IChatFinanceRepository _financeRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public OpenDisputeCommandHandler(
        IChatFinanceRepository financeRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<bool> Handle(OpenDisputeCommand req, CancellationToken ct)
    {
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await _financeRepo.GetItemForUpdateAsync(req.ItemId, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

            EnsureUserCanOpenDispute(item, req.UserId);
            if (item.Status == QuestionItemStatus.Disputed) return;
            EnsureItemCanOpenDispute(item.Status);
            EnsureReasonIsValid(req.Reason);

            var now = DateTime.UtcNow;
            item.Status = QuestionItemStatus.Disputed;
            item.AutoReleaseAt = null;
            item.DisputeWindowStart = now;
            item.DisputeWindowEnd = now.Add(DisputeWindowDuration);
            item.UpdatedAt = now;

            await _financeRepo.UpdateItemAsync(item, transactionCt);
            var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null) await UpdateSessionStatusAsync(session, transactionCt);

            await _financeRepo.SaveChangesAsync(transactionCt);
        }, ct);

        return true;
    }

    private static void EnsureUserCanOpenDispute(dynamic item, Guid userId)
    {
        if (item.PayerId == userId || item.ReceiverId == userId) return;
        throw new BadRequestException("Bạn không có quyền mở tranh chấp cho mục thanh toán này.");
    }

    private static void EnsureItemCanOpenDispute(string status)
    {
        if (status == QuestionItemStatus.Accepted) return;
        throw new BadRequestException($"Câu hỏi ở trạng thái {status}, không thể mở tranh chấp.");
    }

    private static void EnsureReasonIsValid(string reason)
    {
        if (!string.IsNullOrWhiteSpace(reason) && reason.Length >= MinReasonLength) return;
        throw new BadRequestException($"Lý do tranh chấp phải có ít nhất {MinReasonLength} ký tự.");
    }

    private async Task UpdateSessionStatusAsync(dynamic session, CancellationToken cancellationToken)
    {
        session.Status = SessionDisputedStatus;
        await _financeRepo.UpdateSessionAsync(session, cancellationToken);
    }
}
