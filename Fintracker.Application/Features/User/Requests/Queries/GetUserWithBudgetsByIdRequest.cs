using Fintracker.Application.DTO.User;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Queries;

public class GetUserWithBudgetsByIdRequest : IRequest<UserWithBudgetsDTO>
{
    public Guid Id { get; set; }
}