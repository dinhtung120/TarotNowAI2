using FluentValidation;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.Helpers;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

// Validator cho command cập nhật profile.
public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    private const int MinAccountNumberLength = 6;
    private const int MaxAccountNumberLength = 32;

    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu cập nhật profile.
    /// Luồng xử lý: kiểm tra trường cơ bản và bộ thông tin payout bank khi người dùng gửi cập nhật.
    /// </summary>
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("DisplayName is required.")
            .MaximumLength(100)
            .WithMessage("DisplayName must not exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("DateOfBirth is required.")
            .LessThan(DateTime.UtcNow)
            .WithMessage("DateOfBirth must be in the past.");

        RuleFor(x => x.PayoutBankName)
            .MaximumLength(120)
            .When(x => string.IsNullOrWhiteSpace(x.PayoutBankName) == false);

        RuleFor(x => x.PayoutBankBin)
            .MaximumLength(6)
            .When(x => string.IsNullOrWhiteSpace(x.PayoutBankBin) == false);

        RuleFor(x => x.PayoutBankAccountNumber)
            .Must(value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }

                var normalized = value.Trim();
                return normalized.Length >= MinAccountNumberLength
                       && normalized.Length <= MaxAccountNumberLength
                       && normalized.All(char.IsDigit);
            })
            .WithMessage("Số tài khoản không hợp lệ.");

        RuleFor(x => x.PayoutBankAccountHolder)
            .Must(value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }

                return AccountHolderNameValidator.IsValidUppercaseNoAccent(value);
            })
            .WithMessage("Tên chủ tài khoản phải là chữ hoa không dấu.");

        RuleFor(x => x)
            .Custom((command, context) =>
            {
                if (HasAnyPayoutInput(command) == false)
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(command.PayoutBankName))
                {
                    context.AddFailure(nameof(command.PayoutBankName), "Tên ngân hàng là bắt buộc.");
                }

                if (string.IsNullOrWhiteSpace(command.PayoutBankBin))
                {
                    context.AddFailure(nameof(command.PayoutBankBin), "Mã BIN ngân hàng là bắt buộc.");
                }

                if (string.IsNullOrWhiteSpace(command.PayoutBankAccountNumber))
                {
                    context.AddFailure(nameof(command.PayoutBankAccountNumber), "Số tài khoản là bắt buộc.");
                }

                if (string.IsNullOrWhiteSpace(command.PayoutBankAccountHolder))
                {
                    context.AddFailure(nameof(command.PayoutBankAccountHolder), "Tên chủ tài khoản là bắt buộc.");
                }

                if (string.IsNullOrWhiteSpace(command.PayoutBankBin) == false
                    && string.IsNullOrWhiteSpace(command.PayoutBankName) == false
                    && VietnamBankCatalog.IsValidPair(command.PayoutBankBin, command.PayoutBankName) == false)
                {
                    context.AddFailure(nameof(command.PayoutBankBin), "Ngân hàng không nằm trong danh mục hỗ trợ payout.");
                }
            });
    }

    private static bool HasAnyPayoutInput(UpdateProfileCommand command)
    {
        return string.IsNullOrWhiteSpace(command.PayoutBankName) == false
               || string.IsNullOrWhiteSpace(command.PayoutBankBin) == false
               || string.IsNullOrWhiteSpace(command.PayoutBankAccountNumber) == false
               || string.IsNullOrWhiteSpace(command.PayoutBankAccountHolder) == false;
    }
}
