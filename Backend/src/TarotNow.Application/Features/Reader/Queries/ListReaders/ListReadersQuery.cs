/*
 * ===================================================================
 * FILE: ListReadersQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Queries.ListReaders
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh Hỏi Danh Sách Các Thầy Bói Trên Sàn TarotNow (Hành Vi Lướt Chợ).
 *   Hỗ trợ Gom Nhóm, Phân Trang (Mỗi trang vài chục Ông), Tìm Kiếm Theo Lĩnh Vực.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Reader.Queries.ListReaders;

/// <summary>
/// Query phân trang danh sách Reader công khai (directory listing).
/// Hỗ trợ filter: specialty, status, searchTerm.
/// </summary>
public class ListReadersQuery : IRequest<ListReadersResult>
{
    /// <summary>Trang hiện tại (Ví dụ Bấm Sang Trang 2).</summary>
    public int Page { get; set; } = 1;
    
    /// <summary>Số lá bài (Hồ Sơ) dàn ra trên 1 trang lưới Giao Diện. Mặc định 12 ông 1 trang.</summary>
    public int PageSize { get; set; } = 12;
    
    /// <summary>Lọc theo Lĩnh Vực Cụ Thể (Gõ "Tình Yêu" -> Ra toàn các thầy chuyên Tình Duyên).</summary>
    public string? Specialty { get; set; }
    
    /// <summary>Lọc theo Trạng Thái (Thường dùng để Tick chọn: "Chỉ hiện các Thầy đang Online").</summary>
    public string? Status { get; set; }
    
    /// <summary>Hộp Kính lúp Tìm tên Thầy (Phòng khi nhớ mang máng).</summary>
    public string? SearchTerm { get; set; }
}

/// <summary>Kết quả trả về nguyên 1 Xe Chở các DTO Kèm Theo Chỉ Số Cân Nặng (TotalCount) để Phân Trang.</summary>
public class ListReadersResult
{
    public IEnumerable<ReaderProfileDto> Readers { get; set; } = Enumerable.Empty<ReaderProfileDto>();
    
    /// <summary>Biết được Tổng Kho có nhiu Ông Thầy để Tính Số Pagination Pages (Ví dụ 120 / 12 = 10 Trang).</summary>
    public long TotalCount { get; set; }
}
