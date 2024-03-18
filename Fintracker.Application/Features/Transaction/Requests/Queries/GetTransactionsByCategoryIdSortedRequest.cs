using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Models;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsByCategoryIdSortedRequest : IRequest<IReadOnlyList<TransactionBaseDTO>>
{
    public Guid CategoryId { get; set; }

    public QueryParams Params { get; set; } = default!;
}