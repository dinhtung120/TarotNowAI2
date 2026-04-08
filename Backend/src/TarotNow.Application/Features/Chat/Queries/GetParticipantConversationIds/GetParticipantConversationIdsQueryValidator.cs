using FluentValidation;

namespace TarotNow.Application.Features.Chat.Queries.GetParticipantConversationIds;

// Validator đầu vào cho query lấy conversation id theo participant.
public sealed class GetParticipantConversationIdsQueryValidator : AbstractValidator<GetParticipantConversationIdsQuery>
{
    /// <summary>
    /// Khởi tạo rule validate cho GetParticipantConversationIdsQuery.
    /// Luồng xử lý: bắt buộc participant id và giới hạn MaxCount trong khoảng 1-200.
    /// </summary>
    public GetParticipantConversationIdsQueryValidator()
    {
        // ParticipantId bắt buộc để định vị dữ liệu truy vấn.
        RuleFor(x => x.ParticipantId)
            .NotEmpty();

        // Giới hạn số lượng để bảo vệ hiệu năng truy vấn.
        RuleFor(x => x.MaxCount)
            .InclusiveBetween(1, 200);
    }
}
