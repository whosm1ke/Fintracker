using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Commands;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, CreateCommandResponse<CreateWalletDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateWalletCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateCommandResponse<CreateWalletDTO>> Handle(CreateWalletCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<CreateWalletDTO>();


        var wallet = _mapper.Map<Domain.Entities.Wallet>(request.Wallet);
        wallet.Balance = request.Wallet.StartBalance;
        await _unitOfWork.WalletRepository.AddAsync(wallet);
        
        response.Success = true;
        response.Message = "Created successfully";
        response.CreatedObject = request.Wallet;
        response.Id = wallet.Id;
        await _unitOfWork.SaveAsync();


        return response;
    }
}