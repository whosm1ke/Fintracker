using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Category.Handlers.Commands;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeleteCommandResponse<CategoryDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeleteCommandResponse<CategoryDTO>> Handle(DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var response = new DeleteCommandResponse<CategoryDTO>();
        var category = await _unitOfWork.CategoryRepository.GetAsync(request.Id);

        if (category is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Attempt to delete non-existing item by id [{request.Id}]",
                PropertyName = nameof(request.Id)
            },nameof(Domain.Entities.Category));

        var deletedObj = _mapper.Map<CategoryDTO>(category);
        _unitOfWork.CategoryRepository.Delete(category);
        await _unitOfWork.SaveAsync();

        response.DeletedObj = deletedObj;
        response.Message = "Deleted successfully";
        response.Success = true;
        response.Id = deletedObj.Id;

        return response;
    }
}