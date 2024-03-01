using Fintracker.Application.DTO.Transaction;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Requests.Queries;

public class GetTransactionsRequest : IRequest<IReadOnlyList<TransactionBaseDTO>>
{
}