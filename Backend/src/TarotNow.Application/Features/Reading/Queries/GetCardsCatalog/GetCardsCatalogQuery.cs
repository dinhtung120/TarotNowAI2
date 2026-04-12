using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Options;

namespace TarotNow.Application.Features.Reading.Queries.GetCardsCatalog;

// Query lấy toàn bộ catalog lá bài.
public class GetCardsCatalogQuery : IRequest<List<CardCatalogDto>>
{
}

// Handler truy vấn catalog lá bài.
public class GetCardsCatalogQueryHandler : IRequestHandler<GetCardsCatalogQuery, List<CardCatalogDto>>
{
    private readonly ICardsCatalogRepository _cardsCatalogRepository;
    private readonly MediaCdnOptions _cdnOptions;

    /// <summary>
    /// Khởi tạo handler lấy card catalog.
    /// Luồng xử lý: nhận repository catalog để tải danh sách toàn bộ lá bài từ persistence.
    /// </summary>
    public GetCardsCatalogQueryHandler(
        ICardsCatalogRepository cardsCatalogRepository,
        IOptions<MediaCdnOptions> cdnOptions)
    {
        _cardsCatalogRepository = cardsCatalogRepository;
        _cdnOptions = cdnOptions.Value;
    }

    /// <summary>
    /// Xử lý query lấy card catalog.
    /// Luồng xử lý: tải toàn bộ bản ghi card và sắp xếp tăng dần theo id để client hiển thị ổn định.
    /// </summary>
    public async Task<List<CardCatalogDto>> Handle(GetCardsCatalogQuery request, CancellationToken cancellationToken)
    {
        var cards = await _cardsCatalogRepository.GetAllAsync(cancellationToken);
        var ordered = cards.OrderBy(card => card.Id).ToList();
        var baseUrl = _cdnOptions.PublicBaseUrl?.Trim() ?? string.Empty;

        foreach (var card in ordered)
        {
            card.ImageUrl = ResolveCatalogImageUrl(card.ImageUrl, baseUrl);
        }

        return ordered;
    }

    private static string? ResolveCatalogImageUrl(string? imageUrl, string cdnBaseUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return imageUrl;
        }

        var u = imageUrl.Trim();
        if (u.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            u.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return u;
        }

        if (string.IsNullOrWhiteSpace(cdnBaseUrl))
        {
            return u.StartsWith('/') ? u : $"/{u}";
        }

        var path = u.TrimStart('/');
        return $"{cdnBaseUrl.TrimEnd('/')}/{path}";
    }
}
