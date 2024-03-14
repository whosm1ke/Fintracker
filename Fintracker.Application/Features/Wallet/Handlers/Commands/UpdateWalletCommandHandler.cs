using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
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

        var wallet = await _unitOfWork.WalletRepository.GetWalletById(request.Wallet.Id);

        if (wallet is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Wallet.Id}]",
                PropertyName = nameof(request.Wallet.Id)
            }, nameof(Domain.Entities.Wallet));


        var oldObject = _mapper.Map<WalletBaseDTO>(wallet);
        _mapper.Map(request.Wallet, wallet);
        _unitOfWork.WalletRepository.Update(wallet);

        await _unitOfWork.SaveAsync();

        var newObject = _mapper.Map<WalletBaseDTO>(wallet);

        response.Success = true;
        response.Message = "Updated successfully";
        response.Old = oldObject;
        response.New = newObject;
        response.Id = request.Wallet.Id;


        return response;
    }
}