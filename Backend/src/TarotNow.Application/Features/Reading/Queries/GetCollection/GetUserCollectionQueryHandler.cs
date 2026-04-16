using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Queries.GetCollection;

/// <summary>
/// Handler truy vấn bộ sưu tập thẻ của user.
/// </summary>
public sealed class GetUserCollectionQueryHandler : IRequestHandler<GetUserCollectionQuery, List<UserCollectionDto>>
{
    private readonly IUserCollectionRepository _collectionRepo;

    /// <summary>
    /// Khởi tạo handler lấy user collection.
    /// </summary>
    public GetUserCollectionQueryHandler(IUserCollectionRepository collectionRepo)
    {
        _collectionRepo = collectionRepo;
    }

    /// <summary>
    /// Xử lý query lấy collection.
    /// </summary>
    public async Task<List<UserCollectionDto>> Handle(GetUserCollectionQuery request, CancellationToken cancellationToken)
    {
        var collections = await _collectionRepo.GetUserCollectionAsync(request.UserId, cancellationToken);

        return collections.Select(collection => new UserCollectionDto
        {
            CardId = collection.CardId,
            Level = collection.Level,
            Copies = collection.Copies,
            CurrentExp = collection.CurrentExp,
            ExpToNextLevel = collection.ExpToNextLevel,
            LastDrawnAt = collection.LastDrawnAt,
            BaseAtk = collection.BaseAtk,
            BaseDef = collection.BaseDef,
            BonusAtkPercent = collection.BonusAtkPercent,
            BonusDefPercent = collection.BonusDefPercent,
            TotalAtk = collection.TotalAtk,
            TotalDef = collection.TotalDef,
            Atk = collection.TotalAtk,
            Def = collection.TotalDef,
            ExpGained = collection.CurrentExp,
        }).ToList();
    }
}
