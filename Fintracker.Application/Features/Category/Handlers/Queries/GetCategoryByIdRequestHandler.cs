using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Category.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Category.Handlers.Queries;

public class GetCategoryByIdRequestHandler : IRequestHandler<GetCategoryByIdRequest, CategoryDTO>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<CategoryDTO> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoryRepository.GetAsync(request.Id);

        if (category is null)
            throw new NotFoundException(nameof(Domain.Entities.Category), request.Id);

        return _mapper.Map<CategoryDTO>(category);
    }
}