using AutoMapper;
using Fintracker.Application.DTO.User;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.MapProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserBaseDTO>().ReverseMap();
        CreateMap<User, CreateUserDTO>().ReverseMap();
        CreateMap<User, UpdateUserDTO>().ReverseMap();
        CreateMap<UserDetails, UserDetailsDTO>().ReverseMap();
        CreateMap<User, UserWithBudgetsDTO>().ReverseMap();
        CreateMap<User, UserWithWalletsDTO>().ReverseMap();
    }
}