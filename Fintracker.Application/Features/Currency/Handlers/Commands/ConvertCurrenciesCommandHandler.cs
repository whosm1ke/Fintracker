using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Currency.Requests.Commands;
using MediatR;

namespace Fintracker.Application.Features.Currency.Handlers.Commands;

public class ConvertCurrenciesCommandHandler : IRequestHandler<ConvertCurrenciesCommand, ConvertCurrencyDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrencyConverter _currencyConverter;

    public ConvertCurrenciesCommandHandler(IUnitOfWork unitOfWork, ICurrencyConverter currencyConverter)
    {
        _unitOfWork = unitOfWork;
        _currencyConverter = currencyConverter;
    }

    public async Task<ConvertCurrencyDTO> Handle(ConvertCurrenciesCommand request, CancellationToken cancellationToken)
    {
        var fromCurrency = await _unitOfWork.CurrencyRepository.GetAsync(request.From);
        var toCurrency = await _unitOfWork.CurrencyRepository.GetAsync(request.To);

        if (fromCurrency is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by provided symbol {request.From}",
                PropertyName = nameof(request.From)
            }, nameof(Domain.Entities.Currency));
        
        if (toCurrency is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by provided symbol {request.To}",
                PropertyName = nameof(request.To)
            }, nameof(Domain.Entities.Currency));

        return await _currencyConverter.Convert(request.From, request.To, request.Amount);
    }
}