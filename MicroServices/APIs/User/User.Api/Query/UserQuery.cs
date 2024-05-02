using User.Api.Services;
using User.Db.Model;

namespace User.Api.Query;

public class UserQuery
{
    public async Task<Db.Model.User[]> GetUsers([Service] IUserService userService, Guid[] ids) => await userService.GetUsers(ids);
    public async Task<Address[]> GetAddress([Service] IUserService userService, string query) => await userService.GetAddress(query);
}