using MediatR;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public class ResolveDisputeCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }

    public Guid AdminId { get; set; }

    public string Action { get; set; } = string.Empty;

    public int? SplitPercentToReader { get; set; }

    public string? AdminNote { get; set; }
}
