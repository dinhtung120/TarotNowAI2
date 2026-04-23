using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Common.Services;

// Tính toán số lượt follow-up miễn phí và chi phí follow-up theo độ mạnh bộ bài.
public class FollowupPricingService
{
    private readonly ISystemConfigSettings _settings;

    public FollowupPricingService(ISystemConfigSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Mô phỏng card level từ card id để áp dụng rule tính giá follow-up.
    /// Luồng xử lý: kiểm tra card id hợp lệ, map nhóm major arcana, còn lại map theo modulo.
    /// </summary>
    public int GetMockCardLevel(int cardId)
    {
        if (cardId < 0 || cardId > 77)
        {
            // Edge case id ngoài bộ 78 lá: fallback level thấp nhất để không làm vỡ luồng tính phí.
            return 1;
        }

        if (cardId <= 21)
        {
            // Nhóm major arcana được ưu tiên level cao hơn theo thứ tự xuất hiện.
            return 10 + cardId;
        }

        // Nhóm minor arcana: quy đổi level tuần hoàn theo 14 lá mỗi bộ.
        return (cardId % 14) + 1;
    }

    /// <summary>
    /// Tính số lượt follow-up miễn phí dựa trên bộ bài đã rút.
    /// Luồng xử lý: parse JSON card id, lấy level cao nhất, đối chiếu ngưỡng để trả số lượt free.
    /// </summary>
    public int CalculateFreeSlotsAllowed(string cardsDrawnJson)
    {
        if (string.IsNullOrWhiteSpace(cardsDrawnJson))
        {
            // Edge case thiếu dữ liệu bộ bài: không cấp lượt miễn phí.
            return 0;
        }

        try
        {
            var cardIds = ReadingDrawnCardCodec.ExtractCardIds(cardsDrawnJson);
            if (cardIds.Length == 0)
            {
                // Edge case JSON hợp lệ nhưng rỗng: không có cơ sở tính ưu đãi.
                return 0;
            }

            var highestLevel = cardIds.Max(GetMockCardLevel);

            if (highestLevel >= _settings.FollowupFreeSlotThresholdHigh)
            {
                // Rule business: bộ bài cấp rất cao được 3 lượt follow-up miễn phí.
                return 3;
            }

            if (highestLevel >= _settings.FollowupFreeSlotThresholdMid)
            {
                // Rule business: cấp trung cao được 2 lượt miễn phí.
                return 2;
            }

            if (highestLevel >= _settings.FollowupFreeSlotThresholdLow)
            {
                // Rule business: cấp cơ bản đủ điều kiện 1 lượt miễn phí.
                return 1;
            }

            return 0;
        }
        catch
        {
            // JSON sai định dạng: fail-safe về 0 để tránh cấp quyền lợi sai.
            return 0;
        }
    }

    /// <summary>
    /// Tính giá lượt follow-up kế tiếp dựa trên số lượt đã dùng và số lượt miễn phí.
    /// Luồng xử lý: chặn vượt giới hạn, trừ phần miễn phí, map sang tier giá và clamp chỉ số tier.
    /// </summary>
    public int CalculateNextFollowupCost(string cardsDrawnJson, int currentFollowupCount)
    {
        var maxAllowed = _settings.FollowupMaxAllowed;
        if (currentFollowupCount >= maxAllowed)
        {
            // Rule chặn cứng để bảo vệ cân bằng kinh tế và giới hạn phiên đọc bài.
            throw new InvalidOperationException($"Đã đạt giới hạn tối đa {maxAllowed} câu hỏi phụ cho phiên rút bài này.");
        }

        var freeSlots = CalculateFreeSlotsAllowed(cardsDrawnJson);
        if (currentFollowupCount < freeSlots)
        {
            // Nhánh còn trong quota miễn phí: không tính phí lượt kế tiếp.
            return 0;
        }

        var paidTierIndex = currentFollowupCount - freeSlots;
        var priceTiers = _settings.FollowupPriceTiers;
        if (paidTierIndex >= priceTiers.Count)
        {
            // Edge case vượt số bậc giá khai báo: giữ mức giá trần cuối mảng.
            paidTierIndex = priceTiers.Count - 1;
        }

        // Trả chi phí theo bậc tương ứng sau khi trừ phần miễn phí.
        return priceTiers[paidTierIndex];
    }
}
