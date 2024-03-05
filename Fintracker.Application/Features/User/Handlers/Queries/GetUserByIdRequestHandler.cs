﻿using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Queries;

public class GetUserByIdRequestHandler : IRequestHandler<GetUserByIdRequest, UserBaseDTO>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUserByIdRequestHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<UserBaseDTO> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.Id);

        if (user is null)
            throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        return _mapper.Map<UserBaseDTO>(user);
    }
}