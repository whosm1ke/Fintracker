using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Budget.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Commands;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, CreateCommandResponse<CreateBudgetDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<CreateCommandResponse<CreateBudgetDTO>> Handle(CreateBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<CreateBudgetDTO>();
        var validator = new CreateBudgetDtoValidator(_unitOfWork,_userRepository);
        var validationResult = await validator.ValidateAsync(request.Budget);

        if (validationResult.IsValid)
        {
            var budgetEntity = _mapper.Map<Domain.Entities.Budget>(request.Budget);
            await _unitOfWork.BudgetRepository.AddAsync(budgetEntity);

            response.Message = "Created successfully";
            response.Success = true;
            response.Id = budgetEntity.Id;
            response.CreatedObject = request.Budget;
            
            await _unitOfWork.SaveAsync();
        }
        else
        {
            throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        return response;
    }
}