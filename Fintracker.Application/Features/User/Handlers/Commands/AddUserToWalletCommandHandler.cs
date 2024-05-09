using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using Fintracker.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class AddUserToWalletCommandHandler : IRequestHandler<AddUserToWalletCommand, BaseCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly AppSettings _appSettings;

    public AddUserToWalletCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMediator mediator, IOptions<AppSettings> appSettings)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _appSettings = appSettings.Value;
    }

    public async Task<BaseCommandResponse> Handle(AddUserToWalletCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();

        var wallet = await _unitOfWork.WalletRepository.GetWalletById(request.WalletId);
        var user = await _userRepository.GetAsync(request.UserId);
        user!.UserDetails = new UserDetails
        {
            Avatar = $"{_appSettings.BaseUrl}/api/user/avatar/logo.png"
        };
        wallet!.Users.Add(user);
        
        foreach (var budget in wallet.Budgets)
        {
            if (budget.IsPublic)
            {
                user.MemberBudgets.Add(budget);
            }
        }
        await _unitOfWork.SaveAsync();
         
        await _mediator.Send(new PopulateUserWithCategoriesCommand
        {
            UserId = user!.Id,
            PathToFile = request.PathToCategories!
        });


        response.Message = user.Email!;
        response.Success = true;
        response.Id = user.Id;


        return response;
    }
}
