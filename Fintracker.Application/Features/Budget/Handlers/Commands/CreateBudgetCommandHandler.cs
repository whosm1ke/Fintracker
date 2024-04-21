using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Commands;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, CreateCommandResponse<BudgetBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<CreateCommandResponse<BudgetBaseDTO>> Handle(CreateBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<BudgetBaseDTO>();


        var budgetEntity = _mapper.Map<Domain.Entities.Budget>(request.Budget);

        var categories = await _unitOfWork.CategoryRepository
            .GetAllWithIds(request.Budget.CategoryIds, request.Budget.UserId);
        foreach (var category in categories)
        {
            budgetEntity.Categories.Add(category);
        }

        budgetEntity.Wallet = await _unitOfWork.WalletRepository.GetWalletById(request.Budget.WalletId) ?? default!;
        budgetEntity.Currency = await _unitOfWork.CurrencyRepository.GetAsync(request.Budget.CurrencyId) ?? default!;
        budgetEntity.User = await _userRepository.GetAsync(request.Budget.UserId) ?? default!;

        await _unitOfWork.BudgetRepository.AddAsync(budgetEntity);

        var createdBudget = _mapper.Map<BudgetBaseDTO>(budgetEntity);

        response.Message = "Created successfully";
        response.Success = true;
        response.Id = budgetEntity.Id;
        response.CreatedObject = createdBudget;

        await _unitOfWork.SaveAsync();


        return response;
    }
}