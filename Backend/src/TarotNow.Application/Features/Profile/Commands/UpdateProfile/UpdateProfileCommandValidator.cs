

using FluentValidation;
using System;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("DisplayName is required.")
            
            .MaximumLength(100).WithMessage("DisplayName must not exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("DateOfBirth is required.")
            
            .LessThan(DateTime.UtcNow).WithMessage("DateOfBirth must be in the past.");
    }
}
