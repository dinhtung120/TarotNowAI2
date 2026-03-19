/*
 * ===================================================================
 * FILE: ProcessDepositCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Commands.ProcessDeposit
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command + Handler cho Admin XỬ LÝ ĐƠN NẠP TIỀN.
 *   Admin duyệt (approve) hoặc từ chối (reject) đơn nạp tiền.
 *
 * LUỒNG NGHIỆP VỤ:
 *   1. User tạo đơn nạp (DepositController) → trạng thái "Pending"
 *   2. Payment gateway xác nhận → hoặc Admin xác nhận thủ công
 *   3. Approve: Cộng diamond vào ví user → Đánh dấu thành công
 *   4. Reject: Đánh dấu thất bại → Không cộng tiền
 *
 * THỨ TỰ THAO TÁC QUAN TRỌNG: CREDIT TRƯỚC, MARK AFTER
 *   Vấn đề: CreditAsync() gọi SaveChangesAsync() nội bộ.
 *   Nếu MarkAsSuccess() gọi TRƯỚC CreditAsync():
 *   → EF Core change tracker đã track deposit order là "Modified"
 *   → Khi CreditAsync() gọi SaveChangesAsync() → cố gắng lưu CẢ
 *     deposit order (đã modified) → vi phạm unique constraint
 *   → LỖI!
 *   
 *   Giải pháp: Credit ví TRƯỚC (order chưa modified) → MarkAsSuccess SAU.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Admin.Commands.ProcessDeposit;

/// <summary>
/// Command chứa dữ liệu cho thao tác xử lý đơn nạp tiền.
/// </summary>
public class ProcessDepositCommand : IRequest<bool>
{
    /// <summary>ID đơn nạp tiền (UUID từ PostgreSQL).</summary>
    public Guid DepositId { get; set; }

    /// <summary>Hành động: "approve" (duyệt) hoặc "reject" (từ chối). Mặc định "approve".</summary>
    public string Action { get; set; } = "approve";

    /// <summary>
    /// Mã giao dịch thực tế từ payment gateway (nếu có).
    /// Ví dụ: "VNP_12345678" từ VNPay, "MOMO_ABC" từ MoMo.
    /// Nếu admin duyệt thủ công mà không có → tự tạo mã.
    /// </summary>
    public string? TransactionId { get; set; }
}

/// <summary>
/// Handler xử lý việc phê duyệt hoặc từ chối đơn nạp tiền.
/// </summary>
public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, bool>
{
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IWalletRepository _walletRepository;

    public ProcessDepositCommandHandler(
        IDepositOrderRepository depositOrderRepository,
        IWalletRepository walletRepository)
    {
        _depositOrderRepository = depositOrderRepository;
        _walletRepository = walletRepository;
    }

    public async Task<bool> Handle(ProcessDepositCommand request, CancellationToken cancellationToken)
    {
        // Bước 1: Tìm đơn nạp tiền
        var order = await _depositOrderRepository.GetByIdAsync(request.DepositId, cancellationToken);

        // Kiểm tra: order phải tồn tại VÀ đang Pending (chưa xử lý)
        if (order == null || order.Status != "Pending") return false;

        var action = request.Action?.Trim().ToLowerInvariant();
        if (action != "approve" && action != "reject")
        {
            throw new BadRequestException("Action phải là 'approve' hoặc 'reject'.");
        }

        // Tạo transaction ID (nếu admin không cung cấp → tự tạo)
        var txnId = ResolveTransactionId(request.TransactionId, action);

        if (action == "approve")
        {
            /*
             * QUAN TRỌNG: THỨ TỰ THAO TÁC
             * 
             * PHẢI CreditAsync TRƯỚC khi MarkAsSuccess!
             * 
             * Lý do kỹ thuật:
             * - CreditAsync() gọi _dbContext.SaveChangesAsync() bên trong
             * - Nếu MarkAsSuccess() gọi trước → EF Core change tracker
             *   đánh dấu deposit order là "Modified"
             * - Khi CreditAsync() gọi SaveChangesAsync() → EF Core cố lưu
             *   CẢ deposit order (Modified) VÀ wallet transaction mới
             * - Deposit order có unique constraint trên transaction_id
             *   → vi phạm constraint nếu đã có entry
             * 
             * Giải pháp: Credit ví trước (order chưa bị modify),
             * sau đó mới MarkAsSuccess (modify order).
             */

            // Bước 2: Cộng diamond vào ví user TRƯỚC
            await _walletRepository.CreditAsync(
                userId: order.UserId,
                currency: CurrencyType.Diamond,
                type: TransactionType.Deposit,
                amount: order.DiamondAmount,            // Số diamond từ đơn hàng
                referenceSource: "DepositOrder",
                referenceId: order.Id.ToString(),
                description: $"Approved deposit order {order.Id} (+{order.DiamondAmount} Diamond)",
                idempotencyKey: $"deposit_approve_{order.Id}", // Chống trùng
                cancellationToken: cancellationToken
            );

            // Bước 3: SAU KHI credit thành công → cập nhật trạng thái đơn hàng
            order.MarkAsSuccess(txnId, "Admin Manual Approval");
        }
        else
        {
            // Từ chối: đánh dấu thất bại, KHÔNG cộng tiền
            order.MarkAsFailed(txnId);
        }

        // Bước 4: Lưu thay đổi deposit order vào database
        await _depositOrderRepository.UpdateAsync(order, cancellationToken);

        return true;
    }

    /// <summary>
    /// Tạo transaction ID nếu admin không cung cấp.
    ///
    /// Format: ADMIN_APPROVE_abc123... hoặc ADMIN_REJECT_def456...
    ///
    /// Guid.CreateVersion7(): UUID v7 dựa trên timestamp (mới trong .NET 9).
    ///   Ưu điểm so với Guid.NewGuid() (v4):
    ///   - Có thứ tự thời gian (sortable)
    ///   - Tốt hơn cho database indexing
    ///   ":N" format: không có dấu gạch ngang (32 ký tự hex liền).
    /// </summary>
    private static string ResolveTransactionId(string? requestedTransactionId, string action)
    {
        if (!string.IsNullOrWhiteSpace(requestedTransactionId))
        {
            return requestedTransactionId.Trim();
        }

        return $"ADMIN_{action.ToUpperInvariant()}_{Guid.CreateVersion7():N}";
    }
}
