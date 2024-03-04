using Fintracker.Application.DTO.Budget;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetsByWalletIdSortedRequest : IRequest<IReadOnlyList<BudgetBaseDTO>>
{
    public Guid WalletId { get; set; }
    public string SortBy { get; set; }
    
    public bool IsDescending { get; set; }
}