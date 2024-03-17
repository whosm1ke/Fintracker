using AutoMapper;
using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.MapProfiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionBaseDTO>().ReverseMap();
        CreateMap<Transaction, CreateTransactionDTO>().ReverseMap();
        CreateMap<Transaction, UpdateTransactionDTO>().ReverseMap();
        CreateMap<Transaction, TransactionWithUserDTO>().ReverseMap();
        CreateMap<Transaction, TransactionWithWalletAndUserDTO>().ReverseMap();
        CreateMap<Transaction, TransactionWithWalletDTO>().ReverseMap();
        CreateMap<MonoTransactionDTO, Transaction>()
            .ConvertUsing<MonoTransactionDTOToTransactionConverter>();
    }
}