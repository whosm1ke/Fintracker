using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Features.Category.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Category.Handlers.Queries;

public class GetCategoryByTypeRequestHandler : IRequestHandler<GetCategoriesByTypeRequest, IReadOnlyList<CategoryDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByTypeRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<CategoryDTO>> Handle(GetCategoriesByTypeRequest request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoryRepository.GetByTypeAsync(request.Type);

       

        return _mapper.Map<List<CategoryDTO>>(category);
    }
}