namespace TarotNow.Application.Features.Gacha.Dtos;

/// <summary>
/// DTO pool gacha hiển thị cho client.
/// </summary>
public sealed class GachaPoolDto
{
    /// <summary>
    /// Mã pool.
    /// </summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Loại pool.
    /// </summary>
    public string PoolType { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Việt.
    /// </summary>
    public string NameVi { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Anh.
    /// </summary>
    public string NameEn { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Trung.
    /// </summary>
    public string NameZh { get; init; } = string.Empty;

    /// <summary>
    /// Mô tả tiếng Việt.
    /// </summary>
    public string DescriptionVi { get; init; } = string.Empty;

    /// <summary>
    /// Mô tả tiếng Anh.
    /// </summary>
    public string DescriptionEn { get; init; } = string.Empty;

    /// <summary>
    /// Mô tả tiếng Trung.
    /// </summary>
    public string DescriptionZh { get; init; } = string.Empty;

    /// <summary>
    /// Currency cost.
    /// </summary>
    public string CostCurrency { get; init; } = string.Empty;

    /// <summary>
    /// Cost amount cho một lượt pull.
    /// </summary>
    public long CostAmount { get; init; }

    /// <summary>
    /// Odds version.
    /// </summary>
    public string OddsVersion { get; init; } = string.Empty;

    /// <summary>
    /// Pity hiện tại của user.
    /// </summary>
    public int UserCurrentPity { get; init; }

    /// <summary>
    /// Bật/tắt pity.
    /// </summary>
    public bool PityEnabled { get; init; }

    /// <summary>
    /// Ngưỡng hard pity.
    /// </summary>
    public int HardPityCount { get; init; }
}

/// <summary>
/// DTO odds của pool.
/// </summary>
public sealed class GachaPoolOddsDto
{
    /// <summary>
    /// Mã pool.
    /// </summary>
    public string PoolCode { get; init; } = string.Empty;

    /// <summary>
    /// Phiên bản odds.
    /// </summary>
    public string OddsVersion { get; init; } = string.Empty;

    /// <summary>
    /// Danh sách reward rates.
    /// </summary>
    public IReadOnlyList<GachaPoolRewardRateDto> Rewards { get; init; } = Array.Empty<GachaPoolRewardRateDto>();
}

/// <summary>
/// DTO reward rate của pool.
/// </summary>
public sealed class GachaPoolRewardRateDto
{
    /// <summary>
    /// Loại reward.
    /// </summary>
    public string Kind { get; init; } = string.Empty;

    /// <summary>
    /// Độ hiếm.
    /// </summary>
    public string Rarity { get; init; } = string.Empty;

    /// <summary>
    /// Currency nếu reward là tiền.
    /// </summary>
    public string? Currency { get; init; }

    /// <summary>
    /// Amount nếu reward là tiền.
    /// </summary>
    public long? Amount { get; init; }

    /// <summary>
    /// Item definition id nếu reward là item.
    /// </summary>
    public Guid? ItemDefinitionId { get; init; }

    /// <summary>
    /// Item code nếu reward là item.
    /// </summary>
    public string? ItemCode { get; init; }

    /// <summary>
    /// Số lượng item cấp ra.
    /// </summary>
    public int QuantityGranted { get; init; }

    /// <summary>
    /// Xác suất basis points.
    /// </summary>
    public int ProbabilityBasisPoints { get; init; }

    /// <summary>
    /// Xác suất phần trăm.
    /// </summary>
    public double ProbabilityPercent { get; init; }

    /// <summary>
    /// Icon reward.
    /// </summary>
    public string? IconUrl { get; init; }

    /// <summary>
    /// Tên tiếng Việt.
    /// </summary>
    public string NameVi { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Anh.
    /// </summary>
    public string NameEn { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Trung.
    /// </summary>
    public string NameZh { get; init; } = string.Empty;
}

/// <summary>
/// DTO phân trang lịch sử pull gacha.
/// </summary>
public sealed class GachaHistoryPageDto
{
    /// <summary>
    /// Trang hiện tại.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Kích thước trang.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Tổng số bản ghi lịch sử.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Danh sách lịch sử pull-level.
    /// </summary>
    public IReadOnlyList<GachaHistoryEntryDto> Items { get; init; } = Array.Empty<GachaHistoryEntryDto>();
}

/// <summary>
/// DTO lịch sử pull-level của gacha.
/// </summary>
public sealed class GachaHistoryEntryDto
{
    /// <summary>
    /// Tham chiếu pull operation.
    /// </summary>
    public Guid PullOperationId { get; init; }

    /// <summary>
    /// Mã pool.
    /// </summary>
    public string PoolCode { get; init; } = string.Empty;

    /// <summary>
    /// Số lượt pull trong operation.
    /// </summary>
    public int PullCount { get; init; }

    /// <summary>
    /// Giá trị pity trước pull.
    /// </summary>
    public int PityBefore { get; init; }

    /// <summary>
    /// Giá trị pity sau pull.
    /// </summary>
    public int PityAfter { get; init; }

    /// <summary>
    /// Cờ reset pity trong operation.
    /// </summary>
    public bool WasPityReset { get; init; }

    /// <summary>
    /// Thời điểm pull UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; init; }

    /// <summary>
    /// Danh sách reward nhận được trong operation.
    /// </summary>
    public IReadOnlyList<GachaHistoryRewardDto> Rewards { get; init; } = Array.Empty<GachaHistoryRewardDto>();
}

/// <summary>
/// DTO reward trong một lịch sử pull.
/// </summary>
public sealed class GachaHistoryRewardDto
{
    /// <summary>
    /// Loại reward.
    /// </summary>
    public string Kind { get; init; } = string.Empty;

    /// <summary>
    /// Độ hiếm reward.
    /// </summary>
    public string Rarity { get; init; } = string.Empty;

    /// <summary>
    /// Currency nếu reward là tiền.
    /// </summary>
    public string? Currency { get; init; }

    /// <summary>
    /// Amount nếu reward là tiền.
    /// </summary>
    public long? Amount { get; init; }

    /// <summary>
    /// Item code nếu reward là item.
    /// </summary>
    public string? ItemCode { get; init; }

    /// <summary>
    /// Quantity cấp ra.
    /// </summary>
    public int QuantityGranted { get; init; }

    /// <summary>
    /// Icon reward.
    /// </summary>
    public string? IconUrl { get; init; }

    /// <summary>
    /// Tên tiếng Việt.
    /// </summary>
    public string NameVi { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Anh.
    /// </summary>
    public string NameEn { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Trung.
    /// </summary>
    public string NameZh { get; init; } = string.Empty;

    /// <summary>
    /// Cờ reward do pity force.
    /// </summary>
    public bool IsHardPityReward { get; init; }
}
