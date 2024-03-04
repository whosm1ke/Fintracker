using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsByCategoryIdSortedRequest : IRequest<IReadOnlyList<TransactionBaseDTO>>
{
    public Guid CategoryId { get; set; }

    public string SortBy { get; set; }
    
    public bool IsDescending { get; set; }
}