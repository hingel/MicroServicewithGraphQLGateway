using Messages.User;
using User.Api.Request;

namespace User.Api.Services;

public interface IUserService
{
    Task<Db.Model.User?> AddUser(CreateUser message);
    Task<Db.Model.User[]> GetUsers(Guid[] ids);
    Task<Db.Model.User?> GetUser(Guid id);
    Task<Db.Model.User?> UpdateUser(UpdateUserRequest request);
    Task<Db.Model.Address[]> GetAddress(string query);
    Task<string> LogInUser(Guid userId);
}