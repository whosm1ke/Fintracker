using Fintracker.Application.DTO.User;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Queries;

public class GetUsersWithBudgetsByIdRequest : IRequest<IReadOnlyList<UserWithBudgetsDTO>>
{
    public Guid Id { get; set; }
}