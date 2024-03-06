using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Queries;

public class GetUserWithBudgetsByIdRequestHandler : IRequestHandler<GetUserWithBudgetsByIdRequest,UserWithBudgetsDTO>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUserWithBudgetsByIdRequestHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }
    public async Task<UserWithBudgetsDTO> Handle(GetUserWithBudgetsByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserWithBudgetsByIdAsync(request.Id);

        if (user is null)
            throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        return _mapper.Map<UserWithBudgetsDTO>(user);
    }
}