using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Commands;

public class DeleteWalletCommand : IRequest<DeleteCommandResponse<WalletBaseDTO>>
{
    public Guid Id { get; set; }
}