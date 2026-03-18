using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.AddUserBalance;

/// <summary>
/// Command của Admin để cộng tiền (Gold/Diamond) cho người dùng một cách thủ công.
/// </summary>
public class AddUserBalanceCommand : IRequest<bool>
{
    /// <summary>ID của người dùng được cộng tiền.</summary>
    public Guid UserId { get; set; }

    /// <summary>Loại tiền: gold hoặc diamond.</summary>
    public string Currency { get; set; } = CurrencyType.Gold;

    /// <summary>Số lượng tiền cần cộng.</summary>
    public long Amount { get; set; }

    /// <summary>Lý do cộng tiền (ghi vào nhật ký giao dịch).</summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Idempotency key từ client/admin UI để chống double-submit.
    /// </summary>
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>
/// Handler xử lý việc cộng tiền từ Admin.
/// Sử dụng IWalletRepository để đảm bảo tính an toàn và ghi log giao dịch.
/// </summary>
public class AddUserBalanceCommandHandler : IRequestHandler<AddUserBalanceCommand, bool>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;

    public AddUserBalanceCommandHandler(IWalletRepository walletRepository, IUserRepository userRepository)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(AddUserBalanceCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra User có tồn tại không
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return false;

        var normalizedCurrency = request.Currency?.Trim().ToLowerInvariant() ?? string.Empty;
        if (normalizedCurrency != CurrencyType.Gold && normalizedCurrency != CurrencyType.Diamond)
            throw new BadRequestException("Currency không hợp lệ. Chỉ chấp nhận 'gold' hoặc 'diamond'.");

        var idempotencyKey = request.IdempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new BadRequestException("IdempotencyKey là bắt buộc cho thao tác cộng tiền thủ công.");

        // 2. Thực hiện cộng tiền thông qua Repository (gọi Stored Procedure để an toàn)
        // Chúng ta sử dụng TransactionType.AdminTopup để đánh dấu đây là giao dịch từ Admin.
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
