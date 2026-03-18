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
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        var requiredTosVersion = _configuration["LegalSettings:TOSVersion"] ?? "1.0";
        var requiredPrivacyVersion = _configuration["LegalSettings:PrivacyVersion"] ?? "1.0";
        var requiredAiDisclaimerVersion = _configuration["LegalSettings:AiDisclaimerVersion"] ?? "1.0";

        var userConsents = await _consentRepository.GetUserConsentsAsync(request.UserId, cancellationToken);
        var hasConsented =
            userConsents.Any(c => c.DocumentType == "TOS" && c.Version == requiredTosVersion)
            && userConsents.Any(c => c.DocumentType == "PrivacyPolicy" && c.Version == requiredPrivacyVersion)
            && userConsents.Any(c => c.DocumentType == "AiDisclaimer" && c.Version == requiredAiDisclaimerVersion);

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
            Level = user.Level,
            Exp = user.Exp,
            HasConsented = hasConsented
        };
    }
}
