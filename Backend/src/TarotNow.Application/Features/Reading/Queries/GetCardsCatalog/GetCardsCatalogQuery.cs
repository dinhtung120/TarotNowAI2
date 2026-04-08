using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Queries.GetCardsCatalog;

// Query lấy toàn bộ catalog lá bài.
public class GetCardsCatalogQuery : IRequest<List<CardCatalogDto>>
{
}

// Handler truy vấn catalog lá bài.
public class GetCardsCatalogQueryHandler : IRequestHandler<GetCardsCatalogQuery, List<CardCatalogDto>>
{
    private readonly ICardsCatalogRepository _cardsCatalogRepository;

    /// <summary>
    /// Khởi tạo handler lấy card catalog.
    /// Luồng xử lý: nhận repository catalog để tải danh sách toàn bộ lá bài từ persistence.
    /// </summary>
    public GetCardsCatalogQueryHandler(ICardsCatalogRepository cardsCatalogRepository)
    {
        _cardsCatalogRepository = cardsCatalogRepository;
    }

    /// <summary>
    /// Xử lý query lấy card catalog.
    /// Luồng xử lý: tải toàn bộ bản ghi card và sắp xếp tăng dần theo id để client hiển thị ổn định.
    /// </summary>
    public async Task<List<CardCatalogDto>> Handle(GetCardsCatalogQuery request, CancellationToken cancellationToken)
    {
        var cards = await _cardsCatalogRepository.GetAllAsync(cancellationToken);
        return cards.OrderBy(card => card.Id).ToList();
    }
}
