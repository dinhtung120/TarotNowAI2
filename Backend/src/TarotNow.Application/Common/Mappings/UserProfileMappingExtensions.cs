using Mapster;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Common.Mappings;

public static class UserProfileMappingExtensions
{
    public static UserProfileDto ToUserProfileDto(this User user)
    {
        return user.Adapt<UserProfileDto>();
    }
}
