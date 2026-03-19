/*
 * ===================================================================
 * FILE: AddUserBalanceCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Commands.AddUserBalance
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command + Handler cho admin CỘNG TIỀN THỦ CÔNG cho user.
 *   Dùng trong trường hợp: bồi thường lỗi, tặng thưởng, xử lý khiếu nại.
 *
 * CQRS PATTERN:
 *   File này chứa CẢ Command (data) VÀ Handler (logic) cho đơn giản.
 *   Quy ước: file nhỏ (<100 dòng) → gộp chung; file lớn → tách riêng.
 *
 * IDEMPOTENCY KEY:
 *   Mã duy nhất do admin gửi để CHỐNG CỘNG TRÙNG.
 *   Nếu admin click nút 2 lần (double-click) → cùng idempotency key
 *   → database phát hiện trùng → không cộng thêm lần 2.
 *   QUAN TRỌNG cho bất kỳ thao tác tài chính nào.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.AddUserBalance;

/// <summary>
/// Command chứa DỮ LIỆU cho thao tác cộng tiền.
/// IRequest<bool>: gửi qua MediatR → handler trả về bool (thành công/thất bại).
/// </summary>
public class AddUserBalanceCommand : IRequest<bool>
{
    /// <summary>ID của người dùng được cộng tiền (UUID từ PostgreSQL).</summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Loại tiền: "gold" hoặc "diamond".
    /// Mặc định "gold" (CurrencyType.Gold = "gold").
    /// CurrencyType: enum string constants ở Domain layer.
    /// </summary>
    public string Currency { get; set; } = CurrencyType.Gold;

    /// <summary>Số lượng tiền cần cộng (phải > 0).</summary>
    public long Amount { get; set; }

    /// <summary>Lý do cộng tiền (ghi vào sổ cái giao dịch để audit).</summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Idempotency key: mã duy nhất để chống cộng tiền trùng lặp.
    /// Admin UI tạo UUID mới cho mỗi thao tác → nếu gửi lại cùng key → bỏ qua.
    /// </summary>
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>
/// Handler XỬ LÝ LOGIC cộng tiền.
///
/// IRequestHandler<Command, Response>: interface MediatR cho handler.
///   MediatR tự động tìm và gọi handler đúng khi nhận command tương ứng.
/// </summary>
public class AddUserBalanceCommandHandler : IRequestHandler<AddUserBalanceCommand, bool>
{
    /*
     * _walletRepository: thao tác với ví (cộng/trừ tiền, ghi sổ cái).
     * _userRepository: kiểm tra user tồn tại.
     * Cả 2 đều là INTERFACE (không phải class cụ thể) → Dependency Inversion.
     */
    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;

    public AddUserBalanceCommandHandler(IWalletRepository walletRepository, IUserRepository userRepository)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Method chính xử lý command.
    /// Được MediatR gọi khi nhận AddUserBalanceCommand.
    /// </summary>
    public async Task<bool> Handle(AddUserBalanceCommand request, CancellationToken cancellationToken)
    {
        // Bước 1: Kiểm tra User có tồn tại không
        // Guard clause: return false ngay nếu user không tìm thấy
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return false;

        /*
         * Bước 2: Normalize currency (chuẩn hóa input).
         * Trim(): bỏ khoảng trắng đầu/cuối
         * ToLowerInvariant(): chuyển thành chữ thường không phụ thuộc locale
         * "??" (null-coalescing): nếu null → dùng string.Empty
         * 
         * Tại sao normalize? Vì admin có thể nhập "Gold", "GOLD", " gold "
         * → đều phải xử lý thành "gold" để so sánh chính xác.
         */
        var normalizedCurrency = request.Currency?.Trim().ToLowerInvariant() ?? string.Empty;
        if (normalizedCurrency != CurrencyType.Gold && normalizedCurrency != CurrencyType.Diamond)
            throw new BadRequestException("Currency không hợp lệ. Chỉ chấp nhận 'gold' hoặc 'diamond'.");

        // Bước 3: Validate idempotency key
        var idempotencyKey = request.IdempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new BadRequestException("IdempotencyKey là bắt buộc cho thao tác cộng tiền thủ công.");

        /*
         * Bước 4: Cộng tiền qua Repository.
         * CreditAsync: cộng tiền vào ví user + ghi vào sổ cái (ledger).
         *
         * Tham số:
         *   userId: ai được cộng
         *   currency: loại tiền (gold/diamond)
         *   type: TransactionType.AdminTopup → đánh dấu giao dịch admin thủ công
         *   amount: số tiền cộng
         *   referenceSource: nguồn giao dịch ("Admin_Manual")
         *   referenceId: idempotency key
         *   description: mô tả hiển thị trong sổ cái
         *   idempotencyKey: khóa chống trùng lặp (prefix "admin_credit_")
         *
         * IMPORTANT: Repository dùng ACID transaction → đảm bảo:
         *   - Cộng ví VÀ ghi sổ cái xảy ra cùng lúc
         *   - Nếu 1 bước lỗi → cả 2 đều rollback (quay lại trạng thái cũ)
         */
        await _walletRepository.CreditAsync(
            userId: request.UserId,
            currency: normalizedCurrency,
            type: TransactionType.AdminTopup,
            amount: request.Amount,
            referenceSource: "Admin_Manual",
            referenceId: idempotencyKey,
            description: request.Reason ?? $"Admin credited {request.Amount} {request.Currency}",
            idempotencyKey: $"admin_credit_{idempotencyKey}",
            cancellationToken: cancellationToken
        );

        return true;
    }
}
