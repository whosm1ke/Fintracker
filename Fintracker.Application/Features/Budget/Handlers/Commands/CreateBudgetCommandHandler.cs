﻿using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Budget.Validators;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Commands;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, CreateCommandResponse<CreateBudgetDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateCommandResponse<CreateBudgetDTO>> Handle(CreateBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<CreateBudgetDTO>();
        var validator = new CreateBudgetDtoValidator(_unitOfWork);
        var validationResult = await validator.ValidateAsync(request.Budget);

        if (validationResult.IsValid)
        {
            var budgetEntity = _mapper.Map<Domain.Entities.Budget>(request.Budget);
            await _unitOfWork.BudgetRepository.AddAsync(budgetEntity);
            await _unitOfWork.SaveAsync();

            response.Message = "Created successfully";
            response.Success = true;
            response.Id = budgetEntity.Id;
            response.CreatedObject = request.Budget;
        }
        else
        {
            response.Message = "Creation failed";
            response.Success = false;
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        }

        return response;
    }
}