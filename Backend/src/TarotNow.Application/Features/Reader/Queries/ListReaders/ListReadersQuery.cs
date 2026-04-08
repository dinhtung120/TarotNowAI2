using MediatR;
using System.Collections.Generic;
using System.Linq;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Reader.Queries.ListReaders;

// Query liệt kê danh sách Reader theo bộ lọc và phân trang.
public class ListReadersQuery : IRequest<ListReadersResult>
{
    // Trang hiện tại.
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 12;

    // Bộ lọc chuyên môn (tùy chọn).
    public string? Specialty { get; set; }

    // Bộ lọc trạng thái reader (tùy chọn).
    public string? Status { get; set; }

    // Từ khóa tìm kiếm theo tên/metadata (tùy chọn).
    public string? SearchTerm { get; set; }
}

// DTO kết quả danh sách reader.
public class ListReadersResult
{
    // Danh sách reader của trang hiện tại.
    public IEnumerable<ReaderProfileDto> Readers { get; set; } = Enumerable.Empty<ReaderProfileDto>();

    // Tổng số reader khớp bộ lọc.
    public long TotalCount { get; set; }
}
