

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IReaderProfileRepository readerProfileRepository)
    {
        _userRepository = userRepository;
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with Id {request.UserId} not found.");

        
        
        user.UpdateProfile(request.DisplayName, request.AvatarUrl, request.DateOfBirth);

        
        await _userRepository.UpdateAsync(user);

        
        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(user.Id.ToString(), cancellationToken);
        if (readerProfile != null)
        {
            readerProfile.DisplayName = request.DisplayName;
            readerProfile.AvatarUrl = request.AvatarUrl;
            await _readerProfileRepository.UpdateAsync(readerProfile, cancellationToken);
        }

        return true;
    }
}
