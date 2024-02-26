using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsByWalletIdSortedRequest : IRequest<IReadOnlyList<TransactionDTO>>
{
    public Guid WalletId { get; set; }

    public string SortBy { get; set; }
}