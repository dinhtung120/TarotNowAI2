

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private const string EmailExistsCode = "EMAIL_ALREADY_EXISTS";
    private const string UsernameExistsCode = "USERNAME_ALREADY_EXISTS";

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await EnsureEmailAndUsernameAreAvailableAsync(request, cancellationToken);
        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        var newUser = BuildUser(request, hashedPassword);
        await _userRepository.AddAsync(newUser, cancellationToken);
        return newUser.Id;
    }

    private async Task EnsureEmailAndUsernameAreAvailableAsync(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new BusinessRuleException(
                EmailExistsCode,
                $"The email '{request.Email}' is already registered.");
        }

        if (await _userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
        {
            throw new BusinessRuleException(
                UsernameExistsCode,
                $"The username '{request.Username}' is already taken.");
        }
    }

    private static User BuildUser(RegisterCommand request, string hashedPassword)
    {
        var newUser = new User(
            email: request.Email,
            username: request.Username,
            passwordHash: hashedPassword,
            displayName: string.IsNullOrWhiteSpace(request.DisplayName) ? request.Username : request.DisplayName,
            dateOfBirth: DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc),
            hasConsented: request.HasConsented
        );

        return newUser;
    }
}
