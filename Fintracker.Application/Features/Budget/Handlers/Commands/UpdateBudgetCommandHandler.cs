using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Budget.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Commands;

public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, UpdateCommandResponse<BudgetBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateCommandResponse<BudgetBaseDTO>> Handle(UpdateBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var response = new UpdateCommandResponse<BudgetBaseDTO>();
        var validator = new UpdateBudgetDtoValidator(_unitOfWork);
        var validationResult = await validator.ValidateAsync(request.Budget);

        if (validationResult.IsValid)
        {
            var budget = await _unitOfWork.BudgetRepository.GetAsync(request.Budget.Id);

            if (budget is null)
                throw new NotFoundException(nameof(Domain.Entities.Budget), request.Budget.Id);

            var oldBudget = _mapper.Map<BudgetBaseDTO>(budget);

            _mapper.Map(request.Budget, budget);
            await _unitOfWork.BudgetRepository.UpdateAsync(budget);
            await _unitOfWork.SaveAsync();

            var newBudget = _mapper.Map<BudgetBaseDTO>(budget);

            response.Message = "Updated successfully";
            response.Success = true;
            response.Id = budget.Id;
            response.Old = oldBudget;
            response.New = newBudget;
        }
        else
        {
            response.Message = "Update failed";
            response.Success = false;
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        }

        return response;
    }
}