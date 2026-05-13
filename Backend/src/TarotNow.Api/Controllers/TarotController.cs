using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Reading.Commands.InitSession;
using TarotNow.Application.Features.Reading.Queries.GetCardsCatalog;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Reading)]
[Authorize]
[EnableRateLimiting("auth-session")]
// API reading tarot.
// Luồng chính: khởi tạo phiên, reveal kết quả, lấy catalog lá bài và bộ sưu tập người dùng.
public class TarotController : ControllerBase
{
    private const int CollectionCatalogChunkSize = 16;
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller reading tarot.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query reading.</param>
    public TarotController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Khởi tạo reading session mới.
    /// Luồng xử lý: xác thực user id, gắn vào command init, dispatch và trả kết quả phiên.
    /// </summary>
    /// <param name="command">Command khởi tạo session.</param>
    /// <returns>Thông tin session đã khởi tạo.</returns>
    [HttpPost("init")]
    public async Task<IActionResult> InitSession([FromBody] InitReadingSessionCommand command)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn init session khi danh tính user không hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Luôn ghi đè UserId từ token để tránh giả mạo chủ thể trong payload.
        command.UserId = userId;

        var result = await _mediator.SendWithRequestCancellation(HttpContext, command);
        return Ok(result);
    }

    /// <summary>
    /// Reveal bài cho reading session hiện tại.
    /// Luồng xử lý: xác thực user id, gắn vào command reveal, dispatch và trả kết quả.
    /// </summary>
    /// <param name="command">Command reveal lá bài/session.</param>
    /// <returns>Kết quả reveal của session.</returns>
    [HttpPost("reveal")]
    public async Task<IActionResult> RevealCards([FromBody] TarotNow.Application.Features.Reading.Commands.RevealSession.RevealReadingSessionCommand command)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn reveal khi request không có user id hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Gắn user id từ context để handler kiểm tra quyền truy cập session.
        command.UserId = userId;

        var result = await _mediator.SendWithRequestCancellation(HttpContext, command);
        return Ok(result);
    }

    /// <summary>
    /// Lấy catalog lá bài tarot công khai (legacy full payload).
    /// </summary>
    /// <param name="cancellationToken">Token hủy request.</param>
    /// <returns>Danh mục lá bài dùng cho client hiển thị.</returns>
    [HttpGet("cards-catalog")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCardsCatalog(CancellationToken cancellationToken)
    {
        var result = await _mediator.SendWithRequestCancellation(
            HttpContext,
            new GetCardsCatalogQuery(),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Manifest chunked metadata cho collection.
    /// </summary>
    [HttpGet("cards-catalog/manifest")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCardsCatalogManifest(CancellationToken cancellationToken)
    {
        var projection = await BuildCollectionCatalogProjectionAsync(cancellationToken);
        ApplyManifestCacheHeaders();
        Response.Headers.ETag = $"\"{projection.Version}\"";
        return Ok(projection.Manifest);
    }

    /// <summary>
    /// Chunk metadata cho collection theo chunk id.
    /// </summary>
    [HttpGet("cards-catalog/chunks/{chunkId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCardsCatalogChunk([FromRoute] int chunkId, [FromQuery(Name = "v")] string? version, CancellationToken cancellationToken)
    {
        if (chunkId < 0)
        {
            return this.ApiProblem(
                StatusCodes.Status400BadRequest,
                "INVALID_CATALOG_CHUNK_ID",
                "chunkId must be >= 0.");
        }

        var projection = await BuildCollectionCatalogProjectionAsync(cancellationToken);
        if (!VersionMatches(version, projection.Version))
        {
            return this.ApiProblem(
                StatusCodes.Status409Conflict,
                "CATALOG_VERSION_MISMATCH",
                $"Catalog version mismatch. Current version: {projection.Version}.");
        }

        if (!projection.TryGetChunk(chunkId, out var chunk))
        {
            return this.ApiProblem(
                StatusCodes.Status404NotFound,
                "CATALOG_CHUNK_NOT_FOUND",
                $"Chunk {chunkId} not found for catalog version {projection.Version}.");
        }

        ApplyImmutableCacheHeaders();
        Response.Headers.ETag = $"\"{projection.Version}:chunk:{chunkId}\"";
        return Ok(chunk);
    }

    /// <summary>
    /// Detail metadata của card để mở modal diễn giải.
    /// </summary>
    [HttpGet("cards-catalog/details/{cardId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCardsCatalogDetail([FromRoute] int cardId, [FromQuery(Name = "v")] string? version, CancellationToken cancellationToken)
    {
        if (cardId < 0)
        {
            return this.ApiProblem(
                StatusCodes.Status400BadRequest,
                "INVALID_CATALOG_CARD_ID",
                "cardId must be >= 0.");
        }

        var projection = await BuildCollectionCatalogProjectionAsync(cancellationToken);
        if (!VersionMatches(version, projection.Version))
        {
            return this.ApiProblem(
                StatusCodes.Status409Conflict,
                "CATALOG_VERSION_MISMATCH",
                $"Catalog version mismatch. Current version: {projection.Version}.");
        }

        if (!projection.TryGetDetail(cardId, out var detail))
        {
            return this.ApiProblem(
                StatusCodes.Status404NotFound,
                "CATALOG_CARD_NOT_FOUND",
                $"Card {cardId} not found for catalog version {projection.Version}.");
        }

        ApplyImmutableCacheHeaders();
        Response.Headers.ETag = $"\"{projection.Version}:detail:{cardId}\"";
        return Ok(detail);
    }

    /// <summary>
    /// Lấy bộ sưu tập lá bài của người dùng hiện tại.
    /// </summary>
    /// <returns>Dữ liệu collection của user.</returns>
    [HttpGet("collection")]
    public async Task<IActionResult> GetCollection()
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn collection khi không xác định được user.
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.SendWithRequestCancellation(
            HttpContext,
            new TarotNow.Application.Features.Reading.Queries.GetCollection.GetUserCollectionQuery { UserId = userId }
        );
        return Ok(result);
    }

    private async Task<CollectionCatalogProjection> BuildCollectionCatalogProjectionAsync(CancellationToken cancellationToken)
    {
        var cards = await _mediator.SendWithRequestCancellation(
            HttpContext,
            new GetCardsCatalogQuery(),
            cancellationToken);
        return CollectionCatalogProjectionBuilder.Build(cards, CollectionCatalogChunkSize);
    }

    private static bool VersionMatches(string? requestedVersion, string currentVersion)
    {
        return string.IsNullOrWhiteSpace(requestedVersion)
            || string.Equals(requestedVersion, currentVersion, StringComparison.Ordinal);
    }

    private void ApplyManifestCacheHeaders()
    {
        Response.Headers.CacheControl = "public, max-age=60, s-maxage=60, stale-while-revalidate=120";
    }

    private void ApplyImmutableCacheHeaders()
    {
        Response.Headers.CacheControl = "public, max-age=31536000, immutable";
    }
}
