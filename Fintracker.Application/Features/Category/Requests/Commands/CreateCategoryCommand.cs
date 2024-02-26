using Fintracker.Application.DTO.Category;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Commands;

public class CreateCategoryCommand : IRequest<CreateCommandResponse<CategoryDTO>>
{
    public CreateCategoryDTO Category { get; set; }
}