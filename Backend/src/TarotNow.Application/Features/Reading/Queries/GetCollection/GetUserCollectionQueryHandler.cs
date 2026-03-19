/*
 * ===================================================================
 * FILE: GetUserCollectionQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Reading.Queries.GetCollection
 * ===================================================================
 * MỤC ĐÍCH:
 *   Kéo Bộ Sưu Tập 78 Lá Của Khách. Tra Xem Đứa Này Đã Đạt Chuẩn Sưu Tầm Tới Đâu.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Reading.Queries.GetCollection;

public class GetUserCollectionQueryHandler : IRequestHandler<GetUserCollectionQuery, List<UserCollectionDto>>
{
    private readonly IUserCollectionRepository _collectionRepo;

    public GetUserCollectionQueryHandler(IUserCollectionRepository collectionRepo)
    {
        _collectionRepo = collectionRepo;
    }

    public async Task<List<UserCollectionDto>> Handle(GetUserCollectionQuery request, CancellationToken cancellationToken)
    {
        // 1. Nhờ Kẻ Canh Giữ Thư Viện (Repository) Lôi Chồng Sách Của Tên Này Lên (Toàn Bộ Thẻ Của Khách Hàng Này).
        var collections = await _collectionRepo.GetUserCollectionAsync(request.UserId, cancellationToken);
        
        // 2. Map Object Gốc Sang ViewModel Mỏng Gọn Trước Khi Đẩy Dữ Liệu Chạy Ngược Lên Giao Diện (UI).
        return collections.Select(c => new UserCollectionDto
        {
            CardId = c.CardId,
            Level = c.Level,
            Copies = c.Copies,
            ExpGained = c.ExpGained, // Điểm Tích Lũy Từ Lúc Sinh Thành.
            LastDrawnAt = c.LastDrawnAt
        }).ToList();
    }
}
