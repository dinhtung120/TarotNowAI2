using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Contracts;

public static class CollectionCatalogProjectionBuilder
{
    private const int DefaultChunkSize = 16;

    public static CollectionCatalogProjection Build(IEnumerable<CardCatalogDto> sourceCards, int chunkSize = DefaultChunkSize)
    {
        var cards = sourceCards.OrderBy(card => card.Id).ToList();
        var normalizedChunkSize = chunkSize > 0 ? chunkSize : DefaultChunkSize;
        var version = ComputeVersion(cards);

        var chunkRefs = new List<CollectionCatalogChunkRefDto>();
        var chunksById = new Dictionary<int, CollectionCatalogChunkDto>();
        BuildChunks(cards, normalizedChunkSize, version, chunkRefs, chunksById);

        var detailsByCardId = BuildDetails(cards, version);
        var manifest = BuildManifest(cards.Count, normalizedChunkSize, version, chunkRefs);
        return new CollectionCatalogProjection(manifest, chunksById, detailsByCardId);
    }

    private static void BuildChunks(
        IReadOnlyList<CardCatalogDto> cards,
        int chunkSize,
        string version,
        ICollection<CollectionCatalogChunkRefDto> chunkRefs,
        IDictionary<int, CollectionCatalogChunkDto> chunksById)
    {
        var chunkId = 0;
        for (var start = 0; start < cards.Count; start += chunkSize)
        {
            var slice = cards.Skip(start).Take(Math.Min(chunkSize, cards.Count - start)).ToList();
            var cardIds = slice.Select(card => card.Id).ToList();
            chunkRefs.Add(new CollectionCatalogChunkRefDto
            {
                ChunkId = chunkId,
                StartIndex = start,
                EndIndex = start + slice.Count - 1,
                Count = slice.Count,
                CardIds = cardIds,
            });
            chunksById[chunkId] = new CollectionCatalogChunkDto
            {
                Version = version,
                ChunkId = chunkId,
                Items = slice.Select(card => BuildChunkItem(card, version)).ToList(),
            };
            chunkId += 1;
        }
    }

    private static Dictionary<int, CollectionCatalogDetailDto> BuildDetails(
        IEnumerable<CardCatalogDto> cards,
        string version)
    {
        var details = new Dictionary<int, CollectionCatalogDetailDto>();
        foreach (var card in cards)
        {
            details[card.Id] = new CollectionCatalogDetailDto
            {
                Version = version,
                Id = card.Id,
                Name = ResolveCardName(card),
                Arcana = card.Arcana,
                Suit = card.Suit,
                Rarity = ResolveRarity(card),
                ThumbUrl = BuildVersionedImageUrl(card.ImageUrl, version, "thumb"),
                FullUrl = BuildVersionedImageUrl(card.ImageUrl, version, "full"),
                UprightKeywords = card.UprightKeywords ?? [],
                UprightDescription = card.UprightDescription,
                ReversedKeywords = card.ReversedKeywords ?? [],
                ReversedDescription = card.ReversedDescription,
            };
        }
        return details;
    }

    private static CollectionCatalogManifestDto BuildManifest(
        int totalCards,
        int chunkSize,
        string version,
        IEnumerable<CollectionCatalogChunkRefDto> chunkRefs)
    {
        return new CollectionCatalogManifestDto
        {
            Version = version,
            ChunkSize = chunkSize,
            TotalCards = totalCards,
            UpdatedAtUtc = DateTime.UtcNow.ToString("O"),
            Chunks = chunkRefs.ToList(),
        };
    }

    private static CollectionCatalogChunkItemDto BuildChunkItem(CardCatalogDto card, string version)
    {
        return new CollectionCatalogChunkItemDto
        {
            Id = card.Id,
            Name = ResolveCardName(card),
            Arcana = card.Arcana,
            Suit = card.Suit,
            Rarity = ResolveRarity(card),
            ThumbUrl = BuildVersionedImageUrl(card.ImageUrl, version, "thumb"),
            FullUrl = BuildVersionedImageUrl(card.ImageUrl, version, "full"),
        };
    }

    private static string ResolveCardName(CardCatalogDto card)
    {
        if (!string.IsNullOrWhiteSpace(card.NameEn)) return card.NameEn;
        if (!string.IsNullOrWhiteSpace(card.NameVi)) return card.NameVi;
        if (!string.IsNullOrWhiteSpace(card.NameZh)) return card.NameZh;
        return $"Card #{card.Id}";
    }

    private static string ResolveRarity(CardCatalogDto card)
    {
        return string.Equals(card.Arcana, "major", StringComparison.OrdinalIgnoreCase) ? "major" : "minor";
    }

    private static string? BuildVersionedImageUrl(string? source, string version, string variant)
    {
        if (string.IsNullOrWhiteSpace(source)) return source;
        var separator = source.Contains('?') ? '&' : '?';
        return $"{source}{separator}iv={Uri.EscapeDataString(version)}&variant={variant}";
    }

    private static string ComputeVersion(IEnumerable<CardCatalogDto> cards)
    {
        var builder = new StringBuilder();
        foreach (var card in cards)
        {
            builder
                .Append(card.Id).Append('|')
                .Append(card.Code).Append('|')
                .Append(card.NameEn).Append('|')
                .Append(card.NameVi).Append('|')
                .Append(card.NameZh).Append('|')
                .Append(card.Arcana).Append('|')
                .Append(card.Suit).Append('|')
                .Append(card.ImageUrl).Append('|')
                .Append(card.UprightDescription).Append('|')
                .Append(card.ReversedDescription).Append('\n');
        }

        var payload = Encoding.UTF8.GetBytes(builder.ToString());
        var hash = SHA256.HashData(payload);
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
