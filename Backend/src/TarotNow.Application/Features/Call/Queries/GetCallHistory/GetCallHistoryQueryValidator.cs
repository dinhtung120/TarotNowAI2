using FluentValidation;

namespace TarotNow.Application.Features.Call.Queries.GetCallHistory;

public sealed class GetCallHistoryQueryValidator : AbstractValidator<GetCallHistoryQuery>
{
    /// <summary>
    /// Khởi tạo rule validation cho GetCallHistoryQuery.
    /// Luồng xử lý: kiểm tra conversation id, participant id, page và page size hợp lệ.
    /// </summary>
    public GetCallHistoryQueryValidator()
    {
        // ConversationId bắt buộc.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // ParticipantId bắt buộc để kiểm tra quyền.
        RuleFor(x => x.ParticipantId)
            .NotEmpty();

        // Page phải >= 1.
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        // PageSize giới hạn trong [1,50].
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);
    }
}
