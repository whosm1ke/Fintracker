using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.DTO.Wallet.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Commands;

public class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand, UpdateCommandResponse<WalletBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateWalletCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateCommandResponse<WalletBaseDTO>> Handle(UpdateWalletCommand request,
        CancellationToken cancellationToken)
    {
        var response = new UpdateCommandResponse<WalletBaseDTO>();
        var validator = new UpdateWalletDtoValidator(_unitOfWork);
        var validationResult = await validator.ValidateAsync(request.Wallet);

        var wallet = await _unitOfWork.WalletRepository.GetWalletById(request.Wallet.Id);

        if (wallet is null)
            throw new NotFoundException(nameof(Domain.Entities.Wallet), request.Wallet.Id);

        if (validationResult.IsValid)
        {
            var oldObject = _mapper.Map<WalletBaseDTO>(wallet);
            _mapper.Map(request.Wallet, wallet);
            await _unitOfWork.WalletRepository.UpdateAsync(wallet);
            
            await _unitOfWork.SaveAsync();
            
            var newObject = _mapper.Map<WalletBaseDTO>(wallet);

            response.Success = true;
            response.Message = "Updated successfully";
            response.Old = oldObject;
            response.New = newObject;
            response.Id = request.Wallet.Id;

        }
        else
        {
            throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        return response;
    }
}