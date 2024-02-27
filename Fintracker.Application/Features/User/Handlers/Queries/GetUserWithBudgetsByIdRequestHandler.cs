using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Queries;

public class GetUserWithBudgetsByIdRequestHandler : IRequestHandler<GetUserWithBudgetsByIdRequest,UserWithBudgetsDTO>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetUserWithBudgetsByIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<UserWithBudgetsDTO> Handle(GetUserWithBudgetsByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetUserWithBudgetsByIdAsync(request.Id);

        if (user is null)
            throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        return _mapper.Map<UserWithBudgetsDTO>(user);
    }
}