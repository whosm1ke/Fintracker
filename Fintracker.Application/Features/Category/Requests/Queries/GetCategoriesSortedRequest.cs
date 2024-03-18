using Fintracker.Application.DTO.Category;
using Fintracker.Application.Models;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Queries;

public class GetCategoriesSortedRequest : IRequest<IReadOnlyList<CategoryDTO>>
{
    public QueryParams Params { get; set; }
    
    public Guid UserId { get; set; }
}