using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
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


        var budgetEntity = _mapper.Map<Domain.Entities.Budget>(request.Budget);

        var categories = await _unitOfWork.CategoryRepository.GetAllWithIds(request.Budget.CategoryIds);
        foreach (var category in categories)
        {
            budgetEntity.Categories.Add(category);
        }

        await _unitOfWork.BudgetRepository.AddAsync(budgetEntity);

        response.Message = "Created successfully";
        response.Success = true;
        response.Id = budgetEntity.Id;
        response.CreatedObject = request.Budget;

        await _unitOfWork.SaveAsync();


        return response;
    }
}