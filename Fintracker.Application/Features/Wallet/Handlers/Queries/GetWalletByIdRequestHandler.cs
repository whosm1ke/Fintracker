using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Exceptions;
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
        var wallet = await _unitOfWork.WalletRepository.GetWalletById(request.Id);

        if (wallet is null)
            throw new NotFoundException(nameof(Domain.Entities.Wallet), request.Id);

        return _mapper.Map<WalletBaseDTO>(wallet);
    }
}