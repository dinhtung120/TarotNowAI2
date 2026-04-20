using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

// Adapter expose cấu hình callback PayOS cho application.
public sealed class DepositPayOsSettings : IDepositPayOsSettings
{
    /// <summary>
    /// Khởi tạo settings từ section Deposit.
    /// </summary>
    public DepositPayOsSettings(IOptions<DepositOptions> options)
    {
        var value = options.Value;

        ReturnUrl = value.ReturnUrl?.Trim() ?? string.Empty;
        CancelUrl = value.CancelUrl?.Trim() ?? string.Empty;
        LinkExpiryMinutes = value.LinkExpiryMinutes > 0 ? value.LinkExpiryMinutes : 15;
    }

    /// <inheritdoc />
    public string ReturnUrl { get; }

    /// <inheritdoc />
    public string CancelUrl { get; }

    /// <inheritdoc />
    public int LinkExpiryMinutes { get; }
}
