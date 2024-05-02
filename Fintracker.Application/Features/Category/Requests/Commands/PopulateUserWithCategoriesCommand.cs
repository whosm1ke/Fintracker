using Fintracker.Application.Contracts.Helpers;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Commands;

public class PopulateUserWithCategoriesCommand : IRequest<Unit>, INotGetRequest
{
    public string PathToFile { get; set; }

    public Guid UserId { get; set; }

}