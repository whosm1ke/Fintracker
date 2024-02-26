using Fintracker.Application.DTO.Budget;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetByIdRequest : IRequest<BudgetBaseDTO>
{
    public Guid Id { get; set; }
}