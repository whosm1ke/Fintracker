using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Queries;

public class GetUserWithWalletsByIdRequestHandler : IRequestHandler<GetUserWithWalletsByIdRequest,UserWithWalletsDTO>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetUserWithWalletsByIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<UserWithWalletsDTO> Handle(GetUserWithWalletsByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetUserWithWalletsByIdAsync(request.Id);

        if (user is null)
            throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        return _mapper.Map<UserWithWalletsDTO>(user);
    }
}