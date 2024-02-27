using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Queries;

public class GetWalletsByUserIdSortedRequestHandler : IRequestHandler<GetWalletsByUserIdSortedRequest, IReadOnlyList<WalletBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetWalletsByUserIdSortedRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<IReadOnlyList<WalletBaseDTO>> Handle(GetWalletsByUserIdSortedRequest request, CancellationToken cancellationToken)
    {
        var wallets = await _unitOfWork.WalletRepository.GetByUserIdSortedAsync(request.UserId, request.SortBy);
        
        //TODO add validation logic

        return _mapper.Map<List<WalletBaseDTO>>(wallets);
    }
}