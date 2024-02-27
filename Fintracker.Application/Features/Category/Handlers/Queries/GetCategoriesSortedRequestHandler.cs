using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Features.Category.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Category.Handlers.Queries;

public class GetCategoriesSortedRequestHandler : IRequestHandler<GetCategoriesSortedRequest, IReadOnlyList<CategoryDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoriesSortedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CategoryDTO>> Handle(GetCategoriesSortedRequest request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.CategoryRepository.GetAllSortedAsync(request.SortBy);
        
        //TODO add validation logic if needed

        return _mapper.Map<List<CategoryDTO>>(categories);
    }
}