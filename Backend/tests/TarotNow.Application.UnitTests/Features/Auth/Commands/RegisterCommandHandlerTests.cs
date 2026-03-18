using Moq;
using TarotNow.Application.Features.Auth.Commands.Register;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Exceptions;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

/// <summary>
/// Unit test cho RegisterCommandHandler, đảm bảo đúng quy tắc nghiệp vụ:
/// 1. Trùng lặp Email/Username
/// 2. Hash Password trước khi lưu
/// 3. Lưu xuống Database thành công.
/// </summary>
public class RegisterCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _handler = new RegisterCommandHandler(_mockUserRepository.Object, _mockPasswordHasher.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowDomainException_WhenEmailAlreadyExists()
    {
        // Arrange
        var command = new RegisterCommand { Email = "test@example.com" };
        _mockUserRepository.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("EMAIL_ALREADY_EXISTS", exception.ErrorCode);
    }

    [Fact]
    public async Task Handle_ShouldThrowDomainException_WhenUsernameAlreadyExists()
    {
        // Arrange
        var command = new RegisterCommand { Username = "testuser" };
        _mockUserRepository.Setup(r => r.ExistsByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("USERNAME_ALREADY_EXISTS", exception.ErrorCode);
    }

    [Fact]
    public async Task Handle_ShouldCreateUserAndReturnUserId_WhenDataIsValid()
    {
        // Arrange
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

        _mockPasswordHasher.Setup(h => h.HashPassword(command.Password))
            .Returns("hashed_password_mock");

        // Act
        var resultId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        
        // Verify User was saved with correct initial status
        _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => 
            u.Email == command.Email &&
            u.Username == command.Username &&
            u.PasswordHash == "hashed_password_mock" &&
            u.Status == TarotNow.Domain.Enums.UserStatus.Pending), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
