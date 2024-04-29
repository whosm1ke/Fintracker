using AutoMapper;
using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.MapProfiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionBaseDTO>()
            .ForMember(x => x.Username, memOpt => memOpt.MapFrom(t => t.User.UserName))
            .ReverseMap();
        CreateMap<Transaction, TransactionPureDTO>()
            .ReverseMap();
        CreateMap<Transaction, CreateTransactionDTO>().ReverseMap();
        CreateMap<Transaction, UpdateTransactionDTO>().ReverseMap();
        CreateMap<MonoTransactionDTO, Transaction>()
            .ConvertUsing<MonoTransactionDTOToTransactionConverter>();
    }
}