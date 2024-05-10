using Messages.User;

namespace User.Api.Services;

public interface IUserService
{
    Task<Db.Model.User?> AddUser(CreateUser message);
    Task<Db.Model.User[]> GetUsers(Guid[] ids);
    Task<Db.Model.User?> GetUser(Guid id);
    Task<Db.Model.User?> UpdateUser(UpdateUser message);
    Task<string> LogInUser(Guid userId);
}