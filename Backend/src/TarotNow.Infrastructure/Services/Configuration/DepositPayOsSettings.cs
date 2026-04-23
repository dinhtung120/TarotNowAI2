using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

// Adapter expose cấu hình callback PayOS cho application.
public sealed class DepositPayOsSettings : IDepositPayOsSettings
{
    private readonly IOptions<DepositOptions> _options;
    private readonly SystemConfigSnapshotStore _snapshotStore;

    /// <summary>
    /// Khởi tạo settings từ section Deposit + snapshot system configs.
    /// </summary>
    public DepositPayOsSettings(
        IOptions<DepositOptions> options,
        SystemConfigSnapshotStore snapshotStore)
    {
        _options = options;
        _snapshotStore = snapshotStore;
    }

    /// <inheritdoc />
    public string ReturnUrl => _options.Value.ReturnUrl?.Trim() ?? string.Empty;

    /// <inheritdoc />
    public string CancelUrl => _options.Value.CancelUrl?.Trim() ?? string.Empty;

    /// <inheritdoc />
    public int LinkExpiryMinutes
    {
        get
        {
            if (_snapshotStore.TryGetValue("deposit.link_expiry_minutes", out var raw)
                && int.TryParse(raw, out var parsed)
                && parsed > 0)
            {
                return parsed;
            }

            var fallback = _options.Value.LinkExpiryMinutes;
            return fallback > 0 ? fallback : 15;
        }
    }
}
