using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Commands;

public class DeleteTransactionCommand : IRequest<DeleteCommandResponse<TransactionDTO>>
{
    public Guid Id { get; set; }
}