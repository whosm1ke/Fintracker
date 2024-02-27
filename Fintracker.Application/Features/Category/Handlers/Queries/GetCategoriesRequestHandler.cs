using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Features.Category.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Category.Handlers.Queries;

public class GetCategoriesRequestHandler : IRequestHandler<GetCategoriesRequest, IReadOnlyList<CategoryDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoriesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CategoryDTO>> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
        
        //TODO add validation logic if needed

        return _mapper.Map<List<CategoryDTO>>(categories);
    }
}