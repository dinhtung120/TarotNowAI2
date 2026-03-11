using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace TarotNow.Domain.Services;

/// <summary>
/// Domain Service tính toán Logic Follow-up Chat Pricing (Phase 1.5).
/// Bảng giá tính theo số lần follow-up: [1, 2, 4, 8, 16] Diamond.
/// Miễn phí n slot đầu tiên dựa trên cấp độ lá bài cao nhất trong phiên trải bài.
/// </summary>
public class FollowupPricingService
{
    private static readonly int[] PriceTiers = new[] { 1, 2, 4, 8, 16 };
    public const int MAX_FOLLOWUPS_ALLOWED = 5;

    // Giả lập cấp độ lá bài dựa trên ID (0-77). Tạm thời ánh xạ:
    // 0-21 (Major Arcana): Level 10-21
    // 22-77 (Minor Arcana): Level 1-10
    // Trong thực tế, DB thẻ bài (Cards table) sẽ chứa thuộc tính 'Level'.
    public int GetMockCardLevel(int cardId)
    {
        if (cardId < 0 || cardId > 77) return 1;

        if (cardId <= 21)
        {
            // Các lá ẩn chính rất mạnh (The Fool -> The World)
            // Ánh xạ thành Level 10 tới Level 21
            return 10 + cardId; // Vd: The World (21) => Level 31
        }
        else
        {
            // Các lá ẩn phụ (Wands, Cups, Swords, Pentacles)
            // Ánh xạ thành Level 1 tới 10 (tùy lá)
            return (cardId % 14) + 1; // 1 (Ace) tới 14 (King)
        }
    }

    /// <summary>
    /// Ước tính số lượng câu hỏi Follow-up miễn phí dựa trên bộ bài đã bốc.
    /// Level >= 6 -> 1 Free Slot
    /// Level >= 11 -> 2 Free Slots
    /// Level >= 16 -> 3 Free Slots
    /// </summary>
    public int CalculateFreeSlotsAllowed(string cardsDrawnJson)
    {
        if (string.IsNullOrWhiteSpace(cardsDrawnJson)) return 0;

        try
        {
            var cardIds = JsonSerializer.Deserialize<int[]>(cardsDrawnJson) ?? Array.Empty<int>();
            if (!cardIds.Any()) return 0;

            int highestLevel = cardIds.Max(GetMockCardLevel);

            if (highestLevel >= 16) return 3;
            if (highestLevel >= 11) return 2;
            if (highestLevel >= 6) return 1;

            return 0;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Tính toán chi phí Diamond cho lượt hỏi Follow-up tiếp theo.
    /// Giá trị trả về:
    /// 0 -> Miễn phí (Do Free Slot bao phủ)
    /// > 0 -> Giá Diamond theo Progress Tier
    /// Ngoại lệ -> Đã đạt giới hạn Hard Cap
    /// </summary>
    public int CalculateNextFollowupCost(string cardsDrawnJson, int currentFollowupCount)
    {
        if (currentFollowupCount >= MAX_FOLLOWUPS_ALLOWED)
        {
            throw new InvalidOperationException($"Đã đạt giới hạn tối đa {MAX_FOLLOWUPS_ALLOWED} câu hỏi phụ cho phiên rút bài này.");
        }

        int freeSlots = CalculateFreeSlotsAllowed(cardsDrawnJson);

        // Nếu lượt hỏi hiện tại (đếm từ 0) vẫn nằm trong số lượng Free Slots The Vũ Trụ ban tặng
        if (currentFollowupCount < freeSlots)
        {
            return 0; // Miễn phí Diamond (vẫn trừ AI Quota ở Application Layer)
        }

        // Nếu đã dùng hết Free Slots, bắt đầu tính phí theo Paid Tiers
        // Vị trí mảng Paid Tier sẽ tính từ 0.
        int paidTierIndex = currentFollowupCount - freeSlots;

        // Đề phòng out of bounds (mặc dù Hard Cap = 5 đã bao phủ mảng PriceTiers size 5)
        if (paidTierIndex >= PriceTiers.Length)
        {
            paidTierIndex = PriceTiers.Length - 1;
        }

        return PriceTiers[paidTierIndex];
    }
}
