
using MediatR;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

public class CreateDepositOrderCommandHandler : IRequestHandler<CreateDepositOrderCommand, CreateDepositOrderResponse>
{
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IDepositPromotionRepository _promotionRepository;

    public CreateDepositOrderCommandHandler(IDepositOrderRepository depositOrderRepository, IDepositPromotionRepository promotionRepository)
    {
        _depositOrderRepository = depositOrderRepository;
        _promotionRepository = promotionRepository;
    }

    public async Task<CreateDepositOrderResponse> Handle(CreateDepositOrderCommand request, CancellationToken cancellationToken)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);
        var clientTransactionToken = BuildClientTransactionToken(request.UserId, normalizedIdempotencyKey);
        var existingOrder = await _depositOrderRepository.GetByTransactionIdAsync(clientTransactionToken, cancellationToken);
        if (existingOrder != null)
        {
            return BuildResponse(existingOrder);
        }

        var baseDiamondAmount = request.AmountVnd / EconomyConstants.VndPerDiamond;
        var activePromotions = await _promotionRepository.GetActivePromotionsAsync(cancellationToken);
        var bonusDiamond = 0L;

        foreach (var promo in activePromotions)
        {
            if (request.AmountVnd >= promo.MinAmountVnd)
            {
                bonusDiamond = promo.BonusDiamond;
                break;
            }
        }

        var totalDiamondAmount = baseDiamondAmount + bonusDiamond;
        var order = new DepositOrder(request.UserId, request.AmountVnd, totalDiamondAmount);
        order.SetClientTransactionToken(clientTransactionToken);

        await _depositOrderRepository.AddAsync(order, cancellationToken);
        return BuildResponse(order);
    }

    private static string NormalizeIdempotencyKey(string? idempotencyKey)
    {
        var normalized = idempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new ArgumentException("IdempotencyKey là bắt buộc.", nameof(idempotencyKey));
        }

        if (normalized.Length > 128)
        {
            throw new ArgumentException("IdempotencyKey tối đa 128 ký tự.", nameof(idempotencyKey));
        }

        return normalized;
    }

    private static string BuildClientTransactionToken(Guid userId, string normalizedIdempotencyKey)
    {
        var input = $"{userId:N}:{normalizedIdempotencyKey}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var hashPrefix = Convert.ToHexString(hash.AsSpan(0, 12)).ToLowerInvariant();
        return $"client_{userId:N}_{hashPrefix}";
    }

    private static CreateDepositOrderResponse BuildResponse(DepositOrder order)
    {
        return new CreateDepositOrderResponse
        {
            OrderId = order.Id,
            AmountVnd = order.AmountVnd,
            DiamondAmount = order.DiamondAmount
        };
    }
}
