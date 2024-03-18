using Fintracker.Application.DTO.Currency;
using MediatR;

namespace Fintracker.Application.Features.Currency.Requests.Commands;

public class ConvertCurrenciesCommand : IRequest<ConvertCurrencyDTO>
{
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public decimal Amount { get; set; } = 1m;
}