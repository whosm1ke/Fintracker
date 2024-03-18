using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;

namespace Fintracker.Application.Contracts.Persistence;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IReadOnlyList<Category>> GetByTypeAsync(Guid userId, CategoryType type);

    Task<IReadOnlyList<Category>> GetAllSortedAsync(Guid userId, QueryParams queryParams);

    Task<IReadOnlyCollection<Category>> GetAllWithIds(ICollection<Guid> ids, Guid userId);

    Task<IReadOnlyList<Category>> GetAllAsync(Guid userId);

    Task<Guid> GetDefaultBankIncomeCategoryId();
    Task<Guid> GetDefaultBankExpenseCategoryId();

    Task<Category?> GetAsync(Guid userId, Guid id);
}