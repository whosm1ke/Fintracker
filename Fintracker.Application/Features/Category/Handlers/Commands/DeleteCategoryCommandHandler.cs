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
        var category = await _unitOfWork.CategoryRepository.GetAsync(request.UserId, request.Id);

        if (category is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Attempt to delete non-existing item by id [{request.Id}]",
                PropertyName = nameof(request.Id)
            }, nameof(Domain.Entities.Category));

        if (request.UserId != category.UserId)
        {
            throw new ForbiddenException(new ExceptionDetails
            {
                ErrorMessage = "User has no access to change this category",
                PropertyName = nameof(Domain.Entities.Category)
            });
        }

        Domain.Entities.Category? categoryToReplace = null;
        if (request.ShouldReplace)
            categoryToReplace = await _unitOfWork.CategoryRepository.GetAsync(request.CategoryToReplaceId);

        await UpdateTransactionns(category, categoryToReplace);
        await UpdateWallets(category, categoryToReplace);
        await UpdateBudgets(category, categoryToReplace);

        var deletedObj = _mapper.Map<CategoryDTO>(category);
        _unitOfWork.CategoryRepository.Delete(category);
        await _unitOfWork.SaveAsync();

        response.DeletedObj = deletedObj;
        response.Message = "Deleted successfully";
        response.Success = true;
        response.Id = deletedObj.Id;

        return response;
    }

    private async Task UpdateBudgets(Domain.Entities.Category category, Domain.Entities.Category? categoryToReplace)
    {
        throw new NotImplementedException();
    }

    private async Task UpdateWallets(Domain.Entities.Category category, Domain.Entities.Category? categoryToReplace)
    {
        throw new NotImplementedException();
    }

    private async Task UpdateTransactionns(Domain.Entities.Category category, Domain.Entities.Category? categoryToReplace)
    {
        throw new NotImplementedException();
    }
}