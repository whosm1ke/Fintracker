using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Commands;

public class CreateWalletCommand : IRequest<CreateCommandResponse<WalletBaseDTO>>, INotGetRequest
{
    public CreateWalletDTO Wallet { get; set; } = default!;
}