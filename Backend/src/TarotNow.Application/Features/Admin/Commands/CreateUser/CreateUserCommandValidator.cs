using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private static readonly HashSet<string> AllowedRoles = new(StringComparer.Ordinal)
    {
        UserRole.User,
        UserRole.TarotReader,
        UserRole.Admin,
    };

    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain alphanumeric characters and underscores.");

        RuleFor(x => x.DisplayName)
            .MaximumLength(100).WithMessage("DisplayName cannot exceed 100 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => AllowedRoles.Contains((role ?? string.Empty).Trim().ToLowerInvariant()))
            .WithMessage("Role must be one of: user, tarot_reader, admin.");
    }
}
