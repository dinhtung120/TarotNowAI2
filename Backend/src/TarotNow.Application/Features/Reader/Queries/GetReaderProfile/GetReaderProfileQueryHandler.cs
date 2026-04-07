

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.GetReaderProfile;

public class GetReaderProfileQueryHandler : IRequestHandler<GetReaderProfileQuery, ReaderProfileDto?>
{
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    public GetReaderProfileQueryHandler(
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository)
    {
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    public async Task<ReaderProfileDto?> Handle(GetReaderProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _readerProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        
        if (profile != null && string.IsNullOrEmpty(profile.AvatarUrl) && Guid.TryParse(profile.UserId, out var userGuid))
        {
            var userInfoMap = await _userRepository.GetUserBasicInfoMapAsync(new[] { userGuid }, cancellationToken);
            if (userInfoMap.TryGetValue(userGuid, out var info) && !string.IsNullOrEmpty(info.AvatarUrl))
            {
                profile.AvatarUrl = info.AvatarUrl;
            }
        }

        return profile;
    }
}
