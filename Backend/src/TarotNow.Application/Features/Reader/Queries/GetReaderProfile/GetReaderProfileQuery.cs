using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Reader.Queries.GetReaderProfile;

// Query lấy hồ sơ Reader theo user id.
public class GetReaderProfileQuery : IRequest<ReaderProfileDto?>
{
    // Định danh user của reader cần tải profile.
    public string UserId { get; set; } = string.Empty;
}
