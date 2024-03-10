using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Invite.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class AddUserToWalletCommandHandler : IRequestHandler<AddUserToWalletCommand, BaseCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.Entities.User> _userManager;

    public AddUserToWalletCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        ITokenService tokenService, UserManager<Domain.Entities.User> userManager)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<BaseCommandResponse> Handle(AddUserToWalletCommand request, CancellationToken cancellationToken)
    {
        var validator = new AddUserToWalletValidator(_userRepository, _unitOfWork);
        var response = new BaseCommandResponse();
        var validationResult = await validator.ValidateAsync(request);

        var validatedTokenResult = await _tokenService.ValidateToken(request.Token);

        if (!validatedTokenResult.Item1)
        {
            throw new BadRequestException("Token is not valid");
        }

        if (validationResult.IsValid)
        {
            var userId = validatedTokenResult.Item2.Claims.FirstOrDefault(x => x.Type == ClaimTypeConstants.Uid)?.Value;
            var userEmail = validatedTokenResult.Item2.Claims.FirstOrDefault(x => x.Type == ClaimTypeConstants.Email)
                ?.Value;
            var user = await _userRepository.GetUserWithMemberWalletsByIdAsync(Guid.Parse(userId!));

            if (user is null)
            {
                user = await RegisterUserWithTemporaryPassword(userEmail, Guid.Parse(userId ?? Guid.Empty.ToString()));
            }

            var wallet = await _unitOfWork.WalletRepository.GetAsyncNoTracking(request.WalletId);

            user.MemberWallets.Add(wallet!);

            await _unitOfWork.SaveAsync();

            response.Message = "Added wallet successfully";
            response.Success = true;
        }
        else
        {
            throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        return response;
    }

    private async Task<Domain.Entities.User> RegisterUserWithTemporaryPassword(string? email, Guid id)
    {
        if (email is null || id == Guid.Empty)
            throw new BadRequestException(
                $"Can not register new user. Invalid param '{nameof(email)}' or '{nameof(id)}'");

        var user = new Domain.Entities.User()
        {
            UserName = email,
            Email = email,
            Id = id,
            PasswordHash = Guid.NewGuid().ToString()
        };
        var userResult = await _userManager.CreateAsync(user);
        if (userResult.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
                throw new BadRequestException(roleResult.Errors.Select(x => x.Description).ToList());
        }
        else
            throw new BadRequestException(userResult.Errors.Select(x => x.Description).ToList());

        return user;
    }
}