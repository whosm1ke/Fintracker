using System.Linq.Dynamic.Core;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockCategoryRepository
{
    public static Mock<ICategoryRepository> GetCategoryRepository()
    {
        var categories = new List<Category>
        {
            new()
            {
                Id = new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"),
                Type = CategoryType.INCOME,
                Name = "Category 1",
                Image = "Glory",
                IconColour = "pink"
            },
            new()
            {
                Id = new Guid("D670263B-92CF-48C8-923A-EB09188F6077"),
                Type = CategoryType.EXPENSE,
                Name = "Category 2",
                Image = "frog",
                IconColour = "green"
            },
            new()
            {
                Id = new Guid("D8B7FB81-F6D9-49F0-A1C8-3B43B7D39F7C"),
                Type = CategoryType.INCOME,
                Name = "Category 3",
                Image = "Image 1",
                IconColour = "yellow"
            },
            new()
            {
                Id = new Guid("F0872017-AE98-427E-B976-B46AC2004D15"),
                Type = CategoryType.EXPENSE,
                Name = "Category 4",
                Image = "log",
                IconColour = "cyan"
            }
        };
        var mock = new Mock<ICategoryRepository>();
        //Generic Repository
        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return categories.Find(c => c.Id == id) != null; });

        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(categories.Find(x => x.Id == id)); });

        mock.Setup(x => x.GetAsyncNoTracking(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(categories.Find(x => x.Id == id)); });

        mock.Setup(x => x.GetAllAsyncNoTracking())
            .Returns(() => Task.FromResult<IReadOnlyList<Category?>>(categories));

        mock.Setup(x => x.GetAllAsync())
            .Returns(() => Task.FromResult<IReadOnlyList<Category?>>(categories));

        mock.Setup(x => x.GetAllAsync(It.IsAny<Guid>()))
            .Returns((Guid userId) => Task.FromResult<IReadOnlyList<Category>>(categories
                .Where(x => x.UserId == userId || x.UserId == null).ToList()));

        mock.Setup(x => x.AddAsync(It.IsAny<Category>()))
            .Returns((Category c) =>
            {
                categories.Add(c);
                return Task.FromResult(c);
            });

        mock.Setup(x => x.Update(It.IsAny<Category>()))
            .Callback((Category c) =>
            {
                if (categories.Find(x => x.Id == c.Id) != null)
                {
                    int index = categories.FindIndex(x => x.Id == c.Id);
                    categories[index] = c;
                }
            });

        mock.Setup(x => x.Delete(It.IsAny<Category>()))
            .Callback((Category c) =>
            {
                if (categories.Find(x => x.Id == c.Id) != null)
                {
                    int index = categories.FindIndex(x => x.Id == c.Id);
                    categories.RemoveAt(index);
                }
            });

        //ICategoryRepository
        mock.Setup(x => x.GetByTypeAsync(It.IsAny<Guid>(), It.IsAny<CategoryType>()))
            .Returns((Guid userId, CategoryType type) =>
            {
                return Task.FromResult<IReadOnlyList<Category>>(categories
                    .Where(c => c.UserId == null || c.UserId == userId)
                    .Where(x => x.Type == type).ToList());
            });

        mock.Setup(x => x.GetAllWithIds(It.IsAny<ICollection<Guid>>(), It.IsAny<Guid>()))
            .Returns((ICollection<Guid> ids, Guid userId) =>
            {
                IReadOnlyCollection<Category> cats =
                    categories.Where(x => x.UserId == userId && ids.Any(id => x.Id == id)).ToList();
                return Task.FromResult(cats);
            });

        mock.Setup(x => x.GetAllSortedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns((Guid userId, string sortBy, bool isDescending) => Task.FromResult(
                (IReadOnlyList<Category>)categories
                    .AsQueryable()
                    .OrderBy(sortBy)
                    .ToList()));
        return mock;
    }
}