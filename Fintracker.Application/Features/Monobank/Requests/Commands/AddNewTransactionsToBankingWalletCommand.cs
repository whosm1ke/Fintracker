using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Monobank.Requests.Commands;

public class AddNewTransactionsToBankingWalletCommand : IRequest<BaseCommandResponse>
{
    public MonobankPayloadDTO Payload { get; set; }
}