using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Commands;

public class DeleteBudgetCommand : IRequest<DeleteCommandResponse<BudgetDTO>>
{
    public Guid Id { get; set; }
}