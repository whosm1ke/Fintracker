using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Commands;

public class DeleteBudgetCommand : IRequest<DeleteCommandResponse<BudgetBaseDTO>>, INotGetRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
}