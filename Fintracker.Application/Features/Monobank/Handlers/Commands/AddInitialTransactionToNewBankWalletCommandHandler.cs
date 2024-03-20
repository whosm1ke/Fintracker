using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Monobank.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Fintracker.Application.Features.Monobank.Handlers.Commands;

public class AddInitialTransactionToNewBankWalletCommandHandler : IRequestHandler<
    AddInitialTransactionToNewBankWalletCommand,
    CreateCommandResponse<WalletPureDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMonobankService _monobankService;
    private readonly IMemoryCache _cache;

    public AddInitialTransactionToNewBankWalletCommandHandler(IMapper mapper, IUnitOfWork unitOfWork,
        IMonobankService monobankService, IMemoryCache cache)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _monobankService = monobankService;
        _cache = cache;
    }

    public async Task<CreateCommandResponse<WalletPureDTO>> Handle(AddInitialTransactionToNewBankWalletCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<WalletPureDTO>();
        var transactions = _mapper.Map<ICollection<Domain.Entities.Transaction>>(request.Payload.Transactions);
        var defaultCurrency = await _unitOfWork.CurrencyRepository.GetAsync("UAH");
        var expenseCategoryId = await _unitOfWork.CategoryRepository.GetDefaultBankExpenseCategoryId();
        var incomeCategoryId = await _unitOfWork.CategoryRepository.GetDefaultBankIncomeCategoryId();
        var xToken = await _monobankService.GetMonobankTokenAsync(request.Payload.Email);
        var accountBalance = await _monobankService.GetAccountBalance(xToken!, request.Payload.AccountId);

        var bankWallet = new Domain.Entities.Wallet
        {
            Balance = accountBalance / 100m,
            IsBanking = true,
            Name = "Monobank wallet",
            OwnerId = request.Payload.OwnerId,
            CurrencyId = defaultCurrency!.Id
        };

        bankWallet.BankAccountId = request.Payload.AccountId;

        foreach (var transaction in transactions)
        {
            transaction.UserId = request.Payload.OwnerId;
            transaction.WalletId = bankWallet.Id;
            transaction.CurrencyId = defaultCurrency.Id;
            transaction.CategoryId = transaction.Amount < 0 ? expenseCategoryId : incomeCategoryId;
            transaction.Amount = (transaction.Amount < 0 ? transaction.Amount * -1 : transaction.Amount) / 100m;
            bankWallet.Transactions.Add(transaction);
        }


        await _unitOfWork.WalletRepository.AddAsync(bankWallet);
        await _unitOfWork.SaveAsync();
        _cache.Set("mono_from_value", request.Payload.Transactions.Max(x => x.Time) + 1);

        var createdBankWallet = _mapper.Map<WalletPureDTO>(bankWallet);

        response.Id = bankWallet.Id;
        response.Message = "Created successfully";
        response.Success = true;
        response.CreatedObject = createdBankWallet;
        return response;
    }
}