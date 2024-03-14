using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Commands;

public class CreateCategoryCommand : IRequest<CreateCommandResponse<CategoryDTO>>, INotGetRequest
{
    public CreateCategoryDTO Category { get; set; } = default!;
}