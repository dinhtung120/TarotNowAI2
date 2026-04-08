

using FluentAssertions;
using TarotNow.Infrastructure.Services;
using Xunit; 

namespace TarotNow.Application.UnitTests.Rng;

// Unit test cho RngService ở tầng Infrastructure.
public class RngServiceTests
{
    // Service thật vì logic random nội bộ không cần mock phụ thuộc ngoài.
    private readonly RngService _rngService;

    /// <summary>
    /// Khởi tạo fixture RngService.
    /// Luồng này tái sử dụng instance service cho các test shuffle.
    /// </summary>
    public RngServiceTests()
    {
        _rngService = new RngService();
    }

    /// <summary>
    /// Xác nhận ShuffleDeck trả về mảng đủ kích thước và không trùng phần tử.
    /// Luồng này kiểm tra các thuộc tính tối thiểu của một bộ bài đã trộn hợp lệ.
    /// </summary>
    [Fact]
    public void ShuffleDeck_ShouldReturnShuffledArray()
    {
        var deck = _rngService.ShuffleDeck(78);

        deck.Should().NotBeNull();
        deck.Length.Should().Be(78);
        deck.Should().OnlyHaveUniqueItems();
    }
}
