using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Monobank.Requests.Commands;

public class AddInitialTransactionToNewBankWalletCommand : IRequest<CreateCommandResponse<WalletPureDTO>>
{
    public MonobankPayloadDTO Payload { get; set; }
}