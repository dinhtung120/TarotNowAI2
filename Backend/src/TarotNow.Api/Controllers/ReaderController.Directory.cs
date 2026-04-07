using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Reader.Queries.GetReaderProfile;
using TarotNow.Application.Features.Reader.Queries.ListReaders;

namespace TarotNow.Api.Controllers;

public partial class ReaderController
{
        [HttpGet("profile/{userId}")]
    public async Task<IActionResult> GetProfile(string userId)
    {
        var profile = await _mediator.Send(new GetReaderProfileQuery { UserId = userId });
        if (profile == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Reader profile not found",
                detail: "Không tìm thấy hồ sơ Reader.");
        }

        ApplyPresenceStatus(profile);
        return Ok(profile);
    }

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
