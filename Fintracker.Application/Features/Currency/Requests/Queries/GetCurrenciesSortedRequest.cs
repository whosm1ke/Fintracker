using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Models;
using MediatR;

namespace Fintracker.Application.Features.Currency.Requests.Queries;

public class GetCurrenciesSortedRequest : IRequest<IReadOnlyList<CurrencyDTO>>
{
    public QueryParams Params { get; set; } = default!;
}