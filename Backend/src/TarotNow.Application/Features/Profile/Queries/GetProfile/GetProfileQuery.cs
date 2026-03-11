using MediatR;
using System;

namespace TarotNow.Application.Features.Profile.Queries.GetProfile;

public class GetProfileQuery : IRequest<ProfileResponse>
{
    public Guid UserId { get; set; }
}

public class ProfileResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Zodiac { get; set; } = string.Empty;
    public int Numerology { get; set; }
    public int Level { get; set; }
    public long Exp { get; set; }
    public bool HasConsented { get; set; }
}
