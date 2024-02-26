using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsByUserIdSortedRequest : IRequest<IReadOnlyList<TransactionDTO>>
{
    public Guid UserId { get; set; }

    public string SortBy { get; set; }
}