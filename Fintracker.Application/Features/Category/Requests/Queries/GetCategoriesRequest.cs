using Fintracker.Application.DTO.Category;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Queries;

public class GetCategoriesRequest : IRequest<IReadOnlyList<CategoryDTO>>
{
}