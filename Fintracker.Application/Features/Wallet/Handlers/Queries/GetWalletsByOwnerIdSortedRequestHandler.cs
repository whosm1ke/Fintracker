using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Queries;

public class GetWalletsByOwnerIdSortedRequestHandler : IRequestHandler<GetWalletsByOwnerIdSortedRequest, IReadOnlyList<WalletBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetWalletsByOwnerIdSortedRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<IReadOnlyList<WalletBaseDTO>> Handle(GetWalletsByOwnerIdSortedRequest request, CancellationToken cancellationToken)
    {
        var wallets = await _unitOfWork.WalletRepository.GetByOwnerIdSortedAsync(request.OwnerId, request.SortBy);
        
        //TODO add validation logic

        return _mapper.Map<List<WalletBaseDTO>>(wallets);
    }
}