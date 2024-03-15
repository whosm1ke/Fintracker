using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Commands;

public class
    CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand,
    CreateCommandResponse<TransactionBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public CreateTransactionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<CreateCommandResponse<TransactionBaseDTO>> Handle(CreateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<TransactionBaseDTO>();

        var transaction = _mapper.Map<Domain.Entities.Transaction>(request.Transaction);
        await _unitOfWork.TransactionRepository.AddAsync(transaction);

        var createdObj = _mapper.Map<TransactionBaseDTO>(transaction);

        await DecreaseBalanceInWallet(transaction.WalletId, transaction.Amount);
        if (transaction.UserId.HasValue)
            await DecreaseBalanceInBudgets(transaction.CategoryId, transaction.Amount, transaction.UserId.Value);

        response.Success = true;
        response.Message = "Created successfully";
        response.Id = transaction.Id;
        response.CreatedObject = createdObj;

        await _unitOfWork.SaveAsync();


        return response;
    }

    private async Task DecreaseBalanceInBudgets(Guid categoryId, decimal amount, Guid userId)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetsByCategoryId(categoryId);

        foreach (var budget in budgets)
        {
            if (budget.IsPublic || budget.UserId == userId)
            {
                budget.Balance -= amount;
            }
        }
    }

    private async Task DecreaseBalanceInWallet(Guid walletId, decimal amount)
    {
        var wallet = await _unitOfWork.WalletRepository.GetAsync(walletId);

        wallet!.Balance -= amount;
    }
}