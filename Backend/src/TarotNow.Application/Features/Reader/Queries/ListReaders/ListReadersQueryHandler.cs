/*
 * ===================================================================
 * FILE: ListReadersQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Queries.ListReaders
 * ===================================================================
 * MỤC ĐÍCH:
 *   Đại Lý Gom Hàng, Cầm danh sách Tiêu Chí Dò của Khách đi vào Kho MongoDB (Repository)
 *   rồi Rinh Ra Danh Sách Các Thầy Bói thỏa tiêu chí.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.ListReaders;

public class ListReadersQueryHandler : IRequestHandler<ListReadersQuery, ListReadersResult>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public ListReadersQueryHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<ListReadersResult> Handle(ListReadersQuery request, CancellationToken cancellationToken)
    {
        // Quăng Toàn Bộ Thông Số Đầu Vào (Dò, Filter, Trang) cho Repository Xuống Xử Lý Truy Vấn Query Bằng Cú Pháp MongoDB.
        // Hứng lại "Danh sách Thầy" và "Tổng số người" dùng cho Thuật Toán Pagination.
        var (profiles, totalCount) = await _readerProfileRepository.GetPaginatedAsync(
            request.Page,
            request.PageSize,
            request.Specialty,
            request.Status,
            request.SearchTerm,
            cancellationToken);

        // Ném Thẳng Ra Mặt Tiền Cho Khách Lựa Hàng. 
        // Sau đó UI Map "TotalCount / DTO" ra Phân Trang Từng Ô Hình Chữ Nhật Nhỏ.
        return new ListReadersResult
        {
            Readers = profiles,
            TotalCount = totalCount
        };
    }
}
