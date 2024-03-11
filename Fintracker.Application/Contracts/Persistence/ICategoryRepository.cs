using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;

namespace Fintracker.Application.Contracts.Persistence;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IReadOnlyList<Category>> GetByTypeAsync(CategoryType type);

    Task<IReadOnlyList<Category>> GetAllSortedAsync(string sortBy, bool isDescending);

    Task<IReadOnlyCollection<Category>> GetAllWithIds(ICollection<Guid> ids);
}