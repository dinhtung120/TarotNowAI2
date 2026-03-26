using MediatR;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public string Role { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public long DiamondBalance { get; set; }

    public long GoldBalance { get; set; }

    public string IdempotencyKey { get; set; } = string.Empty;
}
