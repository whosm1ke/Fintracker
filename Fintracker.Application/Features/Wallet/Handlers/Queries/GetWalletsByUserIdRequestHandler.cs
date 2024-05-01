using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Queries;

public class
    GetWalletsByUserIdRequestHandler : IRequestHandler<GetWalletsByOwnerIdRequest, IReadOnlyList<WalletBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetWalletsByUserIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<WalletBaseDTO>> Handle(GetWalletsByOwnerIdRequest request,
        CancellationToken cancellationToken)
    {
        var wallets = await _unitOfWork.WalletRepository.GetByUserIdAsync(request.OwnerId);

        
        
        

        return _mapper.Map<List<WalletBaseDTO>>(wallets);
    }
}