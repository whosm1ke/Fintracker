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

        if (!validatedTokenResult.Item1)
        {
            throw new BadRequestException("Token is not valid");
        }

        if (validationResult.IsValid)
        {
            var userId = validatedTokenResult.Item2.Claims.FirstOrDefault(x => x.Type == ClaimTypeConstants.Uid)?.Value;
            var user = await _userRepository.GetAsync(Guid.Parse(userId!));

            if (user is null)
                throw new LoginException("Invalid credentials to add a wallet");

            var wallet = await _unitOfWork.WalletRepository.GetAsyncNoTracking(request.WalletId);

            user.MemberWallets.Add(wallet!);

            await _unitOfWork.SaveAsync();

            response.Message = "Added user to wallet successfully";
            response.Success = true;
        }
        else
        {
            throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        return response;
    }
}