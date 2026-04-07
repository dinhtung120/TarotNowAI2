using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

public class UpdateReaderStatusCommandValidator : AbstractValidator<UpdateReaderStatusCommand>
{
    public UpdateReaderStatusCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(status => ReaderOnlineStatus.TryNormalize(status, out _))
            .WithMessage("Status không hợp lệ.");

        RuleFor(x => x.Status)
            .Must(status =>
            {
                if (!ReaderOnlineStatus.TryNormalize(status, out var normalized))
                {
                    return false;
                }

                return normalized != ReaderOnlineStatus.Online;
            })
            .WithMessage("Không thể đặt status thủ công là 'online'.");
    }
}
