using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

// Validator cho command khởi tạo reading session.
public class InitReadingSessionCommandValidator : AbstractValidator<InitReadingSessionCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu mở phiên reading.
    /// Luồng xử lý: kiểm tra định danh user, spread hợp lệ, độ dài câu hỏi và currency đầu vào.
    /// </summary>
    public InitReadingSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để gắn phiên reading với đúng tài khoản.

        RuleFor(x => x.SpreadType)
            .NotEmpty()
            .Must(spreadType => spreadType is SpreadType.Daily1Card
                or SpreadType.Spread3Cards
                or SpreadType.Spread5Cards
                or SpreadType.Spread10Cards)
            .WithMessage("SpreadType không hợp lệ.");
        // Chỉ chấp nhận spread nằm trong danh mục hệ thống hỗ trợ.

        RuleFor(x => x.Question)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.Question) == false);
        // Câu hỏi tùy chọn nhưng khi có thì cần giới hạn độ dài để tránh payload quá lớn.

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currency =>
            {
                var normalized = currency?.Trim().ToLowerInvariant();
                return normalized is CurrencyType.Gold or CurrencyType.Diamond;
            })
            .WithMessage("Currency phải là gold hoặc diamond.");
        // Chỉ chấp nhận 2 đơn vị tiền hệ thống hỗ trợ để resolve pricing nhất quán.
    }
}
