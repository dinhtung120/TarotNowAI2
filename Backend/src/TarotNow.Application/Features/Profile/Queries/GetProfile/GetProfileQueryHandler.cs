/*
 * ===================================================================
 * FILE: GetProfileQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Profile.Queries.GetProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thi hành việc Góp Nhặt Dữ liệu từ 3 Nguồn: (1) Bảng User, (2) Bảng UserConsent, (3) Hàm Tiện Ích Suy Luận.
 *   Để đúc ra kết quả Hồ Sơ viên mãn nhất trả về.
 * ===================================================================
 */

using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Helpers;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Profile.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserConsentRepository _consentRepository;
    private readonly IConfiguration _configuration;

    public GetProfileQueryHandler(
        IUserRepository userRepository,
        IUserConsentRepository consentRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _consentRepository = consentRepository;
        _configuration = configuration;
    }

    public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        // 1. Quét Bảng User Lấy Căn Cước.
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        // 2. Chọt thẳng xuống Cấu Hình Lấy Rule Pháp Lý "Tắt Phụt Màn Hình" Version Hiện Tại.
        var requiredTosVersion = _configuration["LegalSettings:TOSVersion"] ?? "1.0";
        var requiredPrivacyVersion = _configuration["LegalSettings:PrivacyVersion"] ?? "1.0";
        var requiredAiDisclaimerVersion = _configuration["LegalSettings:AiDisclaimerVersion"] ?? "1.0";

        // Móc toàn bộ Giấy tờ đã kê khai ra đối chuẩn.
        var userConsents = await _consentRepository.GetUserConsentsAsync(request.UserId, cancellationToken);
        var hasConsented =
            userConsents.Any(c => c.DocumentType == "TOS" && c.Version == requiredTosVersion)
            && userConsents.Any(c => c.DocumentType == "PrivacyPolicy" && c.Version == requiredPrivacyVersion)
            && userConsents.Any(c => c.DocumentType == "AiDisclaimer" && c.Version == requiredAiDisclaimerVersion);

        // 3. Đúc Chảo: 
        return new ProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            DateOfBirth = user.DateOfBirth,
            
            // ==========================================
            // LOGIC SUY LUẬN HUYỀN HỌC (OCCULTISM)
            // ==========================================
            // Server Tự nhẩm Cung Hoàng Đạo thay vì bắt User nhập tay vào Form. (Đỡ phiền cho UX).
            Zodiac = ProfileHelper.CalculateZodiac(user.DateOfBirth),
            Numerology = ProfileHelper.CalculateNumerology(user.DateOfBirth),
            
            Level = user.Level,
            Exp = user.Exp,
            HasConsented = hasConsented
        };
    }
}
