using HotChocolate.Authorization;
using Service.Db.Model;

namespace Service.Api.ObjectTypes;

[Authorize]
[ExtendObjectType(typeof(ServiceModel))]
public class ServiceExtensions
{

    [BindMember(nameof(ServiceModel.Description))]
    public string GetServiceModelDescription([Parent] ServiceModel serviceModel)
    {
        return serviceModel.Description + " extended.";
    }
}