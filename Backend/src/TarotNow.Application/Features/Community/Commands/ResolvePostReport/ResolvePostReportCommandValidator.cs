using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.ResolvePostReport;

public class ResolvePostReportCommandValidator : AbstractValidator<ResolvePostReportCommand>
{
    public ResolvePostReportCommandValidator()
    {
        RuleFor(x => x.ReportId)
            .NotEmpty();

        RuleFor(x => x.AdminId)
            .NotEmpty();

        RuleFor(x => x.Result)
            .NotEmpty()
            .Must(result => result is ModerationResult.Warn
                or ModerationResult.RemovePost
                or ModerationResult.FreezeAccount
                or ModerationResult.NoAction)
            .WithMessage("Kết quả xử lý không hợp lệ.");

        RuleFor(x => x.AdminNote)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.AdminNote) == false);
    }
}
