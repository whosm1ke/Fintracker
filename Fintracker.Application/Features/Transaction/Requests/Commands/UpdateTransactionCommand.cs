using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Commands;

public class UpdateTransactionCommand : IRequest<UpdateCommandResponse<TransactionBaseDTO>>
{
    public UpdateTransactionDTO Transaction { get; set; } = default!;
}