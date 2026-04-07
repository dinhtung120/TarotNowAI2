

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

        
        var normalizedCurrency = request.Currency?.Trim().ToLowerInvariant() ?? string.Empty;
        if (normalizedCurrency != CurrencyType.Gold && normalizedCurrency != CurrencyType.Diamond)
            throw new BadRequestException("Currency không hợp lệ. Chỉ chấp nhận 'gold' hoặc 'diamond'.");

        
        var idempotencyKey = request.IdempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new BadRequestException("IdempotencyKey là bắt buộc cho thao tác cộng tiền thủ công.");

        
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
