using MediatR;
using TarotNow.Application.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Legal.Queries.CheckConsent;

public class CheckConsentQueryHandler : IRequestHandler<CheckConsentQuery, CheckConsentResponse>
{
    private readonly IUserConsentRepository _consentRepository;
    private readonly IConfiguration _configuration;

    public CheckConsentQueryHandler(IUserConsentRepository consentRepository, IConfiguration configuration)
    {
        _consentRepository = consentRepository;
        _configuration = configuration;
    }

    public async Task<CheckConsentResponse> Handle(CheckConsentQuery request, CancellationToken cancellationToken)
    {
        var response = new CheckConsentResponse();

        // 2. Xác định các bản version cần kiểm tra (Dùng query params nếu có, nếu không dùng config mặc định)
        var requiredTOSVersion = ResolveConfiguredVersion("TOS");
        var requiredPrivacyVersion = ResolveConfiguredVersion("PrivacyPolicy");
        var requiredAiDisclaimerVersion = ResolveConfiguredVersion("AiDisclaimer");

        var requiredDocs = new Dictionary<string, string>();
        
        if (!string.IsNullOrEmpty(request.DocumentType))
        {
            var normalizedDocType = NormalizeDocumentType(request.DocumentType);
            requiredDocs.Add(normalizedDocType, ResolveRequiredVersion(normalizedDocType, request.Version));
        }
        else
        {
            var globalVersionOverride = string.IsNullOrWhiteSpace(request.Version) ? null : request.Version.Trim();
            requiredDocs.Add("TOS", globalVersionOverride ?? requiredTOSVersion);
            requiredDocs.Add("PrivacyPolicy", globalVersionOverride ?? requiredPrivacyVersion);
            requiredDocs.Add("AiDisclaimer", globalVersionOverride ?? requiredAiDisclaimerVersion);
        }

        // 3. Lấy tất cả consent gần nhất của User này
        var userConsents = await _consentRepository.GetUserConsentsAsync(request.UserId, cancellationToken);

        // 3. Kiểm tra xem mỗi loại Document đã consent version mới nhất chưa
        foreach (var req in requiredDocs)
        {
            var docType = req.Key;
            var requiredVer = req.Value;

            var hasConsented = userConsents.Any(c => c.DocumentType == docType && c.Version == requiredVer);

            if (!hasConsented)
            {
                response.PendingDocuments.Add(docType);
            }
        }

        response.IsFullyConsented = response.PendingDocuments.Count == 0;

        return response;
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
            "TOS" => _configuration["LegalSettings:TOSVersion"] ?? "1.0",
            "PrivacyPolicy" => _configuration["LegalSettings:PrivacyVersion"] ?? "1.0",
            "AiDisclaimer" => _configuration["LegalSettings:AiDisclaimerVersion"] ?? "1.0",
            _ => throw new BadRequestException("DocumentType không hợp lệ.")
        };
    }

    private static string NormalizeDocumentType(string documentType)
    {
        var normalized = documentType.Trim();
        if (string.Equals(normalized, "TOS", System.StringComparison.OrdinalIgnoreCase)) return "TOS";
        if (string.Equals(normalized, "PrivacyPolicy", System.StringComparison.OrdinalIgnoreCase)) return "PrivacyPolicy";
        if (string.Equals(normalized, "AiDisclaimer", System.StringComparison.OrdinalIgnoreCase)) return "AiDisclaimer";
        throw new BadRequestException("DocumentType không hợp lệ.");
    }
}
