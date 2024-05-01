using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
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
    private readonly ICurrencyConverter _currencyConverter;

    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrencyConverter currencyConverter)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currencyConverter = currencyConverter;
    }

    public async Task<CreateCommandResponse<CreateBudgetDTO>> Handle(CreateBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<CreateBudgetDTO>();


        var budgetEntity = _mapper.Map<Domain.Entities.Budget>(request.Budget);
        var categories = await _unitOfWork.CategoryRepository
            .GetAllWithIds(request.Budget.CategoryIds, request.Budget.OwnerId);

        foreach (var category in categories)
        {
            budgetEntity.Categories.Add(category);
        }

        var budgetCurrency = await _unitOfWork.CurrencyRepository.GetAsync(budgetEntity.CurrencyId);

        await PopulateBudgetWithTransactionsAndCalculateBalance(budgetEntity.WalletId, budgetEntity.StartDate,
            budgetEntity.EndDate, budgetEntity, budgetCurrency!.Symbol, request.Budget.CategoryIds);

        await AddBudgetToMemberUsersIfPublic(budgetEntity);
    
        await _unitOfWork.BudgetRepository.AddAsync(budgetEntity);


        response.Message = "Created successfully";
        response.Success = true;
        response.Id = budgetEntity.Id;
        response.CreatedObject = request.Budget;

        await _unitOfWork.SaveAsync();


        return response;
    }

    private async Task AddBudgetToMemberUsersIfPublic(Domain.Entities.Budget budget)
    {
        if (!budget.IsPublic) return;

        var wallet = await _unitOfWork.WalletRepository.GetWalletById(budget.WalletId);
        foreach (var walletUser in wallet!.Users)
        {
            walletUser.MemberBudgets.Add(budget);
        }
    }

    private async Task PopulateBudgetWithTransactionsAndCalculateBalance(Guid walletId, DateTime budgetStart, DateTime budgetEnd, Domain.Entities.Budget budget,
        string budgetCurrencySymbol, ICollection<Guid> categoryIds)
    {
        var transactionsPerBudget = await
            _unitOfWork.TransactionRepository.GetByWalletIdInRangeAsync(walletId, budgetStart, budgetEnd);

        var filteredTransactions = transactionsPerBudget.Where(x => categoryIds.Contains(x.CategoryId))
            .ToList();

        var transactionCurrencySymbols = filteredTransactions.Select(x => x.Currency.Symbol);
        var transactionAmounts = filteredTransactions.Select(x => x.Amount);

        var convertedResult =
            await _currencyConverter.Convert(transactionCurrencySymbols, budgetCurrencySymbol, transactionAmounts);

        decimal totalSpent = 0;
        convertedResult.ForEach(x => totalSpent += x!.Value);

        filteredTransactions.ForEach(x => budget.Transactions.Add(x));
        budget.Balance = budget.StartBalance;
        budget.TotalSpent =  totalSpent;
        budget.Balance -= totalSpent;
    }
}