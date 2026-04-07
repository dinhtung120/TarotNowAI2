

using MediatR;
using TarotNow.Application.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Legal.Queries.CheckConsent;

public class CheckConsentQueryHandler : IRequestHandler<CheckConsentQuery, CheckConsentResponse>
{
    private const string TosDocumentType = "TOS";
    private const string PrivacyPolicyDocumentType = "PrivacyPolicy";
    private const string AiDisclaimerDocumentType = "AiDisclaimer";
    private const string InvalidDocumentTypeMessage = "DocumentType không hợp lệ.";

    private readonly IUserConsentRepository _consentRepository;
    private readonly ILegalVersionSettings _legalVersionSettings;

    public CheckConsentQueryHandler(
        IUserConsentRepository consentRepository,
        ILegalVersionSettings legalVersionSettings)
    {
        _consentRepository = consentRepository;
        _legalVersionSettings = legalVersionSettings;
    }

    public async Task<CheckConsentResponse> Handle(CheckConsentQuery request, CancellationToken cancellationToken)
    {
        var requiredDocs = BuildRequiredDocs(request.DocumentType, request.Version);
        var userConsents = await _consentRepository.GetUserConsentsAsync(request.UserId, cancellationToken);
        var response = BuildResponse(requiredDocs, userConsents);
        response.IsFullyConsented = response.PendingDocuments.Count == 0;
        return response;
    }

    private CheckConsentResponse BuildResponse(
        IReadOnlyDictionary<string, string> requiredDocs,
        IEnumerable<dynamic> userConsents)
    {
        var response = new CheckConsentResponse();
        foreach (var requiredDoc in requiredDocs)
        {
            var hasConsented = userConsents.Any(consent =>
                consent.DocumentType == requiredDoc.Key &&
                consent.Version == requiredDoc.Value);

            if (!hasConsented) response.PendingDocuments.Add(requiredDoc.Key);
        }

        return response;
    }

    private IReadOnlyDictionary<string, string> BuildRequiredDocs(string? documentType, string? version)
    {
        if (!string.IsNullOrWhiteSpace(documentType))
        {
            var normalizedDocType = NormalizeDocumentType(documentType);
            return new Dictionary<string, string>
            {
                [normalizedDocType] = ResolveRequiredVersion(normalizedDocType, version)
            };
        }

        var globalVersionOverride = string.IsNullOrWhiteSpace(version) ? null : version.Trim();
        return new Dictionary<string, string>
        {
            [TosDocumentType] = globalVersionOverride ?? ResolveConfiguredVersion(TosDocumentType),
            [PrivacyPolicyDocumentType] = globalVersionOverride ?? ResolveConfiguredVersion(PrivacyPolicyDocumentType),
            [AiDisclaimerDocumentType] = globalVersionOverride ?? ResolveConfiguredVersion(AiDisclaimerDocumentType)
        };
    }

    private string ResolveRequiredVersion(string documentType, string? requestedVersion)
    {
        if (!string.IsNullOrWhiteSpace(requestedVersion))
        {
            return requestedVersion.Trim();
        }

        return ResolveConfiguredVersion(documentType);
    }

    private string ResolveConfiguredVersion(string documentType)
    {
        return documentType switch
        {
            TosDocumentType => _legalVersionSettings.TOSVersion,
            PrivacyPolicyDocumentType => _legalVersionSettings.PrivacyVersion,
            AiDisclaimerDocumentType => _legalVersionSettings.AiDisclaimerVersion,
            _ => throw new BadRequestException(InvalidDocumentTypeMessage)
        };
    }

    private static string NormalizeDocumentType(string documentType)
    {
        var normalized = documentType.Trim();
        if (string.Equals(normalized, TosDocumentType, System.StringComparison.OrdinalIgnoreCase)) return TosDocumentType;
        if (string.Equals(normalized, PrivacyPolicyDocumentType, System.StringComparison.OrdinalIgnoreCase)) return PrivacyPolicyDocumentType;
        if (string.Equals(normalized, AiDisclaimerDocumentType, System.StringComparison.OrdinalIgnoreCase)) return AiDisclaimerDocumentType;
        throw new BadRequestException(InvalidDocumentTypeMessage);
    }
}
