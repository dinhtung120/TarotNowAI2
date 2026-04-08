using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

// Service random bảo mật cho trộn bộ bài và chọn vật phẩm gacha theo trọng số.
public class RngService : IRngService
{
    /// <summary>
    /// Trộn ngẫu nhiên bộ bài bằng Fisher-Yates với nguồn random bảo mật.
    /// Luồng đảm bảo phân phối đều, tránh thiên lệch thứ tự khi tạo session tarot.
    /// </summary>
    public int[] ShuffleDeck(int deckSize = 78)
    {
        // Khởi tạo bộ bài dạng chỉ số liên tiếp theo kích thước yêu cầu.
        var deck = new int[deckSize];
        for (int i = 0; i < deckSize; i++)
        {
            deck[i] = i;
        }

        // Fisher-Yates: đổi chỗ từ cuối mảng để mỗi phần tử có xác suất ngang nhau.
        for (int i = deckSize - 1; i > 0; i--)
        {
            // Lấy vị trí ngẫu nhiên trong đoạn [0..i] bằng RNG mật mã.
            int j = RandomNumberGenerator.GetInt32(i + 1);

            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }

    /// <summary>
    /// Chọn một item theo trọng số basis points.
    /// Luồng validate dữ liệu đầu vào, sinh entropy audit seed, rồi quét tích lũy trọng số để chọn kết quả.
    /// </summary>
    public GachaRngResult WeightedSelect(System.Collections.Generic.IEnumerable<WeightedItem> items, string? seedForAudit = null)
    {
        var itemList = items.ToList();
        if (!itemList.Any())
        {
            // Edge case: danh sách rỗng không thể chọn ngẫu nhiên.
            throw new System.InvalidOperationException("Cannot select from an empty item list.");
        }

        int totalWeight = itemList.Sum(i => i.WeightBasisPoints);
        if (totalWeight <= 0)
        {
            // Rule nghiệp vụ: tổng trọng số phải dương mới xác định được xác suất.
            throw new System.InvalidOperationException("Total weight must be positive.");
        }

        // Sinh seed ngẫu nhiên để lưu audit/truy vết kết quả quay khi cần.
        byte[] entropy = new byte[16];
        RandomNumberGenerator.Fill(entropy);
        string generatedSeed = Convert.ToHexString(entropy);
        // seedForAudit hiện để mở rộng tương lai, kết quả hiện dùng seed sinh nội bộ.

        int randomPoint = RandomNumberGenerator.GetInt32(totalWeight);

        int currentSum = 0;
        foreach (var item in itemList)
        {
            currentSum += item.WeightBasisPoints;
            if (randomPoint < currentSum)
            {
                return new GachaRngResult
                {
                    SelectedItemId = item.ItemId,
                    RngSeed = generatedSeed
                };
            }
        }

        // Fallback an toàn cho sai số biên hiếm gặp: trả item cuối để luôn có kết quả hợp lệ.
        return new GachaRngResult
        {
            SelectedItemId = itemList.Last().ItemId,
            RngSeed = generatedSeed
        };
    }
}
