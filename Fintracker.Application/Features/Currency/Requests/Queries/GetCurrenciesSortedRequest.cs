using Fintracker.Application.DTO.Currency;
using MediatR;

namespace Fintracker.Application.Features.Currency.Requests.Queries;

public class GetCurrenciesSortedRequest : IRequest<IReadOnlyList<CurrencyDTO>>
{
    public string SortBy { get; set; }
    
    public bool IsDescending { get; set; }
}