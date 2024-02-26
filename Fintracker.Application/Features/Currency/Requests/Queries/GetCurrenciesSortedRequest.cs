using Fintracker.Application.DTO.Currency;
using MediatR;

namespace Fintracker.Application.Features.Currency.Requests.Queries;

public class GetCurrenciesSortedRequest : IRequest<CurrencyDTO>
{
    public string SortBy { get; set; }
}