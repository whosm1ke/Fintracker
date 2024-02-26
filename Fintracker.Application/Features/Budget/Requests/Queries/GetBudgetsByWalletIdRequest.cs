using Fintracker.Application.DTO.Budget;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetsByWalletIdRequest : IRequest<IReadOnlyList<BudgetDTO>>
{
    public Guid UserId { get; set; }
}