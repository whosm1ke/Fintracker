using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Moq;

namespace TestProject1Fintracker.TEST.Repositories;

public class MockCategoryRepository
{
    public static Mock<ICategoryRepository> GetCategoryRepository()
    {
        var categories = new List<Category>()
        {
            new()
            {
                Id = new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6")
                
            },
            new()
            {
                Id = new Guid("D670263B-92CF-48C8-923A-EB09188F6077")
            }
        };
        var mock = new Mock<ICategoryRepository>();

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) =>
            {
                return categories.Find(c => c.Id == id) != null;
            });

        mock.Setup(x => x.GetAllAsyncNoTracking())
            .ReturnsAsync(categories);
        return mock;
    }
}