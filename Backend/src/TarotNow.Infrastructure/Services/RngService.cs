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

        // Fisher-Yates Shuffle O(N)
        for (int i = deckSize - 1; i > 0; i--)
        {
            int j = Random.Shared.Next(i + 1);
            // Đảo vị trí lá bài
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }
}
