using System.Text.Json;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

// Adapter cấu hình hệ thống: pricing, quota, follow-up và policy vận hành.
public sealed partial class SystemConfigSettings : ISystemConfigSettings
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly SystemConfigOptions _options;
    private readonly SystemConfigSnapshotStore _snapshotStore;

    /// <summary>
    /// Khởi tạo settings với options fallback + snapshot từ system_configs.
    /// </summary>
    public SystemConfigSettings(
        IOptions<SystemConfigOptions> options,
        SystemConfigSnapshotStore snapshotStore)
    {
        _options = options.Value;
        _snapshotStore = snapshotStore;
    }
}
