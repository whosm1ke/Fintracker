using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
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

        var budget = await _unitOfWork.BudgetRepository.GetBudgetAsync(request.Budget.Id);

        if (budget is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Budget.Id}]",
                PropertyName = nameof(request.Budget.Id)
            }, nameof(Domain.Entities.Budget));


        var oldBudget = _mapper.Map<BudgetBaseDTO>(budget);

        if (request.Budget.CurrencyId != budget.CurrencyId)
        {
            var currency = await _unitOfWork.CurrencyRepository.GetAsync(request.Budget.CurrencyId);
            budget.Currency = currency ?? budget.Currency;
        }

        _mapper.Map(request.Budget, budget);


        var categories = await _unitOfWork.CategoryRepository
            .GetAllWithIds(request.Budget.CategoryIds, budget.UserId);
        budget.Categories = new HashSet<Domain.Entities.Category>();
        foreach (var category in categories)
        {
            budget.Categories.Add(category);
        }

        _unitOfWork.BudgetRepository.Update(budget);
        await _unitOfWork.SaveAsync();

        var newBudget = _mapper.Map<BudgetBaseDTO>(budget);

        response.Message = "Updated successfully";
        response.Success = true;
        response.Id = budget.Id;
        response.Old = oldBudget;
        response.New = newBudget;


        return response;
    }
}