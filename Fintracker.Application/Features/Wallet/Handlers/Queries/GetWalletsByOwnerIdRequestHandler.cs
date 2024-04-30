using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Queries;

public class
    GetWalletsByOwnerIdRequestHandler : IRequestHandler<GetWalletsByOwnerIdRequest, IReadOnlyList<WalletBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetWalletsByOwnerIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<WalletBaseDTO>> Handle(GetWalletsByOwnerIdRequest request,
        CancellationToken cancellationToken)
    {
        var walletsByOwner = await _unitOfWork.WalletRepository.GetByOwnerIdAsync(request.OwnerId);
        var walletsByMember = await _unitOfWork.WalletRepository.GetByMemberIdAsync(request.OwnerId);

        var unionWallets = walletsByMember.Union(walletsByOwner);
        var distintcWallets = unionWallets.DistinctBy(x => x.Id);
        
        //TODO add validation logic
        
        

        return _mapper.Map<List<WalletBaseDTO>>(distintcWallets);
    }
}