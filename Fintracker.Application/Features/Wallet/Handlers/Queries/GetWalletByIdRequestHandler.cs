using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Queries;

public class GetWalletByIdRequestHandler : IRequestHandler<GetWalletByIdRequest, WalletBaseDTO>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetWalletByIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<WalletBaseDTO> Handle(GetWalletByIdRequest request, CancellationToken cancellationToken)
    {
        var wallets = await _unitOfWork.WalletRepository.GetWalletById(request.Id);
        
        //TODO add validation logic

        return _mapper.Map<WalletBaseDTO>(wallets);
    }
}