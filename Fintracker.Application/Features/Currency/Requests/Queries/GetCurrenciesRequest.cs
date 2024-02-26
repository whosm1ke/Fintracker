using Fintracker.Application.DTO.Currency;
using MediatR;

namespace Fintracker.Application.Features.Currency.Requests.Queries;

public class GetCurrenciesRequest : IRequest<IReadOnlyList<CurrencyDTO>>
{
    
}