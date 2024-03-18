using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Exceptions;
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
            nameof(Domain.Entities.Category.Name).ToLowerInvariant(),
            nameof(Domain.Entities.Category.Type).ToLowerInvariant()
        };
    }

    public async Task<IReadOnlyList<CategoryDTO>> Handle(GetCategoriesSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.Params.SortBy))
            throw new BadRequestException(new ExceptionDetails
            {
                PropertyName = nameof(request.Params.SortBy),
                ErrorMessage = $"Allowed values for sort by are {string.Join(", ", _allowedSortColumns)}"
            });

        var categories =
            await _unitOfWork.CategoryRepository.GetAllSortedAsync(request.UserId, request.Params.SortBy,
                request.Params.IsDescending);

        return _mapper.Map<List<CategoryDTO>>(categories);
    }
}