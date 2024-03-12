using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Currency.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Currency.Handlers.Queries;

public class GetCurrencyByIdRequestHandler : IRequestHandler<GetCurrencyByIdRequest, CurrencyDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCurrencyByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CurrencyDTO> Handle(GetCurrencyByIdRequest request, CancellationToken cancellationToken)
    {
        var currency = await _unitOfWork.CurrencyRepository.GetAsync(request.Id);

        if (currency is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Id}]",
                PropertyName = nameof(request.Id)
            },nameof(Domain.Entities.Currency));

        return _mapper.Map<CurrencyDTO>(currency);
    }
}