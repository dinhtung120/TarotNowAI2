namespace TarotNow.Api.Contracts;

public sealed class CollectionCatalogManifestDto
{
    public string Version { get; set; } = string.Empty;
    public int ChunkSize { get; set; }
    public int TotalCards { get; set; }
    public string UpdatedAtUtc { get; set; } = string.Empty;
    public List<CollectionCatalogChunkRefDto> Chunks { get; set; } = [];
}

public sealed class CollectionCatalogChunkRefDto
{
    public int ChunkId { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public int Count { get; set; }
    public List<int> CardIds { get; set; } = [];
}

public sealed class CollectionCatalogChunkDto
{
    public string Version { get; set; } = string.Empty;
    public int ChunkId { get; set; }
    public List<CollectionCatalogChunkItemDto> Items { get; set; } = [];
}

public sealed class CollectionCatalogChunkItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Arcana { get; set; } = string.Empty;
    public string? Suit { get; set; }
    public string Rarity { get; set; } = string.Empty;
    public string? ThumbUrl { get; set; }
    public string? FullUrl { get; set; }
}

public sealed class CollectionCatalogDetailDto
{
    public string Version { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Arcana { get; set; } = string.Empty;
    public string? Suit { get; set; }
    public string Rarity { get; set; } = string.Empty;
    public string? ThumbUrl { get; set; }
    public string? FullUrl { get; set; }
    public List<string> UprightKeywords { get; set; } = [];
    public string UprightDescription { get; set; } = string.Empty;
    public List<string> ReversedKeywords { get; set; } = [];
    public string ReversedDescription { get; set; } = string.Empty;
}

public sealed class CollectionCatalogProjection
{
    private readonly Dictionary<int, CollectionCatalogChunkDto> _chunksById;
    private readonly Dictionary<int, CollectionCatalogDetailDto> _detailsByCardId;

    public CollectionCatalogProjection(
        CollectionCatalogManifestDto manifest,
        Dictionary<int, CollectionCatalogChunkDto> chunksById,
        Dictionary<int, CollectionCatalogDetailDto> detailsByCardId)
    {
        Manifest = manifest;
        Version = manifest.Version;
        _chunksById = chunksById;
        _detailsByCardId = detailsByCardId;
    }

    public CollectionCatalogManifestDto Manifest { get; }
    public string Version { get; }

    public bool TryGetChunk(int chunkId, out CollectionCatalogChunkDto chunk)
        => _chunksById.TryGetValue(chunkId, out chunk!);

    public bool TryGetDetail(int cardId, out CollectionCatalogDetailDto detail)
        => _detailsByCardId.TryGetValue(cardId, out detail!);
}
