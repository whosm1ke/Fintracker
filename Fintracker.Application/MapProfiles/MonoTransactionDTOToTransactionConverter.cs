using AutoMapper;
using Fintracker.Application.DTO.Monobank;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.MapProfiles;

public class MonoTransactionDTOToTransactionConverter : ITypeConverter<MonoTransactionDTO, Transaction>
{
    public Transaction Convert(MonoTransactionDTO source, Transaction destination, ResolutionContext context)
    {
        return new Transaction()
        {
            Amount = source.Amount,
            Label = source.Comment,
            Note = source.Description,
            IsBankTransaction = true,
            CreatedAt = DateTime.UnixEpoch.AddSeconds(source.Time)
        };
    }
}