/*
 * ===================================================================
 * FILE: OpenDisputeCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Escrow.Commands.OpenDispute
 * ===================================================================
 * MỤC ĐÍCH:
 *   Ban Tòa Án (Mở Tranh Chấp) - User không hài lòng với kết luận của Thầy bói Tarot.
 *   
 * QUY TRÌNH:
 *   Khách hàng (User) cảm thấy Reader luận giải tào lao / gian dối:
 *   -> Bấm Chức năng "Mở Tranh Chấp".
 *   -> Cục tiền Cọc (QuestionItem) bị đổi trạng thái sàn DISPUTED.
 *   -> Bot AutoRelease/AutoRefund SẼ BỊ NGƯNG HOẠT ĐỘNG.
 *   -> Bộ đếm đứng im, chờ Nhân viên CSKH (Admin) của TarotNow nhảy vào Check Tin Nhắn
 *      và đưa ra Phán quyết Trọng Tài Ký Quỹ xử thắng thuộc về ai!
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.OpenDispute;

/// <summary>
/// Gói Lệnh: Đệ Đơn Kiện Lên Tòa TarotNow.
/// </summary>
public class OpenDisputeCommand : IRequest<bool>
{
    /// <summary>Mã Hóa Đơn Trả lời gây bức xúc.</summary>
    public Guid ItemId { get; set; }
    
    /// <summary>ID Nguyên Cáo (Chỉ Khách Mua Hàng mới được Kiện, Thầy bói cấm mếu).</summary>
    public Guid UserId { get; set; }
    
    /// <summary>Bài Vè Kể tội Thầy Bói Bíp Bíp. Ít nhất 10 Chữ.</summary>
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
        // Nhốt ACID Database: Vừa Kiện cáo vừa Giữ trạng thái (Không để kẹt lúc Bot Release tiền).
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            // Fork Khóa Đồng Xò (FOR UPDATE)
            var item = await _financeRepo.GetItemForUpdateAsync(req.ItemId, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

            // Hàng rào 1: Ai xì tiền thì người đó làm Cha nội. Chỉ Payer mới được quyền Tranh Chấp.
            if (item.PayerId != req.UserId)
                throw new BadRequestException("Chỉ người đặt câu hỏi mới được mở tranh chấp.");

            // Hàng rào 2: Nếu tiền đã trôi về Ví Thợ ròi (Thấy dở nhưng vẫn bấm Rút/Release), 
            // Thì không còn cơ hội mở Kiện Cáo nữa. Hàng đã bóc tem.
            if (item.Status != QuestionItemStatus.Accepted)
                throw new BadRequestException($"Câu hỏi ở trạng thái {item.Status}, không thể mở tranh chấp.");

            // Hàng rào 3: Thầy bói chưa coi bài lá nào mà đã vu oan dỏm? Cấm mở Tranh chấp.
            if (item.RepliedAt == null)
                throw new BadRequestException("Reader chưa trả lời, chưa thể mở tranh chấp.");

            // Hàng rào 4: Kẻ gian (User) lợi dụng lúc Đồng Hồ Cát gần hết (23H59) rồi mới Bấm Giải Quyết 
            // Đề phòng Bot Auto chạy. Tính năng này chặn giới hạn AutoWindow (Thường là 2-3 Ngày Tùy Admin).
            var now = DateTime.UtcNow;
            if (item.AutoReleaseAt != null && now > item.AutoReleaseAt)
                throw new BadRequestException("Đã quá thời hạn mở tranh chấp.");

            // Bức tâm thư Tối Thiểu 10 Chữ. "Lag" hay "Bịp" 1 chữ là Cấm Kiện.
            if (string.IsNullOrWhiteSpace(req.Reason) || req.Reason.Length < 10)
                throw new BadRequestException("Lý do tranh chấp phải có ít nhất 10 ký tự.");

            // -----------------------------------------------------------------
            //  THI HÀNH LỆNH BẮT GIỮ:
            //  Đóng băng Bot Tự Động. Gắn Mác Tù Tội (Disputed).
            // -----------------------------------------------------------------
            item.Status = QuestionItemStatus.Disputed;
            item.AutoReleaseAt = null; // <= Quan trọng nhất. Tắt Trigger tự cộng tiền cho Reader.
            item.UpdatedAt = now;

            await _financeRepo.UpdateItemAsync(item, transactionCt);

            // Cũng cập nhật cờ ở cấp độ Bố Mẹ (Session lớn) để biết Box trò chuyện này có Biến (Gốc Đỏ).
            var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null)
            {
                session.Status = "disputed";
                await _financeRepo.UpdateSessionAsync(session, transactionCt);
            }

            // In dấu tay Mực vào Sổ DB
            await _financeRepo.SaveChangesAsync(transactionCt);
            
        }, ct);

        return true;
    }
}
