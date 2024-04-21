using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Commands;

public class CreateBudgetCommand : IRequest<CreateCommandResponse<BudgetBaseDTO>>, INotGetRequest
{
    public CreateBudgetDTO Budget { get; set; } = default!;
}