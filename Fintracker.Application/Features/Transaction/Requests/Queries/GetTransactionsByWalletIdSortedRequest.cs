using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsByWalletIdSortedRequest : IRequest<IReadOnlyList<TransactionBaseDTO>>
{
    public Guid WalletId { get; set; }

    public string SortBy { get; set; }
    
    public bool IsDescending { get; set; }
}