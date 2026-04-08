
using System;

namespace TarotNow.Domain.Entities;

// Entity log kết quả quay gacha để truy vết xác suất, pity và idempotency theo từng lượt quay.
public class GachaRewardLog
{
    // Định danh log quay.
    public Guid Id { get; private set; }

    // Người dùng thực hiện lượt quay.
    public Guid UserId { get; private set; }

    // Banner được quay.
    public Guid BannerId { get; private set; }

    // Item trúng thưởng.
    public Guid BannerItemId { get; private set; }

    // Phiên bản odds dùng tại thời điểm quay.
    public string OddsVersion { get; private set; } = string.Empty;

    // Chi phí Diamond đã tiêu cho lượt quay.
    public long SpentDiamond { get; private set; }

    // Độ hiếm của phần thưởng.
    public string Rarity { get; private set; } = string.Empty;

    // Loại phần thưởng.
    public string RewardType { get; private set; } = string.Empty;

    // Giá trị phần thưởng.
    public string RewardValue { get; private set; } = string.Empty;

    // Pity count ngay trước lượt quay.
    public int PityCountAtSpin { get; private set; }

    // Cờ cho biết lượt quay này có kích hoạt pity hay không.
    public bool WasPityTriggered { get; private set; }

    // Seed random để phục vụ audit khi cần.
    public string? RngSeed { get; private set; }

    // Khóa idempotency của lượt quay.
    public string IdempotencyKey { get; private set; } = string.Empty;

    // Thời điểm tạo log.
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF dựng entity từ dữ liệu đã lưu.
    /// </summary>
    protected GachaRewardLog() { }

    /// <summary>
    /// Constructor nội bộ tạo log quay từ request đã chuẩn hóa.
    /// Luồng xử lý: gán toàn bộ dữ liệu truy vết và chốt CreatedAt tại thời điểm tạo.
    /// </summary>
    private GachaRewardLog(GachaRewardLogCreateRequest request)
    {
        Id = Guid.NewGuid();
        UserId = request.UserId;
        BannerId = request.BannerId;
        BannerItemId = request.BannerItemId;
        OddsVersion = request.OddsVersion;
        SpentDiamond = request.SpentDiamond;
        Rarity = request.Rarity;
        RewardType = request.RewardType;
        RewardValue = request.RewardValue;
        PityCountAtSpin = request.PityCountAtSpin;
        WasPityTriggered = request.WasPityTriggered;
        RngSeed = request.RngSeed;
        IdempotencyKey = request.IdempotencyKey;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Factory tạo mới log quay gacha để giữ một điểm khởi tạo nhất quán.
    /// Luồng xử lý: nhận request đầu vào và trả entity GachaRewardLog đã được khởi tạo đầy đủ.
    /// </summary>
    public static GachaRewardLog Create(GachaRewardLogCreateRequest request)
    {
        return new GachaRewardLog(request);
    }
}

// Request tạo log quay gacha mang đầy đủ dữ liệu đối soát của một lượt quay.
public sealed record GachaRewardLogCreateRequest(
    Guid UserId,
    Guid BannerId,
    Guid BannerItemId,
    string OddsVersion,
    long SpentDiamond,
    string Rarity,
    string RewardType,
    string RewardValue,
    int PityCountAtSpin,
    bool WasPityTriggered,
    string? RngSeed,
    string IdempotencyKey);
