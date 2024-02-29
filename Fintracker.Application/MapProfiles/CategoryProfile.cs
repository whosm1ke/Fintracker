using AutoMapper;
using Fintracker.Application.DTO.Category;
using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;

namespace Fintracker.Application.MapProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Category, CreateCategoryDTO>().ReverseMap();
        CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
        CreateMap<CategoryType, CategoryTypeEnum>().ReverseMap();
    }
}