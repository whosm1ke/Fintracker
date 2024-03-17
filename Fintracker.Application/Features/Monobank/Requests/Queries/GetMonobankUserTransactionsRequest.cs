using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.Models.Monobank;
using MediatR;

namespace Fintracker.Application.Features.Monobank.Requests.Queries;

public class GetMonobankUserTransactionsRequest : IRequest<IReadOnlyList<MonoTransactionDTO>>
{
    public MonobankConfiguration Configuration { get; set; }
    
    public string Email { get; set; }
}