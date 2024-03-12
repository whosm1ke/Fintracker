using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Responses;
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
            throw new NotFoundException(nameof(Domain.Entities.Budget), request.Id);

        var budgetBaseDto = _mapper.Map<BudgetBaseDTO>(budget);
        await _unitOfWork.BudgetRepository.DeleteAsync(budget);
        
        response.Success = true;
        response.Message = "Deleted successfully";
        response.DeletedObj = budgetBaseDto;
        response.Id = budgetBaseDto.Id;
        
        await _unitOfWork.SaveAsync();

        return response;
    }
}