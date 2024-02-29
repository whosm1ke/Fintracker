using AutoMapper;
using Fintracker.Application.DTO.Currency;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.MapProfiles;

public class CurrencyProfile : Profile
{
    public CurrencyProfile()
    {
        CreateMap<Currency, CurrencyDTO>().ReverseMap();
    }
}