/*
 * ===================================================================
 * FILE: SubmitReaderRequestValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest
 * ===================================================================
 * MỤC ĐÍCH:
 *   Kiểm duyệt lời Lời Giới Thiệu (Cover Letter). 
 *   Phải viết tử tế (min 20 kí tự) chứ không thể chấm '.' gởi đi cho xong chuyện.
 * ===================================================================
 */

using FluentValidation;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

public class SubmitReaderRequestValidator : AbstractValidator<SubmitReaderRequestCommand>
{
    public SubmitReaderRequestValidator()
    {
        // Chống Null Truyền linh tinh.
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId không được để trống.");

        // Lời Mở Đầu Bắt Buộc. Ngắn Cụt Lủn hoặc Dài Lê Thê Đều Rớt Đài.
        RuleFor(x => x.IntroText)
            .NotEmpty()
            .WithMessage("Lời giới thiệu không được để trống.")
            // Ít nhất 20 ký tự (Để Admin đọc vào còn biết đây là người thật có đam mê Tarot).
            .MinimumLength(20)
            .WithMessage("Lời giới thiệu phải có ít nhất 20 ký tự.")
            // Tối đa 2000 ký tự (Chống spam nhét Truyện Kiều vào làm lag Server).
            .MaximumLength(2000)
            .WithMessage("Lời giới thiệu không được vượt quá 2000 ký tự.");
    }
}
