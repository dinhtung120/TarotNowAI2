/*
 * ===================================================================
 * FILE: AcceptOfferCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Escrow.Commands.AcceptOffer
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh chuyên biệt cho chức năng Giao Dịch Đảm Bảo (Escrow) - Chấp nhận báo giá.
 *   
 * BỐI CẢNH (BUSINESS CONTEXT):
 *   Khi Reader gửi thông điệp yêu cầu thanh toán (Payment Offer) cho Khách hàng.
 *   Khách hàng nhấn nút "Đồng Ý Trả Tiền". API này sẽ được kích hoạt để 
 *   ĐÓNG BĂNG (Freeze) số Diamond tương ứng trong ví, biến nó thành Khoản Tiền Đảm Bảo.
 *   Tiền lúc này CHƯA vào túi Reader. Giống hệ thống Shopee, tiền nằm chờ đến 
 *   khi mục vụ xem Tarot hoàn tất.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.AcceptOffer;

/// <summary>
/// Command: User bấm Chấp Nhận Báo Giá → Đóng băng Diamond, tạo phiên Tài chính/Giao dịch Cụ Thể.
/// Tính Idempotency (Lũy Đẳng): IdempotencyKey chống hiện tượng trừ tiền 2 lần (Double-freeze).
/// </summary>
public class AcceptOfferCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid ReaderId { get; set; }
    
    /// <summary>Mã ObjectID trỏ tới Box Chat hiện tại trên MongoDB.</summary>
    public string ConversationRef { get; set; } = string.Empty;
    
    /// <summary>Số lượng tiền Diamond đóng băng.</summary>
    public long AmountDiamond { get; set; }
    
    /// <summary>Mã ID của Dòng Tin Nhắn (Báo giá) để biết User đang đồng ý cục báo giá nào.</summary>
    public string? ProposalMessageRef { get; set; }
    
    /// <summary>Chìa khoá sinh từ thiết bị rớt mạng mạng yếu (Client-side UUID). Chống click đúp.</summary>
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>
/// Handler xử lý Cốt Lõi Quy Trình Ký Quỹ (Escrow).
/// Chứa Transaction chặn mất tiền.
/// </summary>
public partial class AcceptOfferCommandHandler : IRequestHandler<AcceptOfferCommand, Guid>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IConversationRepository _conversationRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public AcceptOfferCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        IConversationRepository conversationRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _conversationRepo = conversationRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<Guid> Handle(AcceptOfferCommand req, CancellationToken ct)
    {
        var idempotencyKey = ValidateIdempotencyKey(req.IdempotencyKey);
        var existing = await _financeRepo.GetItemByIdempotencyKeyAsync(idempotencyKey, ct);
        if (existing != null)
        {
            return existing.Id;
        }

        await ValidateConversationAsync(req, ct);
        return await ExecuteAcceptOfferAsync(req, idempotencyKey, ct);
    }
}
