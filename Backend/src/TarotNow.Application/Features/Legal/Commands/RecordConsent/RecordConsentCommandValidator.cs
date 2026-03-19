/*
 * ===================================================================
 * FILE: RecordConsentCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Legal.Commands.RecordConsent
 * ===================================================================
 * MỤC ĐÍCH:
 *   Người gác cổng rà soát Giấy Tờ Pháp Lý Đầu Vào. Chặn rác dữ liệu.
 * ===================================================================
 */

using FluentValidation;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

public class RecordConsentCommandValidator : AbstractValidator<RecordConsentCommand>
{
    public RecordConsentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("DocumentType is required.")
            // CHỈ CHẤP NHẬN BẤM ĐỒNG Ý CHO 3 LOẠI VĂN BẢN DUY NHẤT. Truyền lụi vào là Chặn!
            .Must(type => type == "TOS" || type == "PrivacyPolicy" || type == "AiDisclaimer")
            .WithMessage("DocumentType must be TOS, PrivacyPolicy, or AiDisclaimer.");

        RuleFor(x => x.Version)
            .NotEmpty().WithMessage("Version is required.");
    }
}
