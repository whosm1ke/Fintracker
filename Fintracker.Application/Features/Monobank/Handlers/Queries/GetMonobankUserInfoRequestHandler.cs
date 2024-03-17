using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Monobank.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Monobank.Handlers.Queries;

public class
    GetMonobankUserInfoRequestHandler : IRequestHandler<GetMonobankUserInfoRequest, MonobankUserInfoDTO>
{
    private readonly IMonobankService _monobankService;

    public GetMonobankUserInfoRequestHandler(IMonobankService monobankService)
    {
        _monobankService = monobankService;
    }

    public async Task<MonobankUserInfoDTO> Handle(GetMonobankUserInfoRequest request,
        CancellationToken cancellationToken)
    {
        var userInfo = await _monobankService.GetUserFullInfo(request.Token);
        

        return userInfo;
    }
}