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

        await UpdateTransactionns(category, categoryToReplace, request.ShouldReplace);
        await UpdateWallets(category, request.ShouldReplace, categoryToReplace.Type);
        await UpdateBudgets(category, categoryToReplace, request.ShouldReplace);

        var deletedObj = _mapper.Map<CategoryDTO>(category);
        _unitOfWork.CategoryRepository.Delete(category);
        await _unitOfWork.SaveAsync();

        response.DeletedObj = deletedObj;
        response.Message = "Deleted successfully";
        response.Success = true;
        response.Id = deletedObj.Id;

        return response;
    }

    private async Task UpdateBudgets(Domain.Entities.Category category, Domain.Entities.Category categoryToReplace,
        bool replace)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetsByCategoryId(category.Id, category.UserId);

        foreach (var budget in budgets)
        {
            if (!replace)
            {
                await HandleCategoryRemoval(category, budget);
            }
            else
            {
                await HandleCategoryReplacement(category, categoryToReplace, budget);
            }
        }
    }

    private async Task HandleCategoryRemoval(Domain.Entities.Category category, Domain.Entities.Budget budget)
    {
        budget.Categories.Remove(category);
        await UpdateBudgetForTransactions(category, budget, true);
    }

    private async Task HandleCategoryReplacement(Domain.Entities.Category category,
        Domain.Entities.Category categoryToReplace, Domain.Entities.Budget budget)
    {
        budget.Categories.Remove(category);

        if (categoryToReplace.Type == CategoryType.EXPENSE)
        {
            budget.Categories.Add(categoryToReplace);
            await UpdateBudgetForTransactions(category, budget, true);
            await UpdateBudgetForTransactions(categoryToReplace, budget, false);
        }
        else
        {
            await UpdateBudgetForTransactions(category, budget, true);
        }
    }

    private async Task UpdateBudgetForTransactions(Domain.Entities.Category category, Domain.Entities.Budget budget,
        bool isRemoving)
    {
        var transactions = isRemoving
            ? budget.Transactions.Where(t => t.CategoryId == category.Id).ToList()
            : await _unitOfWork.TransactionRepository.GetByCategoryIdAsync(category.Id, category.UserId);
        
        if(transactions.Count == 0) return;
        
        decimal transactionsAmount = transactions.Sum(x => x.Amount);
        var transactionSymbols = transactions.Select(t => t.Currency.Symbol).ToList();

        var convertCurrency =
            await _currencyConverter.Convert(transactionSymbols, budget.Currency.Symbol, transactionsAmount);
        decimal totalSpent = convertCurrency.Sum(x => x.Value);

        budget.Balance += isRemoving ? totalSpent : -totalSpent;
        budget.TotalSpent += isRemoving ? -totalSpent : totalSpent;

        foreach (var transaction in transactions)
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


    private async Task UpdateWallets(Domain.Entities.Category category, bool replace, CategoryType newCategoryType)
    {
        var transactions = await _unitOfWork.TransactionRepository.GetByCategoryIdAsync(category.Id, category.UserId);

        foreach (var transaction in transactions)
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


    private async Task UpdateTransactionns(Domain.Entities.Category category,
        Domain.Entities.Category categoryToReplace, bool replace)
    {
        var transactions = await _unitOfWork.TransactionRepository.GetByCategoryIdAsync(category.Id, category.UserId);

        if (replace)
            foreach (var transaction in transactions)
            {
                transaction.Category = categoryToReplace;
                categoryToReplace.TransactionCount += 1;
            }
        else
            foreach (var transaction in transactions)
            {
                _unitOfWork.TransactionRepository.Delete(transaction);
            }
    }
}