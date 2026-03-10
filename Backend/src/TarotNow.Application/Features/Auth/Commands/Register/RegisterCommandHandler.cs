using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Exceptions;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Application.Features.Auth.Commands.Register;

/// <summary>
/// Xử lý logic nghiệp vụ cho RegisterCommand.
/// Dùng Argon2id để băm mật khẩu, kiểm tra trùng lặp email/username.
/// Khởi tạo User mới ở trạng thái Pending.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra Email hoặc Username đã tồn tại chưa
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new DomainException("EMAIL_ALREADY_EXISTS", $"The email '{request.Email}' is already registered.");
        }

        if (await _userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
        {
            throw new DomainException("USERNAME_ALREADY_EXISTS", $"The username '{request.Username}' is already taken.");
        }

        // 2. Hash mật khẩu bằng Argon2id
        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        // 3. Tạo Entity User mới
        var newUser = new User(
            email: request.Email,
            username: request.Username,
            passwordHash: hashedPassword,
            displayName: string.IsNullOrWhiteSpace(request.DisplayName) ? request.Username : request.DisplayName,
            dateOfBirth: DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc),
            hasConsented: request.HasConsented
        );

        // 4. Lưu vào Database
        await _userRepository.AddAsync(newUser, cancellationToken);

        // TODO: (Trong bước tiếp theo) Xóa hoặc Publish DomainEvent để gửi Email verify OTP

        // 5. Trả về Id của User mới
        return newUser.Id;
    }
}
