using Fintracker.Application.DTO.Budget;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetsByUserIdSortedRequest : IRequest<IReadOnlyList<BudgetBaseDTO>>
{
    public Guid UserId { get; set; }
    public string SortBy { get; set; }
}