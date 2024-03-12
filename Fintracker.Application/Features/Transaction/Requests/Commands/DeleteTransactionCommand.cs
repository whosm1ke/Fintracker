using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Commands;

public class DeleteTransactionCommand : IRequest<DeleteCommandResponse<TransactionBaseDTO>>
{
    public Guid Id { get; set; }
}