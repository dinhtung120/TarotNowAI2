/*
 * ===================================================================
 * FILE: EscrowTimerService.cs
 * NAMESPACE: TarotNow.Infrastructure.BackgroundJobs
 * ===================================================================
 * MỤC ĐÍCH:
 *   Background Worker Kinh Điển Chạy Nhầm Nhè Ngầm Dưới Cống Hầm Lúc App Web Mở.
 *   Xoay Kim Đồng Hồ Cứ 60 Giây Ra Soát 1 Vòng Dọn Rác Giúp Refund Lại Tiền Chat Nếu Thầy Chết Hụt / Auto Tát Lấy Lại Kim Cương Về Thầy Nêu Thằng Khách Ngủ Qúa Hạn.
 * ===================================================================
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace TarotNow.Infrastructure.BackgroundJobs;

/// <summary>
/// Background Hosted Service (Worker Băng Tải Lọc Xoáy Background Hầm Mở Cửa 60s/lần) Cho 3 Cái Hẹn Giờ Tiền Khóa Escrow Thúi Mùi:
/// 
/// 1. Expired offers → Quét Lệnh Rác Thách Giá Gửi Xong Éo Ai Rep Đưa Giam Xéo Đi.
/// 2. No-reply 24h → Auto-refund (Ngập Trả Tiền Giam Đá Diamond Về Bank Lại Cho User Nếu Thợ Bói Tật Câm Ko Gõ Chữ Rep).
/// 3. Replied + no dispute 24h → Auto-release (- 10% fee) (Cưa Phát Giam Giành Quyền Phóng Giải Đá Cứng Cầm Trong Áo Đẩy Sổ Nát Ra Tung Túi Diamond Cho Reader Hưởng Và Trích Phí Ăn Của App 10%).
///
/// Lý do Méo Gắn DB Context Trực Tiếp: Vì Thằng Móc Túy Background Dốc Cổ Mày Này Sinh Cùng Server Chết Cùng Giây Server Nằm Lưng Trên IHostedService (Singleton) Trong Khi Tụi Móc Nối Bóp Entity `repositories` Đều Thuộc Áo Tức Cắt Khúc Lát Cát Kiểu Scope. Nên Ném Cho Nó Cái Thùng Factory Phá Khóa Tình Để Nó Rút Đè Khi Cần Gấp Chống Lỗi Memory Leak "Cannot inject scoped into singleton".
/// </summary>
public class EscrowTimerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EscrowTimerService> _logger;

    /// <summary>Khoảng Cuộn Nghỉ Mắt Kim Vàng Đo Cứ 60 Giây Dò 1 Quét Quý.</summary>
    private static readonly TimeSpan ScanInterval = TimeSpan.FromSeconds(60);

    public EscrowTimerService(IServiceScopeFactory scopeFactory, ILogger<EscrowTimerService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <summary>Tương Xoay Tít Cho Cuốn Bạc Hoạt Động Cốt Yếu Lòng Ngầm Vòng Lặp Vô Hạn.</summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[EscrowTimer] Service started.");

        // Nhắm Sáng Không Gặp Bão Cấm Lưới Thì Bò Lết Tiến Theo.
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Đi Nhặt Vét Thức
                await ProcessTimers(stoppingToken);
            }
            catch (Exception ex)
            {
                // Đóng Án Trắng Báo Nghẽn To Cấm Lỗi Bị Văng Chết Crashe Background Của Cả System Sát.
                _logger.LogError(ex, "[EscrowTimer] Unhandled error in timer loop.");
            }

            // Đi Cất Cuốc 60 Giây Rùi Lại Xuống Nham Thạch Xem Mẻ Kế Tiếp.
            await Task.Delay(ScanInterval, stoppingToken);
        }

        _logger.LogInformation("[EscrowTimer] Service stopped.");
    }

    /// <summary>Thực Xáo Phát Khớp Tàu Lên Gác (Ném Mở Khuôn Thùng Repository Cầm Khúa Giải Tiền).</summary>
    private async Task ProcessTimers(CancellationToken ct)
    {
        // Rút Bọc Scope Mới Cực Kì Mới Ngang Phím Dạng Bức Giấy Call HTTP Request Trắng Tươi Cấp Gói DI Mới Ở Đầu Vong.
        using var scope = _scopeFactory.CreateScope();
        var financeRepo = scope.ServiceProvider.GetRequiredService<IChatFinanceRepository>();
        var walletRepo = scope.ServiceProvider.GetRequiredService<IWalletRepository>();
        var transactionCoordinator = scope.ServiceProvider.GetRequiredService<ITransactionCoordinator>();

        // 1. Dọn Kéo Khách Thò Đầu Giá Xong 1 Rổ Ngủ Đi Ngáy Mất → Cắt Bỏ Mối Bòi Này Tránh Rác Server List Chơi DB Chết Lắc.
        await ProcessExpiredOffers(financeRepo, ct);

        // 2. Tát Refund Đuổi Lấy Lại Thẻ Trắng Trả Khách Sau 24H Tranh Bóp Chờ (Luật Giữ Chữ Tín Thương Hiệu Khách Ai Lỗi Bỏ Kệ).
        await ProcessAutoRefunds(financeRepo, walletRepo, transactionCoordinator, ct);

        // 3. Đá Trúng Luột Chút Cắt Trả Cửa Auto Sang Mở Release Quăng Sang Pháo Đá Sang Thợ Bói Lụm Ăn Trích Đoạn Cắn App Kèm Chi Phí Theo (Tránh Lũy Tích Cục Thạch DB Quá Ác Tít Nặng Túi Cầm Hộ).
        await ProcessAutoReleases(financeRepo, walletRepo, transactionCoordinator, ct);
    }

    /// <summary>
    /// Pending offers quá hạn → status = refunded (Hết Cắn Đá 2 Bên Náo Thì Chả Lòng Cáo Đơn Án Đi Luôn).
    /// </summary>
    private async Task ProcessExpiredOffers(IChatFinanceRepository repo, CancellationToken ct)
    {
        var expired = await repo.GetExpiredOffersAsync(ct);
        foreach (var item in expired)
        {
            try
            {
                // Set Chuyển Trạng Cáo Rỗng Và Chết Tranh Ngay Status Khóa Bơm Trắng Vạch Cho Thả Xích Khỏi Rác UI (Lưu Ý Là Này Chưa Bóp Tiền Từ DB Nên Refund Status Nghĩa Gốc Hủy Đi Khách Chưa Giam Mất Đi Giọt Diamond Cả).
                item.Status = QuestionItemStatus.Refunded;
                item.RefundedAt = DateTime.UtcNow;
                await repo.UpdateItemAsync(item, ct);
                _logger.LogInformation("[EscrowTimer] Expired offer cancelled: {ItemId}", item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Failed to cancel offer: {ItemId}", item.Id);
            }
        }
        if (expired.Count > 0) await repo.SaveChangesAsync(ct); // Đẩy Đi Gom Code Chút Bớt Call DB Nặng Ác.
    }

    /// <summary>
    /// Bộ 1: Bọc Refund Cho Vắng Hơi Lẻ Thợ Sợ Nằm. Accepted Cóc Chờ Qua 24 Giờ Lại Rep Chữ Ko Gọi Nhúc Nhích Không Trả Ai. 
    /// Dành Cục Mệnh Điều Phối Coordinator Kẹp Giảng Tách Transaction Lỗi Tịt Refund Nửa Vời Khỏi Mắc Mớ DB Nát Tiền Ảo Khách Bố Ác Hơn Bank Rút Hụt Áp Quát.
    /// </summary>
    private async Task ProcessAutoRefunds(
        IChatFinanceRepository repo,
        IWalletRepository wallet,
        ITransactionCoordinator transactionCoordinator,
        CancellationToken ct)
    {
        var candidates = await repo.GetItemsForAutoRefundAsync(ct);
        foreach (var candidate in candidates)
        {
            try
            {
                // Transaction Giam Dễ Giết Rạch Cục Bộ Cửa.
                await transactionCoordinator.ExecuteAsync(async transactionCt =>
                {
                    // Lóc Nõn Row Đó Dò Mã Nét Lưới Có Bị Ai Can Thiệp Dính Hay Chưa (Update Lock Phanh Hãm Select For Update Của Gốc C#).
                    var item = await repo.GetItemForUpdateAsync(candidate.Id, transactionCt);
                    if (item == null) return;

                    var now = DateTime.UtcNow;
                    // Double Check Cái Kim Cực Độc Cầm Đã Vượt Hẹn Giờ Kêu "Lọc Đi" Chưa Và Status Phải Đang Ép.
                    var eligible = item.Status == QuestionItemStatus.Accepted
                                   && item.RepliedAt == null
                                   && item.AutoRefundAt != null
                                   && item.AutoRefundAt <= now;
                    if (!eligible) return;

                    // Múc Từ Ví Oa Trích Refund Đá Lên Chủ Cũ Đi Phạch Trả Hàng (Có Ném Idempotency Key Chặn Trùng Quăng Túi DB).
                    await wallet.RefundAsync(
                        item.PayerId, item.AmountDiamond,
                        referenceSource: "chat_question_item",
                        referenceId: item.Id.ToString(),
                        description: $"Auto-refund {item.AmountDiamond}💎 (reader không reply trong 24h)",
                        idempotencyKey: $"settle_refund_{item.Id}",
                        cancellationToken: transactionCt);

                    // Xóa Vạch Tiết Cọc Thay Nhãn Sổ Án Cho Refund DB Đỡ Bắt Thù Lỗi 
                    item.Status = QuestionItemStatus.Refunded;
                    item.RefundedAt = now;
                    
                    // Thức Cọc Chẻ Xong Giữa Đống Oa Còn Nảy Vụ (Cho Xin 24h Nhấn Nút Hủy Án Đòi Bắt Mày Quậy). Tường Xảy Cửa Chờ Ra Réo.
                    item.DisputeWindowStart = now;
                    item.DisputeWindowEnd = now.AddHours(24);
                    await repo.UpdateItemAsync(item, transactionCt);

                    baseSession(repo, item, transactionCt);

                    await repo.SaveChangesAsync(transactionCt); // Gắn Gắn Dập Dập Cộc Data Thả Băng Rơi Lệnh Tủ.
                }, ct);

                _logger.LogInformation("[EscrowTimer] Auto-refund: {ItemId}, {Amount}💎", candidate.Id, candidate.AmountDiamond);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-refund failed: {ItemId}", candidate.Id);
            }
        }
    }

    /// <summary>
    /// Bộ 2: Bóp Release Đem Oăn Mớ Kim Nuốt Xoáy Quất Khóa Ném 10% Vay Lường Lọc.
    /// 24H Tính Tự Lúc Vừa Đọc Bài Rep Vỡ Phím Nếu Mày Không Khóc Ai Tranh Chấp Trả Bãi Tiền. Thì Đồng Lâu Khách Đã Kêu Done Không Í Ớ Tát Bàn Nhảy Quát Refund Tiền Gì. Xử Lý Đá Cho Vô Túi Reader Thợ Ăn Rõ.
    /// </summary>
    private async Task ProcessAutoReleases(
        IChatFinanceRepository repo,
        IWalletRepository wallet,
        ITransactionCoordinator transactionCoordinator,
        CancellationToken ct)
    {
        var candidates = await repo.GetItemsForAutoReleaseAsync(ct);
        foreach (var candidate in candidates)
        {
            try
            {
                // Trói Cuốn Chặn Sạch Bug (Giam For Update Transaction Chống Nuốt Báo Gọi Call Vô Lại Đụng Khóa Án Tử Data).
                await transactionCoordinator.ExecuteAsync(async transactionCt =>
                {
                    var item = await repo.GetItemForUpdateAsync(candidate.Id, transactionCt);
                    if (item == null) return;

                    var now = DateTime.UtcNow;
                    
                    // Double Cốc Xem Dữ Giữa Dã Cắn Chưa Thể Giam Tranh Cáo (Eligible Đưa Gốc Rep Chưa Và Hồi Nạp Chênh Hạn 24H Trôi Hết Cãi Giải Đua Oãi Quýt Ra).
                    var eligible = item.Status == QuestionItemStatus.Accepted
                                   && item.RepliedAt != null
                                   && item.AutoReleaseAt != null
                                   && item.AutoReleaseAt <= now;
                    if (!eligible) return;

                    // Luộc 10% Cho Cái Ổ Platform Mở Web Hốc Dư Lấy Server Gánh (Áp Phí Từ Giá Cấu Hỏi Khách Trừ Gốc Admin Rác).
                    var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);
                    // Rớt Túi Rỗng Sót Cầm Cụ Reader Được Trọc Đá Còn Sân (Ví Về Chủ Tới Nơi).
                    var readerAmount = item.AmountDiamond - fee;

                    // Giải Bóng Băng Cục (Chuyền Chảo Tiền Rớt Release Không Cấn Khóa Wallet Qua Đằng Reader Tách Phí Ra).
                    await wallet.ReleaseAsync(
                        item.PayerId, item.ReceiverId, readerAmount,
                        referenceSource: "chat_question_item",
                        referenceId: item.Id.ToString(),
                        description: $"Auto-release {readerAmount}💎 (fee {fee}💎)",
                        idempotencyKey: $"settle_release_{item.Id}",
                        cancellationToken: transactionCt);

                    if (fee > 0)
                    {
                        // Giương Rác Bóp Ăn Lõ Cục (Consume Đáy Phầm Đạn Dư Bay Ẩn Biến Hình Vô Đáy Admin Thùng App Coi Nạp Hệ Tiêu Lượm Lên Nuốt Dứt Data Ko Trả Cho Ai Cả Báo Kiếm Thả Tách).
                        await wallet.ConsumeAsync(
                            item.PayerId, fee,
                            referenceSource: "platform_fee",
                            referenceId: item.Id.ToString(),
                            description: $"Platform fee auto 10% = {fee}💎",
                            idempotencyKey: $"settle_fee_{item.Id}",
                            cancellationToken: transactionCt);
                    }

                    // Tường Rách Dán Tờ (Giật Code Trạng Kẹt "Released" Đi Dò Data Chống Rối).
                    item.Status = QuestionItemStatus.Released;
                    item.ReleasedAt = now;
                    item.DisputeWindowStart = now;
                    item.DisputeWindowEnd = now.AddHours(24);
                    await repo.UpdateItemAsync(item, transactionCt);

                    baseSession(repo, item, transactionCt);

                    await repo.SaveChangesAsync(transactionCt);
                    _logger.LogInformation("[EscrowTimer] Auto-release: {ItemId}, {Amount}💎 (fee {Fee}💎)", item.Id, readerAmount, fee);
                }, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-release failed: {ItemId}", candidate.Id);
            }
        }
    }

    /// <summary>
    /// Hàm Chặn Sập Nhanh Để Gầm Rút DB Giống Đoạn Base Logic Cho Dễ Cấu: 
    /// Hạ Rào Bức Tổng Lạm Frozen Báo Tiền Tự Phiên Gốc Tổng Chứa Cái Thống Session Ngoài Chật Bể Nếu Nuốt Ngon Xong. Không Ẩn Gây Chén Cửa Treo Khói Đen Dày Ríu Total Frozen Náo Sai Nghỉ Khác Bug Ra Report Data Analyst Soi Ra Dịch Sợ Hỏng Data Kho.
    /// </summary>
    private async void baseSession(IChatFinanceRepository repo, ChatQuestionItem item, CancellationToken transactionCt)
    {
         var session = await repo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
         if (session != null)
         {
             // Trừ bớt cục Tổng Treo
             session.TotalFrozen -= item.AmountDiamond;
             if (session.TotalFrozen < 0) session.TotalFrozen = 0;
             await repo.UpdateSessionAsync(session, transactionCt);
         }
    }
}
