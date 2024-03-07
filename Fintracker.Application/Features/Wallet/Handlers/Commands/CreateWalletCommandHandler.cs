using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.DTO.Wallet.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Responses;
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

    public async Task<CreateCommandResponse<WalletBaseDTO>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<WalletBaseDTO>();
        var validator = new CreateWalletDtoValidator(_unitOfWork,_userRepository);
        var validationResult = await validator.ValidateAsync(request.Wallet);

        if (validationResult.IsValid)
        {
            var wallet = _mapper.Map<Domain.Entities.Wallet>(request.Wallet);

            await _unitOfWork.WalletRepository.AddAsync(wallet);
            var createdObject = _mapper.Map<WalletBaseDTO>(wallet);
            
            response.Success = true;
            response.Message = "Created successfully";
            response.CreatedObject = createdObject;
            await _unitOfWork.SaveAsync();
        }
        else
        {
            throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        return response;
    }
}