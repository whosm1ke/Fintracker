using Fintracker.Application.DTO.Category;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Commands;

public class UpdateCategoryCommand : IRequest<UpdateCommandResponse<CategoryDTO>>
{
    public UpdateCategoryDTO Category { get; set; }
}