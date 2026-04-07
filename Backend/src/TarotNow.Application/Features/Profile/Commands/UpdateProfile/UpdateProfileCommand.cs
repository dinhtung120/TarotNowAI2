

using MediatR;
using System;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    
        public string DisplayName { get; set; } = string.Empty;
    
        public string? AvatarUrl { get; set; }
    
        public DateTime DateOfBirth { get; set; }
}
