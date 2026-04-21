using TarotNow.Application.Common;
using TarotNow.Application.Common.Helpers;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class ReaderRequestReviewRequestedDomainEventHandler
{
    private const int MinYearsOfExperience = 1;
    private const long MinDiamondPerQuestion = 50;

    private static void EnsurePendingRequest(ReaderRequestDto request)
    {
        if (request.Status != ReaderApprovalStatus.Pending)
        {
            throw new BadRequestException($"Đơn này đã được xử lý ({request.Status}).");
        }
    }

    private static Guid ParseUserId(string userId)
    {
        if (!Guid.TryParse(userId, out var parsed))
        {
            throw new BadRequestException("Reader request chứa UserId không hợp lệ.");
        }

        return parsed;
    }

    private static void EnsureRequestHasMandatoryFieldsForApproval(ReaderRequestDto request)
    {
        if (ReaderSpecialties.NormalizeDistinct(request.Specialties).Count == 0)
        {
            throw new BadRequestException("Đơn đăng ký thiếu chuyên môn hợp lệ. Vui lòng từ chối và yêu cầu user nộp lại.");
        }

        if (request.YearsOfExperience < MinYearsOfExperience)
        {
            throw new BadRequestException("Đơn đăng ký thiếu số năm kinh nghiệm hợp lệ. Vui lòng từ chối và yêu cầu user nộp lại.");
        }

        if (request.DiamondPerQuestion < MinDiamondPerQuestion)
        {
            throw new BadRequestException("Đơn đăng ký thiếu giá dịch vụ hợp lệ. Vui lòng từ chối và yêu cầu user nộp lại.");
        }

        if (!ReaderSocialUrlValidator.HasAtLeastOneSocialLink(request.FacebookUrl, request.InstagramUrl, request.TikTokUrl))
        {
            throw new BadRequestException("Đơn đăng ký thiếu link mạng xã hội. Vui lòng từ chối và yêu cầu user nộp lại.");
        }

        if (!ReaderSocialUrlValidator.IsValidFacebookUrl(request.FacebookUrl)
            || !ReaderSocialUrlValidator.IsValidInstagramUrl(request.InstagramUrl)
            || !ReaderSocialUrlValidator.IsValidTikTokUrl(request.TikTokUrl))
        {
            throw new BadRequestException("Đơn đăng ký chứa link mạng xã hội không hợp lệ. Vui lòng từ chối và yêu cầu user nộp lại.");
        }
    }
}
