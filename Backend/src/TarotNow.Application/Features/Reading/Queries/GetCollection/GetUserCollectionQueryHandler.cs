using MediatR;
using TarotNow.Application.Interfaces;

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
        var collections = await _collectionRepo.GetUserCollectionAsync(request.UserId, cancellationToken);
        
        return collections.Select(c => new UserCollectionDto
        {
            CardId = c.CardId,
            Level = c.Level,
            Copies = c.Copies,
            ExpGained = c.ExpGained,
            LastDrawnAt = c.LastDrawnAt
        }).ToList();
    }
}
