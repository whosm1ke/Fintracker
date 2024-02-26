using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Commands;

public class CreateTransactionCommand : IRequest<CreateCommandResponse<TransactionDTO>>
{
    public CreateTransactionDTO Transaction { get; set; }
}