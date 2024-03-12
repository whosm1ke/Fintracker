using AutoMapper;
using Fintracker.Application.DTO.User;
using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;

namespace Fintracker.Application.MapProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserBaseDTO>().ReverseMap();
        CreateMap<User, UpdateUserDTO>().ReverseMap();
        CreateMap<UserDetails, UserDetailsDTO>().ReverseMap();
        CreateMap<User, UserWithBudgetsDTO>().ReverseMap();
        CreateMap<User, UserWithOwnedWalletsDTO>().ReverseMap();
        CreateMap<User, UserWithMemberWalletsDTO>().ReverseMap();
        CreateMap<Language, LanguageTypeEnum>().ReverseMap();
    }
}