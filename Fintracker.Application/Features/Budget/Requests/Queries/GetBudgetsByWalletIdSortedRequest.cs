using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Models;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetsByWalletIdSortedRequest : IRequest<IReadOnlyList<BudgetBaseDTO>>
{
    public Guid WalletId { get; set; }
    
    public QueryParams Params { get; set; } = default!;
    public bool? IsPublic { get; set; }
}