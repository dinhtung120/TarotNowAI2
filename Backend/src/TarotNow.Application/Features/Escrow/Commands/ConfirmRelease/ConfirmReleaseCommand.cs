/*
 * ===================================================================
 * FILE: ConfirmReleaseCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Escrow.Commands.ConfirmRelease
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh KHOÁN LẠI TIỀN CHO READER (Giải Phóng Ký Quỹ - Giải Ngân).
 *
 * CƠ CHẾ HOẠT ĐỘNG (RELEASE & PLATFORM FEE):
 *   Giống như nút "Đã nhận được hàng" trên Shopee:
 *   - Khi User (Khách) Bấm Xác Nhận: Tiền Kim Cương đang Đóng Băng (Frozen) 
 *     sẽ được rã đông, và Bắn sang túi của Thợ bói (Reader).
 *   - LÚC NÀY, Hệ thống TarotNow sẽ CHIẾT KHẤU TỰ ĐỘNG (Platform Fee):
 *     Cắn 10% phí hoa hồng trên tổng số tiền chuyển. Thợ chỉ ăn được 90% mồ hôi nước mắt.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.ConfirmRelease;

/// <summary>
/// Gói lệnh do Khách hàng bấm: Giải ngân tiền cọc.
/// Kết quả: (1) Reader nhận tiền; (2) Admin thu phí bảo kê 10%.
/// </summary>
public class ConfirmReleaseCommand : IRequest<bool>
{
    /// <summary>Mã Hóa Đơn Trả lời (Trong 1 phiên chat có nhiều câu hỏi - Xác nhận câu nào thì Release câu đó).</summary>
    public Guid ItemId { get; set; }
    
    /// <summary>Người ra lệnh Bấm giải ngân (Bắt buộc phải là User, chống việc Thợ tự bấm Release tiền).</summary>
    public Guid UserId { get; set; }
}

public class ConfirmReleaseCommandHandler : IRequestHandler<ConfirmReleaseCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public ConfirmReleaseCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<bool> Handle(ConfirmReleaseCommand req, CancellationToken ct)
    {
        // Khóa cửa Transaction: Ngăn chặn 2 tay cùng chuyển tiền.
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            // Lock Tệp Hồ Sơ (FOR UPDATE): Ngăn quá trình Auto-Refund chạy đè gãy giao dịch.
            var item = await _financeRepo.GetItemForUpdateAsync(req.ItemId, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

            // Hàng phòng ngự 1: Chỉ Kẻ Ráp Khế Ước (Payer-Guest) mới rã băng được tiền nhà mình.
            if (item.PayerId != req.UserId)
                throw new BadRequestException("Chỉ người đặt câu hỏi mới được confirm release.");

            // Hàng phòng ngự 2: Item phải được "Đã Nhận Kèo" (Accepted) thì mới rã đông được.
            if (item.Status != QuestionItemStatus.Accepted)
                throw new BadRequestException($"Câu hỏi ở trạng thái {item.Status}, không thể release.");

            // Hàng phòng ngự 3: Thợ bói chưa trả lời dòng nào mà đã đòi lấy tiền à?
            if (item.RepliedAt == null)
                throw new BadRequestException("Reader chưa trả lời. Không thể release.");

            // Hàng phòng ngự 4: Cảnh báo Lag Server bấm đúp (Idempotency thủ công).
            if (item.ReleasedAt != null)
                throw new BadRequestException("Đã release rồi.");

            // ===================================
            // TOÁN TÀI CHÍNH KINH TẾ HỌC (ECONOMY)
            // ===================================
            
            // 1. Phí Bảo Kê (Platform Fee) - 10%, làm tròn lên (Tránh bị thọt 0.1 Cents).
            var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);
            
            // 2. Thu nhập Thân Chủ Thực Tế mang về (Take-home balance).
            var readerAmount = item.AmountDiamond - fee;

            // Hành Động 1: Sang tên Đổi Chủ Wallet từ User -> Reader. Trừ đi phí nhà sàn.
            await _walletRepo.ReleaseAsync(
                item.PayerId, item.ReceiverId, readerAmount,
                referenceSource: "chat_question_item",
                referenceId: item.Id.ToString(),
                description: $"Release {readerAmount}💎 (fee {fee}💎) cho reader",
                idempotencyKey: $"settle_release_{item.Id}",
                cancellationToken: transactionCt);

            // Hành Động 2: (Trừ thẳng vào túi của KHÁCH nhưng thực chất là xẻo trên cục băng gốc)
            // Thuế 10% sẽ bị "Hắc Vô Thường" (Admin) rút vào chân không quỹ hệ thống (Burn coin).
            if (fee > 0)
            {
                await _walletRepo.ConsumeAsync(
                    item.PayerId, fee,
                    referenceSource: "platform_fee",
                    referenceId: item.Id.ToString(),
                    description: $"Platform fee 10% = {fee}💎",
                    idempotencyKey: $"settle_fee_{item.Id}",
                    cancellationToken: transactionCt);
            }

            var now = DateTime.UtcNow;
            
            // Cập nhật thẻ bài: Trạng thái Giải Ngân Thành Công.
            item.Status = QuestionItemStatus.Released;
            item.ReleasedAt = now;
            item.ConfirmedAt = now;
            item.AutoReleaseAt = null; // Đã Release bằng cơm thì tắt Bot Auto Release đi.
            
            // Thời gian Khiếu nại đếm ngược: (Cho phép 24H Khiếu Nại sau khi đọc Kết quả bói Tarot).
            item.DisputeWindowStart = now;
            item.DisputeWindowEnd = now.AddHours(24);

            await _financeRepo.UpdateItemAsync(item, transactionCt);

            // Bất biến: Tổng tiền kẹt băng trong Phòng Chat (Session) này đã Giảm xuống.
            var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null)
            {
                session.TotalFrozen -= item.AmountDiamond;
                if (session.TotalFrozen < 0) session.TotalFrozen = 0; // Backup Safety.
                await _financeRepo.UpdateSessionAsync(session, transactionCt);
            }

            // Ghi vết vào DB
            await _financeRepo.SaveChangesAsync(transactionCt);
            
        }, ct);

        return true;
    }
}
