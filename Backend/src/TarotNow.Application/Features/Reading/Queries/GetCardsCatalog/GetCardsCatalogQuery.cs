using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Queries.GetCardsCatalog;

public class GetCardsCatalogQuery : IRequest<List<CardCatalogDto>>
{
}

public class GetCardsCatalogQueryHandler : IRequestHandler<GetCardsCatalogQuery, List<CardCatalogDto>>
{
    private readonly ICardsCatalogRepository _cardsCatalogRepository;

    public GetCardsCatalogQueryHandler(ICardsCatalogRepository cardsCatalogRepository)
    {
        _cardsCatalogRepository = cardsCatalogRepository;
    }

    public async Task<List<CardCatalogDto>> Handle(GetCardsCatalogQuery request, CancellationToken cancellationToken)
    {
        var cards = await _cardsCatalogRepository.GetAllAsync(cancellationToken);
        return cards.OrderBy(card => card.Id).ToList();
    }
}
