/*
 * FILE: RegisterCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler đăng ký tài khoản (Register).
 *
 *   CÁC TEST CASE:
 *   1. Handle_ShouldThrowDomainException_WhenEmailAlreadyExists:
 *      → Email đã tồn tại → DomainException EMAIL_ALREADY_EXISTS
 *   2. Handle_ShouldThrowDomainException_WhenUsernameAlreadyExists:
 *      → Username đã tồn tại → DomainException USERNAME_ALREADY_EXISTS
 *   3. Handle_ShouldCreateUserAndReturnUserId_WhenDataIsValid:
 *      → Happy path: tạo User + hash password + status=Pending → trả userId
 *
 *   KIỂM TRA:
 *   → Unique constraints: email + username phải duy nhất
 *   → Password hash: lưu hash, KHÔNG lưu plaintext
 *   → Initial status: User mới = Pending (chưa verify email)
 */

using Moq;
using TarotNow.Application.Features.Auth.Commands.Register;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Exceptions;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

/// <summary>
/// Test registration: duplicate check, password hash, initial status.
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

    /// <summary>Email đã tồn tại → EMAIL_ALREADY_EXISTS.</summary>
    [Fact]
    public async Task Handle_ShouldThrowDomainException_WhenEmailAlreadyExists()
    {
        var command = new RegisterCommand { Email = "test@example.com" };
        _mockUserRepository.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("EMAIL_ALREADY_EXISTS", exception.ErrorCode);
    }

    /// <summary>Username đã tồn tại → USERNAME_ALREADY_EXISTS.</summary>
    [Fact]
    public async Task Handle_ShouldThrowDomainException_WhenUsernameAlreadyExists()
    {
        var command = new RegisterCommand { Username = "testuser" };
        _mockUserRepository.Setup(r => r.ExistsByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("USERNAME_ALREADY_EXISTS", exception.ErrorCode);
    }

    /// <summary>
    /// Happy path: tạo User thành công.
    /// Verify: password được hash, User lưu DB với status=Pending, trả userId ≠ empty.
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

        _mockPasswordHasher.Setup(h => h.HashPassword(command.Password))
            .Returns("hashed_password_mock"); // Verify hash KHÔNG phải plaintext

        var resultId = await _handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, resultId);
        
        // Verify: User lưu với email, username, hashed password, status Pending
        _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => 
            u.Email == command.Email &&
            u.Username == command.Username &&
            u.PasswordHash == "hashed_password_mock" &&
            u.Status == TarotNow.Domain.Enums.UserStatus.Pending), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
