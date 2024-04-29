using Fintracker.Application.Contracts.Identity;
using Fintracker.Domain.Entities;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockUserRepository
{
    public static Mock<IUserRepository> GetUserRepository()
    {
        var users = new List<User>
        {
            new()
            {
                Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
                Email = "user1@gmail.com",
                UserName = "username1",
            },
            //GetUserById
            new()
            {
                Id = new Guid("6090BFC8-4D8D-4AF5-9D37-060581F051A7"),
                Email = "user2gmail.com",
                UserName = "username2",
                UserDetails = new()
                {
                    Avatar = "avatar",
                }
            },
            //GetUsersAccessedToWalled, GetUserWithMemberWallets
            new()
            {
                Id = new Guid("2F566F81-4723-4D28-AB7C-A3004F98735C"),
                Email = "accessToWalletUser1",
            },
            //GetUsersAccessedToWalled
            new()
            {
                Id = new Guid("D4577085-22CE-4DE3-91E2-7C454C9653BE"),
                Email = "accessToWalletUser2",
            },
            //GetUserWithBudgets
            new()
            {
                Id = new Guid("E126CEFE-57A3-4E2A-93A6-5EE7F819B10C"),
                UserName = "With budget",
                Budgets = new List<Budget>
                {
                    new()
                    {
                        Id = new Guid("31F50DEB-14C4-4D36-A0B9-1CF1C316CE43"),
                        Name = "Budget 1"
                    },
                    new()
                    {
                        Id = new Guid("BE9D9B0D-B483-44D7-BE3B-59D9197AD5C6"),
                        Name = "Budget 2"
                    }
                }
            },
            //GetUserWithOwnedWallets
            new()
            {
                Id = new Guid("5718AD4F-3065-4E46-85A4-785E64F60EC5"),
                UserName = "With owned wallets",
            }
        };
        var mock = new Mock<IUserRepository>();

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(users.Find(c => c.Id == id) != null); });

        mock.Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .Returns((string email) => { return Task.FromResult(users.Find(c => c.Email == email) != null); });


        mock.Setup(x => x.GetAllAsync())
            .Returns(() => Task.FromResult<IReadOnlyList<User?>>(users));

        mock.Setup(x => x.GetAsNoTrackingAsync(It.IsAny<string>()))
            .Returns((string email) => { return Task.FromResult(users.Find(c => c.Email == email)); });

        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(users.Find(c => c.Id == id)); });
        

        mock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Returns((User b) =>
            {
                if (users.Find(x => x.Id == b.Id) != null)
                {
                    int index = users.FindIndex(x => x.Id == b.Id);
                    users[index] = b;
                }

                return Task.CompletedTask;
            });

        mock.Setup(x => x.DeleteAsync(It.IsAny<User>()))
            .Returns((User b) =>
            {
                if (users.Find(x => x.Id == b.Id) != null)
                {
                    int index = users.FindIndex(x => x.Id == b.Id);
                    users.RemoveAt(index);
                }

                return Task.CompletedTask;
            });


        return mock;
    }
}