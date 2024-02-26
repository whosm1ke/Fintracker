using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionWithWalletByIdRequest : IRequest<TransactionDTO>
{
    public Guid Id { get; set; }
}