/*
 * ===================================================================
 * FILE: ReaderReplyCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Escrow.Commands.ReaderReply
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh khi Thầy bói Gửi Câu Trả Lời (Reply) về Quẻ Bói cho Khách hàng.
 *
 * TẦM QUAN TRỌNG:
 *   Phải có API này thì Hệ thống Dao động đếm ngược mới hoạt động!
 *   - LƯỚI BẢO VỆ 1: Khi chưa trả lời -> Có Trigger tự hoàn tiền cho khách (Auto Refund).
 *   - KHỞI ĐỘNG LƯỚI 2: Khi VỪA TRẢ LỜI XONG -> Xóa Trigger Hoàn Tiền, bám Trigger KHÔNG KHIẾU NẠI THÌ AUTO CHUYỂN TIỀN (Auto Release) = +24 Giờ.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.ReaderReply;

/// <summary>
/// Mốc Lưu Trữ Thời Gian (Timestamp) đánh dấu Thợ bói đã Nhả chữ cho Khách.
/// Ngay khoảnh khắc này, Số phận của Cục Tiền Cọc sẻ được đẩy qua trạng thái Chờ Quyết Toán Tự động (Auto Release).
/// </summary>
public class ReaderReplyCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }
    
    /// <summary>Phải đúng tên đứa Thợ Bói đang nhét trong bill mới được Rep.</summary>
    public Guid ReaderId { get; set; }
}

public class ReaderReplyCommandHandler : IRequestHandler<ReaderReplyCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public ReaderReplyCommandHandler(
        IChatFinanceRepository financeRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<bool> Handle(ReaderReplyCommand req, CancellationToken ct)
    {
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await _financeRepo.GetItemForUpdateAsync(req.ItemId, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

            // Chỉ reader nhận kèo đầu tiên mới được quyền xớ rớ vào.
            if (item.ReceiverId != req.ReaderId)
                throw new BadRequestException("Bạn không phải reader của câu hỏi này.");

            // Tiền cọc phải được ghi nhận rồi (Accepted status) mới có cớ trả lời.  
            if (item.Status != QuestionItemStatus.Accepted)
                throw new BadRequestException($"Câu hỏi ở trạng thái {item.Status}, không thể reply.");

            // Đã trả lời 1 lần rồi thì thoáy khỏi Logic Trigger, 
            // tránh Bot tính toán lại Giờ Giao Tiền.
            if (item.RepliedAt != null)
                throw new BadRequestException("Câu hỏi đã được trả lời.");

            var now = DateTime.UtcNow;
            item.RepliedAt = now;
            
            // -------------------------------------------------------------
            // PHÉP MÀU TÀI CHÍNH TỰ ĐỘNG (ESCROW MAGIC)
            // Đặt đồng hồ Hẹn Giờ (Bomb nổ): 24 Giờ sau khoảnh khắc này,
            // Nếu Khách Hàng KHÔNG BẤM "Mở Tranh Chấp" hoặc KHÔNG BẤM "Giải Ngân",
            // JOB ngầm của Server SẼ TỰ ĐỘNG THU TIỀN VỀ CHO READER. Khách cấm đòi!!!
            // -------------------------------------------------------------
            item.AutoReleaseAt = now.AddHours(24);
            
            // Xóa bộ Hẹn Giờ Auto Hoàn Tiền (Vì Thợ đã bỏ công bói rồi, không được hoàn nữa).
            item.AutoRefundAt = null;

            await _financeRepo.UpdateItemAsync(item, transactionCt);
            await _financeRepo.SaveChangesAsync(transactionCt);
        }, ct);

        return true;
    }
}
