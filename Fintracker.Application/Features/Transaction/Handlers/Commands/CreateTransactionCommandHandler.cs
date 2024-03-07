using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.DTO.Transaction.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Commands;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, CreateCommandResponse<TransactionBaseDTO>>
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

    public async Task<CreateCommandResponse<TransactionBaseDTO>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<TransactionBaseDTO>();
        var validator = new CreateTransactionDtoValidator(_unitOfWork,_userRepository);
        var validationResult = await validator.ValidateAsync(request.Transaction);

        if (validationResult.IsValid)
        {
            var transaction = _mapper.Map<Domain.Entities.Transaction>(request.Transaction);
            await _unitOfWork.TransactionRepository.AddAsync(transaction);

            var createdObj = _mapper.Map<TransactionBaseDTO>(transaction);

            await DecreaseBalanceInWallet(transaction.WalletId, transaction.Amount);

            response.Success = true;
            response.Message = "Created successfully";
            response.Id = createdObj.Id;
            response.CreatedObject = createdObj;

            await _unitOfWork.SaveAsync();
        }
        else
        {
            throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }
        
        return response;
    }

    private async Task DecreaseBalanceInWallet(Guid walletId, decimal amount)
    {
        var wallet = await _unitOfWork.WalletRepository.GetAsync(walletId);

        if (wallet is null)
            throw new BadRequestException($"Cannot find wallet with id [{walletId.ToString()}]");

        wallet.Balance -= amount;

        await _unitOfWork.WalletRepository.UpdateAsync(wallet);
    }
}