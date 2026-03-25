using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private static readonly DateTime DefaultDateOfBirth = new(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        var username = request.Username.Trim();
        var displayName = string.IsNullOrWhiteSpace(request.DisplayName) ? username : request.DisplayName.Trim();
        var normalizedRole = NormalizeRole(request.Role);

        if (await _userRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            throw new BusinessRuleException("EMAIL_ALREADY_EXISTS", $"The email '{email}' is already registered.");
        }

        if (await _userRepository.ExistsByUsernameAsync(username, cancellationToken))
        {
            throw new BusinessRuleException("USERNAME_ALREADY_EXISTS", $"The username '{username}' is already taken.");
        }

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
            user.PromoteToAdmin();
        }
        else if (normalizedRole == UserRole.TarotReader)
        {
            user.ApproveAsReader();
        }

        await _userRepository.AddAsync(user, cancellationToken);
        return user.Id;
    }

    private static string NormalizeRole(string? role)
    {
        var normalizedRole = role?.Trim().ToLowerInvariant();
        return string.IsNullOrWhiteSpace(normalizedRole) ? UserRole.User : normalizedRole;
    }
}
