using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Features.Currency.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Currency.Handlers.Queries;

public class GetCurrenciesSortedRequestHandler : IRequestHandler<GetCurrenciesSortedRequest, IReadOnlyList<CurrencyDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly List<string> _allowedSortColumns;

    public GetCurrenciesSortedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _allowedSortColumns = new()
        {
            nameof(Domain.Entities.Currency.Name),
            nameof(Domain.Entities.Currency.Symbol)
        };
    }
    public async Task<IReadOnlyList<CurrencyDTO>> Handle(GetCurrenciesSortedRequest request, CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.SortBy))
            throw new BadRequestException(
                $"Invalid sortBy parameter. Allowed values {string.Join(',', _allowedSortColumns)}");
        
        var currencies = await _unitOfWork.CurrencyRepository.GetCurrenciesSorted(request.SortBy, request.IsDescending);

        return _mapper.Map<List<CurrencyDTO>>(currencies);
    }
}