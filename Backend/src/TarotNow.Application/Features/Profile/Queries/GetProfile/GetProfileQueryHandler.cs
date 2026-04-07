

using MediatR;
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
    private readonly ILegalVersionSettings _legalVersionSettings;

    public GetProfileQueryHandler(
        IUserRepository userRepository,
        IUserConsentRepository consentRepository,
        ILegalVersionSettings legalVersionSettings)
    {
        _userRepository = userRepository;
        _consentRepository = consentRepository;
        _legalVersionSettings = legalVersionSettings;
    }

    public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        
        var requiredTosVersion = _legalVersionSettings.TOSVersion;
        var requiredPrivacyVersion = _legalVersionSettings.PrivacyVersion;
        var requiredAiDisclaimerVersion = _legalVersionSettings.AiDisclaimerVersion;

        
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
