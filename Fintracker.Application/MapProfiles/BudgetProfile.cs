using AutoMapper;
using Fintracker.Application.DTO.Budget;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.MapProfiles;

public class BudgetProfile : Profile
{
    public BudgetProfile()
    {
        CreateMap<Budget, BudgetBaseDTO>().ReverseMap();
        CreateMap<Budget, BudgetWithUserDTO>().ReverseMap();
        CreateMap<Budget, BudgetWithWalletDTO>().ReverseMap();
        CreateMap<Budget, CreateBudgetDTO>().ReverseMap();
        CreateMap<Budget, UpdateBudgetDTO>().ReverseMap();
    }
}