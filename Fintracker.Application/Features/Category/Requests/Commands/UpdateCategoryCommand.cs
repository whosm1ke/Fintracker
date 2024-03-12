using Fintracker.Application.DTO.Category;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Commands;

public class UpdateCategoryCommand : IRequest<UpdateCommandResponse<CategoryDTO>>
{
    public UpdateCategoryDTO Category { get; set; }
}