/*
 * FILE: UpdateReaderProfileCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler cập nhật hồ sơ công khai Reader.
 *
 *   CÁC TEST CASE (4 scenarios):
 *   1. Handle_ProfileNotFound_ThrowsNotFoundException: chưa approved → 404
 *   2. Handle_InvalidPricing_ThrowsBadRequestException: giá ≤ 0 → 400 ([Theory] 0, -1, -100)
 *   3. Handle_PartialUpdate_OnlyUpdatesProvidedFields:
 *      → Chỉ gửi BioVi + DiamondPerQuestion → BioEn/BioZh/Specialties giữ nguyên
 *   4. Handle_FullUpdate_UpdatesAllFields: gửi tất cả → tất cả fields đổi
 *
 *   PATTERN: Partial update (PATCH semantic) → chỉ fields non-null được cập nhật
 *   → Tránh race condition khi 2 tab cùng edit profile
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

/// <summary>
/// Test update reader profile: partial update, pricing validation, i18n fields.
/// </summary>
public class UpdateReaderProfileCommandHandlerTests
{
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    private readonly UpdateReaderProfileCommandHandler _handler;

    public UpdateReaderProfileCommandHandlerTests()
    {
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _handler = new UpdateReaderProfileCommandHandler(_mockProfileRepo.Object);
    }

    /// <summary>Profile chưa tồn tại (chưa approved) → NotFoundException.</summary>
    [Fact]
    public async Task Handle_ProfileNotFound_ThrowsNotFoundException()
    {
        var command = new UpdateReaderProfileCommand { UserId = Guid.NewGuid() };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.UserId.ToString(), default)).ReturnsAsync((ReaderProfileDto)null!);

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Không tìm thấy hồ sơ Reader", ex.Message);
    }

    /// <summary>Giá ≤ 0 → BadRequest (business invariant).</summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task Handle_InvalidPricing_ThrowsBadRequestException(long invalidPrice)
    {
        var userId = Guid.NewGuid();
        var command = new UpdateReaderProfileCommand { UserId = userId, DiamondPerQuestion = invalidPrice };
        var existingProfile = new ReaderProfileDto { UserId = userId.ToString(), DiamondPerQuestion = 50 };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default)).ReturnsAsync(existingProfile);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("lớn hơn 0", ex.Message);
    }

    /// <summary>
    /// Partial update: chỉ BioVi + DiamondPerQuestion đổi.
    /// BioEn, BioZh, Specialties giữ nguyên (PATCH semantic).
    /// </summary>
    [Fact]
    public async Task Handle_PartialUpdate_OnlyUpdatesProvidedFields()
    {
        var userId = Guid.NewGuid();
        var command = new UpdateReaderProfileCommand { UserId = userId, BioVi = "Bio mới tiếng Việt", DiamondPerQuestion = 75 };
        var existingProfile = new ReaderProfileDto
        {
            UserId = userId.ToString(), BioVi = "Bio cũ", BioEn = "Old English Bio",
            BioZh = "旧中文简介", DiamondPerQuestion = 50, Specialties = new List<string> { "love", "career" }
        };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default)).ReturnsAsync(existingProfile);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("Bio mới tiếng Việt", existingProfile.BioVi);
        Assert.Equal(75, existingProfile.DiamondPerQuestion);
        Assert.Equal("Old English Bio", existingProfile.BioEn); // Giữ nguyên
        Assert.Equal("旧中文简介", existingProfile.BioZh); // Giữ nguyên
        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }

    /// <summary>Full update: tất cả fields đổi.</summary>
    [Fact]
    public async Task Handle_FullUpdate_UpdatesAllFields()
    {
        var userId = Guid.NewGuid();
        var command = new UpdateReaderProfileCommand
        {
            UserId = userId, BioVi = "Bio VN", BioEn = "Bio EN", BioZh = "Bio ZH",
            DiamondPerQuestion = 100, Specialties = new List<string> { "tarot", "astrology" }
        };
        var existingProfile = new ReaderProfileDto
        {
            UserId = userId.ToString(), BioVi = "Old", BioEn = "Old", BioZh = "Old",
            DiamondPerQuestion = 50, Specialties = new List<string> { "love" }
        };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default)).ReturnsAsync(existingProfile);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("Bio VN", existingProfile.BioVi);
        Assert.Equal("Bio EN", existingProfile.BioEn);
        Assert.Equal("Bio ZH", existingProfile.BioZh);
        Assert.Equal(100, existingProfile.DiamondPerQuestion);
        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }
}
