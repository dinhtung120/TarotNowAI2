using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Deposit.Queries.ListMyDepositOrders;

/// <summary>
/// Validator query phân trang lịch sử nạp.
/// </summary>
public sealed class ListMyDepositOrdersQueryValidator : AbstractValidator<ListMyDepositOrdersQuery>
{
    private static readonly string[] AllowedStatuses =
    [
        DepositOrderStatus.Pending,
        DepositOrderStatus.Success,
        DepositOrderStatus.Failed
    ];

    /// <summary>
    /// Khởi tạo rule validate cho query lịch sử nạp.
    /// </summary>
    public ListMyDepositOrdersQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);

        RuleFor(x => x.Status)
            .Must(IsValidStatus)
            .WithMessage("Status filter is invalid.");
    }

    private static bool IsValidStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return true;
        }

        return AllowedStatuses.Any(allowed => string.Equals(allowed, status.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
