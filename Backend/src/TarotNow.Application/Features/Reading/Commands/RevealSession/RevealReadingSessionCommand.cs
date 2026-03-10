using MediatR;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

public class RevealReadingSessionCommand : IRequest<RevealReadingSessionResult>
{
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
}

public class RevealReadingSessionResult
{
    // Mảng chứa ID các lá bài được bốc (vd: [0, 4, 12])
    public int[] Cards { get; set; } = Array.Empty<int>();
}
