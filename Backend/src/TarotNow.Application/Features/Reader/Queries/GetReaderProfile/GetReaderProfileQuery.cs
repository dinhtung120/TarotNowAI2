

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Reader.Queries.GetReaderProfile;

public class GetReaderProfileQuery : IRequest<ReaderProfileDto?>
{
        public string UserId { get; set; } = string.Empty;
}
