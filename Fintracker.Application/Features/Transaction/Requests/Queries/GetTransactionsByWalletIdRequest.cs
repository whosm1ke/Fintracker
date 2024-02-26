using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsByWalletIdRequest : IRequest<IReadOnlyList<TransactionBaseDTO>>
{
    public Guid WalletId { get; set; }
}