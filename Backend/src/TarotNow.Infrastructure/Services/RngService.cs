using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Core Service chịu trách nhiệm xào bài đơn giản.
/// </summary>
public class RngService : IRngService
{
    public int[] ShuffleDeck(int deckSize = 78)
    {
        // Khởi tạo mảng bài ở vị trí nguyên thủy
        var deck = new int[deckSize];
        for (int i = 0; i < deckSize; i++)
        {
            deck[i] = i;
        }

        // Fisher-Yates Shuffle O(N) với CSPRNG bảo mật.
        // Tại sao dùng RandomNumberGenerator.GetInt32?
        // - Random.Shared (LCG) có tính chu kỳ và có thể bị dự đoán hạt giống (seed).
        // - CSPRNG (Cryptographically Secure Pseudo-Random Number Generator) cung cấp
        //   độ hỗn loạn cao, đáp ứng yêu cầu Provably Fair cho bói toán/cờ bạc.
        for (int i = deckSize - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            // Đảo vị trí lá bài
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }
}
