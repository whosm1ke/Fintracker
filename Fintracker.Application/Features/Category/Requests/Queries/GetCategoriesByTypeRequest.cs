using Fintracker.Application.DTO.Category;
using Fintracker.Domain.Enums;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Queries;

public class GetCategoriesByTypeRequest : IRequest<IReadOnlyList<CategoryDTO>>
{
    public CategoryType Type { get; set; }

    public Guid UserId { get; set; }
}

