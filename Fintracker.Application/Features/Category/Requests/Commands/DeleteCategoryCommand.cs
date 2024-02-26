using Fintracker.Application.DTO.Category;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Commands;

public class DeleteCategoryCommand : IRequest<DeleteCommandResponse<CategoryDTO>>
{
    public Guid Id { get; set; }
}