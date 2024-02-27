using AutoMapper;
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
    private readonly IUnitOfWork _unitOfWork;

    public GetUsersAccessedToWalletRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<UserBaseDTO>> Handle(GetUsersAccessedToWalletRequest request,
        CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.UserRepository.GetAllAccessedToWalletAsync(request.WalletId);

        return _mapper.Map<List<UserBaseDTO>>(users);
    }
}