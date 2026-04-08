using FluentValidation;

namespace TarotNow.Application.Features.Call.Commands.RespondCall;

public sealed class RespondCallCommandValidator : AbstractValidator<RespondCallCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho RespondCallCommand.
    /// Luồng xử lý: kiểm tra call session id và responder id đều bắt buộc.
    /// </summary>
    public RespondCallCommandValidator()
    {
        // CallSessionId bắt buộc để xác định cuộc gọi cần phản hồi.
        RuleFor(x => x.CallSessionId)
            .NotEmpty();

        // ResponderId bắt buộc để kiểm tra participant.
        RuleFor(x => x.ResponderId)
            .NotEmpty();
    }
}
