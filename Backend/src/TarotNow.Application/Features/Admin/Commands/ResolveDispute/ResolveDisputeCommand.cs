/*
 * ===================================================================
 * FILE: ResolveDisputeCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Commands.ResolveDispute
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command + Handler cho Admin GIẢI QUYẾT TRANH CHẤP (Dispute) trong Escrow.
 *
 * DISPUTE LÀ GÌ?
 *   Khi user đặt câu hỏi cho reader → tiền bị ĐÓNG BĂNG (escrow/freeze).
 *   Nếu user không hài lòng với câu trả lời → user tạo dispute (tranh chấp).
 *   Admin xem xét và quyết định:
 *     - "release": trả tiền cho reader (câu trả lời OK) → trừ phí 10%
 *     - "refund": hoàn tiền cho user (câu trả lời kém) → trả lại 100%
 *
 * TRANSACTION COORDINATOR:
 *   Dùng ITransactionCoordinator để đảm bảo ACID transaction.
 *   Tất cả thao tác (wallet + finance item + finance session) 
 *   xảy ra trong 1 transaction → hoặc tất cả thành công, hoặc tất cả rollback.
 *
 * PHÍ PLATFORM (10%):
 *   Khi release → reader nhận 90%, platform giữ 10% phí.
 *   Phí được "consume" (trừ khỏi số tiền đóng băng của user).
 * ===================================================================
 */

using System.Text.Json;
using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

/// <summary>Command chứa dữ liệu cho thao tác giải quyết tranh chấp.</summary>
public class ResolveDisputeCommand : IRequest<bool>
{
    /// <summary>ID câu hỏi bị tranh chấp (UUID từ PostgreSQL).</summary>
    public Guid ItemId { get; set; }

    /// <summary>UUID admin giải quyết.</summary>
    public Guid AdminId { get; set; }

    /// <summary>Hành động: "release" (trả reader) hoặc "refund" (hoàn user).</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Ghi chú admin (lý do quyết định).</summary>
    public string? AdminNote { get; set; }
}

