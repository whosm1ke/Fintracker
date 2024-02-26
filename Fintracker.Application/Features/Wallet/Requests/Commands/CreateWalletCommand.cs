using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Commands;

public class CreateWalletCommand : IRequest<CreateCommandResponse<WalletBaseDTO>>
{
    public CreateWalletDTO Wallet { get; set; }
}