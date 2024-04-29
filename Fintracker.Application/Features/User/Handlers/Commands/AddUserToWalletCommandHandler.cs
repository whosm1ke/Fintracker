using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class AddUserToWalletCommandHandler : IRequestHandler<AddUserToWalletCommand, BaseCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMediator _mediator;

    public AddUserToWalletCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        ITokenService tokenService, IMediator mediator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mediator = mediator;
    }

    public async Task<BaseCommandResponse> Handle(AddUserToWalletCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();

        var wallet = await _unitOfWork.WalletRepository.GetWalletByIdOnlyUsers(request.WalletId);
        var user = await _userRepository.GetAsync(request.UserId);

        wallet!.Users.Add(user!);
         
        await _mediator.Send(new PopulateUserWithCategoriesCommand
        {
            UserId = user!.Id,
            PathToFile = request.PathToCategories!
        });

        await _unitOfWork.SaveAsync();

        response.Message = user.Email!;
        response.Success = true;
        response.Id = user.Id;


        return response;
    }
}

public class RemoveUserFromWalletHandler : IRequestHandler<RemoveUserFromWallet, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RemoveUserFromWalletHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<Unit> Handle(RemoveUserFromWallet request, CancellationToken cancellationToken)
    {
        var validatedTokenResult = await _tokenService.ValidateToken(request.Token);

        if (!validatedTokenResult)
        {
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Provided token is not valid",
                PropertyName = nameof(request.Token)
            });
        }

        var userId = _tokenService.GetUidClaimValue(request.Token);

        if (!userId.HasValue)
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Invalid token format",
                PropertyName = nameof(request.Token)
            });

        var user = await _userRepository.GetAsync(userId.Value);

        if (user is null)
            throw new LoginException(new ExceptionDetails
            {
                ErrorMessage = "Invalid credentials",
                PropertyName = nameof(Domain.Entities.User)
            });

        await _userRepository.DeleteAsync(user);

        return Unit.Value;
    }
}