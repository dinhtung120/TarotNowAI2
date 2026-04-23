

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.AddQuestion;

// Command thêm câu hỏi phát sinh trong phiên chat và freeze kim cương tương ứng.
public class AddQuestionCommand : IRequest<Guid>
{
    // Định danh user trả tiền cho câu hỏi.
    public Guid UserId { get; set; }

    // Định danh conversation/session tham chiếu.
    public string ConversationRef { get; set; } = string.Empty;

    // Số kim cương cần freeze cho câu hỏi.
    public long AmountDiamond { get; set; }

    // Message đề xuất liên quan (nếu có).
    public string? ProposalMessageRef { get; set; }

    // Idempotency key để chống tạo question item trùng.
    public string IdempotencyKey { get; set; } = string.Empty;
}

// Handler điều phối luồng add-question.
public partial class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Guid>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler add question.
    /// Luồng xử lý: nhận finance repo, wallet repo và transaction coordinator cho luồng freeze + ghi item nhất quán.
    /// </summary>
    public AddQuestionCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator,
        IDomainEventPublisher domainEventPublisher,
        ISystemConfigSettings systemConfigSettings)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
        _domainEventPublisher = domainEventPublisher;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Xử lý command add question.
    /// Luồng xử lý: validate idempotency key, kiểm tra item đã tồn tại theo key, nếu chưa có thì thực thi workflow tạo item trong transaction.
    /// </summary>
    public async Task<Guid> Handle(AddQuestionCommand req, CancellationToken ct)
    {
        var idempotencyKey = ValidateIdempotencyKey(req.IdempotencyKey);
        var existing = await _financeRepo.GetItemByIdempotencyKeyAsync(idempotencyKey, ct);
        if (existing != null)
        {
            // Idempotency: trả item đã tạo trước đó nếu client gửi lại cùng key.
            return existing.Id;
        }

        return await ExecuteAddQuestionAsync(req, idempotencyKey, ct);
    }
}
