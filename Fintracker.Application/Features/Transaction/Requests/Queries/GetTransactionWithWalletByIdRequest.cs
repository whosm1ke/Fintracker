using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionWithWalletByIdRequest : IRequest<TransactionWithWalletDTO>
{
    public Guid Id { get; set; }
}