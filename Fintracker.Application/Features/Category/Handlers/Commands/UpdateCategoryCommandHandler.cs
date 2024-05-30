using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Category.Handlers.Commands;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, UpdateCommandResponse<CategoryDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateCommandResponse<CategoryDTO>> Handle(UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var response = new UpdateCommandResponse<CategoryDTO>();

        var category = await _unitOfWork.CategoryRepository.GetAsync(request.UserId, request.Category.Id);
        

        if (request.UserId != category!.UserId)
        {
            throw new ForbiddenException(new ExceptionDetails
            {
                ErrorMessage = "User has no access to change this category",
                PropertyName = nameof(Domain.Entities.Category)
            });
        }
        
        var oldCategory = _mapper.Map<CategoryDTO>(category);
        _mapper.Map(request.Category, category);

        
        _unitOfWork.CategoryRepository.Update(category);
        await _unitOfWork.SaveAsync();

        var newCategory = _mapper.Map<CategoryDTO>(category);

        response.Success = true;
        response.Message = "Updated successfully";
        response.Old = oldCategory;
        response.New = newCategory;
        response.Id = category.Id;


        return response;
    }
}