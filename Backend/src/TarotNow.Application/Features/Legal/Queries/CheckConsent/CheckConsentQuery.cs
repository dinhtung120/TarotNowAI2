using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Legal.Queries.CheckConsent;

// Query kiểm tra trạng thái consent của user với bộ tài liệu pháp lý bắt buộc.
public class CheckConsentQuery : IRequest<CheckConsentResponse>
{
    // Định danh user cần kiểm tra consent.
    public Guid UserId { get; set; }

    // Loại tài liệu cần kiểm tra riêng (tùy chọn); để trống thì kiểm tra toàn bộ.
    public string? DocumentType { get; set; }

    // Phiên bản cần kiểm tra (tùy chọn); để trống thì dùng version cấu hình hiện hành.
    public string? Version { get; set; }
}

// DTO trả về trạng thái consent và danh sách tài liệu còn thiếu.
public class CheckConsentResponse
{
    // Cờ cho biết user đã đồng ý đầy đủ theo tập tài liệu yêu cầu hay chưa.
    public bool IsFullyConsented { get; set; }

    // Danh sách document type mà user chưa consent đúng version yêu cầu.
    public List<string> PendingDocuments { get; set; } = new();
}
