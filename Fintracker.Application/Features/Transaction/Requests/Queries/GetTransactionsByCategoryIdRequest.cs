using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsByCategoryIdRequest : IRequest<IReadOnlyList<TransactionBaseDTO>>
{
    public Guid CategoryId { get; set; }
}