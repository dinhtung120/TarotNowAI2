using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Common.Mappings;

public static class UserProfileMappingExtensions
{
    public static UserProfileDto ToUserProfileDto(this User user)
    {
        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Level = user.Level,
            Exp = user.Exp,
            Role = user.Role,
            Status = user.Status.ToString()
        };
    }
}
