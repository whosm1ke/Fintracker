using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.User.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateCommandResponse<UserBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }
    public async Task<UpdateCommandResponse<UserBaseDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateUserDtoValidator(_userRepository);
        var response = new UpdateCommandResponse<UserBaseDTO>();
        var validationResult = await validator.ValidateAsync(request.User);

        if (validationResult.IsValid)
        {
            var user = await _userRepository.GetAsync(request.User.Id);

            if (user is null)
                throw new NotFoundException(nameof(Domain.Entities.User), request.User.Id);
            
            var oldUser = _mapper.Map<UserBaseDTO>(user);
            _mapper.Map(request.User, user);
            var newUser = _mapper.Map<UserBaseDTO>(user);
            
            await _userRepository.UpdateAsync(user);

            response.Message = "Updated successfully";
            response.Success = true;
            response.Id = user.Id;
            response.Old = oldUser;
            response.New = newUser;
        }
        else
        {
            response.Message = "Update failed";
            response.Success = false;
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        }

        return response;
    }
}