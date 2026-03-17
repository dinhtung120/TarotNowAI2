using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

/// <summary>
/// Unit tests cho UpdateReaderProfileCommandHandler — cập nhật hồ sơ công khai Reader.
///
/// Handler này dùng partial update pattern:
/// → Chỉ cập nhật fields NOT NULL trong request.
/// → Giữ nguyên fields cũ nếu request không gửi.
///
/// Tại sao partial update thay vì full replace?
/// → UX: Reader có thể chỉ muốn đổi giá mà không cần gửi lại bio.
/// → API: PATCH semantic — chỉ gửi fields cần thay đổi.
/// → Tránh race condition khi 2 tab cùng edit profile.
///
/// Chiến lược test:
/// 1. Profile không tồn tại → NotFoundException (chỉ approved reader mới có)
/// 2. Giá âm/0 → BadRequestException (business rule)
/// 3. Partial update → chỉ fields non-null được cập nhật
/// 4. Full update → tất cả fields
/// </summary>
public class UpdateReaderProfileCommandHandlerTests
{
    /* Mock reader profile repository (MongoDB) */
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    private readonly UpdateReaderProfileCommandHandler _handler;

    public UpdateReaderProfileCommandHandlerTests()
    {
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _handler = new UpdateReaderProfileCommandHandler(_mockProfileRepo.Object);
    }

    /// <summary>
    /// TEST CASE: Profile không tồn tại → NotFoundException.
    ///
    /// Khi nào xảy ra?
    /// → User chưa được admin approve → chưa có reader_profiles document.
    /// → User thường (role=user) thử gọi update profile API.
    /// </summary>
    [Fact]
    public async Task Handle_ProfileNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new UpdateReaderProfileCommand { UserId = Guid.NewGuid() };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.UserId.ToString(), default))
            .ReturnsAsync((ReaderProfileDto)null!);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Không tìm thấy hồ sơ Reader", ex.Message);
    }

    /// <summary>
    /// TEST CASE: DiamondPerQuestion <= 0 → BadRequestException.
    ///
    /// Tại sao giá phải dương?
    /// → Business rule: Reader không được set giá 0 hoặc âm.
    ///   Giá 0 → user nhận đọc bài miễn phí → mất ý nghĩa escrow.
    ///   Giá âm → lỗi logic, Reader "trả tiền" cho user.
    /// → Validate ở handler thay vì FluentValidation vì đây là business invariant.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task Handle_InvalidPricing_ThrowsBadRequestException(long invalidPrice)
    {
        // Arrange — profile tồn tại nhưng giá không hợp lệ
        var userId = Guid.NewGuid();
        var command = new UpdateReaderProfileCommand
        {
            UserId = userId,
            DiamondPerQuestion = invalidPrice
        };
        var existingProfile = new ReaderProfileDto
        {
            UserId = userId.ToString(),
            DiamondPerQuestion = 50
        };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default))
            .ReturnsAsync(existingProfile);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("lớn hơn 0", ex.Message);
    }

    /// <summary>
    /// TEST CASE: Partial update — chỉ gửi BioVi và DiamondPerQuestion,
    /// các fields khác (BioEn, BioZh, Specialties) giữ nguyên.
    ///
    /// Tại sao test partial update?
    /// → Đảm bảo handler không ghi đè fields cũ bằng null.
    ///   Nếu bug: BioEn bị set = null → profile bị mất dữ liệu.
    /// </summary>
    [Fact]
    public async Task Handle_PartialUpdate_OnlyUpdatesProvidedFields()
    {
        // Arrange — profile có sẵn bio en/zh
        var userId = Guid.NewGuid();
        var command = new UpdateReaderProfileCommand
        {
            UserId = userId,
            BioVi = "Bio mới tiếng Việt",
            DiamondPerQuestion = 75
            // BioEn, BioZh, Specialties = null → giữ nguyên
        };
        var existingProfile = new ReaderProfileDto
        {
            UserId = userId.ToString(),
            BioVi = "Bio cũ",
            BioEn = "Old English Bio",
            BioZh = "旧中文简介",
            DiamondPerQuestion = 50,
            Specialties = new List<string> { "love", "career" }
        };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default))
            .ReturnsAsync(existingProfile);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert — trả về true
        Assert.True(result);

        // Assert — BioVi và DiamondPerQuestion đã đổi
        Assert.Equal("Bio mới tiếng Việt", existingProfile.BioVi);
        Assert.Equal(75, existingProfile.DiamondPerQuestion);

        // Assert — BioEn, BioZh, Specialties giữ nguyên (QUAN TRỌNG!)
        Assert.Equal("Old English Bio", existingProfile.BioEn);
        Assert.Equal("旧中文简介", existingProfile.BioZh);
        Assert.Equal(new List<string> { "love", "career" }, existingProfile.Specialties);

        // Assert — UpdateAsync được gọi
        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }

    /// <summary>
    /// TEST CASE: Full update — gửi tất cả fields.
    ///
    /// Verify rằng khi gửi đầy đủ, tất cả fields đều được cập nhật đúng.
    /// </summary>
    [Fact]
    public async Task Handle_FullUpdate_UpdatesAllFields()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateReaderProfileCommand
        {
            UserId = userId,
            BioVi = "Bio VN",
            BioEn = "Bio EN",
            BioZh = "Bio ZH",
            DiamondPerQuestion = 100,
            Specialties = new List<string> { "tarot", "astrology" }
        };
        var existingProfile = new ReaderProfileDto
        {
            UserId = userId.ToString(),
            BioVi = "Old",
            BioEn = "Old",
            BioZh = "Old",
            DiamondPerQuestion = 50,
            Specialties = new List<string> { "love" }
        };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default))
            .ReturnsAsync(existingProfile);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Equal("Bio VN", existingProfile.BioVi);
        Assert.Equal("Bio EN", existingProfile.BioEn);
        Assert.Equal("Bio ZH", existingProfile.BioZh);
        Assert.Equal(100, existingProfile.DiamondPerQuestion);
        Assert.Equal(new List<string> { "tarot", "astrology" }, existingProfile.Specialties);

        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }
}
