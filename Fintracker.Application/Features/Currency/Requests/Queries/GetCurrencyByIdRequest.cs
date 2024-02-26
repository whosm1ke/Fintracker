using Fintracker.Application.DTO.Currency;
using MediatR;

namespace Fintracker.Application.Features.Currency.Requests.Queries;

public class GetCurrencyByIdRequest : IRequest<CurrencyDTO>
{
    public Guid Id { get; set; }
}