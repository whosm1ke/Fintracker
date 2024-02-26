using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletWithBudgetsByIdRequest : IRequest<WalletWithBudgetsDTO>
{
    public Guid Id { get; set; }
}