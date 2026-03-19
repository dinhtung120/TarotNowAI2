/*
 * ===================================================================
 * FILE: UpdateProfileCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Profile.Commands.UpdateProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gác Cổng validation lúc User Đổi Thông Tin.
 * ===================================================================
 */

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
            // Tên Đệm Tối Đa 100 Kí Tự Thôi, Dài quá tràn Layout UI.
            .MaximumLength(100).WithMessage("DisplayName must not exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("DateOfBirth is required.")
            // Hack Não: Sinh Ngày Tương Lai (Bé chưa đẻ) thì cấm chơi bài Tarot.
            .LessThan(DateTime.UtcNow).WithMessage("DateOfBirth must be in the past.");
    }
}
