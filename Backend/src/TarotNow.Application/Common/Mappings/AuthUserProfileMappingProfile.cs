using AutoMapper;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Common.Mappings;

public sealed class AuthUserProfileMappingProfile : Profile
{
    public AuthUserProfileMappingProfile()
    {
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
            .ForMember(dest => dest.Exp, opt => opt.MapFrom(src => src.Exp));
    }
}
