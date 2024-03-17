using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;

namespace Fintracker.Application.Contracts.Persistence;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IReadOnlyList<Category>> GetByTypeAsync(Guid userId, CategoryType type);

    Task<IReadOnlyList<Category>> GetAllSortedAsync(Guid userId, string sortBy, bool isDescending);

    Task<IReadOnlyCollection<Category>> GetAllWithIds(ICollection<Guid> ids);

    Task<IReadOnlyList<Category>> GetAllAsync(Guid userId);

    Task<Guid> GetDefaultBankIncomeCategoryId();
    Task<Guid> GetDefaultBankExpenseCategoryId();
}