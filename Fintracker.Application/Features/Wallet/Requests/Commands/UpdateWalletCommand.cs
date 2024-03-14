using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Commands;

public class UpdateWalletCommand : IRequest<UpdateCommandResponse<WalletBaseDTO>>
{
    public UpdateWalletDTO Wallet { get; set; } = default!;
}