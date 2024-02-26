using Fintracker.Application.DTO.Category;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Queries;

public class GetCategoryByTypeRequest : IRequest<CategoryDTO>
{
    public CategoryTypeEnum Type { get; set; }
}