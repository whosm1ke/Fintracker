using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockUserRepository
{
    public static Mock<IUserRepository> GetUserRepository()
    {
        var users = new List<User>()
        {
            new()
            {
                Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
                
            },
            new()
            {
                Id = new Guid("6090BFC8-4D8D-4AF5-9D37-060581F051A7")
            }
        };
        var mock = new Mock<IUserRepository>();

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) =>
            {
                return users.Find(c => c.Id == id) != null;
            });
        
        return mock;
    }
}