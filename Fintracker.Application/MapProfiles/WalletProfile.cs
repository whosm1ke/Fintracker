using AutoMapper;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.MapProfiles;

public class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<Wallet, WalletBaseDTO>().ReverseMap();
        CreateMap<Wallet, CreateWalletDTO>().ReverseMap();
        CreateMap<Wallet, UpdateWalletDTO>().ReverseMap();
        CreateMap<Wallet, WalletWithBudgetsDTO>().ReverseMap();
        CreateMap<Wallet, WalletWithMembersDTO>().ReverseMap();
        CreateMap<Wallet, WalletWithTransactionsDTO>().ReverseMap();
        
    }
}