using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Commands;

public record GrantAllTitlesCommand(Guid UserId) : IRequest<bool>;

public class GrantAllTitlesCommandHandler : IRequestHandler<GrantAllTitlesCommand, bool>
{
    private readonly ITitleRepository _titleRepository;

    public GrantAllTitlesCommandHandler(ITitleRepository titleRepository)
    {
        _titleRepository = titleRepository;
    }

    public async Task<bool> Handle(GrantAllTitlesCommand request, CancellationToken cancellationToken)
    {
        var titles = await _titleRepository.GetAllTitlesAsync(cancellationToken);
        foreach (var title in titles)
        {
            if (!await _titleRepository.OwnsTitleAsync(request.UserId, title.Code, cancellationToken))
            {
                await _titleRepository.GrantTitleAsync(request.UserId, title.Code, cancellationToken);
            }
        }

        return true;
    }
}
