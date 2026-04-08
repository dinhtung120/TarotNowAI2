using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.CreateUser;

// Handler tạo tài khoản mới và gán vai trò ban đầu theo command từ admin.
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    // DOB mặc định cho tài khoản tạo thủ công khi admin không nhập ngày sinh.
    private static readonly DateTime DefaultDateOfBirth = new(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Khởi tạo handler tạo user.
    /// Luồng xử lý: nhận repository user để kiểm tra trùng/lưu mới và hasher để mã hóa mật khẩu.
    /// </summary>
    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Xử lý command tạo user mới.
    /// Luồng xử lý: chuẩn hóa dữ liệu đầu vào, kiểm tra trùng email/username, tạo entity, gán role, lưu DB.
    /// </summary>
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        var username = request.Username.Trim();
        var displayName = string.IsNullOrWhiteSpace(request.DisplayName) ? username : request.DisplayName.Trim();
        var normalizedRole = NormalizeRole(request.Role);

        if (await _userRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            // Rule nghiệp vụ: email duy nhất toàn hệ thống để tránh xung đột đăng nhập.
            throw new BusinessRuleException("EMAIL_ALREADY_EXISTS", $"The email '{email}' is already registered.");
        }

        if (await _userRepository.ExistsByUsernameAsync(username, cancellationToken))
        {
            // Rule nghiệp vụ: username duy nhất để đảm bảo định danh người dùng ổn định.
            throw new BusinessRuleException("USERNAME_ALREADY_EXISTS", $"The username '{username}' is already taken.");
        }

        // Băm mật khẩu trước khi khởi tạo entity để không lưu plain text xuống database.
        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        var user = new User(
            email: email,
            username: username,
            passwordHash: hashedPassword,
            displayName: displayName,
            dateOfBirth: DefaultDateOfBirth,
            hasConsented: true
        );

        user.Activate();

        if (normalizedRole == UserRole.Admin)
        {
            // Nhánh admin: nâng quyền ngay sau khi tạo.
            user.PromoteToAdmin();
        }
        else if (normalizedRole == UserRole.TarotReader)
        {
            // Nhánh reader: duyệt reader ngay để tài khoản dùng được luồng reader.
            user.ApproveAsReader();
        }

        // Persist user sau khi hoàn tất toàn bộ rule khởi tạo và gán quyền.
        await _userRepository.AddAsync(user, cancellationToken);
        return user.Id;
    }

    /// <summary>
    /// Chuẩn hóa role đầu vào về lowercase và fallback mặc định user khi trống.
    /// Luồng xử lý: trim+lower role, trả user nếu role null/rỗng.
    /// </summary>
    private static string NormalizeRole(string? role)
    {
        var normalizedRole = role?.Trim().ToLowerInvariant();
        return string.IsNullOrWhiteSpace(normalizedRole) ? UserRole.User : normalizedRole;
    }
}
