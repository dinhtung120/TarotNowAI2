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

public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Guid>
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
        var idempotencyKey = req.IdempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new BadRequestException("IdempotencyKey là bắt buộc.");

        if (idempotencyKey.Length > 128)
            throw new BadRequestException("IdempotencyKey quá dài (tối đa 128 ký tự).");

        // 1. Quét khiên chặn Lũy đẳng (Idempotency) - Xem đã thao tác cục tiền này chưa
        var existing = await _financeRepo.GetItemByIdempotencyKeyAsync(idempotencyKey, ct);
        if (existing != null) return existing.Id;

        Guid createdItemId = Guid.Empty;
        
        // 2. ACID Transaction: Bắt đầu giao dịch nguyên tử (Không được đứt gánh giữa chừng)
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            // 3. Truy xuất Phiên Giao Dịch Phụ Huynh (Session)
            var session = await _financeRepo.GetSessionByConversationRefAsync(req.ConversationRef, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy phiên trò chuyện.");

            // Chỉ người Bấm nút Gốc mới có quyền mở mồm hỏi thêm
            if (session.UserId != req.UserId)
                throw new BadRequestException("Bạn không phải chủ phiên.");

            // Chỉ khi Box Chat còn Tồn tại / Đang chạy mới được phép bơm thêm tiền
            if (session.Status != "active" && session.Status != "pending")
                throw new BadRequestException("Phiên đã kết thúc, không thể thêm câu hỏi.");

            // 4. Ký quỹ Đóng băng tiền trong ví Database
            await _walletRepo.FreezeAsync(
                req.UserId, req.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: idempotencyKey,
                description: $"Escrow add-question {req.AmountDiamond}💎",
                idempotencyKey: $"freeze_{idempotencyKey}",
                cancellationToken: transactionCt);

            var now = DateTime.UtcNow;
            
            // 5. Nặn ra hóa đơn con (Lưu là AddQuestion chứ không phải MainQuestion)
            var item = new ChatQuestionItem
            {
                FinanceSessionId = session.Id,
                ConversationRef = req.ConversationRef,
                PayerId = req.UserId,
                ReceiverId = session.ReaderId,
                Type = QuestionItemType.AddQuestion, // <--- Loại hình câu hỏi Phụ.
                AmountDiamond = req.AmountDiamond,
                Status = QuestionItemStatus.Accepted,
                ProposalMessageRef = req.ProposalMessageRef,
                AcceptedAt = now,
                ReaderResponseDueAt = now.AddHours(24),
                AutoRefundAt = now.AddHours(24),
                IdempotencyKey = idempotencyKey,
            };
            await _financeRepo.AddItemAsync(item, transactionCt);

            // 6. Cập nhật Số tổng cục diện cho Phiên Mẹ (Session)
            session.TotalFrozen += req.AmountDiamond;
            await _financeRepo.UpdateSessionAsync(session, transactionCt);
            
            // Commit vĩnh viễn
            await _financeRepo.SaveChangesAsync(transactionCt);

            createdItemId = item.Id;
            
        }, ct);

        return createdItemId;
    }
}
