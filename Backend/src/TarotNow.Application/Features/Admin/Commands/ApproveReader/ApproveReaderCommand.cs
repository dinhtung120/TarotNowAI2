

using MediatR;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

public class ApproveReaderCommand : IRequest<bool>
{
        public string RequestId { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string? AdminNote { get; set; }

        public Guid AdminId { get; set; }
}
