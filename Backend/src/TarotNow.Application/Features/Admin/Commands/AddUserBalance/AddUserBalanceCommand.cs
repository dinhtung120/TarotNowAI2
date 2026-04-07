

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.AddUserBalance;

public class AddUserBalanceCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public string Currency { get; set; } = CurrencyType.Gold;

    public long Amount { get; set; }

    public string? Reason { get; set; }

    public string IdempotencyKey { get; set; } = string.Empty;
}

public class AddUserBalanceCommandHandler : IRequestHandler<AddUserBalanceCommand, bool>
{
    private const string AdminReferenceSource = "Admin_Manual";
    private const string IdempotencyPrefix = "admin_credit_";
    private const string InvalidCurrencyMessage = "Currency không hợp lệ. Chỉ chấp nhận 'gold' hoặc 'diamond'.";
    private const string MissingIdempotencyKeyMessage = "IdempotencyKey là bắt buộc cho thao tác cộng tiền thủ công.";

    
    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;

    public AddUserBalanceCommandHandler(IWalletRepository walletRepository, IUserRepository userRepository)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(AddUserBalanceCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return false;

        var normalizedCurrency = NormalizeAndValidateCurrency(request.Currency);
        var idempotencyKey = NormalizeAndValidateIdempotencyKey(request.IdempotencyKey);
        await CreditUserAsync(request, normalizedCurrency, idempotencyKey, cancellationToken);
        return true;
    }

    private static string NormalizeAndValidateCurrency(string? currency)
    {
        var normalizedCurrency = currency?.Trim().ToLowerInvariant() ?? string.Empty;
        if (normalizedCurrency == CurrencyType.Gold || normalizedCurrency == CurrencyType.Diamond)
        {
            return normalizedCurrency;
        }

        throw new BadRequestException(InvalidCurrencyMessage);
    }

    private static string NormalizeAndValidateIdempotencyKey(string? idempotencyKey)
    {
        var normalizedKey = idempotencyKey?.Trim();
        if (!string.IsNullOrWhiteSpace(normalizedKey))
        {
            return normalizedKey;
        }

        throw new BadRequestException(MissingIdempotencyKeyMessage);
    }

    private async Task CreditUserAsync(
        AddUserBalanceCommand request,
        string normalizedCurrency,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        await _walletRepository.CreditAsync(
            userId: request.UserId,
            currency: normalizedCurrency,
            type: TransactionType.AdminTopup,
            amount: request.Amount,
            referenceSource: AdminReferenceSource,
            referenceId: idempotencyKey,
            description: request.Reason ?? $"Admin credited {request.Amount} {request.Currency}",
            idempotencyKey: IdempotencyPrefix + idempotencyKey,
            cancellationToken: cancellationToken);
    }
}
