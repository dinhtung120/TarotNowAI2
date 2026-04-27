using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Queries.ListReaders;

public sealed class ListReadersQueryValidator : AbstractValidator<ListReadersQuery>
{
    public ListReadersQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThan(0);

        RuleFor(query => query.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(query => query.Status)
            .Must(status => string.IsNullOrWhiteSpace(status) || ReaderOnlineStatus.TryNormalize(status, out _))
            .WithMessage("Status không hợp lệ.");

        RuleFor(query => query.SearchTerm)
            .MaximumLength(120);
    }
}
