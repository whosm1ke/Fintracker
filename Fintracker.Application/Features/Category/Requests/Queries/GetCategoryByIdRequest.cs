using Fintracker.Application.DTO.Category;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Queries;

public class GetCategoryByIdRequest : IRequest<CategoryDTO>
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
}