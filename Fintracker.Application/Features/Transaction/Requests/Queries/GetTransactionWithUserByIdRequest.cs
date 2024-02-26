using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionWithUserByIdRequest : IRequest<TransactionWithUserDTO>
{
    public Guid Id { get; set; }
}