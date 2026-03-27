/*
 * ===================================================================
 * FILE: AddQuestionCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Escrow.Commands.AddQuestion
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh cho phép Khách Hàng (User) "Gạn Hỏi Thêm" (Add Question) 
 *   sau khi đã thanh toán câu đầu tiên.
 *
 * BỐI CẢNH BIZ:
 *   Trong quá trình xem bài Tarot, người dùng thấy Reader bói đúng quá, 
 *   muốn hỏi thêm 1 câu phụ. Lúc này thay vì tạo 1 Box Chat mới, hệ thống
 *   sẽ tạo thêm 1 khoản tiền cọc (Question Item) nhét chung vào Phiên Giao Dịch 
 *   (Session) hiện tại.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.AddQuestion;

/// <summary>
/// Gói Lệnh: Bơm thêm tiền Cọc để hỏi thêm câu mới trong Session.
/// </summary>
public class AddQuestionCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    
    /// <summary>Mã Box Chat.</summary>
    public string ConversationRef { get; set; } = string.Empty;
    
    /// <summary>Số lượng Diamond sẽ bị Đóng băng thêm.</summary>
    public long AmountDiamond { get; set; }
    
    public string? ProposalMessageRef { get; set; }
    
    /// <summary>Chống kẹt nút, click đúp trừ x2 lần tiền.</summary>
    public string IdempotencyKey { get; set; } = string.Empty;
}

public partial class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Guid>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public AddQuestionCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<Guid> Handle(AddQuestionCommand req, CancellationToken ct)
    {
        var idempotencyKey = ValidateIdempotencyKey(req.IdempotencyKey);
        var existing = await _financeRepo.GetItemByIdempotencyKeyAsync(idempotencyKey, ct);
        if (existing != null)
        {
            return existing.Id;
        }

        return await ExecuteAddQuestionAsync(req, idempotencyKey, ct);
    }
}
