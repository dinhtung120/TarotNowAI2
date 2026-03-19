/*
 * ===================================================================
 * FILE: GetReaderProfileQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Queries.GetReaderProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lệnh Thực thi: Kéo Dữ Liệu Hồ Sơ Thầy Bói Từ MongoDB.
 *   Nếu gọi trúng ID tào lao (Không phải Thầy Bói) -> Hàm sẽ văng Null 
 *   Và Frontend sẽ hiển thị Lỗi 404 Cảnh Cáo Khách nạp nhầm người.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.GetReaderProfile;

public class GetReaderProfileQueryHandler : IRequestHandler<GetReaderProfileQuery, ReaderProfileDto?>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public GetReaderProfileQueryHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<ReaderProfileDto?> Handle(GetReaderProfileQuery request, CancellationToken cancellationToken)
    {
        // Nhờ DB lôi ngay hồ sơ ID này lên. 
        // LƯU Ý: Frontend KHÔNG NÊN cache chết API này vì Giá Tiền / Status Online của Thầy đổi liên tục.
        return await _readerProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
    }
}
