

using Moq;
using TarotNow.Application.Features.Auth.Commands.Register;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

// Unit test cho handler đăng ký tài khoản mới.
public class RegisterCommandHandlerTests
{
    // Mock user repository để kiểm soát kiểm tra trùng email/username và thao tác add user.
    private readonly Mock<IUserRepository> _mockUserRepository;
    // Mock password hasher để xác nhận hash password trước khi lưu.
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    // Handler cần kiểm thử.
    private readonly RegisterCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho RegisterCommandHandler.
    /// Luồng dùng mock để test riêng logic đăng ký không phụ thuộc persistence thật.
    /// </summary>
    public RegisterCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _handler = new RegisterCommandHandler(_mockUserRepository.Object, _mockPasswordHasher.Object);
    }

    /// <summary>
    /// Xác nhận email đã tồn tại trả lỗi EMAIL_ALREADY_EXISTS.
    /// Luồng này bảo vệ rule duy nhất cho địa chỉ email đăng ký.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowBusinessRuleException_WhenEmailAlreadyExists()
    {
        var command = new RegisterCommand { Email = "test@example.com" };
        _mockUserRepository.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("EMAIL_ALREADY_EXISTS", exception.ErrorCode);
    }

    /// <summary>
    /// Xác nhận username đã tồn tại trả lỗi USERNAME_ALREADY_EXISTS.
    /// Luồng này ngăn tạo tài khoản trùng username trong hệ thống.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowBusinessRuleException_WhenUsernameAlreadyExists()
    {
        var command = new RegisterCommand { Username = "testuser" };
        _mockUserRepository.Setup(r => r.ExistsByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("USERNAME_ALREADY_EXISTS", exception.ErrorCode);
    }

    /// <summary>
    /// Xác nhận dữ liệu hợp lệ sẽ tạo user mới và trả về UserId.
    /// Luồng kiểm tra hash password và trạng thái mặc định Pending khi vừa đăng ký.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldCreateUserAndReturnUserId_WhenDataIsValid()
    {
        var command = new RegisterCommand
        {
            Email = "newuser@example.com",
            Username = "newuser",
            Password = "SecurePassword123!",
            DateOfBirth = new DateTime(2000, 1, 1),
            HasConsented = true
        };

        _mockUserRepository.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockUserRepository.Setup(r => r.ExistsByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Stub hash password để assert giá trị lưu vào entity.
        _mockPasswordHasher.Setup(h => h.HashPassword(command.Password))
            .Returns("hashed_password_mock");

        var resultId = await _handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, resultId);

        // Xác minh AddAsync nhận user với dữ liệu đã được chuẩn hóa đúng.
        _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.Email == command.Email &&
            u.Username == command.Username &&
            u.PasswordHash == "hashed_password_mock" &&
            u.Status == TarotNow.Domain.Enums.UserStatus.Pending),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
