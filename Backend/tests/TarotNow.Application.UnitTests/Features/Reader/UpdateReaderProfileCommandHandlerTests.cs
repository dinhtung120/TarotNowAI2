

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

// Unit test cho handler cập nhật hồ sơ Reader.
public class UpdateReaderProfileCommandHandlerTests
{
    // Mock profile repo để điều khiển dữ liệu hồ sơ reader hiện có.
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    // Handler cần kiểm thử.
    private readonly UpdateReaderProfileCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho UpdateReaderProfileCommandHandler.
    /// Luồng dùng mock repository để cô lập logic cập nhật từng trường.
    /// </summary>
    public UpdateReaderProfileCommandHandlerTests()
    {
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _handler = new UpdateReaderProfileCommandHandler(_mockProfileRepo.Object);
    }

    /// <summary>
    /// Xác nhận profile không tồn tại trả NotFoundException.
    /// Luồng này chặn cập nhật trên hồ sơ reader không hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_ProfileNotFound_ThrowsNotFoundException()
    {
        var command = new UpdateReaderProfileCommand { UserId = Guid.NewGuid() };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.UserId.ToString(), default)).ReturnsAsync((ReaderProfileDto)null!);

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Không tìm thấy hồ sơ Reader", ex.Message);
    }

    /// <summary>
    /// Xác nhận giá DiamondPerQuestion không hợp lệ bị từ chối.
    /// Luồng này bảo vệ business rule giá hỏi phải lớn hơn 0.
    /// </summary>
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
    /// Xác nhận cập nhật một phần chỉ thay đổi các trường được cung cấp.
    /// Luồng này kiểm tra không ghi đè nhầm các trường không truyền trong request.
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
        Assert.Equal("Old English Bio", existingProfile.BioEn);
        Assert.Equal("旧中文简介", existingProfile.BioZh);
        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }

    /// <summary>
    /// Xác nhận full update cập nhật đầy đủ các trường hồ sơ.
    /// Luồng này kiểm tra mapping toàn bộ bio, pricing và specialties.
    /// </summary>
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
