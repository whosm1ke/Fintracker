using Fintracker.Application.DTO.Budget;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetWithWalletByIdRequest : IRequest<BudgetDTO>
{
    public Guid Id { get; set; }
}