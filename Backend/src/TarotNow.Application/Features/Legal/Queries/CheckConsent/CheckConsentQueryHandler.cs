using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Legal.Queries.CheckConsent;

// Handler kiểm tra user đã consent đầy đủ theo bộ tài liệu pháp lý bắt buộc chưa.
public class CheckConsentQueryHandler : IRequestHandler<CheckConsentQuery, CheckConsentResponse>
{
    // Document type Terms of Service chuẩn hóa dùng nội bộ.
    private const string TosDocumentType = "TOS";

    // Document type Privacy Policy chuẩn hóa dùng nội bộ.
    private const string PrivacyPolicyDocumentType = "PrivacyPolicy";

    // Document type AI Disclaimer chuẩn hóa dùng nội bộ.
    private const string AiDisclaimerDocumentType = "AiDisclaimer";

    // Thông điệp lỗi chung khi document type không thuộc danh sách hợp lệ.
    private const string InvalidDocumentTypeMessage = "DocumentType không hợp lệ.";

    private readonly IUserConsentRepository _consentRepository;
    private readonly ILegalVersionSettings _legalVersionSettings;

    /// <summary>
    /// Khởi tạo handler kiểm tra consent.
    /// Luồng xử lý: nhận repository để đọc consent user và settings để resolve version pháp lý hiện hành.
    /// </summary>
    public CheckConsentQueryHandler(
        IUserConsentRepository consentRepository,
        ILegalVersionSettings legalVersionSettings)
    {
        _consentRepository = consentRepository;
        _legalVersionSettings = legalVersionSettings;
    }

    /// <summary>
    /// Xử lý query kiểm tra consent của user.
    /// Luồng xử lý: xác định tập document bắt buộc, tải consent hiện có của user, rồi tính danh sách document còn thiếu.
    /// </summary>
    public async Task<CheckConsentResponse> Handle(CheckConsentQuery request, CancellationToken cancellationToken)
    {
        var requiredDocs = BuildRequiredDocs(request.DocumentType, request.Version);
        // Xác định tập tài liệu + version cần so khớp dựa theo input và cấu hình.

        var userConsents = await _consentRepository.GetUserConsentsAsync(request.UserId, cancellationToken);
        // Tải toàn bộ consent hiện tại để đánh giá trạng thái thiếu/đủ theo từng document.

        var response = BuildResponse(requiredDocs, userConsents);
        response.IsFullyConsented = response.PendingDocuments.Count == 0;
        // Gắn cờ tổng hợp để caller không phải tự suy luận lại từ danh sách PendingDocuments.

        return response;
    }

    /// <summary>
    /// Dựng response từ tập document bắt buộc và danh sách consent hiện có.
    /// Luồng xử lý: duyệt từng document bắt buộc, kiểm tra tồn tại consent đúng version, thiếu thì thêm vào PendingDocuments.
    /// </summary>
    private static CheckConsentResponse BuildResponse(
        IReadOnlyDictionary<string, string> requiredDocs,
        IEnumerable<UserConsent> userConsents)
    {
        var response = new CheckConsentResponse();

        foreach (var requiredDoc in requiredDocs)
        {
            var hasConsented = userConsents.Any(consent =>
                consent.DocumentType == requiredDoc.Key &&
                consent.Version == requiredDoc.Value);

            if (!hasConsented)
            {
                // Document chưa được consent đúng version thì đưa vào danh sách còn thiếu.
                response.PendingDocuments.Add(requiredDoc.Key);
            }
        }
        // Kết thúc vòng lặp, PendingDocuments phản ánh chính xác các tài liệu còn thiếu.

        return response;
    }

    /// <summary>
    /// Xây tập document bắt buộc cần kiểm tra.
    /// Luồng xử lý: nếu truyền DocumentType thì chỉ kiểm tra một loại; nếu không thì kiểm tra cả bộ tài liệu pháp lý chuẩn.
    /// </summary>
    private IReadOnlyDictionary<string, string> BuildRequiredDocs(string? documentType, string? version)
    {
        if (!string.IsNullOrWhiteSpace(documentType))
        {
            var normalizedDocType = NormalizeDocumentType(documentType);
            // Chuẩn hóa đầu vào để so khớp nhất quán và loại bỏ sai khác chữ hoa/thường.

            return new Dictionary<string, string>
            {
                [normalizedDocType] = ResolveRequiredVersion(normalizedDocType, version)
            };
        }

        var globalVersionOverride = string.IsNullOrWhiteSpace(version) ? null : version.Trim();
        // Khi truyền version toàn cục, ưu tiên dùng cho toàn bộ document để hỗ trợ kiểm tra theo một mốc release.

        return new Dictionary<string, string>
        {
            [TosDocumentType] = globalVersionOverride ?? ResolveConfiguredVersion(TosDocumentType),
            [PrivacyPolicyDocumentType] = globalVersionOverride ?? ResolveConfiguredVersion(PrivacyPolicyDocumentType),
            [AiDisclaimerDocumentType] = globalVersionOverride ?? ResolveConfiguredVersion(AiDisclaimerDocumentType)
        };
    }

    /// <summary>
    /// Resolve version bắt buộc cho một document.
    /// Luồng xử lý: ưu tiên version từ request khi có, nếu không thì fallback về version cấu hình hệ thống.
    /// </summary>
    private string ResolveRequiredVersion(string documentType, string? requestedVersion)
    {
        if (!string.IsNullOrWhiteSpace(requestedVersion))
        {
            // Caller đã chỉ định version cụ thể thì dùng trực tiếp để tránh lệch kết quả kỳ vọng.
            return requestedVersion.Trim();
        }

        return ResolveConfiguredVersion(documentType);
    }

    /// <summary>
    /// Lấy version cấu hình hiện hành theo document type.
    /// Luồng xử lý: map document type chuẩn hóa sang cấu hình tương ứng; loại không hợp lệ sẽ ném BadRequest.
    /// </summary>
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

    /// <summary>
    /// Chuẩn hóa document type đầu vào về key nội bộ.
    /// Luồng xử lý: trim và so sánh không phân biệt hoa/thường để chấp nhận input linh hoạt nhưng vẫn khóa về giá trị chuẩn.
    /// </summary>
    private static string NormalizeDocumentType(string documentType)
    {
        var normalized = documentType.Trim();

        if (string.Equals(normalized, TosDocumentType, StringComparison.OrdinalIgnoreCase))
        {
            // Quy về key chuẩn để đồng nhất truy vấn consent và tra cứu version.
            return TosDocumentType;
        }

        if (string.Equals(normalized, PrivacyPolicyDocumentType, StringComparison.OrdinalIgnoreCase))
        {
            // Quy về key chuẩn để đồng nhất truy vấn consent và tra cứu version.
            return PrivacyPolicyDocumentType;
        }

        if (string.Equals(normalized, AiDisclaimerDocumentType, StringComparison.OrdinalIgnoreCase))
        {
            // Quy về key chuẩn để đồng nhất truy vấn consent và tra cứu version.
            return AiDisclaimerDocumentType;
        }

        // Edge case: document type ngoài whitelist phải bị chặn ngay để tránh trả kết quả sai ngữ nghĩa.
        throw new BadRequestException(InvalidDocumentTypeMessage);
    }
}
