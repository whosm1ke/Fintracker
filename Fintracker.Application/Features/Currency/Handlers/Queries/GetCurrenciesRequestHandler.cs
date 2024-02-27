using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Features.Currency.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Currency.Handlers.Queries;

public class GetCurrenciesRequestHandler : IRequestHandler<GetCurrenciesRequest, IReadOnlyList<CurrencyDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCurrenciesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<CurrencyDTO>> Handle(GetCurrenciesRequest request, CancellationToken cancellationToken)
    {
        var currencies = await _unitOfWork.CurrencyRepository.GetAllAsync();

        return _mapper.Map<List<CurrencyDTO>>(currencies);
    }
}