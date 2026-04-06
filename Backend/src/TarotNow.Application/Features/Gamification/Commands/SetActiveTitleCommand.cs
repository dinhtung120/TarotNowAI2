using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Commands;

public record SetActiveTitleCommand(Guid UserId, string TitleCode) : IRequest<bool>;

public class SetActiveTitleCommandHandler : IRequestHandler<SetActiveTitleCommand, bool>
{
    private readonly ITitleRepository _titleRepo;
    private readonly IUserRepository _userRepo;

    public SetActiveTitleCommandHandler(ITitleRepository titleRepo, IUserRepository userRepo)
    {
        _titleRepo = titleRepo;
        _userRepo = userRepo;
    }

    public async Task<bool> Handle(SetActiveTitleCommand request, CancellationToken cancellationToken)
    {
        var owns = await _titleRepo.OwnsTitleAsync(request.UserId, request.TitleCode, cancellationToken);
        if (!owns && !string.IsNullOrEmpty(request.TitleCode)) return false;

        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return false;

        user.SetActiveTitle(string.IsNullOrEmpty(request.TitleCode) ? null : request.TitleCode);
        await _userRepo.UpdateAsync(user, cancellationToken);
        return true;
    }
}
