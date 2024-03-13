using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Invite.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class AddUserToWalletCommandHandler : IRequestHandler<AddUserToWalletCommand, BaseCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public AddUserToWalletCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<BaseCommandResponse> Handle(AddUserToWalletCommand request, CancellationToken cancellationToken)
    {
        var validator = new AddUserToWalletValidator(_userRepository, _unitOfWork);
        var response = new BaseCommandResponse();
        var validationResult = await validator.ValidateAsync(request);

        var validatedTokenResult = await _tokenService.ValidateToken(request.Token);

        if (!validatedTokenResult)
        {
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Provided token is not valid",
                PropertyName = nameof(request.Token)
            });
        }

        if (validationResult.IsValid)
        {
            var userId = _tokenService.GetUidClaimValue(request.Token);
            
            if(!userId.HasValue)
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

            var wallet = await _unitOfWork.WalletRepository.GetAsyncNoTracking(request.WalletId);

            user.MemberWallets.Add(wallet!);

            await _unitOfWork.SaveAsync();

            response.Message = "Added user to wallet successfully";
            response.Success = true;
            response.Id = user.Id;
        }
        else
        {
            throw new BadRequestException(validationResult.Errors.Select(x => new ExceptionDetails
                { ErrorMessage = x.ErrorMessage, PropertyName = x.PropertyName }).ToList());
        }

        return response;
    }
}