using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
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

    public async Task<UpdateCommandResponse<UserBaseDTO>> Handle(UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var response = new UpdateCommandResponse<UserBaseDTO>();

        var user = await _userRepository.GetAsync(request.User.Id);

        if (user is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.User.Id}]",
                PropertyName = nameof(request.User.Id)
            }, nameof(Domain.Entities.User));

        if (request.User.Avatar != null)
        {
            var fileName = Path.GetFileName(request.User.Avatar.FileName);
            var filePath = Path.Combine(request.WWWRoot,"images" ,fileName);
            using (var stream = File.Create(filePath))
            {
                await request.User.Avatar.CopyToAsync(stream);
            }
            request.User.UserDetails.Avatar = filePath;
        }
        
        
        var oldUser = _mapper.Map<UserBaseDTO>(user);
        _mapper.Map(request.User, user);
        var newUser = _mapper.Map<UserBaseDTO>(user);

        await _userRepository.UpdateAsync(user);

        response.Message = "Updated successfully";
        response.Success = true;
        response.Id = user.Id;
        response.Old = oldUser;
        response.New = newUser;


        return response;
    }
}