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
public class AcceptOfferCommandHandler : IRequestHandler<AcceptOfferCommand, Guid>
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
        var idempotencyKey = req.IdempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new BadRequestException("IdempotencyKey là bắt buộc.");

        if (idempotencyKey.Length > 128)
            throw new BadRequestException("IdempotencyKey quá dài (tối đa 128 ký tự).");

        // -------------------------------------------------------------
        //  1. CỘT MỐC LŨY ĐẲNG (IDEMPOTENCY GATECHECK)
        //  Tránh việc User bị lag bấm "Accept" 2 lần bị trừ tiền 2 lần.
        // -------------------------------------------------------------
        var existing = await _financeRepo.GetItemByIdempotencyKeyAsync(idempotencyKey, ct);
        if (existing != null) return existing.Id; // Trả về ID cũ, không tính tiền thêm.

        // -------------------------------------------------------------
        //  1.5 CỔNG AN NINH NGỮ CẢNH (AUTHORIZATION)
        //  Không bao giờ tin tưởng Client (UserId / ReaderId) mù quáng.
        //  Phải lôi dữ liệu thật từ MongoDB ra kiểm tra chéo (Cross-check).
        // -------------------------------------------------------------
        var conversation = await _conversationRepo.GetByIdAsync(req.ConversationRef, ct)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        // Bắt quả tang 1: Giao dịch được thực hiện bởi người không có trong phòng Chat.
        if (conversation.UserId != req.UserId.ToString() || conversation.ReaderId != req.ReaderId.ToString())
            throw new BadRequestException("Thông tin phiên trò chuyện không hợp lệ cho giao dịch escrow.");

        // Bắt quả tang 2: Phòng Chat đã kết thúc (Closed/Ended) mà vẫn dấn thân ném tiền.
        if (conversation.Status != ConversationStatus.Pending && conversation.Status != ConversationStatus.Active)
            throw new BadRequestException($"Cuộc trò chuyện ở trạng thái '{conversation.Status}', không thể accept offer.");

        Guid createdItemId = Guid.Empty;
        
        // -------------------------------------------------------------
        //  2. BẮT ĐẦU PHIÊN GIAO DỊCH DATABASE LIÊN TỤC (ACID TRANSACTION)
        //  Tất cả hoặc không gì cả: Giữ tiền Ví (PostgreSQL) + Tạo Log Finance (PostgreSQL) phải thành công CÙNG MỘT LÚC.
        // -------------------------------------------------------------
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            // Trích xuất "Gia phả" (Session) giao dịch của Phòng Chat này.
            var session = await _financeRepo.GetSessionByConversationRefAsync(req.ConversationRef, transactionCt);
            if (session == null)
            {
                // Nếu đây là lần đặt cọc đầu tiên của Box Chat -> Sinh ra Session gốc.
                session = new ChatFinanceSession
                {
                    ConversationRef = req.ConversationRef,
                    UserId = req.UserId,
                    ReaderId = req.ReaderId,
                    Status = "active",
                    TotalFrozen = 0,
                };
                await _financeRepo.AddSessionAsync(session, transactionCt);
            }

            // -------------------------------------------------------------
            //  3. THỰC THI KHÔNG GIAN KÝ QUỸ (FREEZE WALLET)
            // -------------------------------------------------------------
            // Khóa (Freeze) tiền của User. Nếu không đủ tiền -> WalletRepo sẽ thẩy Lỗi (Throw Excepcion) 
            // -> Toàn bộ Block Transaction sẽ RollBack (Trả lại bình thường).
            await _walletRepo.FreezeAsync(
                req.UserId, req.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: idempotencyKey,
                description: $"Escrow freeze {req.AmountDiamond}💎 cho conversation {req.ConversationRef}",
                idempotencyKey: $"freeze_{idempotencyKey}",
                cancellationToken: transactionCt);

            var now = DateTime.UtcNow;
            
            // Xây dựng Hóa Đơn Chi Tiết (Question Item) mô tả khoản tiền này dành cho việc gì.
            var item = new ChatQuestionItem
            {
                FinanceSessionId = session.Id,
                ConversationRef = req.ConversationRef,
                PayerId = req.UserId,
                ReceiverId = req.ReaderId,
                Type = QuestionItemType.MainQuestion,
                AmountDiamond = req.AmountDiamond,
                Status = QuestionItemStatus.Accepted, // Note: "Đã Chấp Nhận Thanh toán/Cọc". Đợi trả lời.
                ProposalMessageRef = req.ProposalMessageRef,
                AcceptedAt = now,
                ReaderResponseDueAt = now.AddHours(24), // Reader phải trả lời trong vòng 24H
                AutoRefundAt = now.AddHours(24), // Quá 24H -> Tự động Hoàn tiền lại (Job Chạy Ngầm).
                IdempotencyKey = idempotencyKey,
            };
            await _financeRepo.AddItemAsync(item, transactionCt);

            // Cập nhật gia phả: Cộng dồn Số Tiền Tổng của toàn bộ Box Chat này đang bị giam là bao nhiêu.
            session.TotalFrozen += req.AmountDiamond;
            await _financeRepo.UpdateSessionAsync(session, transactionCt);
            
            // Chốt hạ Data
            await _financeRepo.SaveChangesAsync(transactionCt);

            createdItemId = item.Id;
            
        }, ct);

        return createdItemId;
    }
}
