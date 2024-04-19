using AutoMapper;
using Fintracker.Application.DTO.Budget;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.MapProfiles;

public class BudgetProfile : Profile
{
    public BudgetProfile()
    {
        CreateMap<Budget, BudgetBaseDTO>().ReverseMap();
        CreateMap<Budget, BudgetPureDTO>().ReverseMap();
        CreateMap<Budget, CreateBudgetDTO>().ReverseMap();
        CreateMap<Budget, UpdateBudgetDTO>().ReverseMap();
    }
}