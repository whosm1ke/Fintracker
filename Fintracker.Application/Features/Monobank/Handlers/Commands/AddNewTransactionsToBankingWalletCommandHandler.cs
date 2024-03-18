using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Monobank.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Fintracker.Application.Features.Monobank.Handlers.Commands;

public class
    AddNewTransactionsToBankingWalletCommandHandler : IRequestHandler<AddNewTransactionsToBankingWalletCommand,
    BaseCommandResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMonobankService _monobankService;
    private readonly IMemoryCache _cache;

    public AddNewTransactionsToBankingWalletCommandHandler(IMapper mapper, IUnitOfWork unitOfWork,
        IMonobankService monobankService, IMemoryCache cache)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _monobankService = monobankService;
        _cache = cache;
    }

    public async Task<BaseCommandResponse> Handle(AddNewTransactionsToBankingWalletCommand request,
        CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var transactions = _mapper.Map<ICollection<Domain.Entities.Transaction>>(request.Payload.Transactions);
        var defaultCurrency = await _unitOfWork.CurrencyRepository.GetAsync("UAH");
        var expenseCategoryId = await _unitOfWork.CategoryRepository.GetDefaultBankExpenseCategoryId();
        var incomeCategoryId = await _unitOfWork.CategoryRepository.GetDefaultBankIncomeCategoryId();
        var xToken = await _monobankService.GetMonobankTokenAsync(request.Payload.Email);
        var accountBalance = await _monobankService.GetAccountBalance(xToken!, request.Payload.AccountId);


        var bankWallet = await _unitOfWork.WalletRepository.GetWalletByBankAccount(request.Payload.AccountId);

        if (bankWallet is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by provided id [{request.Payload.AccountId}]",
                PropertyName = nameof(request.Payload.AccountId)
            }, nameof(Domain.Entities.Wallet));

        foreach (var transaction in transactions)
        {
            transaction.UserId = request.Payload.OwnerId;
            transaction.WalletId = bankWallet.Id;
            transaction.CurrencyId = defaultCurrency!.Id;
            transaction.CategoryId = transaction.Amount < 0 ? expenseCategoryId : incomeCategoryId;
            transaction.Amount = (transaction.Amount < 0 ? transaction.Amount * -1 : transaction.Amount) / 100m;
            bankWallet.Transactions.Add(transaction);
        }

        bankWallet.Balance = accountBalance / 100m;
        _unitOfWork.WalletRepository.Update(bankWallet);
        await _unitOfWork.SaveAsync();
        _cache.Set("mono_from_value", request.Payload.Transactions.Max(x => x.Time) + 1);


        response.Id = bankWallet.Id;
        response.Message = "Updated successfully";
        response.Success = true;
        return response;
    }
}