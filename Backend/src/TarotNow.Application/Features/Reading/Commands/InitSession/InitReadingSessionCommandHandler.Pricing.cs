using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

public partial class InitReadingSessionCommandHandler
{
    // Cấu trúc dữ liệu tạm để truyền thông tin pricing giữa các bước khởi tạo session.
    private readonly record struct SessionPricing(
        long CostGold,
        long CostDiamond,
        string CurrencyUsed,
        long AmountCharged);

    /// <summary>
    /// Bảo đảm user tồn tại trước khi mở phiên reading.
    /// Luồng xử lý: tải user theo id và ném NotFound nếu không có bản ghi.
    /// </summary>
    private async Task EnsureUserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            // Edge case: user không tồn tại thì dừng ngay để tránh phát sinh giao dịch/định giá vô nghĩa.
            throw new NotFoundException("User not found");
        }
    }

    /// <summary>
    /// Resolve pricing cuối cùng cho phiên reading.
    /// Luồng xử lý: chuẩn hóa currency, kiểm tra giới hạn daily, lấy base price theo spread và trả pricing kết quả.
    /// </summary>
    private async Task<SessionPricing> ResolvePricingAsync(
        InitReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        var currency = NormalizeCurrency(request.Currency);
        await EnsureDailyCardLimitAsync(request, cancellationToken);
        // Khóa logic daily-card trước khi tính phí để chặn mở phiên vượt giới hạn miễn phí trong ngày.

        var (costGold, costDiamond) = ResolveBasePricing(request.SpreadType, currency);
        var amountCharged = costGold > 0 ? costGold : costDiamond;
        return new SessionPricing(costGold, costDiamond, currency, amountCharged);
    }

    /// <summary>
    /// Lấy giá cơ sở theo spread type và currency.
    /// Luồng xử lý: map spread sang cấu hình giá tương ứng, sau đó chọn giá gold/diamond theo currency đã chuẩn hóa.
    /// </summary>
    private (long CostGold, long CostDiamond) ResolveBasePricing(string spreadType, string currency)
    {
        return spreadType switch
        {
            SpreadType.Spread3Cards => GetConfiguredPrice(
                currency,
                _systemConfigSettings.Spread3GoldCost,
                _systemConfigSettings.Spread3DiamondCost),
            SpreadType.Spread5Cards => GetConfiguredPrice(
                currency,
                _systemConfigSettings.Spread5GoldCost,
                _systemConfigSettings.Spread5DiamondCost),
            SpreadType.Spread10Cards => GetConfiguredPrice(
                currency,
                _systemConfigSettings.Spread10GoldCost,
                _systemConfigSettings.Spread10DiamondCost),
            _ => (0L, 0L)
        };
    }

    /// <summary>
    /// Kiểm tra giới hạn rút Daily1Card trong ngày.
    /// Luồng xử lý: chỉ áp dụng cho spread Daily1Card; nếu user đã rút trong ngày thì ném BadRequest.
    /// </summary>
    private async Task EnsureDailyCardLimitAsync(
        InitReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        if (request.SpreadType != SpreadType.Daily1Card)
        {
            // Chỉ spread daily miễn phí bị giới hạn theo ngày; các spread khác bỏ qua check này.
            return;
        }

        var alreadyDrawn = await _readingRepo.HasDrawnDailyCardAsync(request.UserId, DateTime.UtcNow, cancellationToken);
        if (alreadyDrawn)
        {
            // Business rule: mỗi ngày chỉ được rút một Daily1Card miễn phí.
            throw new BadRequestException("You have already drawn your free daily card today. Please try other spreads.");
        }
    }

    /// <summary>
    /// Chọn giá theo currency đã chuẩn hóa.
    /// Luồng xử lý: nếu thanh toán diamond thì trả costDiamond, ngược lại dùng costGold.
    /// </summary>
    private static (long CostGold, long CostDiamond) GetConfiguredPrice(
        string currency,
        long goldPrice,
        long diamondPrice)
    {
        return currency == CurrencyType.Diamond
            ? (0, diamondPrice)
            : (goldPrice, 0);
    }

    /// <summary>
    /// Chuẩn hóa currency đầu vào.
    /// Luồng xử lý: trim + lower và fallback về gold khi input khác diamond.
    /// </summary>
    private static string NormalizeCurrency(string? currency)
    {
        var normalized = currency?.Trim().ToLowerInvariant();
        return normalized == CurrencyType.Diamond ? CurrencyType.Diamond : CurrencyType.Gold;
    }
}
