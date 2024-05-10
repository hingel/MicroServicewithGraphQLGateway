using HotChocolate.Authorization;
using User.Api.Services;

namespace User.Api.Query;

public class Query
{
    [Authorize]
    public async Task<Db.Model.User[]> GetUsers([Service] IUserService userService, Guid[] ids) => await userService.GetUsers(ids);
    [Authorize]
    public async Task<Db.Model.User?> GetUser([Service] IUserService userService, Guid id) => await userService.GetUser(id);
    public async Task<string> LogInUser([Service] IUserService userService, Guid userId) => await userService.LogInUser(userId);
}