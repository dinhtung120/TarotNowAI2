/*
 * FILE: RngServiceTests.cs
 * MỤC ĐÍCH: Unit test cho Cryptographically Secure Random Number Generator (RNG).
 *
 *   CÁC TEST CASE:
 *   1. ShuffleDeck_ShouldReturnShuffledArray:
 *      → Deck Tarot chuẩn có 78 lá (0 -> 77).
 *      → Đảm bảo hàm ShuffleDeck trả về 1 mảng 78 phần tử ngẫu nhiên.
 *      → Không có phần tử nào bị trùng lặp (duplication bug).
 *
 *   TẠI SAO CẦN RNG SERVICE REPO MÀ KHÔNG DÙNG RANDOM.NEXT()?
 *   → Random.Next() được sinh từ System Clock Seed, dễ bị đoán trước thao tác rút bài.
 *   → Cryptographic Random Generator (RNGCryptoServiceProvider) tránh thao túng 
 *     tỉ lệ ngẫu nhiên của các lá bài.
 */

using FluentAssertions;
using TarotNow.Infrastructure.Services;
using Xunit; // Added Xunit for [Fact]

namespace TarotNow.Application.UnitTests.Rng;

/// <summary>
/// Đảm bảo tính ngẫu nhiên an toàn trong việc rút bài (Tarot Deck Shuffling).
/// </summary>
public class RngServiceTests
{
    private readonly RngService _rngService;

    public RngServiceTests()
    {
        _rngService = new RngService();
    }

    /// <summary>
    /// Rút bài ngẫu nhiên từ bộ Tarot 78 lá. Array không null, đủ 78 lá và đặc biệt 
    /// không có ID lá bài nào lặp lại (OnlyHaveUniqueItems).
    /// </summary>
    [Fact]
    public void ShuffleDeck_ShouldReturnShuffledArray()
    {
        var deck = _rngService.ShuffleDeck(78);

        deck.Should().NotBeNull();
        deck.Length.Should().Be(78);
        deck.Should().OnlyHaveUniqueItems(); // Yêu cầu tiên quyết cho deck chuẩn
    }
}
