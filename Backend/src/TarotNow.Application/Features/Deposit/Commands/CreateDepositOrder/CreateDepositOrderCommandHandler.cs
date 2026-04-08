
using MediatR;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

// Handler tạo deposit order và áp dụng promotion đang hoạt động.
public class CreateDepositOrderCommandHandler : IRequestHandler<CreateDepositOrderCommand, CreateDepositOrderResponse>
{
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler create deposit order.
    /// Luồng xử lý: nhận repository đơn nạp và promotion để xử lý idempotency + tính kim cương thưởng.
    /// </summary>
    public CreateDepositOrderCommandHandler(IDepositOrderRepository depositOrderRepository, IDepositPromotionRepository promotionRepository)
    {
        _depositOrderRepository = depositOrderRepository;
        _promotionRepository = promotionRepository;
    }

    /// <summary>
    /// Xử lý command tạo đơn nạp tiền.
    /// Luồng xử lý: chuẩn hóa idempotency key, kiểm tra đơn đã tồn tại theo transaction token, tính base diamond + bonus promotion, tạo order mới và lưu repository.
    /// </summary>
    public async Task<CreateDepositOrderResponse> Handle(CreateDepositOrderCommand request, CancellationToken cancellationToken)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);
        var clientTransactionToken = BuildClientTransactionToken(request.UserId, normalizedIdempotencyKey);
        var existingOrder = await _depositOrderRepository.GetByTransactionIdAsync(clientTransactionToken, cancellationToken);
        if (existingOrder != null)
        {
            // Idempotency: nếu client gửi lại cùng key thì trả đơn đã tạo trước đó.
            return BuildResponse(existingOrder);
        }

        var baseDiamondAmount = request.AmountVnd / EconomyConstants.VndPerDiamond;
        var activePromotions = await _promotionRepository.GetActivePromotionsAsync(cancellationToken);
        var bonusDiamond = 0L;

        foreach (var promo in activePromotions)
        {
            if (request.AmountVnd >= promo.MinAmountVnd)
            {
                // Lấy bonus đầu tiên thỏa ngưỡng vì danh sách promotion đã được sắp theo ưu tiên.
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

    /// <summary>
    /// Chuẩn hóa và validate idempotency key của request.
    /// Luồng xử lý: trim chuỗi, chặn giá trị rỗng và giới hạn tối đa 128 ký tự.
    /// </summary>
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

    /// <summary>
    /// Dựng client transaction token ổn định từ user id + idempotency key.
    /// Luồng xử lý: băm SHA-256 chuỗi đầu vào, lấy prefix hash và ghép thành token chuẩn lưu trong order.
    /// </summary>
    private static string BuildClientTransactionToken(Guid userId, string normalizedIdempotencyKey)
    {
        var input = $"{userId:N}:{normalizedIdempotencyKey}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var hashPrefix = Convert.ToHexString(hash.AsSpan(0, 12)).ToLowerInvariant();
        return $"client_{userId:N}_{hashPrefix}";
    }

    /// <summary>
    /// Map entity DepositOrder sang response DTO.
    /// Luồng xử lý: trả về order id, amount VND và diamond amount.
    /// </summary>
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
