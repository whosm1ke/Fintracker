using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Commands;

public class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand, DeleteCommandResponse<WalletBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteWalletCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeleteCommandResponse<WalletBaseDTO>> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        var response = new DeleteCommandResponse<WalletBaseDTO>();

        var wallet = await _unitOfWork.WalletRepository.GetWalletById(request.Id);

        if (wallet is null)
            throw new NotFoundException(nameof(Domain.Entities.Wallet), request.Id);

        var deletedObj = _mapper.Map<WalletBaseDTO>(wallet);
        await _unitOfWork.WalletRepository.DeleteAsync(wallet);
        
        response.Success = true;
        response.Message = "Deleted successfully";
        response.DeletedObj = deletedObj;
        response.Id = deletedObj.Id;
        
        await _unitOfWork.SaveAsync();
        
        return response;
    }
}