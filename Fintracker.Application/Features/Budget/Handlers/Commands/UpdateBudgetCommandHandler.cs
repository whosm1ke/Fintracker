using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Commands;

public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, UpdateCommandResponse<BudgetBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrencyConverter _currencyConverter;

    public UpdateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrencyConverter currencyConverter)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currencyConverter = currencyConverter;
    }

    public async Task<UpdateCommandResponse<BudgetBaseDTO>> Handle(UpdateBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var response = new UpdateCommandResponse<BudgetBaseDTO>();

        var budget = await _unitOfWork.BudgetRepository.GetBudgetByIdAsync(request.Budget.Id);

        if (budget is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Budget.Id}]",
                PropertyName = nameof(request.Budget.Id)
            }, nameof(Domain.Entities.Budget));


        var oldBudget = _mapper.Map<BudgetBaseDTO>(budget);


        await UpdateBudget(budget, request.Budget);
        await UpdateBudgetAccessibility(budget, request.Budget);

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

    private async Task UpdateBudgetAccessibility(Domain.Entities.Budget budget, UpdateBudgetDTO newBudget)
    {
        if (newBudget.IsPublic == budget.IsPublic) return;
        
        var wallet = await _unitOfWork.WalletRepository.GetWalletById(budget.WalletId);
        if (newBudget.IsPublic && !budget.IsPublic)
        {
            foreach (var walletUser in wallet!.Users)
            {
                walletUser.MemberBudgets.Add(budget);
            }

            budget.IsPublic = newBudget.IsPublic;
        }

        if (!newBudget.IsPublic && budget.IsPublic)
        {
            foreach (var walletUser in wallet!.Users)
            {
                walletUser.MemberBudgets.Remove(budget);
            }

            budget.IsPublic = newBudget.IsPublic;
        }
    }

    private async Task UpdateBudget(Domain.Entities.Budget oldBudget, UpdateBudgetDTO newBudget)
    {
        oldBudget.Name = newBudget.Name;
        oldBudget.StartBalance = newBudget.StartBalance;
        oldBudget.Transactions = new HashSet<Domain.Entities.Transaction>();
        oldBudget.Categories = new HashSet<Domain.Entities.Category>();
        oldBudget.StartDate = newBudget.StartDate;
        oldBudget.EndDate = newBudget.EndDate;


        var newStartDate = newBudget.StartDate;
        var newEndDate = newBudget.EndDate;
        var newCurrency = await _unitOfWork.CurrencyRepository.GetAsync(newBudget.CurrencyId);

        var categories = await _unitOfWork.CategoryRepository
            .GetAllWithIds(newBudget.CategoryIds, oldBudget.OwnerId);
        foreach (var category in categories)
        {
            oldBudget.Categories.Add(category);
        }

        var transactionsPerBudget = await
            _unitOfWork.TransactionRepository.GetByWalletIdInRangeAsync(newBudget.WalletId, newStartDate, newEndDate);

        var filteredTransactions = transactionsPerBudget.Where(x => newBudget.CategoryIds.Contains(x.CategoryId))
            .ToList();


        var transactionCurrencySymbols = filteredTransactions.Select(x => x.Currency.Symbol).ToList();
        var transactionAmounts = filteredTransactions.Select(x => x.Amount).ToList();


        decimal totalSpent = 0;
        if (oldBudget.CurrencyId != newBudget.CurrencyId)
        {
            List<ConvertCurrencyDTO?> convertedResult =
                await _currencyConverter.Convert(transactionCurrencySymbols, newCurrency!.Symbol, transactionAmounts);
            convertedResult.ForEach(x => totalSpent += x.Value);
            oldBudget.Currency = newCurrency;
        }
        else
        {
            filteredTransactions.ForEach(t => totalSpent += t.Amount);
        }

        filteredTransactions.ForEach(x => oldBudget.Transactions.Add(x));
        oldBudget.TotalSpent = totalSpent;
        oldBudget.Balance = oldBudget.StartBalance - totalSpent;
    }
}