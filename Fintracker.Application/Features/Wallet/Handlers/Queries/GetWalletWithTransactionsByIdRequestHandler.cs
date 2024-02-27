using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Queries;

public class GetWalletWithTransactionsByIdRequestHandler : IRequestHandler<GetWalletWithTransactionsByIdRequest, WalletWithTransactionsDTO>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetWalletWithTransactionsByIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<WalletWithTransactionsDTO> Handle(GetWalletWithTransactionsByIdRequest request, CancellationToken cancellationToken)
    {
        var wallet = await _unitOfWork.WalletRepository.GetWalletWithTransactionsAsync(request.Id);

        if (wallet is null)
            throw new NotFoundException(nameof(Domain.Entities.Wallet), request.Id);

        return _mapper.Map<WalletWithTransactionsDTO>(wallet);
    }
}