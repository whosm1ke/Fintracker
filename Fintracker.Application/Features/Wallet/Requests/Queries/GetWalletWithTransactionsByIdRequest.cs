using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletWithTransactionsByIdRequest : IRequest<WalletWithTransactionsDTO>
{
    public Guid Id { get; set; }
}