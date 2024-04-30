using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Models;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetsByWalletIdSortedRequest : IRequest<IReadOnlyList<BudgetBaseDTO>>
{
    public Guid WalletId { get; set; }

    public Guid UserId { get; set; }
    public BudgetQueryParams Params { get; set; } = default!;
}