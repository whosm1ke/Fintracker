using Fintracker.Application.Contracts.Identity;
using Fintracker.Domain.Entities;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockUserRepository
{
    public static List<User> Users = new List<User>
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
            Email = "user2@gmail.com",
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
    };
    public static Mock<IUserRepository> GetUserRepository()
    {
        

        var mock = new Mock<IUserRepository>();

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(Users.Find(c => c.Id == id) != null); });

        mock.Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .Returns((string email) => { return Task.FromResult(Users.Find(c => c.Email == email) != null); });


        mock.Setup(x => x.GetAllAsync())
            .Returns(() => Task.FromResult<IReadOnlyList<User?>>(Users));

        mock.Setup(x => x.GetAsNoTrackingAsync(It.IsAny<string>()))
            .Returns((string email) => { return Task.FromResult(Users.Find(c => c.Email == email)); });

        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(Users.Find(c => c.Id == id)); });


        mock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Returns((User b) =>
            {
                if (Users.Find(x => x.Id == b.Id) != null)
                {
                    int index = Users.FindIndex(x => x.Id == b.Id);
                    Users[index] = b;
                }

                return Task.CompletedTask;
            });

        mock.Setup(x => x.DeleteAsync(It.IsAny<User>()))
            .Returns((User b) =>
            {
                if (Users.Find(x => x.Id == b.Id) != null)
                {
                    int index = Users.FindIndex(x => x.Id == b.Id);
                    Users.RemoveAt(index);
                }

                return Task.CompletedTask;
            });

        mock.Setup(x => x.RegisterUserWithTemporaryPassword(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync((string? email, Guid id, string tempPass) =>
            {
                var newUser = new User
                {
                    Id = id,
                    Email = email,
                    UserName = email
                };
                Users.Add(newUser);
                return newUser;
            });

        mock.Setup(x => x.HasMemberWallet(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync((Guid walletId, string userEmail) =>
            {
                // Logic to determine if the user with the given email has the wallet as a member wallet
                var user = Users.FirstOrDefault(u => u.Email == userEmail);
                return user != null && user.MemberWallets.Any(w => w.Id == walletId);
            });


        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => Users.FirstOrDefault(u => u.Id == id));

        mock.Setup(x => x.GetAsNoTrackingAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => Users.FirstOrDefault(u => u.Email == email));

        mock.Setup(x => x.GetAsNoTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId) => Users.FirstOrDefault(u => u.Id == userId));

        mock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => Users.FirstOrDefault(u => u.Email == email));

        mock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => Users); // Returns the entire list of users


        return mock;
    }
}