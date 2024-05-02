using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using Fintracker.Domain.Enums;
using MediatR;

namespace Fintracker.Application.Features.Category.Handlers.Commands;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeleteCommandResponse<CategoryDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrencyConverter _currencyConverter;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrencyConverter currencyConverter)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currencyConverter = currencyConverter;
    }

    public async Task<DeleteCommandResponse<CategoryDTO>> Handle(DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var response = new DeleteCommandResponse<CategoryDTO>();
        var category = await _unitOfWork.CategoryRepository.GetAsync(request.UserId, request.Id);

        if (category is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Attempt to delete non-existing item by id [{request.Id}]",
                PropertyName = nameof(request.Id)
            }, nameof(Domain.Entities.Category));

        if (request.UserId != category.UserId)
        {
            throw new ForbiddenException(new ExceptionDetails
            {
                ErrorMessage = "User has no access to change this category",
                PropertyName = nameof(Domain.Entities.Category)
            });
        }

        Domain.Entities.Category categoryToReplace = (request.ShouldReplace
            ? await _unitOfWork.CategoryRepository.GetAsync(request.CategoryToReplaceId)
            : category)!;

        var transactionToUpdate =
            await _unitOfWork.TransactionRepository.GetByCategoryIdAsync(category.Id, category.UserId);

        UpdateTransactionns(transactionToUpdate, category, categoryToReplace, request.ShouldReplace);
        await UpdateWallets(transactionToUpdate, category, request.ShouldReplace, categoryToReplace.Type);
        await UpdateBudgets(transactionToUpdate, category, categoryToReplace, request.ShouldReplace);

        var deletedObj = _mapper.Map<CategoryDTO>(category);
        _unitOfWork.CategoryRepository.Delete(category);
        await _unitOfWork.SaveAsync();

        response.DeletedObj = deletedObj;
        response.Message = "Deleted successfully";
        response.Success = true;
        response.Id = deletedObj.Id;

        return response;
    }

    private async Task UpdateBudgets(IReadOnlyList<Domain.Entities.Transaction> transactionToUpdate,
        Domain.Entities.Category category, Domain.Entities.Category categoryToReplace,
        bool replace)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetsByUserIdAsync(category.UserId, null);

        foreach (var budget in budgets)
        {
            if (!replace)
            {
                await HandleCategoryRemoval(transactionToUpdate, category, budget);
            }
            else
            {
                await HandleCategoryReplacement(transactionToUpdate, category, categoryToReplace, budget);
            }

            if (budget.Categories.Count == 0)
            {
                _unitOfWork.BudgetRepository.Delete(budget);
            }
        }
    }

    private async Task HandleCategoryRemoval(IReadOnlyList<Domain.Entities.Transaction> transactionToUpdate,
        Domain.Entities.Category category, Domain.Entities.Budget budget)
    {
        budget.Categories.Remove(category);
        await UpdateBudgetForTransactions(transactionToUpdate, budget, true);
        await CalculateBudgetBalance(budget, transactionToUpdate);
    }

    private async Task HandleCategoryReplacement(IReadOnlyList<Domain.Entities.Transaction> transactionToUpdate,
        Domain.Entities.Category category,
        Domain.Entities.Category categoryToReplace, Domain.Entities.Budget budget)
    {
        budget.Categories.Remove(category);

        if (categoryToReplace.Type == CategoryType.EXPENSE)
        {
            budget.Categories.Add(categoryToReplace);
            await UpdateBudgetForTransactions(transactionToUpdate, budget, true);
            await UpdateBudgetForTransactions(transactionToUpdate, budget, false);
            await CalculateBudgetBalance(budget, transactionToUpdate);
        }
        else
        {
            await UpdateBudgetForTransactions(transactionToUpdate, budget, true);
            await CalculateBudgetBalance(budget, transactionToUpdate);
        }
    }

    private async Task UpdateBudgetForTransactions(IReadOnlyList<Domain.Entities.Transaction> transactionToUpdate,
        Domain.Entities.Budget budget,
        bool isRemoving)
    {
        if (transactionToUpdate.Count == 0) return;

        foreach (var transaction in transactionToUpdate)
        {
            if (isRemoving)
            {
                budget.Transactions.Remove(transaction);
            }
            else
            {
                budget.Transactions.Add(transaction);
            }
        }
    }

    private async Task CalculateBudgetBalance(Domain.Entities.Budget budget,
        IReadOnlyList<Domain.Entities.Transaction> transactions)
    {
        decimal transactionsAmount = transactions.Sum(x =>
        {
            if (x.Category.Type == CategoryType.EXPENSE)
                return -x.Amount;
            return x.Amount;
        });
        var transactionSymbols = transactions.Select(t => t.Currency.Symbol).ToList();

        var convertCurrency =
            await _currencyConverter.Convert(transactionSymbols, budget.Currency.Symbol, transactionsAmount);
        decimal totalSpent = convertCurrency.Sum(x => x.Value);

        budget.Balance += totalSpent;
        budget.TotalSpent = budget.StartBalance - budget.Balance;
    }

    /*private async Task RemoveTransactionsFromBudget(Domain.Entities.Category category, Domain.Entities.Budget budget)
    {
        var transactions = budget.Transactions.Where(t => t.CategoryId == category.Id).ToList();
        if(transactions.Count == 0) return;

        decimal transactionsAmount = transactions.Sum(x => x.Amount);
        var transactionSymbols = transactions.Select(t => t.Currency.Symbol).ToList();

        var convertCurrency =
            await _currencyConverter.Convert(transactionSymbols, budget.Currency.Symbol, transactionsAmount);
        decimal totalSpent = convertCurrency.Sum(x => x.Value);

        budget.Balance += totalSpent;
        budget.TotalSpent = budget.StartBalance - budget.Balance;

        foreach (var transaction in transactions)
        {
            budget.Transactions.Remove(transaction);
        }
    }

    private async Task AddTransactionsToBudget(Domain.Entities.Category category, Domain.Entities.Budget budget)
    {
        var transactions = await _unitOfWork.TransactionRepository.GetByCategoryIdAsync(category.Id, category.UserId);
        if(transactions.Count == 0) return;

        decimal transactionsAmount = transactions.Sum(x => x.Amount);
        var transactionSymbols = transactions.Select(t => t.Currency.Symbol).ToList();

        var convertCurrency =
            await _currencyConverter.Convert(transactionSymbols, budget.Currency.Symbol, transactionsAmount);
        decimal totalSpent = convertCurrency.Sum(x => x.Value);

        budget.Balance -= totalSpent;
        budget.TotalSpent = budget.StartBalance - budget.Balance;

        foreach (var transaction in transactions)
        {
            budget.Transactions.Add(transaction);
        }
    }*/


    private async Task UpdateWallets(IReadOnlyList<Domain.Entities.Transaction> transactionToUpdate,
        Domain.Entities.Category category, bool replace, CategoryType newCategoryType)
    {
        foreach (var transaction in transactionToUpdate)
        {
            var wallet = transaction.Wallet;
            decimal transAmount = transaction.Amount;
            if (transaction.CurrencyId != transaction.Wallet.CurrencyId)
            {
                var convertCurrency =
                    await _currencyConverter.Convert(transaction.Currency.Symbol, wallet.Currency.Symbol, transAmount);
                transAmount = convertCurrency?.Value ?? transAmount;
            }

            if (replace)
            {
                UpdateWalletForReplacement(wallet, category.Type, newCategoryType, transAmount);
            }
            else
            {
                UpdateWalletForDeletion(wallet, category.Type, transAmount);
            }
        }
    }

    private void UpdateWalletForReplacement(Domain.Entities.Wallet wallet, CategoryType oldCategoryType,
        CategoryType newCategoryType, decimal amount)
    {
        if (oldCategoryType == CategoryType.INCOME && newCategoryType == CategoryType.EXPENSE)
        {
            wallet.Balance -= 2 * amount;
            wallet.TotalSpent += amount;
        }
        else if (oldCategoryType == CategoryType.EXPENSE && newCategoryType == CategoryType.INCOME)
        {
            wallet.Balance += 2 * amount;
            wallet.TotalSpent -= amount;
        }
    }

    private void UpdateWalletForDeletion(Domain.Entities.Wallet wallet, CategoryType oldCategoryType, decimal amount)
    {
        if (oldCategoryType == CategoryType.INCOME)
        {
            wallet.Balance -= amount;
        }
        else
        {
            wallet.Balance += amount;
            wallet.TotalSpent -= amount;
        }
    }


    private void UpdateTransactionns(IReadOnlyList<Domain.Entities.Transaction> transactionToUpdate,
        Domain.Entities.Category category,
        Domain.Entities.Category categoryToReplace, bool replace)
    {
        if (replace)
            foreach (var transaction in transactionToUpdate)
            {
                transaction.Category = categoryToReplace;
                categoryToReplace.TransactionCount += 1;
            }
        else
            foreach (var transaction in transactionToUpdate)
            {
                _unitOfWork.TransactionRepository.Delete(transaction);
            }
    }
}