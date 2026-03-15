using MediatR;
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
        var requiredTOSVersion = request.Version ?? _configuration["LegalSettings:TOSVersion"] ?? "1.0";
        var requiredPrivacyVersion = request.Version ?? _configuration["LegalSettings:PrivacyVersion"] ?? "1.0";
        var requiredAiDisclaimerVersion = request.Version ?? _configuration["LegalSettings:AiDisclaimerVersion"] ?? "1.0";

        var requiredDocs = new Dictionary<string, string>();
        
        if (!string.IsNullOrEmpty(request.DocumentType))
        {
            requiredDocs.Add(request.DocumentType, request.Version ?? "1.0");
        }
        else
        {
            requiredDocs.Add("TOS", requiredTOSVersion);
            requiredDocs.Add("PrivacyPolicy", requiredPrivacyVersion);
            requiredDocs.Add("AiDisclaimer", requiredAiDisclaimerVersion);
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
}
