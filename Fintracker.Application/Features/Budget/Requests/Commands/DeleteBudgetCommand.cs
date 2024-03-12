using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Commands;

public class DeleteBudgetCommand : IRequest<DeleteCommandResponse<BudgetBaseDTO>>
{
    public Guid Id { get; set; }
}