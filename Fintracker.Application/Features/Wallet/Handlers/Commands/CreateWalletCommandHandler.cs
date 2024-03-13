using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Commands;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, CreateCommandResponse<WalletBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public CreateWalletCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<CreateCommandResponse<WalletBaseDTO>> Handle(CreateWalletCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<WalletBaseDTO>();


        var wallet = _mapper.Map<Domain.Entities.Wallet>(request.Wallet);

        await _unitOfWork.WalletRepository.AddAsync(wallet);
        var createdObject = _mapper.Map<WalletBaseDTO>(wallet);

        response.Success = true;
        response.Message = "Created successfully";
        response.CreatedObject = createdObject;
        response.Id = wallet.Id;
        await _unitOfWork.SaveAsync();


        return response;
    }
}