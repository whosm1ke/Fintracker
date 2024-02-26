using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsByCategoryIdSortedRequest : IRequest<IReadOnlyList<TransactionDTO>>
{
    public Guid CategoryId { get; set; }

    public string SortBy { get; set; }
}