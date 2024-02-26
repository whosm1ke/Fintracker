using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Commands;

public class UpdateBudgetCommand : IRequest<UpdateCommandResponse<BudgetDTO>>
{
    public UpdateBudgetDTO Budget { get; set; }
}