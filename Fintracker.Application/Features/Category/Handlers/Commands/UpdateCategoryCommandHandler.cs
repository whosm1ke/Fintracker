﻿using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Category.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Responses;
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
        var validator = new UpdateCategoryDtoValidator(_unitOfWork);
        var validationResult = await validator.ValidateAsync(request.Category);

        var category = await _unitOfWork.CategoryRepository.GetAsync(request.Category.Id);
        
        if (category is null)
            throw new NotFoundException(nameof(Domain.Entities.Category), request.Category.Id);
        
        if (validationResult.IsValid)
        {
            var oldCategory = _mapper.Map<CategoryDTO>(category);
            _mapper.Map(request.Category, category);
            
            await _unitOfWork.CategoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            
            var newCategory = _mapper.Map<CategoryDTO>(category);

            response.Success = true;
            response.Message = "Updated successfully";
            response.Old = oldCategory;
            response.New = newCategory;
            response.Id = category.Id;
        }
        else
        {
            throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        return response;
    }
}