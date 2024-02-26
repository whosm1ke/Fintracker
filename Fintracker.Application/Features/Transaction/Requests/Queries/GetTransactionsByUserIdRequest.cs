using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsByUserIdRequest : IRequest<IReadOnlyList<TransactionDTO>>
{
    public Guid UserId { get; set; }
}