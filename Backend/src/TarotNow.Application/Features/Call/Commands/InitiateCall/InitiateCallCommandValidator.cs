using FluentValidation;

namespace TarotNow.Application.Features.Call.Commands.InitiateCall;

public sealed class InitiateCallCommandValidator : AbstractValidator<InitiateCallCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho InitiateCallCommand.
    /// Luồng xử lý: kiểm tra conversation id, initiator id và call type hợp lệ.
    /// </summary>
    public InitiateCallCommandValidator()
    {
        // ConversationId bắt buộc để xác định phòng gọi.
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        // InitiatorId bắt buộc để kiểm tra quyền participant.
        RuleFor(x => x.InitiatorId)
            .NotEmpty();

        // Type chỉ chấp nhận audio/video theo contract cuộc gọi.
        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(type => type is "audio" or "video")
            .WithMessage("Call type must be 'audio' or 'video'.");
    }
}
