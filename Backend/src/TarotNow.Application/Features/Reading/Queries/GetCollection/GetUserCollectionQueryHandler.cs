using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Queries.GetCollection;

// Handler truy vấn bộ sưu tập thẻ của user.
public class GetUserCollectionQueryHandler : IRequestHandler<GetUserCollectionQuery, List<UserCollectionDto>>
{
    private readonly IUserCollectionRepository _collectionRepo;

    /// <summary>
    /// Khởi tạo handler lấy user collection.
    /// Luồng xử lý: nhận repository collection để tải dữ liệu thẻ người dùng.
    /// </summary>
    public GetUserCollectionQueryHandler(IUserCollectionRepository collectionRepo)
    {
        _collectionRepo = collectionRepo;
    }

    /// <summary>
    /// Xử lý query lấy collection.
    /// Luồng xử lý: tải collection theo user id và map sang DTO trả về cho client.
    /// </summary>
    public async Task<List<UserCollectionDto>> Handle(GetUserCollectionQuery request, CancellationToken cancellationToken)
    {
        var collections = await _collectionRepo.GetUserCollectionAsync(request.UserId, cancellationToken);

        return collections.Select(collection => new UserCollectionDto
        {
            CardId = collection.CardId,
            Level = collection.Level,
            Copies = collection.Copies,
            ExpGained = collection.ExpGained,
            LastDrawnAt = collection.LastDrawnAt,
            Atk = collection.Atk,
            Def = collection.Def
        }).ToList();
    }
}
