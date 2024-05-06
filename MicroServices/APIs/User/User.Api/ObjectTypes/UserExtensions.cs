using HotChocolate.Authorization;

namespace User.Api.ObjectTypes;

[Authorize]
[ExtendObjectType(typeof(Db.Model.User))]
public class UserExtensions
{
    
}