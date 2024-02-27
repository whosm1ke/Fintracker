using Fintracker.Application.DTO.Budget;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetWithUserByIdRequest : IRequest<BudgetWithUserDTO>
{
    public Guid Id { get; set; }
}