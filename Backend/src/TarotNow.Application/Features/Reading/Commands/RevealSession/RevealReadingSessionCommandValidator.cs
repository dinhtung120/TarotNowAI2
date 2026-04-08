using FluentValidation;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

// Validator cho command reveal reading session.
public class RevealReadingSessionCommandValidator : AbstractValidator<RevealReadingSessionCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu reveal session.
    /// Luồng xử lý: kiểm tra UserId và SessionId bắt buộc để xác định đúng phiên cần mở bài.
    /// </summary>
    public RevealReadingSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để đối chiếu quyền sở hữu session.

        RuleFor(x => x.SessionId)
            .NotEmpty();
        // SessionId bắt buộc để truy xuất chính xác phiên reading cần reveal.
    }
}
