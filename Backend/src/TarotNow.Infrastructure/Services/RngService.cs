/*
 * FILE: RngService.cs
 * MỤC ĐÍCH: Service xào bài Tarot — random ngẫu nhiên có kiểm soát.
 *
 *   TẠI SAO DÙNG CSPRNG THAY VÌ RANDOM THƯỜNG?
 *   → Random.Shared (System.Random) dùng LCG (Linear Congruential Generator).
 *     → Có tính chu kỳ, có thể dự đoán nếu biết seed → KHÔNG công bằng.
 *   → RandomNumberGenerator (CSPRNG) dùng entropy từ OS → không thể dự đoán.
 *     → Đáp ứng yêu cầu "Provably Fair" cho bói toán/game.
 *
 *   THUẬT TOÁN: Fisher-Yates Shuffle (Durstenfeld variant)
 *   → Thời gian: O(N) — chỉ duyệt 1 lần qua mảng.
 *   → Không gian: O(1) — shuffle in-place (không tạo mảng phụ).
 *   → Mỗi hoán vị (permutation) có xác suất bằng nhau → đảm bảo công bằng.
 *
 *   DECK MẶC ĐỊNH: 78 lá (Tarot tiêu chuẩn: 22 Major Arcana + 56 Minor Arcana).
 */

using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Implement IRngService — xào bài Tarot bằng CSPRNG + Fisher-Yates.
/// </summary>
public class RngService : IRngService
{
    /// <summary>
    /// Xào bộ bài: tạo mảng [0, 1, ..., deckSize-1] → shuffle ngẫu nhiên.
    /// 
    /// Fisher-Yates Shuffle:
    /// → Duyệt từ cuối→đầu: mỗi vị trí i, đổi với vị trí ngẫu nhiên j ∈ [0, i].
    /// → RandomNumberGenerator.GetInt32(n): sinh số ngẫu nhiên trong [0, n) — CSPRNG.
    /// → Tuple swap (deck[i], deck[j]) = (deck[j], deck[i]): C# 7.0 syntax.
    /// 
    /// Trả về: mảng int[] đã xào — index = vị trí, value = card ID.
    /// </summary>
    public int[] ShuffleDeck(int deckSize = 78)
    {
        // Tạo bộ bài chưa xào: [0, 1, 2, ..., 77]
        var deck = new int[deckSize];
        for (int i = 0; i < deckSize; i++)
        {
            deck[i] = i;
        }

        // Fisher-Yates Shuffle — O(N), in-place, CSPRNG
        for (int i = deckSize - 1; i > 0; i--)
        {
            // Sinh số ngẫu nhiên j ∈ [0, i] bằng CSPRNG
            int j = RandomNumberGenerator.GetInt32(i + 1);
            // Đổi vị trí lá bài
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }
}
