

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

            if (item.PayerId != req.UserId && item.ReceiverId != req.UserId)
                throw new BadRequestException("Bạn không có quyền mở tranh chấp cho mục thanh toán này.");

            if (item.Status == QuestionItemStatus.Disputed)
            {
                return;
            }

            if (item.Status != QuestionItemStatus.Accepted)
                throw new BadRequestException($"Câu hỏi ở trạng thái {item.Status}, không thể mở tranh chấp.");

            
            if (string.IsNullOrWhiteSpace(req.Reason) || req.Reason.Length < 10)
                throw new BadRequestException("Lý do tranh chấp phải có ít nhất 10 ký tự.");

            var now = DateTime.UtcNow;

            
            
            
            
            item.Status = QuestionItemStatus.Disputed;
            item.AutoReleaseAt = null; 
            item.DisputeWindowStart = now;
            item.DisputeWindowEnd = now.AddHours(48);
            item.UpdatedAt = now;

            await _financeRepo.UpdateItemAsync(item, transactionCt);

            
            var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null)
            {
                session.Status = "disputed";
                await _financeRepo.UpdateSessionAsync(session, transactionCt);
            }

            
            await _financeRepo.SaveChangesAsync(transactionCt);
            
        }, ct);

        return true;
    }
}
