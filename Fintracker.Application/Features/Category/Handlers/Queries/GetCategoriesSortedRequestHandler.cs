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
    private readonly List<string> _allowedSortColumns;

    public GetCategoriesSortedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _allowedSortColumns = new()
        {
            nameof(Domain.Entities.Category.Name),
            nameof(Domain.Entities.Category.Type)
        };
    }

    public async Task<IReadOnlyList<CategoryDTO>> Handle(GetCategoriesSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.SortBy))
            throw new BadRequestException(
                $"Invalid sortBy parameter. Allowed values {string.Join(',', _allowedSortColumns)}");
        
        var categories = await _unitOfWork.CategoryRepository.GetAllSortedAsync(request.SortBy, request.IsDescending);

        return _mapper.Map<List<CategoryDTO>>(categories);
    }
}