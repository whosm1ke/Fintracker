using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Commands;

public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, DeleteCommandResponse<BudgetBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeleteCommandResponse<BudgetBaseDTO>> Handle(DeleteBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var response = new DeleteCommandResponse<BudgetBaseDTO>();

        var budget = await _unitOfWork.BudgetRepository.GetAsync(request.Id);

        if (budget is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Attempt to delete non-existing item with by id [{request.Id}]",
                PropertyName = nameof(request.Id)
            },nameof(Domain.Entities.Budget));
  
        if(budget.OwnerId != request.UserId)
            throw new ForbiddenException(new ExceptionDetails
            {
                ErrorMessage = "Attempt to delete wallet with other owner",
                PropertyName = nameof(request.Id)
            });
        
        var budgetBaseDto = _mapper.Map<BudgetBaseDTO>(budget);

        await DeleteBudgetFromMemberUsersIfPublic(budget);
        
        _unitOfWork.BudgetRepository.Delete(budget);

        response.Success = true;
        response.Message = "Deleted successfully";
        response.DeletedObj = budgetBaseDto;
        response.Id = budgetBaseDto.Id;

        await _unitOfWork.SaveAsync();

        return response;
    }
    
    private async Task DeleteBudgetFromMemberUsersIfPublic(Domain.Entities.Budget budget)
    {
        if (!budget.IsPublic) return;

        var wallet = await _unitOfWork.WalletRepository.GetWalletById(budget.WalletId);
        foreach (var walletUser in wallet!.Users)
        {
            walletUser.MemberBudgets.Remove(budget);
        }
    }
}

