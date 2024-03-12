using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Queries;

public class
    GetUserWithMemberWalletsByIdRequestHandler : IRequestHandler<GetUserWithMemberWalletsByIdRequest,
    UserWithMemberWalletsDTO>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUserWithMemberWalletsByIdRequestHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<UserWithMemberWalletsDTO> Handle(GetUserWithMemberWalletsByIdRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserWithMemberWalletsByIdAsync(request.Id);

        if (user is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Id}]",
                PropertyName = nameof(request.Id)
            },nameof(Domain.Entities.User));

        return _mapper.Map<UserWithMemberWalletsDTO>(user);
    }
}