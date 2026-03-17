using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Reader.Queries.ListReaders;

/// <summary>
/// Query phân trang danh sách Reader công khai (directory listing).
/// Hỗ trợ filter: specialty, status, searchTerm.
/// </summary>
public class ListReadersQuery : IRequest<ListReadersResult>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public string? Specialty { get; set; }
    public string? Status { get; set; }
    public string? SearchTerm { get; set; }
}

/// <summary>Kết quả phân trang cho danh sách Reader.</summary>
public class ListReadersResult
{
    public IEnumerable<ReaderProfileDto> Readers { get; set; } = Enumerable.Empty<ReaderProfileDto>();
    public long TotalCount { get; set; }
}
