using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Reader.Queries.GetReaderProfile;
using TarotNow.Application.Features.Reader.Queries.ListReaders;

namespace TarotNow.Api.Controllers;

public partial class ReaderController
{
    /// <summary>
    /// Lấy hồ sơ công khai của một Reader theo user id.
    /// </summary>
    [HttpGet("profile/{userId}")]
    public async Task<IActionResult> GetProfile(string userId)
    {
        var profile = await _mediator.Send(new GetReaderProfileQuery { UserId = userId });
        if (profile == null)
        {
            return NotFound(new { message = "Không tìm thấy hồ sơ Reader." });
        }

        ApplyPresenceStatus(profile);
        return Ok(profile);
    }

    /// <summary>
    /// Lấy danh sách Reader công khai với bộ lọc và phân trang.
    /// </summary>
    [HttpGet(ApiRoutes.ReadersAbsolute)]
    public async Task<IActionResult> ListReaders([FromQuery] ListReadersQuery query)
    {
        var result = await _mediator.Send(query);
        foreach (var reader in result.Readers)
        {
            ApplyPresenceStatus(reader);
        }

        return Ok(result);
    }
}
