/*
 * ===================================================================
 * FILE: CheckConsentQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Legal.Queries.CheckConsent
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thi hành nghiệp vụ Đối Khớp Giấy Tờ.
 *   
 * THUẬT TOÁN ĐỐI KHỚP PHÁP LÝ:
 *   Ta có File Cấu Hình `appsettings.json` ấn định Luật Hiện Tại:
 *   - TOSVersion = "2.0"
 *   - PrivacyPolicy = "1.1"
 *   Hệ thống sẽ bốc hết Lịch Sử (History) Ký tá của User trong DB ra đối chiếu.
 *   Chỉ Cần 1 Món bị lệch Version (Ví dụ DB ghi chú nó rớt mồng "1.0") -> Nó Bị Liệt Kê vào danh sách Cần Ký (PendingDocuments).
 * ===================================================================
 */

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
    
    // IConfiguration giúp đọc biến Môi Trường chứa các Version Luật mới nhất.
    private readonly IConfiguration _configuration;

    public CheckConsentQueryHandler(IUserConsentRepository consentRepository, IConfiguration configuration)
    {
        _consentRepository = consentRepository;
        _configuration = configuration;
    }

    public async Task<CheckConsentResponse> Handle(CheckConsentQuery request, CancellationToken cancellationToken)
    {
        var response = new CheckConsentResponse();

        // 1. NGỬI XEM LUẬT NHÀ NƯỚC HIỆN TẠI ĐANG LÀ VERSION MẤY (Lấy từ Config .NET)
        var requiredTOSVersion = ResolveConfiguredVersion("TOS");
        var requiredPrivacyVersion = ResolveConfiguredVersion("PrivacyPolicy");
        var requiredAiDisclaimerVersion = ResolveConfiguredVersion("AiDisclaimer");

        var requiredDocs = new Dictionary<string, string>();
        
        // 2. Tùy Chọn: Truyền cụ thể muốn hỏi thăm cái nào, hay Kiểm Tra Hết Một Lượt?
        if (!string.IsNullOrEmpty(request.DocumentType))
        {
            // Kiểm tra lẻ tẻ (VD: User bấm link coi thử Điều khoản).
            var normalizedDocType = NormalizeDocumentType(request.DocumentType);
            requiredDocs.Add(normalizedDocType, ResolveRequiredVersion(normalizedDocType, request.Version));
        }
        else
        {
            // Auto Quét Hàng Loạt (Hành vi Mặc định khi Mở App).
            var globalVersionOverride = string.IsNullOrWhiteSpace(request.Version) ? null : request.Version.Trim();
            requiredDocs.Add("TOS", globalVersionOverride ?? requiredTOSVersion);
            requiredDocs.Add("PrivacyPolicy", globalVersionOverride ?? requiredPrivacyVersion);
            requiredDocs.Add("AiDisclaimer", globalVersionOverride ?? requiredAiDisclaimerVersion);
        }

        // 3. Rút Hồ Sơ Lịch Sử Ký Tá của Khách về.
        var userConsents = await _consentRepository.GetUserConsentsAsync(request.UserId, cancellationToken);

        // 4. KIỂM TOÁN TỪNG ĐẦU MỤC MỘT
        foreach (var req in requiredDocs)
        {
            var docType = req.Key;
            var requiredVer = req.Value; // Ví dụ: "2.0"

            // Trong hồ sơ cũ có lưu tờ nào Trùng Tên, Trùng Version "2.0" này chưa?
            var hasConsented = userConsents.Any(c => c.DocumentType == docType && c.Version == requiredVer);

            // Chưa hả? Ghi giấy Nợ vào trả về cho Frontend hiển thị Popup đỏ lè.
            if (!hasConsented)
            {
                response.PendingDocuments.Add(docType);
            }
        }

        // 5. Chốt Số: Mảng Trống Nghĩa Là Hồ Sơ Hoàn Hảo (Tất Cả Là Đúng Version Nhất).
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

    /// <summary>
    /// Đọc Version được chốt cứng trong Cấu hình Application.
    /// Nếu Quên Điền Thì Cho Chạy Default (Version 1.0 Chào đời).
    /// </summary>
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

    /// <summary>
    /// Chuẩn hoá chuỗi: Tránh lỗi người ta truyền chữ Hoa chữ Thường lung tung.
    /// Ép thành 1 chuẩn chung Cứng Nhắc bảo vệ hệ thống so sánh String.
    /// </summary>
    private static string NormalizeDocumentType(string documentType)
    {
        var normalized = documentType.Trim();
        if (string.Equals(normalized, "TOS", System.StringComparison.OrdinalIgnoreCase)) return "TOS";
        if (string.Equals(normalized, "PrivacyPolicy", System.StringComparison.OrdinalIgnoreCase)) return "PrivacyPolicy";
        if (string.Equals(normalized, "AiDisclaimer", System.StringComparison.OrdinalIgnoreCase)) return "AiDisclaimer";
        throw new BadRequestException("DocumentType không hợp lệ.");
    }
}
