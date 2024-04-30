using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Category.Requests.Commands;
using MediatR;
using Newtonsoft.Json;

namespace Fintracker.Application.Features.Category.Handlers.Commands;

public class PopulateUserWithCategoriesCommandHandler : IRequestHandler<PopulateUserWithCategoriesCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PopulateUserWithCategoriesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(PopulateUserWithCategoriesCommand request, CancellationToken cancellationToken)
    {
        string categoriesJson = await File.ReadAllTextAsync(request.PathToFile, cancellationToken);
        var categories = JsonConvert.DeserializeObject<IEnumerable<Domain.Entities.Category>>(categoriesJson);

        var existingCategories = await _unitOfWork.CategoryRepository.GetAllAsync(request.UserId);

        if (existingCategories.Count == 0)
        {
            foreach (var category in categories!)
            {
                category.UserId = request.UserId;
                await _unitOfWork.CategoryRepository.AddAsync(category);
            }
        }
        else
        {
            // Отримуємо лише ті категорії, яких немає в existingCategories
            var categoriesToAdd = categories!.Except(existingCategories, new CategoryEqualityComparer()).ToList();

            foreach (var category in categoriesToAdd)
            {
                category.UserId = request.UserId;
                await _unitOfWork.CategoryRepository.AddAsync(category);
            }
        }


        await _unitOfWork.SaveAsync();
        return Unit.Value;
    }
}

public class CategoryEqualityComparer : IEqualityComparer<Domain.Entities.Category>
{
    public bool Equals(Domain.Entities.Category? x, Domain.Entities.Category? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
        return x.UserId == y.UserId;
    }

    public int GetHashCode(Domain.Entities.Category obj)
    {
        return obj.UserId.GetHashCode();
    }
}