/// <summary>
/// Handler giải quyết tranh chấp escrow.
/// Thao tác trong ACID transaction đảm bảo tính toàn vẹn tài chính.
/// </summary>
public class ResolveDisputeCommandHandler : IRequestHandler<ResolveDisputeCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;  // Quản lý finance sessions + items
    private readonly IWalletRepository _walletRepo;         // Thao tác ví (release/refund/consume)
    private readonly ITransactionCoordinator _transactionCoordinator; // Quản lý ACID transaction

    public ResolveDisputeCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<bool> Handle(ResolveDisputeCommand request, CancellationToken cancellationToken)
    {
        // Validate action
        var action = request.Action?.Trim().ToLowerInvariant();
        if (action != "release" && action != "refund")
            throw new BadRequestException("Action phải là 'release' hoặc 'refund'.");

        // Tạo audit metadata (JSON) ghi lại ai giải quyết, lúc nào
        var auditMetadata = BuildResolveAuditMetadata(request.AdminId, action, request.AdminNote);

        /*
         * _transactionCoordinator.ExecuteAsync:
         * Bọc TẤT CẢ thao tác trong 1 ACID transaction.
         * "transactionCt": CancellationToken riêng cho transaction.
         * 
         * Nếu bất kỳ thao tác nào throw exception:
         * → Toàn bộ transaction ROLLBACK → database như chưa thay đổi.
         * Đây là cách an toàn nhất cho thao tác tài chính.
         */
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            /*
             * GetItemForUpdateAsync: Lấy item VÀ KHÓA HÀNG (row lock).
             * "ForUpdate" → SELECT ... FOR UPDATE trong SQL.
             * Ngăn chặn 2 admin cùng giải quyết dispute này cùng lúc.
             * (Pessimistic locking)
             */
            var item = await _financeRepo.GetItemForUpdateAsync(request.ItemId, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

            // Guard: item phải đang ở trạng thái "Disputed"
            if (item.Status != QuestionItemStatus.Disputed)
                throw new BadRequestException("Câu hỏi không ở trạng thái dispute.");

            // Guard: chưa được settle (xử lý) trước đó
            if (item.ReleasedAt != null || item.RefundedAt != null)
                throw new BadRequestException("Dispute này đã được settle trước đó.");

            if (action == "release")
            {
                /*
                 * === RELEASE FLOW: Trả tiền cho reader ===
                 * Tính phí platform: 10% (làm tròn LÊN)
                 * Math.Ceiling: 15 * 0.10 = 1.5 → ceiling = 2 (ưu tiên platform)
                 * readerAmount = total - fee
                 */
                var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);
                var readerAmount = item.AmountDiamond - fee;

                // ReleaseAsync: giải phóng tiền đóng băng → chuyển cho reader
                await _walletRepo.ReleaseAsync(
                    item.PayerId, item.ReceiverId, readerAmount,
                    referenceSource: "admin_dispute_resolve",
                    referenceId: item.Id.ToString(),
                    description: $"Admin {request.AdminId} resolve: release {readerAmount}💎",
                    metadataJson: auditMetadata,
                    idempotencyKey: $"settle_release_{item.Id}",
                    cancellationToken: transactionCt);

                // ConsumeAsync: trừ phí platform (từ số tiền đóng băng của user)
                if (fee > 0)
                {
                    await _walletRepo.ConsumeAsync(
                        item.PayerId, fee,
                        referenceSource: "platform_fee",
                        referenceId: item.Id.ToString(),
                        description: $"Admin {request.AdminId} settle fee {fee}💎",
                        metadataJson: auditMetadata,
                        idempotencyKey: $"settle_fee_{item.Id}",
                        cancellationToken: transactionCt);
                }

                // Cập nhật trạng thái item → Released
                item.Status = QuestionItemStatus.Released;
                item.ReleasedAt = DateTime.UtcNow;
            }
            else
            {
                /*
                 * === REFUND FLOW: Hoàn tiền cho user ===
                 * RefundAsync: giải phóng tiền đóng băng → trả lại cho user
                 * Hoàn 100% (không trừ phí vì reader chưa hoàn thành tốt)
                 */
                await _walletRepo.RefundAsync(
                    item.PayerId, item.AmountDiamond,
                    referenceSource: "admin_dispute_resolve",
                    referenceId: item.Id.ToString(),
                    description: $"Admin {request.AdminId} resolve: refund {item.AmountDiamond}💎",
                    metadataJson: auditMetadata,
                    idempotencyKey: $"settle_refund_{item.Id}",
                    cancellationToken: transactionCt);

                // Cập nhật trạng thái item → Refunded
                item.Status = QuestionItemStatus.Refunded;
                item.RefundedAt = DateTime.UtcNow;
            }

            // Lưu thay đổi item
            await _financeRepo.UpdateItemAsync(item, transactionCt);

            /*
             * Cập nhật Finance Session: giảm TotalFrozen
             * TotalFrozen: tổng số tiền đang bị đóng băng trong session.
             * Sau khi release/refund → giảm đi số tiền của item này.
             * Clamp: nếu < 0 thì set = 0 (safety net, tránh số âm).
             */
            var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null)
            {
                session.TotalFrozen -= item.AmountDiamond;
                if (session.TotalFrozen < 0) session.TotalFrozen = 0;
                await _financeRepo.UpdateSessionAsync(session, transactionCt);
            }

            // SaveChangesAsync: commit tất cả thay đổi trong transaction
            await _financeRepo.SaveChangesAsync(transactionCt);
        }, cancellationToken);

        return true;
    }

    /// <summary>
    /// Tạo JSON metadata cho audit trail.
    /// Ghi lại: ai giải quyết, hành động gì, ghi chú gì.
    /// Lưu cùng transaction trong database để kiểm toán sau này.
    ///
    /// AdminNote bị giới hạn 500 ký tự (tránh abuse gửi text dài).
    /// "[..500]": range operator C# 8+ → lấy 500 ký tự đầu tiên.
    /// </summary>
    private static string BuildResolveAuditMetadata(Guid adminId, string action, string? adminNote)
    {
        var normalizedAdminNote = string.IsNullOrWhiteSpace(adminNote)
            ? null
            : adminNote.Trim();

        // Giới hạn độ dài admin note
        if (normalizedAdminNote is { Length: > 500 })
        {
            normalizedAdminNote = normalizedAdminNote[..500]; // Lấy 500 ký tự đầu
        }

        // Serialize thành JSON: { "adminId": "...", "action": "release", "adminNote": "..." }
        return JsonSerializer.Serialize(new
        {
            adminId,
            action,
            adminNote = normalizedAdminNote
        });
    }
}
