using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.Helpers;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Profile.Queries.GetProfile;

// Handler truy vấn hồ sơ người dùng kèm trạng thái consent pháp lý.
public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserConsentRepository _consentRepository;
    private readonly ILegalVersionSettings _legalVersionSettings;

    /// <summary>
    /// Khởi tạo handler lấy profile.
    /// Luồng xử lý: nhận repository người dùng, repository consent và settings version pháp lý để tổng hợp dữ liệu hồ sơ.
    /// </summary>
    public GetProfileQueryHandler(
        IUserRepository userRepository,
        IUserConsentRepository consentRepository,
        ILegalVersionSettings legalVersionSettings)
    {
        _userRepository = userRepository;
        _consentRepository = consentRepository;
        _legalVersionSettings = legalVersionSettings;
    }

    /// <summary>
    /// Xử lý query lấy profile theo user id.
    /// Luồng xử lý: tải user, resolve version pháp lý yêu cầu, kiểm tra consent đủ bộ và map sang DTO trả về.
    /// </summary>
    public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        var requiredTosVersion = _legalVersionSettings.TOSVersion;
        var requiredPrivacyVersion = _legalVersionSettings.PrivacyVersion;
        var requiredAiDisclaimerVersion = _legalVersionSettings.AiDisclaimerVersion;
        // Chốt version mục tiêu từ cấu hình để đánh giá consent nhất quán trên toàn hệ thống.

        var userConsents = await _consentRepository.GetUserConsentsAsync(request.UserId, cancellationToken);
        var hasConsented =
            userConsents.Any(c => c.DocumentType == "TOS" && c.Version == requiredTosVersion) &&
            userConsents.Any(c => c.DocumentType == "PrivacyPolicy" && c.Version == requiredPrivacyVersion) &&
            userConsents.Any(c => c.DocumentType == "AiDisclaimer" && c.Version == requiredAiDisclaimerVersion);
        // Business rule: chỉ khi user đã consent đủ 3 tài liệu đúng version bắt buộc mới được coi là fully-consented.

        return new ProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            DateOfBirth = user.DateOfBirth,
            Zodiac = ProfileHelper.CalculateZodiac(user.DateOfBirth),
            Numerology = ProfileHelper.CalculateNumerology(user.DateOfBirth),
            // Tính toán thuộc tính chiêm tinh/thần số học tại thời điểm query để tránh lưu dữ liệu dẫn xuất dư thừa.
            Level = user.Level,
            Exp = user.Exp,
            HasConsented = hasConsented,
            PayoutBankName = user.PayoutBankName,
            PayoutBankBin = user.PayoutBankBin,
            PayoutBankAccountNumber = user.PayoutBankAccountNumber,
            PayoutBankAccountHolder = user.PayoutBankAccountHolder
        };
    }
}
