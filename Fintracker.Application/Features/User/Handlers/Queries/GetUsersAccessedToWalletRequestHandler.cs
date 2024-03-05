using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Features.User.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Queries;

public class
    GetUsersAccessedToWalletRequestHandler : IRequestHandler<GetUsersAccessedToWalletRequest,
    IReadOnlyList<UserBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUsersAccessedToWalletRequestHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<UserBaseDTO>> Handle(GetUsersAccessedToWalletRequest request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAccessedToWalletAsync(request.WalletId);

        return _mapper.Map<List<UserBaseDTO>>(users);
    }
}