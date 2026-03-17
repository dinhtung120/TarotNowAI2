using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.ListReaders;

/// <summary>
/// Handler cho directory listing — delegate sang repository.
///
/// Tại sao handler đơn giản chỉ gọi repository?
/// → Clean Architecture: Controller → MediatR → Handler → Repository.
/// → Tương lai có thể thêm logic (cache, enrichment, recommendations) mà không sửa Controller.
/// → Pipeline behaviors (logging, validation) tự động áp dụng qua MediatR.
/// </summary>
public class ListReadersQueryHandler : IRequestHandler<ListReadersQuery, ListReadersResult>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public ListReadersQueryHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<ListReadersResult> Handle(ListReadersQuery request, CancellationToken cancellationToken)
    {
        // Chuyển tiếp tất cả filter parameters sang repository
        var (profiles, totalCount) = await _readerProfileRepository.GetPaginatedAsync(
            request.Page,
            request.PageSize,
            request.Specialty,
            request.Status,
            request.SearchTerm,
            cancellationToken);

        return new ListReadersResult
        {
            Readers = profiles,
            TotalCount = totalCount
        };
    }
}
