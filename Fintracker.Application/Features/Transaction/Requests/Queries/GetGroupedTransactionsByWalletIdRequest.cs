using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetGroupedTransactionsByWalletIdRequest : IRequest<IReadOnlyList<GroupedTransactionByDateDTO>>
{
    public Guid WalletId { get; set; }
}