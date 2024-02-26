using Fintracker.Application.DTO.Budget;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetWithUserByIdRequest : IRequest<BudgetWithWalletDTO>
{
    public Guid Id { get; set; }
}